package main

import (
	_ "github.com/projectindustries/projectmonitors/publishers/adidas"
	"github.com/projectindustries/projectmonitors/publishers/footlocker"
)

func main() {
	footlocker.StartFootlocker()
	//adidas.Start()
}
