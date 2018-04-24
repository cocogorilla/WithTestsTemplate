using AutoFixture;
using Dummy.Core;

namespace Dummy.Tests
{
    public class PreConfiguredAppConfigCustomization : ICustomization
    {
        private readonly string _testConnectionString;

        public PreConfiguredAppConfigCustomization(string testConnectionString = null)
        {
            _testConnectionString = testConnectionString;
        }
        public void Customize(IFixture fixture)
        {
            fixture.Customize<AppConfig>(c =>
                c.With(x => x.ConnectionString, _testConnectionString ?? ""));
        }
    }
}