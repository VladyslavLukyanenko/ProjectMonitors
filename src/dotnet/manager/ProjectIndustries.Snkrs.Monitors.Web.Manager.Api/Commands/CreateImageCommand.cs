using System.Collections.Generic;
using ProjectIndustries.Snkrs.Monitors.Core.Manager;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Commands
{
  public class CreateImageCommand
  {
    public string ImageName { get; set; }
    public string Slug { get; set; }
    public ImageType ImageType { get; set; }
    public List<string> RequiredSpawnParameters { get; set; } = new List<string>();
  }
}