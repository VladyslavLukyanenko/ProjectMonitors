using System.Diagnostics.CodeAnalysis;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation.Model
{
  public class ApiContract<T>
  {
    public ApiContract([MaybeNull]T payload)
      : this(payload, null)
    {
    }

    public ApiContract(ApiError? error)
      : this(default!, error)
    {
    }

    protected ApiContract([MaybeNull] T payload, ApiError? error)
    {
      Payload = payload;
      Error = error;
    }

    public ApiError? Error { get; }

    [MaybeNull]
    public T Payload { get; }
  }
}