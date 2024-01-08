package context

import (
	"database/sql"
	"github.com/projectindustries/projectmonitors/core/util"
	"github.com/projectindustries/projectmonitors/webhooks/manager/bus"
	"github.com/projectindustries/projectmonitors/webhooks/manager/db"
)

type ManagerContext struct {
	Database      *sql.DB
	Bus           *bus.DiscordHooksConsumerContext
	SettingsCache *map[string][]string
	Publishers    *[]*PublisherClient
}

func NewContext(amqpConnStr string, postgresConStr string) *ManagerContext {
	dbConnection, err := db.EstablishDbConnection(postgresConStr)
	util.FailOnError(err, "Can't establish db connection")

	busCtx := bus.EstablishAmqpConnection(amqpConnStr)

	var settingsCache map[string][]string
	var publishers []*PublisherClient

	ctx := ManagerContext{
		Database:      dbConnection,
		Bus:           busCtx,
		SettingsCache: &settingsCache,
		Publishers:    &publishers,
	}

	return &ctx
}

func (ctx *ManagerContext) Dispose() {
	ctx.Bus.Close()
	for _, p := range *ctx.Publishers {
		p.Dispose()
	}

	err := ctx.Database.Close()
	util.FailOnError(err, "Failed to close database connection")
}
