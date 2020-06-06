using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Amazon.Lambda.Core;
using Amazon.StorageGateway;
using Amazon.StorageGateway.Model;

using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace EventTrigger
{
  public class Handler
  {
    private IConfiguration _configuration;

    public async Task StorageGatewayRefreshCache(RefreshCacheRequest request, ILambdaContext context)
    {
      InitConfig();

      // AmazonStorageGatewayClient client = new AmazonStorageGatewayClient();
      // client.RefreshCacheAsync(request);

      // Serialize the event object
      string result = JsonConvert.SerializeObject(request);

      // Send the Slack message
      SlackClient slackClient = new SlackClient(_configuration["SlackWebhookUrl"]);
      await slackClient.PostMessage($"RefreshCache Started: {result}");

    }

    public async Task ListFileShares(ListFileSharesRequest request)
    {
      InitConfig();

      AmazonStorageGatewayClient client = new AmazonStorageGatewayClient();
      ListFileSharesResponse fileShares = await client.ListFileSharesAsync(request);
      StringBuilder sbFileShares = new StringBuilder();
      foreach(FileShareInfo info in fileShares.FileShareInfoList)
      {
        sbFileShares.AppendLine(info.FileShareARN);
      }

      // return JsonConvert.SerializeObject(sbFileShares.ToString());
      // Send the Slack message
      SlackClient slackClient = new SlackClient(_configuration["SlackWebhookUrl"]);
      await slackClient.PostMessage($"File shares: \n{sbFileShares.ToString()}");

    }

    private void InitConfig()
    {
      _configuration = new ConfigurationBuilder()
      .AddEnvironmentVariables()
      .Build();
    }

  }

  public class Response
  {
    public string Message { get; set; }
    public Request Request { get; set; }

    public Response(string message, Request request)
    {
      Message = message;
      Request = request;
    }
  }

  public class Request
  {
    public string Key1 { get; set; }
    public string Key2 { get; set; }
    public string Key3 { get; set; }
  }
}
