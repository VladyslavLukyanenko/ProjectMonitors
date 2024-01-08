package db

import (
	"database/sql"
	"log"
	"time"
)

func EstablishDbConnection(connectionString string) (*sql.DB, error) {
	var db *sql.DB
	var err error
	for db, err = connect(connectionString); err != nil; {
		delay := time.Second * 2
		log.Printf("Can't establish connection with db: %v. Retry in %v\n", err, delay)
		time.Sleep(delay)
		return EstablishDbConnection(connectionString)
	}

	return db, err
}

func connect(connectionString string) (*sql.DB, error) {
	db, err := sql.Open("postgres", connectionString)

	createDbStatement, err := db.Prepare("CREATE TABLE IF NOT EXISTS public.subscriptions (id SERIAL PRIMARY KEY, slug VARCHAR(255) NOT NULL, discord_webhook_url VARCHAR(4096) NOT NULL)")
	if err != nil {
		return nil, err
	}

	_, err = createDbStatement.Exec()
	if err != nil {
		return nil, err
	}

	//createPublishersDbStatement, _ := db.Prepare("CREATE TABLE IF NOT EXISTS public.publishers (id INT PRIMARY KEY, url VARCHAR(255) NOT NULL")
	//createPublishersDbStatement.Exec()

	return db, err
}
