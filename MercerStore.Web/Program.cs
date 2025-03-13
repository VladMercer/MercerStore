using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Models.Users;
using MercerStore.Web.Application.Services;
using MercerStore.Web.Infrastructure.Data;
using MercerStore.Web.Infrastructure.Extentions;
using MercerStore.Web.Infrastructure.Helpers;
using MercerStore.Web.Infrastructure.Repositories;
using MercerStore.Web.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using StackExchange.Redis;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<HttpContextAccessor>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISKUUpdater, SKUUpdater>();
builder.Services.AddScoped<ISKUService, SKUService>();
builder.Services.AddScoped<ICartProductRepository, CartProductRepository>();
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();
builder.Services.AddScoped<IReviewProductRepository, ReviewProductRepostitory>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IUserIdentifierService, UserIdentifierService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IRequestContextService, RequestContextService>();
builder.Services.AddScoped<IUserActivityRepository, UserActivityRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserActivityService, UserActivityService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IMetricService, MetricService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ISaleService, SaleService>();


builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddElasticSearch(builder.Configuration);
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        var apiControllerAction = apiDesc.ActionDescriptor.EndpointMetadata
            .Any(em => em is ApiControllerAttribute);
        return apiControllerAction;
    });
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver =
            new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.Formatting = Formatting.None;
    });

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect("localhost:6379")
);
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(("VHG5TQGxzE2tEMzplusK1pqTH4UwTwdC")))
    };

    options.Events = new JwtBearerEvents
    {

        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["OohhCookies"] ??
            context.Request.Cookies["OhCookies"];
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("BlacklistRolesPolicy", policy => policy.RequireAssertion(context =>
    {
        var blacklistedRoles = new List<string> { "Guest", "Banned" };
        return !blacklistedRoles.Any(role => context.User.IsInRole(role));
    }));

builder.Logging.AddConsole();
builder.Services.AddLogging();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentityCore<AppUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddRoles<IdentityRole>()
    .AddSignInManager()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog();
});
ConfigureLogging();
builder.Host.UseSerilog();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        .Build();

    var consoleLogger = new LoggerConfiguration()
       .Enrich.FromLogContext()
       .WriteTo.Console()
       .CreateLogger();

    var elasticLogger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("CustomLog", false)
        .Filter.ByIncludingOnly(logEvent =>
        logEvent.Level >= LogEventLevel.Warning ||
        (logEvent.Properties.TryGetValue("CustomLog", out var customLogValue) &&
            customLogValue is ScalarValue { Value: bool boolValue } &&
            boolValue)
        )
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
        .CreateLogger();

    Log.Logger = new LoggerConfiguration()
        .WriteTo.Logger(consoleLogger)
        .WriteTo.Logger(elasticLogger)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly()
        .GetName().Name
        .ToLower()
        .Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}