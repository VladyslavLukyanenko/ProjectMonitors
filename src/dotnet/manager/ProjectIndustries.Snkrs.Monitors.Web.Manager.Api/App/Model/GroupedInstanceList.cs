using System.Collections.Generic;
using ProjectIndustries.Snkrs.Monitors.Core.Manager;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.App.Model
{
  public class GroupedInstanceList
  {
    public string ProviderName { get; set; }
    public List<ServerInstance> Instances { get; set; } = new List<ServerInstance>();
  }
}