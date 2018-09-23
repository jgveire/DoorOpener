namespace DoorWebsite
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;

    public sealed class DoorManager
    {
        private readonly IConfiguration _configuration;
        private readonly object _lockObject = new object();

        public DoorManager(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Open()
        {
            lock (_lockObject)
            {
                var path = _configuration.GetSection("Door").GetValue<string>("WatchFile");
                File.WriteAllText(path, DateTime.UtcNow.ToString("u"));
            }
        }
    }
}
