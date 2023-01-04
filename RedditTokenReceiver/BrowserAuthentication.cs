using System.Diagnostics;

namespace RedditTokenReceiver
{
    internal class BrowserAuthentication
    {
        private readonly int _port;

        private readonly string _appId;

        private readonly string _browserPath;

        private readonly string _tokenSavePath;

        private readonly string _appSecret = "";

        private readonly string _host = "127.0.0.1";

        private string DefaultScope => $"creddits%20" +
            $"modcontributors%20" +
            $"modmail%20modconfig%20" +
            $"subscribe%20" +
            $"structuredstyles%20" +
            $"vote%20" +
            $"wikiedit%20" +
            $"mysubreddits%20" +
            $"submit%20modlog%20" +
            $"modposts%20" +
            $"modflair%20" +
            $"save%20" +
            $"modothers%20" +
            $"read%20" +
            $"privatemessages%20" +
            $"report%20" +
            $"identity%20" +
            $"livemanage%20" +
            $"account%20" +
            $"modtraffic%20" +
            $"wikiread%20" +
            $"edit%20" +
            $"modwiki%20" +
            $"modself%20" +
            $"history%20" +
            $"flair";

        private readonly ReceiverHttpServer _server;

        public BrowserAuthentication(string appId, string browserPath, string tokenSavePath, string scope, string host = "127.0.0.1", int port = 8080)
        {
            _appId = appId;
            _port = port;
            _browserPath = browserPath;
            _tokenSavePath = tokenSavePath;
            _server = new ReceiverHttpServer(_host, _port, _tokenSavePath);
        }

        public BrowserAuthentication(string appId, string appSecret, string browserPath, string tokenSavePath, string scope, string host = "127.0.0.1", int port = 8080) : this(appId, browserPath, tokenSavePath, scope, host, port)
        {
            _appSecret = appSecret;
        }

        public BrowserAuthentication Authenticate()
        {
            OpenBrowser(AuthURL());

            _server.Start();

            return this;
        }

        public async Task<Token> GetRefreshToken()
        {
            return await _server.GetToken();

        }

        private string AuthURL(string scope = "")
        {
            if (string.IsNullOrEmpty(scope))
            {
                scope = DefaultScope;
            }

            return
                $"https://www.reddit.com/api/v1/authorize?client_id={_appId}" +
                $"&response_type=code&state={_appId}:{_appSecret}" +
                $"&redirect_uri=http://{_host}:{_port}/" +
                $"&duration=permanent" +
                $"&scope={scope}";
        }

        private void OpenBrowser(string authUrl)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
                Process.Start(processStartInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(_browserPath)
                {
                    Arguments = authUrl
                };
                Process.Start(processStartInfo);
            }
        }
    }
}
