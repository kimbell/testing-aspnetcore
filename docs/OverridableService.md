# Overriding a service

If you have external dependencies, you want to replace parts of your service with something you have complete control over. 
In order to achieve this, you need to do things in the right way.  

It helps a lot of if your service implenents an interface.  

When adding things to DI, use the TryXYZ() methods

```CS
services.TryAddSingleton<IOverridableService, OverridableService>();
```

In your test, you can use the extensibility points. 

```CS
[Fact]
public async Task UseOverridenImplemenation()
{
    using (var tc = new TestContext(_output))
    {
        tc.AdditionalServices = (_, services) =>
        {
            var mockService = new Mock<IOverridableService>();
            mockService.Setup(x => x.ProduceMessage()).ReturnsAsync("I'm a pony with a party hat");

            // since the original service was registered with TryAddSingleton(), we can replace it. 
            services.AddSingleton(mockService.Object);
        };

        var response = await tc.ApiClient.ProduceMessageAsync().ConfigureAwait(false);

        Assert.Equal("I'm a pony with a party hat", response.Message);
    }
}
```