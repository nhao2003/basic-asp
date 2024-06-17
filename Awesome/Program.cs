using Awesome.Authentication;
using Awesome.Data;
using Awesome.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Awesome.Utils;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpLogging(logging =>
{
    // Log
    logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
                            HttpLoggingFields.ResponsePropertiesAndHeaders;
    logging.RequestHeaders.Add("Authorization");
    logging.ResponseHeaders.Add("Authorization");
    logging.MediaTypeOptions.AddText("application/json");
    logging.MediaTypeOptions.AddText("application/xml");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});
using var loggerFactory = LoggerFactory.Create(b => b.SetMinimumLevel(LogLevel.Trace).AddConsole());

var secret = builder.Configuration["JWT:AccessSecretKey"] ??
             throw new InvalidOperationException("Secret not configured");
builder.Services.AddTransient<CryptoUtils>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<JwtBearerHandler, JwtAuthenticationHandler>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<JwtBearerOptions, JwtAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme,
        options => { options.Events = new JwtAuthenticationBearEvent(); });
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});


// add-migration InitialCreate
// update-database

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Use custom Filter
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpLogging();

app.UseStaticFiles();

app.MapGet("/", () => "Hello World!");

app.Run();

Task LogAttempt(IHeaderDictionary headers, string eventType)
{
    var logger = loggerFactory.CreateLogger<Program>();

    var authorizationHeader = headers["Authorization"].FirstOrDefault();

    if (authorizationHeader is null)
        logger.LogInformation($"{eventType}. JWT not present");
    else
    {
        var jwtString = authorizationHeader["Bearer ".Length..];

        var jwt = new JwtSecurityToken(jwtString);

        // Print reason why the token is invalid
        if (jwt.ValidTo < DateTime.UtcNow)
            logger.LogInformation(
                $"{eventType}. Token expired at {jwt.ValidTo.ToLongTimeString()}. System time: {DateTime.UtcNow.ToLongTimeString()}");
        else if (jwt.ValidFrom > DateTime.UtcNow)
            logger.LogInformation(
                $"{eventType}. Token not valid until {jwt.ValidFrom.ToLongTimeString()}. System time: {DateTime.UtcNow.ToLongTimeString()}");
        else
            logger.LogInformation($"{eventType}. Token is valid");
    }

    return Task.CompletedTask;
}