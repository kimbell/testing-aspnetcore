using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Testing.AspNetCore.PetStore;
using Testing.AspNetCore.Services;

namespace Testing.AspNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(jsonOptions =>
                {
                    jsonOptions.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                ;
            
            // configure our PetStore client
            // we use a named typed client. One benefit of this is that we can change settings in our tests
            services
                .Configure<PetStoreOptions>(Configuration.GetSection("PetStore"))
                .AddHttpClient(PetStoreOptions.HttpClientName)
                .AddTypedClient((httpClient, serviceProvider) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<PetStoreOptions>>();

                    var c = new PetStoreClient(httpClient)
                    {
                        BaseUrl = options.Value.Url
                    };

                    return c;
                });

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Testing Demo API", Version = "v1" });
            });

            services.Configure<GenericOption>(Configuration.GetSection("Generic"));

            // need to use TryXXXX so that we can replace the implementation in tests
            services.TryAddSingleton<IOverridableService, OverridableService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger(options => { options.SerializeAsV2 = false; });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Demo API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
