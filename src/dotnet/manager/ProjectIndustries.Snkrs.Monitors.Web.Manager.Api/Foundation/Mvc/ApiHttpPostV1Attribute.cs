using Microsoft.AspNetCore.Mvc;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation.Mvc
{
  public class ApiHttpPostV1Attribute : HttpPostAttribute
  {
    public ApiHttpPostV1Attribute()
      : this("")
    {
    }

    public ApiHttpPostV1Attribute(string template, bool fromRoot = false)
      : base(fromRoot && !template.StartsWith("/")
        ? "/" + ApiV1HttpRouteAttribute.Prefix + template
        : ApiV1HttpRouteAttribute.Prefix + template)
    {
    }
  }
}