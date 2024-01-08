namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Commands
{
  public class StartOrRunServerInstanceCommand
  {
    public string ProviderName { get; set; }
    public string StoppedInstanceId { get; set; }
  }
}