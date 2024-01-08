package footlocker

import (
	"github.com/projectindustries/projectmonitors/core/http"
	"github.com/projectindustries/projectmonitors/publishers/footlocker/config"
)

type ProcessingContext struct {
	cfg         *config.FootlockerMonitorCfg
	pageClient  *http.HttpClient
	sizesClient *http.HttpClient
}

func NewContext(rawUrl string, publishQueueName string, publishExchange string) *ProcessingContext {
	cfg := config.New(rawUrl, publishQueueName, publishExchange)
	ctx := ProcessingContext{
		cfg:         cfg,
		pageClient:  http.New(cfg.GetUrl(), ""),
		sizesClient: http.NewNotInitialized(),
	}

	ctx.sizesClient.SetReferer(cfg.GetUrl())
	ctx.sizesClient.SetHost(cfg.GetHost())

	return &ctx
}

func (ctx *ProcessingContext) Release() {
	ctx.pageClient.Release()
	ctx.sizesClient.Release()
}
