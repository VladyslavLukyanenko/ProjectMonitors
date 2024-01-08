FROM golang:latest as buildenv

WORKDIR /build

COPY go.mod go.sum ./

# Download all the dependencies
RUN go mod download

COPY . .

WORKDIR /build/webhooks/manager

RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o webhooks_manager .

FROM ubuntu
MAINTAINER Andrii Kondratiuk <andriikondratiukv@gmail.com>
WORKDIR /app
COPY --from=buildenv /build/webhooks/manager/webhooks_manager .

ENTRYPOINT ["./webhooks_manager"]