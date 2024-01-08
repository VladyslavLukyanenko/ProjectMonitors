package context

func RefreshSettings(ctx *ManagerContext) error {
	settingsCache := make(map[string][]string)
	rows, err := ctx.Database.Query("SELECT slug, discord_webhook_url from public.subscriptions")
	if err != nil {
		return err
	}

	for rows.Next() {
		var slug string
		var url string
		_ = rows.Scan(&slug, &url)

		list, ok := settingsCache[slug]
		if !ok {
			list = []string{}
		}

		settingsCache[slug] = append(list, url)
	}

	ctx.SettingsCache = &settingsCache
	return nil
}
