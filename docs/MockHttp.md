#Mocking Http service calls

Chances are that you are calling some service using ```HttpClient```  
For this scenario I use the MockHttp library together with the ```HttpClientFactory``` system provided in ASP.NET Core. 
This allows for full mocking and a lot of options. 

In order to use this approach, we need to set things up correctly. 

In your service, set up a named client in the startup code. 
```CS
services.AddHttpClient(PetStoreOptions.HttpClientName)
        .AddTypedClient((httpClient, serviceProvider) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<PetStoreOptions>>();

            var c = new PetStoreClient(httpClient)
            {
                BaseUrl = options.Value.Url
            };

            return c;
        });
```
In the ```TestContext```, create a property for your mocking handler. 
```CS
public MockHttpMessageHandler PetStoreMockHandler { get; } = new MockHttpMessageHandler();
```
In the ```TestContext``` ConfigreServices, reconfigure the named client. 
```CS
.ConfigureServices((context, services) =>
{
    // configure our petstore with a unique host so that we can mock it using different urls than other services we might want to mock
    services.PostConfigure<PetStoreOptions>(o =>
    {
        o.Url = "http://petstore";
    });

    // since we registered with a named client, we can configure it so that we can mock all the requests and responses
    services.AddHttpClient(PetStoreOptions.HttpClientName).ConfigurePrimaryHttpMessageHandler(() => PetStoreMockHandler);

    AdditionalServices?.Invoke(context, services);
})
```
Now it becomes possible to mock ```HttpClient``` based calls for each test. 
The mocking library has multiple overloads to suit all kinds of need. 
```CS
[Fact]
public async Task HandlePost()
{
    using (var tc = new TestContext(_output))
    {
        // set up your mock
        tc.PetStoreMockHandler
            .Expect(HttpMethod.Post, "http://petstore/pet")
            .WithPartialContent("Garfield")
            .Respond(HttpStatusCode.NoContent);

        // make a request that depends on the mock
        await tc.ApiClient.AddPetAsync();

        tc.PetStoreMockHandler.VerifyNoOutstandingRequest();
    }
}
```

You can also return JSON back to your code. I tend to build an object structure based on the auto-generated code from the Open API contract. 
NSwag can generate a ToJson() metod for each object making it easy to convert it into a string that MockHttp can use. 

I prefer this approach over external JSON files for a number of reasons. 
1. I can generate an ultimate object structure that is the basis for all test. Then modify it for different test needs. 
2. When I update the contract and generate the code again, if things have been removed, I get a compile time error. Much easier to find than searching tons of JSON files. 