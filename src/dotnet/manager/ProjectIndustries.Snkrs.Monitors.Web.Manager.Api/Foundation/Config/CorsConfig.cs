using System.Collections.Generic;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation.Config
{
  public class CorsConfig
  {
    public bool UseCors { get; set; }
    public List<string> AllowedHosts { get; set; }
  }
}