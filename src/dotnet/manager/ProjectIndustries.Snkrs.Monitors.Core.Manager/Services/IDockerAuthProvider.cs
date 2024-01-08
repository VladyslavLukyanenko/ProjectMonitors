using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace ProjectIndustries.Snkrs.Monitors.Core.Manager.Services
{
  public interface IDockerAuthProvider
  {
    Task<AuthConfig> GetAuthConfigAsync(CancellationToken ct = default);
  }
}