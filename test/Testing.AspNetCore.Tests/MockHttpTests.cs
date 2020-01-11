using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Testing.AspNetCore.Tests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Testing.AspNetCore.Tests
{
    public class MockHttpTests
    {
        private readonly ITestOutputHelper _output;

        public MockHttpTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task HandlePost()
        {
            using (var tc = new TestContext(_output))
            {
                tc.PetStoreMockHandler
                   .Expect(HttpMethod.Post, "http://petstore/pet")
                   .WithPartialContent("Garfield")
                   .Respond(HttpStatusCode.NoContent);

                await tc.ApiClient.AddPetAsync();

                tc.PetStoreMockHandler.VerifyNoOutstandingRequest();
            }
        }

        [Fact]
        public async Task HandleGet()
        {
            using (var tc = new TestContext(_output))
            {
                // build up the response data using the auto generated code from the contract
                // if we generate it again and things go missing, we get a compile time error
                // makes it easier to find than search through external json files
                var responseData = new List<Pet>
                {
                    new Pet
                    {
                        Id = 100,
                        Name = "Garfield",
                        Status = PetStatus.Available,
                        Category = new Category
                        {
                            Id = 2,
                            Name = "Cat"
                        }
                    }
                };

                var json = "[" + string.Join(',', responseData.Select(x => x.ToJson())) + "]";

                tc.PetStoreMockHandler
                   .Expect(HttpMethod.Get, "http://petstore/pet/findByStatus?status=available")
                   .Respond(HttpStatusCode.OK, "application/json", json);

                var response = await tc.ApiClient.GetPetsAsync();

                Assert.Single(response);
                Assert.Equal("Garfield", response[0].Name);
                
                tc.PetStoreMockHandler.VerifyNoOutstandingRequest();
            }
        }
    }
}
