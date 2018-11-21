using log4net;
using System;
using System.Reflection;

namespace ExampleService
{
    public class MyServer : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Start()
        {
            Log.Info($"Starting server {Assembly.GetExecutingAssembly().GetName().Version}");
        }

        public void Dispose()
        {
            Log.Info("Stopping");
        }
    }
}
