using System.Diagnostics.CodeAnalysis;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation.Model
{
  public static class ApiContractExtensions
  {
    public static ApiContract<T> ToApiContract<T>([MaybeNull] this T self)
    {
      return new ApiContract<T>(self);
    }
  }
}