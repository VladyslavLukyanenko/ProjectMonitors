using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using ProjectIndustries.Snkrs.Monitors.Core.Manager;
using ProjectIndustries.Snkrs.Monitors.Core.Manager.Services;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.App.Services
{
  public class ImagesRuntimeInfoService : IImagesRuntimeInfoService
  {
    private readonly IDockerClientsProvider _dockerClientsProvider;

    private readonly IImageInfoRepository _imageInfoRepository;
    // private readonly IEnumerable<IInfrastructureClient> _infrastructureClients;

    public ImagesRuntimeInfoService(IDockerClientsProvider dockerClientsProvider,
      IImageInfoRepository imageInfoRepository)
    {
      _dockerClientsProvider = dockerClientsProvider;
      _imageInfoRepository = imageInfoRepository;
    }

    public async Task RefreshStateAsync(IEnumerable<ServerInstance> nodes, CancellationToken ct = default)
    {
      var monitors = await _imageInfoRepository.ListAllAsync(ct);
      var clients = new Dictionary<ServerInstance, DockerClient>();
      foreach (var serverNodeInfo in nodes)
      {
        if (!serverNodeInfo.IsRunning)
        {
          serverNodeInfo.Checked(false);
          continue;
        }

        DockerClient client;
        if (!_dockerClientsProvider.TryGetClient(serverNodeInfo, out client))
        {
          _dockerClientsProvider.AddClient(serverNodeInfo);
          client = _dockerClientsProvider.GetClient(serverNodeInfo);
        }

        clients[serverNodeInfo] = client;
      }

      var tasks = clients.Select(async p =>
      {
        var client = p.Value;
        var instance = p.Key;
        instance.Checked(instance.DockerRemoteApiUrl != null && await IsRemoteApiAvailableAsync(client, ct));
        if (instance.IsAvailable)
        {
          var containers = await client.Containers.ListContainersAsync(new ContainersListParameters {All = true}, ct);
          var images = new List<ImageRuntimeInfo>();
          foreach (var monitor in monitors)
          {
            var image = containers.Where(_ => _.ImageID == monitor.Id)
              .Select(c => new ImageRuntimeInfo(instance, monitor, c.ID, c.Status, c.State, c.Created));

            images.AddRange(image);
          }

          instance.Merge(images);
        }
      });

      await Task.WhenAll(tasks);
    }

    private async Task<bool> IsRemoteApiAvailableAsync(DockerClient client, CancellationToken ct = default)
    {
      try
      {
        var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        cts.CancelAfter(TimeSpan.FromSeconds(5));
        await client.System.PingAsync(cts.Token);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}