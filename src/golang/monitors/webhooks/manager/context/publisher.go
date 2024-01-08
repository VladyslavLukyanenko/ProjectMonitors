package context

import (
	"bytes"
	jsoniter "github.com/json-iterator/go"
	"github.com/projectindustries/projectmonitors/core/http"
	"net/url"
)

type PublisherClient struct {
	http *http.HttpClient
	Url  *url.URL
}

type PublishPayload struct {
	Payload     string   `json:"payload"`
	Subscribers []string `json:"subscribers"`
}

func (c *PublisherClient) Dispose() {
	c.http.Release()
}

func (c *PublisherClient) Publish(n *NotificationPayload, subscribers *[]string) error {
	payload := PublishPayload{
		Payload:     n.Payload,
		Subscribers: *subscribers,
	}

	data, err := jsoniter.Marshal(payload)
	if err != nil {
		return err
	}

	reader := bytes.NewReader(data)
	c.http.SetBody(reader)
	_, err = c.http.Execute()
	if err != nil {
		return err
	}

	return nil
}

func CreatePublisher(rawUrl string) (*PublisherClient, error) {
	parsedUrl, err := url.Parse(rawUrl)
	if err != nil {
		return nil, err
	}

	httpClient := http.New(rawUrl, "")
	httpClient.SetMethod("POST")
	httpClient.SetHeader("Content-Type", "application/json")
	publisher := PublisherClient{
		http: httpClient,
		Url:  parsedUrl,
	}

	return &publisher, nil
}
