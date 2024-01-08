package main

import (
	"fmt"
	jsoniter "github.com/json-iterator/go"
	"github.com/projectindustries/projectmonitors/core/domain/webhook"
	"github.com/projectindustries/projectmonitors/core/util"
	"github.com/streadway/amqp"
	"log"
	"os"
	"time"
)

const (
	//webHookUrl = "https://discordapp.com/api/webhooks/735735485695131699/1HQCmXtkA5Ov-auFWgmoR0kDFT3r17iJiTYnjour-szIofeG1Ixs8PTtqPfLF4J-a_-f"
	monitorSlug = "fake_monitor"
)

type WebhookPayload struct {
	Content  string `json:"content"`
	Username string `json:"username"`
}

type NotificationPayload struct {
	Slug    string `json:"slug"`
	Payload string `json:"payload"`
}

func main() {
	//amqpConn := "amqp://guest:guest@localhost:5672/"
	amqpConn := os.Getenv("CONNECTION_STRINGS_RABBITMQ")
	go startConsumer(amqpConn)
	util.WaitForShutdown()
}

func startConsumer(amqpConnStr string) {
	conn, err := amqp.Dial(amqpConnStr)
	util.FailOnError(err, "Failed to connect to rabbit mq")
	defer conn.Close()

	ch, err := conn.Channel()
	util.FailOnError(err, "Failed to open a channel")
	defer ch.Close()

	//console := bufio.NewReader(os.Stdin)
	for {
		//fmt.Println("To publish test payload press [ENTER]")
		//_, _, _ = console.ReadLine()
		time.Sleep(time.Second * 15)

		content := WebhookPayload{
			Content:  fmt.Sprintf("This is a fake webhook content. Sent at '%s'", time.Now().UTC().String()),
			Username: "FAKE_MONITOR",
		}

		json, _ := jsoniter.Marshal(content)
		payload := NotificationPayload{
			Slug:    monitorSlug,
			Payload: string(json),
		}

		body, _ := jsoniter.Marshal(payload)
		err = ch.Publish(
			webhook.PublishExchangeName,
			webhook.BalancerExchangeKey,
			false,
			false,
			amqp.Publishing{
				ContentType: "application/json",
				Body:        body,
			},
		)

		log.Println("Published")
	}
}
