# HTTP data
In most cases you want to include extra data when sending HTTP requests. E.g. headers or cookies. 

#Headers 
Headers can easily be added to the data being sent to the server

```CS
[Fact]
public async Task HandlePost()
{
    using (var tc = new TestContext(_output))
    {
        tc.HttpClient.DefaultRequestHeaders.Add("header1", "value1");
        tc.HttpClient.DefaultRequestHeaders.Add("header2", "value2");

        await tc.ApiClient.DoSomething().ConfigureAwait(false);
    }
}
```

#Cookies
Cookies are a variant on the headers.

```CS
[Fact]
public async Task HandlePost()
{
    using (var tc = new TestContext(_output))
    {
        var cookies = new CookieContainer();
        cookies.Add(tc.HttpClient.BaseAddress, new Cookie("c1", "v1"));
        cookies.Add(tc.HttpClient.BaseAddress, new Cookie("c2", "v2"));
        tc.HttpClient.DefaultRequestHeaders.Add(HeaderNames.Cookie, cookies.GetCookieHeader(tc.HttpClient.BaseAddress));

        var response = await tc.ApiClient.DoSomething().ConfigureAwait(false);
    }
}
```