using CPICacheService.Utilities;

namespace CPICacheService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Set port to 5000
            builder.WebHost.UseUrls("http://localhost:5000");

            // Add services to the container.
            builder.Services.AddControllers();

            // Add caching services
            builder.Services.AddMemoryCache();

            // Register the IRepository service and its dependencies
            builder.Services.AddScoped<IRepository, Repository>();

            builder.Services.AddScoped<IApiClient, MockApiClient>();
            //builder.Services.AddScoped<IApiClient, ApiClient>();

            builder.Services.AddScoped<ICacheClient, CacheClient>();
            builder.Services.AddScoped<IPropertyValidator, PropertyValidator>();
            builder.Services.AddScoped<IApiResponseConverter, ApiResponseConverter>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}