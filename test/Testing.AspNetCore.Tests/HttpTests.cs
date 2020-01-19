using System.Net;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Testing.AspNetCore.Tests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Testing.AspNetCore.Tests
{
    public class HttpTests
    {
        private readonly ITestOutputHelper _output;

        public HttpTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task CheckHttpHeaders()
        {
            using (var tc = new TestContext(_output))
            {
                tc.HttpClient.DefaultRequestHeaders.Add("header1", "value1");
                tc.HttpClient.DefaultRequestHeaders.Add("header2", "value2");

                // this method just returns the headers being sent in
                var headers = await tc.ApiClient.GetHeadersAsync().ConfigureAwait(false);

                Assert.Equal("value1", headers["header1"]);
                Assert.Equal("value2", headers["header2"]);
            }
        }

        [Fact]
        public async Task CheckCookies()
        {
            using (var tc = new TestContext(_output))
            {
                var cookies = new CookieContainer();
                cookies.Add(tc.HttpClient.BaseAddress, new Cookie("c1", "v1"));
                cookies.Add(tc.HttpClient.BaseAddress, new Cookie("c2", "v2"));
                tc.HttpClient.DefaultRequestHeaders.Add(HeaderNames.Cookie, cookies.GetCookieHeader(tc.HttpClient.BaseAddress));

                // this method just returns the cookies being sent in
                var response = await tc.ApiClient.GetCookiesAsync().ConfigureAwait(false);

                Assert.Equal("v1", response["c1"]);
                Assert.Equal("v2", response["c2"]);
            }
        }
    }
}
