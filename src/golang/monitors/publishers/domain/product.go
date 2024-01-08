package domain

type ProductSize struct {
	Id string
	DisplayName string
}

type Product struct {
	PictureUrl string
	DisplayName string
	Url string
	Price string
	Sizes *[]ProductSize
	IsAvailable bool
}
