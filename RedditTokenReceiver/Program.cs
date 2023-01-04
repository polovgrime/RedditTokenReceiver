using Newtonsoft.Json;

namespace RedditTokenReceiver
{
    class Program
    {
        private static Settings settings = new Settings();

        private static string savePath = "";

        public static async Task Main(string[] args)
        {
            settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("Settings.json"))!;
            savePath = Path.IsPathFullyQualified(settings.TokenSavePath) ? settings.TokenSavePath : Directory.GetCurrentDirectory() + settings.TokenSavePath;

            var tokens = await GetToken();

            Console.WriteLine($"Refresh Token: {tokens.RefreshToken}\n" +
                $"Access Token: {tokens.AccessToken}");

            SaveToken(tokens, savePath);

            Console.WriteLine($"Saved at {savePath}");
            if (settings.WaitForInput)
            {
                Console.ReadLine();
            }
        }

        private static void SaveToken(Token tokens, string path)
        {

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText(path + "reddit-tokens.json", JsonConvert.SerializeObject(tokens));
        }

        private static async Task<Token> GetToken()
        {
            var auth = new BrowserAuthentication(settings.AppId, settings.BrowserPath, savePath, settings.Scope).Authenticate();
            var refreshToken = await auth.GetRefreshToken();
            return refreshToken;
        }
    }
}