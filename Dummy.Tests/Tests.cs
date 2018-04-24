using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Kernel;
using Dummy.Api;
using Dummy.Core;
using Xunit;
using Moq;
using AutoFixture.Xunit2;
using Dapper;
using Dummy.Infrastructure;

namespace Dummy.Tests
{
    public class UnitTest
    {
        [Theory, Gen]
        public async Task RepoCallIsCorrect(
            string input,
            IEnumerable<MainModel> expected,
            [Frozen] Mock<IDummyModelProcessor> proccessor,
            [Frozen] Mock<IModelRepo> repo,
            DummyController sut)
        {
            proccessor.Setup(x => x.Process(input)).Returns(expected);
            var response = await sut.Test(input).Unwrap();
            Assert.True(response.IsSuccessStatusCode);
            var actual = await response.Unwrap<IEnumerable<MainModel>>();
            repo.Verify(x => x.PostData(expected), Times.Once());
            repo.Verify(x => x.PostData(It.IsNotIn<IEnumerable<MainModel>>(expected)), Times.Never());
            Assert.Equal(expected, actual);
        }
    }

    public class PartialIntegrationTest : IntegrationTestBaseClass
    {
        [Theory, Gen(TestConnectionString)]
        public async Task DummyModelRepoIsCorrect(
            List<MainModel> expected,
            ModelRepo sut)
        {
            await sut.PostData(expected);
            using (var conn = new SqlConnection(TestConnectionString))
            {
                var actual = conn.Query<MainModel>("select ModelChars from DummyModel").ToList();

                Assert.True(
                    expected.Select(x => x.ModelChars).AsSet().SetEquals(actual.Select(x => x.ModelChars)));
            }
        }

        protected override void CleanupDb()
        {
            using (var conn = new SqlConnection(TestConnectionString))
            {
                conn.Execute("truncate table DummyModel;");
            }
        }
    }

    public class FullIntegrationTest : IntegrationTestBaseClass, IClassFixture<WebServerFixture>
    {
        [Theory, Gen]
        public async Task CanGetRepoData(
            string input,
            DummyModelProcessor sut)
        {
            var expected = sut.Process(input);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://mylocal:8082");
                var response = await client.PostAsJsonAsync("/ping", input);
                Assert.True(response.IsSuccessStatusCode);
                var actual = await response.Unwrap<IEnumerable<MainModel>>();

                Assert.True(
                    expected.Select(x => x.ModelChars).AsSet().SetEquals(actual.Select(x => x.ModelChars)));
            }
        }

        protected override void CleanupDb()
        {
            using (var conn = new SqlConnection(TestConnectionString))
            {
                conn.Execute("truncate table DummyModel;");
            }
        }
    }
}
