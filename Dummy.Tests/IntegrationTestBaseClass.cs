using System;

namespace Dummy.Tests
{
    public abstract class IntegrationTestBaseClass : IDisposable
    {
        public const string TestConnectionString =
           "Data Source=CHURCHILL\\SQL2017FULL;Database=DummyModel;Integrated Security=True";

        protected IntegrationTestBaseClass()
        {
            CleanupDb();
        }

        public void Dispose()
        {
            CleanupDb();
        }

        protected abstract void CleanupDb();
    }
}