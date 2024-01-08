package bus

import (
	"github.com/projectindustries/projectmonitors/core/domain/webhook"
	"github.com/projectindustries/projectmonitors/core/util"
	"github.com/streadway/amqp"
	"log"
	"time"
)

type DiscordHooksConsumerContext struct {
	connection *amqp.Connection
	Channel    *amqp.Channel
	Queue      *amqp.Queue
}

func (ctx *DiscordHooksConsumerContext) Close() {
	_ = ctx.connection.Close()
	_ = ctx.Channel.Close()
}

func EstablishAmqpConnection(amqpConnStr string) *DiscordHooksConsumerContext {
	var conn *amqp.Connection
	var err error
	for conn, err = amqp.Dial(amqpConnStr); err != nil; {
		delay := time.Second * 2
		log.Printf("Failed to connect to rabbit mq: %v. Retry in %v", err, delay)
	}

	ch, err := conn.Channel()
	util.FailOnError(err, "Failed to open a channel")

	queue, err := ch.QueueDeclare(
		"",
		true,
		false,
		false,
		false,
		nil,
	)
	util.FailOnError(err, "Can't declare a queue")

	err = ch.ExchangeDeclare(
		webhook.PublishExchangeName,
		"direct",
		true,
		false,
		false,
		false,
		nil,
	)

	util.FailOnError(err, "Failed to declare an exchange")

	err = ch.QueueBind(
		queue.Name,
		webhook.BalancerExchangeKey,
		webhook.PublishExchangeName,
		false,
		nil,
	)
	util.FailOnError(err, "Failed to bind a queue")

	ctx := DiscordHooksConsumerContext{
		connection: conn,
		Channel:    ch,
		Queue:      &queue,
	}

	return &ctx
}
