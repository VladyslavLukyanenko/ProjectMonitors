FROM golang:latest as buildenv

WORKDIR /build

COPY go.mod go.sum ./

# Download all the dependencies
RUN go mod download

COPY . .

WORKDIR /build/webhooks/test_publisher

RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o test_publisher .

FROM ubuntu
MAINTAINER Andrii Kondratiuk <andriikondratiukv@gmail.com>
WORKDIR /app
COPY --from=buildenv /etc/ssl/certs/ca-certificates.crt /etc/ssl/certs/
COPY --from=buildenv /build/webhooks/test_publisher/test_publisher .

ENTRYPOINT ["./test_publisher"]