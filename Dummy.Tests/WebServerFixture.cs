using System;
using System.Configuration;
using System.Diagnostics;
using Dummy.Api;
using Microsoft.Owin.Hosting;

namespace Dummy.Tests
{
    public class WebServerFixture : IDisposable
    {
        private readonly IDisposable _server;
        public WebServerFixture()
        {
            try
            {
                _server = WebApp.Start<SelfHostStartup>("http://mylocal:8082");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                try
                {
                    Dispose();
                }
                catch { }
            }
        }

        public void Dispose()
        {
            _server?.Dispose();
        }
    }
}