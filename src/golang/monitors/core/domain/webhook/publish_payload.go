package webhook


type PublishPayload struct {
	Payload    string `json:"payload"`
	Subscriber string `json:"subscriber"`
}
