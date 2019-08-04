using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using Testing.AspNetCore.Tests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Testing.AspNetCore.Tests
{
    public class CodeGenerators
    {
        private readonly ITestOutputHelper _output;

        public CodeGenerators(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// The purpose of this test is to download and create a strongly typed client for your API
        /// This approach was developed for ASP.NET Core 2.2
        /// New functionality in 3.0 should be able to replace this 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GenerateTestClient()
        {
            using var tc = new TestContext(_output);

            // download the swagger contract
            var json = await tc.HttpClient.GetStringAsync("http://localhost/swagger/v1/swagger.json");
            var document = await OpenApiDocument.FromJsonAsync(json);

            var settings = new CSharpClientGeneratorSettings
            {
                UseBaseUrl = true,
                GenerateBaseUrlProperty = true,
                GenerateOptionalParameters = true,
                InjectHttpClient = true,
                ClassName = "ApiClient",
                WrapDtoExceptions = true,
                ParameterArrayType = "System.Collections.Generic.IEnumerable",
                ParameterDictionaryType = "System.Collections.Generic.IDictionary",
                ResponseArrayType = "System.Collections.Generic.List",
                ResponseDictionaryType = "System.Collections.Generic.Dictionary",
                CSharpGeneratorSettings =
                {
                    Namespace = "Testing.AspNetCore.Tests.External",

                    GenerateDefaultValues = true,
                    GenerateDataAnnotations = true,
                    GenerateJsonMethods = true,
                    ClassStyle = NJsonSchema.CodeGeneration.CSharp.CSharpClassStyle.Poco,
                    DateType = "System.DateTime",
                    DateTimeType = "System.DateTimeOffset",
                    TimeType = "System.TimeSpan",
                    TimeSpanType = "System.TimeSpan",
                    ArrayType = "System.Collections.Generic.List",
                    ArrayBaseType = "System.Collections.Generic.List",
                    ArrayInstanceType = "System.Collections.Generic.List",
                    DictionaryBaseType = "System.Collections.Generic.Dictionary"
                }
            };

            // Generate a client based on our Swagger contract
            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();

            SaveCode(code);
        }

        private static void SaveCode(string code, [CallerFilePath] string path = "")
        {
            var directory = new DirectoryInfo(path);
            var project = directory.Parent;

            File.WriteAllText(Path.Combine(project?.FullName, "External", "ApiClient.cs"), code);
        }
    }
}
