using API_Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using StudentAPI.Extension;
using StudentAPI.Model;
using StudentAPI.Util;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using StudentAPI.Interface;
using StudentAPI.Adapter;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                    new HeaderApiVersionReader("x-api-version"),
                                                    new MediaTypeApiVersionReader("x-api-version"));
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    // set version type as v1, v2 instead of 1.0, 2.0
    setup.GroupNameFormat = "'v'VVV";
    // remove the version parameter from api description
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// adding a health check for the service with name ping. Later a endpoint will be 
// mapped to ping which will be used to check if service is healthy
builder.Services.AddHealthChecks()
   .AddCheck("ping",
                () => HealthCheckResult.Healthy(),
                tags: new[] { "ready" })
   .AddCheck<CosmosDbHealthCheck>("cosmosDb", failureStatus: HealthStatus.Unhealthy, tags: new[] {"db-check"});;

var app = builder.Build();

// Configure the HTTP request pipeline.

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

// health check call
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    // map health check endpoint (in our case ping)
    endpoints.MapHealthCheckEndpoints();
});

app.Run();

// method to configure all services (in our case database, mapper)
void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton(builder.Configuration);
    services.AddAutoMapper(typeof(Program));

    ConfigureAutoMapper(services);

    DbConfiguration dbConfiguration = new();
    builder.Configuration.GetSection("CosmosDbSettings").Bind(dbConfiguration);

    services.RegisterDbDependencies(dbConfiguration).GetAwaiter().GetResult();
}

// logic to configure mapper
void ConfigureAutoMapper(IServiceCollection services)
{
    var config = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<UpdateStudentDto, Student>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        cfg.CreateMap<Student, StudentInfo>()
            .ForMember(dest => dest.MotherName, opt => opt.MapFrom(src => src.Mother))
            .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.Father))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

    });

    IMapper mapper = config.CreateMapper();
    services.AddSingleton(mapper);
}