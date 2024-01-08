using Microsoft.AspNetCore.Mvc;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation.Mvc
{
  public class ApiV1HttpRouteAttribute : RouteAttribute
  {
    public const string Prefix = "v1/";

    public ApiV1HttpRouteAttribute(string template)
      : base(Prefix + template)
    {
    }
  }
}