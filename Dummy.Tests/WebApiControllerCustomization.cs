﻿using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using AutoFixture;

namespace Dummy.Tests
{
    public class WebApiControllerCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<HttpConfiguration>(c => c
                .OmitAutoProperties());

            fixture.Customize<HttpRequestMessage>(c => c
                .Do(x =>
                    x.Properties.Add(
                        HttpPropertyKeys.HttpConfigurationKey,
                        fixture.Create<HttpConfiguration>())));

            fixture.Customize<HttpRequestContext>(c => c
                .Without(x => x.ClientCertificate));
        }
    }
}