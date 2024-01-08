using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using ProjectIndustries.Snkrs.Monitors.Core.Manager.Services;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.App.Services
{
  public class LoginPwdDockerAuthProvider : IDockerAuthProvider
  {
    private readonly LoginPwdDockerAuthConfig _config;

    public LoginPwdDockerAuthProvider(LoginPwdDockerAuthConfig config)
    {
      _config = config;
    }

    public Task<AuthConfig> GetAuthConfigAsync(CancellationToken ct = default)
    {
      var auth = new AuthConfig
      {
        Username = _config.Username,
        Password = _config.Password
      };

      return Task.FromResult(auth);
    }
  }
}