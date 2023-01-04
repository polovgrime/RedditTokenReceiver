using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTokenReceiver
{
    class Settings
    {
        public string AppId { get; set; } = string.Empty;

        public string BrowserPath { get; set; } = string.Empty;

        public int Port { get; set; } = 8080;

        public bool WaitForInput { get; set; } = true;

        public string TokenSavePath { get; set; } = string.Empty;

        public string Scope { get; set; } = string.Empty;
    }

}
