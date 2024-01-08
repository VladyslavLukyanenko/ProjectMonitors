package main

import (
	"bytes"
	jsoniter "github.com/json-iterator/go"
	"github.com/projectindustries/projectmonitors/core/domain/webhook"
	"github.com/projectindustries/projectmonitors/core/http"
	"github.com/projectindustries/projectmonitors/core/util"
	"github.com/streadway/amqp"
	"log"
	"os"
	"time"
)

type DiscordRateLimitedError struct {
	RetryAfter int32 `json:"retry_after"`
}

func main() {
	httpClient := http.New("https://discordapp.com/", "")
	httpClient.SetMethod("POST")
	httpClient.SetHeader("Content-Type", "application/json")
	defer httpClient.Release()
	amqpConnStr := os.Getenv("CONNECTION_STRINGS_RABBITMQ")
	//amqpConnStr := "amqp://guest:guest@localhost:5672/"
	senderName := "DefaultSender"
	go startConsumer(amqpConnStr, httpClient, senderName)
	util.WaitForShutdown()
}

func submitWebhook(payload *[]byte, client *http.HttpClient, senderName string) (int32, error) {
	t := time.Now().UnixNano()

	serializedPayload := bytes.NewReader(*payload)
	client.SetBody(serializedPayload)
	r, err := client.Execute()
	if err != nil {
		rateLimited := DiscordRateLimitedError{}
		unmarshalErr := jsoniter.Unmarshal(r, &rateLimited)
		if unmarshalErr == nil && rateLimited.RetryAfter > 0 {
			return rateLimited.RetryAfter, err
		} else {
			log.Printf("[%s] [ERR] Error occurred on webhook sending %s", senderName, err.Error())
		}
	} else {
		log.Printf("[%s] ==== Webhook sent in '%d'ms====\nResponse: %s\n", senderName, (time.Now().UnixNano()-t)/1000_000, string(r))
	}

	return 0, nil
}

func startConsumer(amqpConnStr string, httpClient *http.HttpClient, senderName string) {
	conn, err := amqp.Dial(amqpConnStr)
	util.FailOnError(err, "Failed to connect to rabbit mq")
	defer conn.Close()

	ch, err := conn.Channel()
	util.FailOnError(err, "Failed to open a channel")
	defer ch.Close()

	queue, err := ch.QueueDeclare(
		senderName,
		true,
		false,
		false,
		false,
		nil,
	)
	util.FailOnError(err, "Can't declare a queue")

	err = ch.QueueBind(
		queue.Name,
		webhook.SenderExchangeKey,
		webhook.PublishExchangeName,
		false,
		nil,
	)
	util.FailOnError(err, "Failed to bind a queue")

	payloads, err := ch.Consume(
		queue.Name,
		"",
		false,
		false,
		false,
		false,
		nil,
	)
	util.FailOnError(err, "Failed to register consumer")

	log.Printf("[%s] Waiting for notifications. To exit press CTRL+C\n", senderName)
	for payload := range payloads {
		var notification webhook.PublishPayload
		err := jsoniter.Unmarshal(payload.Body, &notification)
		util.FailOnError(err, "Failed to parse notification payload")
		httpClient.SetUrl(notification.Subscriber)

		p := []byte(notification.Payload)
		delay, err := submitWebhook(&p, httpClient, senderName)
		for err != nil && delay > 0 {
			log.Printf("[%s] We are rate limited. Retry to send webhook in %dms\n", senderName, delay)
			delayDuration := time.Millisecond * time.Duration(delay)
			time.Sleep(delayDuration)
			delay, err = submitWebhook(&p, httpClient, senderName)
		}

		_ = payload.Ack(false)
	}

}
