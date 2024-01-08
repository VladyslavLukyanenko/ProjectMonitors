namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation.Config
{
  public class DataSourceConfig
  {
    public string PostgresConnectionString { get; set; }

    public int MaxRetryCount { get; set; }
  }
}