package context

type NotificationPayload struct {
	Slug    string `json:"slug"`
	Payload string `json:"payload"`
}
