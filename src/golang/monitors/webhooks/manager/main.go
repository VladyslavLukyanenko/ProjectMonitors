package main

import (
	jsoniter "github.com/json-iterator/go"
	_ "github.com/lib/pq"
	"github.com/projectindustries/projectmonitors/core/domain/webhook"
	"github.com/projectindustries/projectmonitors/core/util"
	"github.com/projectindustries/projectmonitors/webhooks/manager/context"
	"github.com/streadway/amqp"
	"log"
	"os"
	"time"
)

func main() {
	amqpConn := os.Getenv("CONNECTION_STRINGS_RABBITMQ")
	psqlConn := os.Getenv("CONNECTION_STRINGS_POSTGRESQL")
	if amqpConn == "" {
		panic("no amqp connection string provided")
	}
	if psqlConn == "" {
		panic("no psql connection string provided")
	}

	//amqpConn := "amqp://guest:guest@localhost:5672/"
	//psqlConn := "amqp://guest:guest@localhost:5672/"
	ctx := context.NewContext(amqpConn, psqlConn)
	defer ctx.Dispose()

	//go schedulePublishersRefresh(ctx)
	go scheduleSettingsRefresh(ctx)
	go startConsumer(ctx)

	util.WaitForShutdown()
}

//
//func schedulePublishersRefresh(ctx *ManagerContext) {
//	for true {
//		_ = RefreshPublishers(ctx)
//		time.Sleep(30 * time.Second)
//	}
//}

func scheduleSettingsRefresh(ctx *context.ManagerContext) {
	for true {
		_ = context.RefreshSettings(ctx)
		time.Sleep(30 * time.Second)
	}
}

func startConsumer(ctx *context.ManagerContext) {
	channel := ctx.Bus.Channel
	payloads, err := channel.Consume(
		ctx.Bus.Queue.Name,
		"",
		false,
		false,
		false,
		false,
		nil,
	)
	util.FailOnError(err, "Failed to register consumer")

	forever := make(chan bool)

	go func() {
		for payload := range payloads {
			var notification context.NotificationPayload
			err := jsoniter.Unmarshal(payload.Body, &notification)
			util.FailOnError(err, "Failed to parse notification payload")
			list, ok := (*ctx.SettingsCache)[notification.Slug]
			if !ok {
				log.Printf("Can't find config for %s", notification.Slug)
				continue
			}

			publishPayload := webhook.PublishPayload{
				Payload: notification.Payload,
			}
			for _, webHookUrl := range list {
				publishPayload.Subscriber = webHookUrl

				body, _ := jsoniter.Marshal(publishPayload)
				err = channel.Publish(
					webhook.PublishExchangeName,
					webhook.SenderExchangeKey,
					false,
					false,
					amqp.Publishing{
						ContentType: "application/json",
						Body:        body,
					},
				)

				if err != nil {
					log.Printf("[ERR] Can't publish payload to the queue")
				} else {
					log.Printf("Published webhook from monitor '%s' url '%s'", notification.Slug, webHookUrl)
				}
			}

			_ = payload.Ack(false)
		}
	}()

	log.Println("Waiting for notifications. To exit press CTRL+C")
	<-forever
}
