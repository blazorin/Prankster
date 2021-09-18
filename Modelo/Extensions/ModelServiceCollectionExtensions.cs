using Microsoft.Extensions.DependencyInjection;
using Model.Data;
using Model.Mapping;
using Model.Services;
using Model.Services.Http;

namespace Model.Extensions
{
    public static class ModelServiceCollectionExtensions
    {
        public static IServiceCollection AddMauiPModelServices(this IServiceCollection services)
        {
            // Essential
            services.AddDbContext<MauiPContext>();
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            //User
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IProfileServices, ProfileServices>();

            // Bitsbi httpClient
            services.AddHttpClient<Bitsbi>();

            return services;
        }
    }
}