using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Dummy.Tests
{
    public class Gen : AutoDataAttribute
    {
        public Gen() : this(testConnectionString: null)
        {
        }

        public Gen(string testConnectionString = null) : base(() => new Fixture().Customize(new CompositeCustomization(
            new PreConfiguredAppConfigCustomization(testConnectionString),
            new WebApiControllerCustomization(),
            new AutoMoqCustomization())))
        {

        }
    }
}