using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dummy.Tests
{
    public static class TestHelp
    {
        public static async Task<HttpResponseMessage> Unwrap(this Task<IHttpActionResult> @this)
        {
            var a = await @this;
            var b = await a.ExecuteAsync(CancellationToken.None);
            return b;
        }

        public static async Task<HttpResponseMessage> Unwrap(this IHttpActionResult @this)
        {
            var a = await @this.ExecuteAsync(CancellationToken.None);
            return a;
        }
        public static async Task<T> Unwrap<T>(this Task<HttpResponseMessage> @this)
        {
            var item = await @this;
            return await item.Unwrap<T>();
        }

        public static async Task<T> Unwrap<T>(this HttpResponseMessage @this)
        {
            return await @this.Content.ReadAsAsync<T>();
        }

        public static async Task<T> Unwrap<T>(this Task<IHttpActionResult> @this)
        {
            var result = await @this.Unwrap();
            var item = await result.Content.ReadAsAsync<T>();
            return item;
        }

        public static async Task<T> Unwrap<T>(this IHttpActionResult @this)
        {
            var result = await @this.Unwrap();
            var item = await result.Content.ReadAsAsync<T>();
            return item;
        }

        public static HashSet<T> AsSet<T>(this IEnumerable<T> input)
        {
            return new HashSet<T>(input);
        }
    }
}