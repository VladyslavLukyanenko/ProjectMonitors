using System.Threading.Tasks;
using Amazon;
using Amazon.EC2;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Compute.v1;
using Google.Apis.Http;
using Google.Apis.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectIndustries.Snkrs.Monitors.Core.Manager;
using ProjectIndustries.Snkrs.Monitors.Core.Manager.Primitives;
using ProjectIndustries.Snkrs.Monitors.Core.Manager.Services;
using ProjectIndustries.Snkrs.Monitors.Core.Manager.Services.Aws;
using ProjectIndustries.Snkrs.Monitors.Core.Manager.Services.GoogleComputeEngine;
using ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.App.Services;
using ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.App.Web;
using ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation;
using ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Foundation.SwaggerSupport.Swashbuckle;
using ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Infra;
using ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.Infra.Repositories;

namespace ProjectIndustries.Snkrs.Monitors.Web.Manager.Api
{
  public class Startup
  {
    private const string ApiVersion = "v1";
    private const string ApiTitle = "SNKRS Monitors Management API";
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddHostedService<ServerInstancesStatsHostedService>();

      services.AddScoped<IImageInfoRepository, EfImageInfoRepository>()
        .AddScoped<IImagesManager, ImagesManager>()
        .AddScoped<IInfrastructureClient, AwsInfrastructureClient>()
        // .AddScoped<IInfrastructureClient, GoogleComputeEngineInfrastructureClient>()
        .AddScoped<IDockerManager, DockerManager>()
        .AddScoped<IDockerAuthProvider, LoginPwdDockerAuthProvider>()
        .AddScoped<IImagesRuntimeInfoService, ImagesRuntimeInfoService>()
        .AddScoped<IUnitOfWork, DbContextUnitOfWork>();

      services.AddSingleton<IDockerClientsProvider, DockerClientsProvider>();
      services.AddSingleton(c =>
      {
        var config = c.GetService<AwsConfig>();
        return new AmazonEC2Client(config.AccessKeyId, config.SecretAccessKey,
          RegionEndpoint.GetBySystemName(config.PlacementRegion));
      });
      // services.AddSingleton(c =>
      // {
      //   var config = c.GetService<GoogleComputeEngineConfig>();
      //   return new ComputeService(new BaseClientService.Initializer
      //   {
      //     ApplicationName = config.ApplicationName,
      //     HttpClientInitializer = GetCredential(config)
      //   });
      // });

      services
        .InitializeConfiguration(_configuration)
        .AddApplicationDbContext(_configuration)
        .AddConfiguredCors(_configuration)
        .AddConfiguredMvc()
        .AddConfiguredSignalR()
        .AddConfiguredAuthentication(_configuration)
        .AddConfiguredSwagger(ApiVersion, ApiTitle);


      services.AddIdentityServer(options => { options.EmitStaticAudienceClaim = true; })
        .AddDeveloperSigningCredential()
        .AddInMemoryClients(IdentityServerStaticConfig.GetClients())
        .AddInMemoryApiResources(IdentityServerStaticConfig.GetApiResources())
        .AddInMemoryIdentityResources(IdentityServerStaticConfig.GetIdentityResources())
        .AddTestUsers(IdentityServerStaticConfig.GetUsers());
      services.AddIdentityServerConfiguredCors(_configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      var corsConfig = app.UseCommonHttpBehavior(env);
      app.UseStaticFiles();
      app.UseIdentityServer();
      app.UseRouting();
      app.UseConfiguredCors(corsConfig);

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapDefaultControllerRoute();
      });

      app.UseConfiguredSwagger(ApiVersion, ApiTitle);
    }

    private static IConfigurableHttpClientInitializer GetCredential(GoogleComputeEngineConfig config)
    {
      GoogleCredential credential = Task.Run(() => GoogleCredential.FromFile(config.CredentialsPath)).Result;
      if (credential.IsCreateScopedRequired)
      {
        credential = credential.CreateScoped("https://www.googleapis.com/auth/cloud-platform");
      }

      return credential;
    }
  }
}