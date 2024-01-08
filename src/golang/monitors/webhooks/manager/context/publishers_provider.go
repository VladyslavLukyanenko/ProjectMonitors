package context

import "github.com/projectindustries/projectmonitors/core/util"

func RefreshPublishers(ctx *ManagerContext) error {
	rows, err := ctx.Database.Query("SELECT url from public.publishers")
	if err != nil {
		return err
	}

	publishers := make(map[string]*PublisherClient)
	for _, p := range *ctx.Publishers {
		publishers[(*p).Url.String()] = p
	}

	var receivedUrls []string
	for rows.Next() {
		var rawUrl string
		_ = rows.Scan(&rawUrl)

		receivedUrls = append(receivedUrls, rawUrl)
		if _, ok := publishers[rawUrl]; ok {
			continue
		}

		publisher, err := CreatePublisher(rawUrl)
		util.FailOnError(err, "Can't create publisher")

		v := append(*ctx.Publishers, publisher)
		ctx.Publishers = &v
	}

	for idx, p := range *ctx.Publishers {
		if !contains(&receivedUrls, p.Url.String()) {
			p.Dispose()
			remove(ctx.Publishers, idx)
		}
	}

	return nil
}

func contains(s *[]string, e string) bool {
	for _, a := range *s {
		if a == e {
			return true
		}
	}
	return false
}

func remove(slice *[]*PublisherClient, s int) *[]*PublisherClient {
	v := append((*slice)[:s], (*slice)[s+1:]...)

	return &v
}
