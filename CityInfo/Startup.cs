using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CityInfo
{
    public class Startup

    {   // Configure configuration file "appSettings.json"
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Dependancy injection
            services.AddMvc();

#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif

            var connectionString = Startup.Configuration["connectionStrings:ConnectionString"];
            services.AddDbContext<CityInfoContext>(c => c.UseSqlServer(connectionString));

            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
            // Add support for XML output
            //.AddMvcOptions(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoContext cityInfoContext)
        {
            //loggerFactory.AddConsole();
            //loggerFactory.AddDebug();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Models.City, Dtos.CityWithoutPointsDto>();
                cfg.CreateMap<Models.City, Dtos.CityDto>();
                cfg.CreateMap<Models.PointOfInterest, Dtos.PointsOfInterestDto>();
                cfg.CreateMap<Dtos.PointsOfInterestCreateDto, Models.PointOfInterest>();
                cfg.CreateMap<Dtos.PointsOfInterestUpdateDto, Models.PointOfInterest>();

            });

            loggerFactory.AddNLog();
            cityInfoContext.EnsureSeedData();
            // Displays status code in browser
            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
