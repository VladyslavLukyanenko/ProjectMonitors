using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Snkrs.Monitors.Core.Manager.Services
{
  public interface IDockerManager
  {
    Task<bool> IsRemoteApiAvailableAsync(Uri serverEndpoint, CancellationToken ct = default);
    Task<string> CreateImageAsync(string image, CancellationToken ct = default);
  }
}