using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace EventTrigger
{
  class SlackClient
  {
    private readonly Uri _uri;
    private readonly Encoding _encoding = new UTF8Encoding();

    private readonly HttpClient _httpClient;

    public SlackClient(string urlWithAccessToken)
    {
      _uri = new Uri(urlWithAccessToken);
      _httpClient = new HttpClient();
    }

    public async Task PostMessage(string text)
    {
      Payload payload = new Payload()
      {
        Text = text
      };

      await PostMessage(payload);
    }

    public async Task PostMessage(string text, string icon_emoji = null)
    {
      Payload payload = new Payload()
      {
        Text = text,
        Icon_Emoji = icon_emoji
      };

      await PostMessage(payload);
    }

    public async Task PostMessage(string text, string channel = null, string username = null, string icon_emoji = null)
    {
      Payload payload = new Payload()
      {
        Channel = channel,
        Username = username,
        Text = text,
        Icon_Emoji = icon_emoji
      };

      await PostMessage(payload);
    }

    //Post a message using a Payload object
    public async Task PostMessage(Payload payload)
    {
      string payloadJson = JsonConvert.SerializeObject(payload);
			await _httpClient.PostAsync(_uri, new StringContent(payloadJson, Encoding.Default, "application/json"));
    }

  }

  //This class serializes into the Json payload required by Slack Incoming WebHooks
  public class Payload
  {
    [JsonProperty("channel")]
    public string Channel { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("icon_emoji")]
    public string Icon_Emoji { get; set; }
  }

}
