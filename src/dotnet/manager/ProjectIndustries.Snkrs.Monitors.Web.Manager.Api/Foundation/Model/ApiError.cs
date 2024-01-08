using System;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation.Model
{
  public class ApiError
  {
    public ApiError(Exception exc)
      : this(exc.Message)
    {
    }

    public ApiError(string code, object? details = null)
    {
      Message = code;
      Details = details;
    }

    public string Message { get; }

    public object? Details { get; }
  }
}