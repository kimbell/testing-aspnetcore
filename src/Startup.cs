using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Testing.AspNetCore.PetStore;

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
            services.AddControllers();
            
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
                c.DescribeStringEnumsInCamelCase();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Testing Demo API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

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
