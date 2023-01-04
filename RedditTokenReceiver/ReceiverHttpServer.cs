using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Text;

namespace RedditTokenReceiver
{
    internal class ReceiverHttpServer
    {
        private HttpListener listener;

        private string url;

        private string _tokenSavePath;

        private TaskCompletionSource<Token> tcs = new TaskCompletionSource<Token>();

        private readonly string _successTemplate;

        public ReceiverHttpServer(string host, int port, string tokenSavePath)
        {
            url = $"http://{host}:{port}/";
            listener = new HttpListener();
            _tokenSavePath = tokenSavePath;
            _successTemplate = Directory.GetCurrentDirectory() + "\\Templates\\Success.html";
        }

        public void Start()
        {
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);
            var listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();
            listener.Close();
        }

        public async Task<Token> GetToken()
        {
            return await tcs.Task;
        }

        private async Task HandleIncomingConnections()
        {
            var ctx = await listener.GetContextAsync();
            var resp = ctx.Response;
            var code = ctx.Request.QueryString["code"];
            var state = ctx.Request.QueryString["state"];

            Console.WriteLine("Accepted redirect from reddit.");

            if (code == null || state == null)
            {
                throw new Exception("Code And State are required");
            }

            var sourceToken = ExecuteRequest(CreateRequest(code, state));
            var token = JsonConvert.DeserializeObject<Token>(sourceToken);
            var data = Encoding.UTF8.GetBytes(PrintTemplate(token));

            resp.ContentType = "text/html";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            await resp.OutputStream.WriteAsync(data, 0, data.Length);

            resp.Close();
            tcs.SetResult(token);
        }

        private RestRequest CreateRequest(string code, string state)
        {
            var restRequest = new RestRequest("/api/v1/access_token", Method.POST);

            restRequest.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(state)));
            restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("grant_type", "authorization_code");
            restRequest.AddParameter("code", code);
            restRequest.AddParameter("redirect_uri", url);

            return restRequest;
        }

        private string ExecuteRequest(RestRequest restRequest)
        {
            IRestResponse restResponse = new RestClient("https://www.reddit.com").Execute(restRequest);
            if (restResponse != null && restResponse.IsSuccessful)
            {
                return restResponse.Content;
            }

            throw new Exception("API returned non-success response.")
            {
                Data = 
                {
                    {
                        (object)"res",
                        (object?)restResponse
                    } 
                }
            };
        }

        private string PrintTemplate(Token token)
        {
            var successTemplate = ReadTemplate(_successTemplate);

            successTemplate = successTemplate
                .Replace("ACCESS_TOKEN", token.AccessToken)
                .Replace("REFRESH_TOKEN", token.RefreshToken)
                .Replace("TOKEN_LOCATION", _tokenSavePath);

            return successTemplate;
        }

        private string ReadTemplate(string path)
        {
            return File.ReadAllText(path);
        }

    }
}
