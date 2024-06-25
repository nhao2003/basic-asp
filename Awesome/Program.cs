using Awesome.Authentication;
using Awesome.Data;
using Awesome.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Awesome.Middlewares;
using Awesome.Profiles;
using Awesome.Services.BlogService;
using Awesome.Services.Category;
using Awesome.Services.FileUploadService;
using Awesome.Services.MailService;
using Awesome.Services.SmsService;
using Awesome.Services.UserService;
using Awesome.Utils;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(
        Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "DefaultConnection"));
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
builder.Services.Configure<MailKitEmailSenderOptions>(options =>
{
    options.Host_Address = builder.Configuration["ExternalProviders:MailKit:SMTP:Address"];
    options.Host_Port = Convert.ToInt32(builder.Configuration["ExternalProviders:MailKit:SMTP:Port"]);
    options.Host_Username = builder.Configuration["ExternalProviders:MailKit:SMTP:Account"];
    options.Host_Password = builder.Configuration["ExternalProviders:MailKit:SMTP:Password"];
    options.Sender_EMail = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"];
    options.Sender_Name = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderName"];
});
var account = new Account(
    builder.Configuration["ExternalProviders:Cloudinary:CloudName"],
    builder.Configuration["ExternalProviders:Cloudinary:ApiKey"],
    builder.Configuration["ExternalProviders:Cloudinary:ApiSecret"]
);
var cloudinary = new Cloudinary(account);

builder.Services.AddSingleton(cloudinary);

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();
builder.Services.AddTransient<ISmsService, VonageSmsMessage>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IBlogService, BlogService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<JwtBearerHandler, JwtAuthenticationHandler>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<JwtBearerOptions, JwtAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme,
        options => { options.Events = new JwtAuthenticationBearEvent(); });
builder.Services.AddTransient<IFileUploadService, FileUploadService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

// add-migration InitialCreate
// update-database

var app = builder.Build();

// Add this block of code to update the database at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

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
app.UseMiddleware<ExceptionHandlingMiddleware>();
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