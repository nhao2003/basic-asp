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
using Awesome.Repositories.Blog;
using Awesome.Repositories.Category;
using Awesome.Repositories.Session;
using Awesome.Repositories.User;
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
    options.Host_Address = builder.Configuration["ExternalProviders:MailKit:SMTP:Address"] ?? throw new InvalidOperationException();
    options.Host_Port = Convert.ToInt32(builder.Configuration["ExternalProviders:MailKit:SMTP:Port"]);
    options.Host_Username = builder.Configuration["ExternalProviders:MailKit:SMTP:Account"] ?? throw new InvalidOperationException();
    options.Host_Password = builder.Configuration["ExternalProviders:MailKit:SMTP:Password"] ?? throw new InvalidOperationException();
    options.Sender_EMail = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderEmail"] ?? throw new InvalidOperationException();
    options.Sender_Name = builder.Configuration["ExternalProviders:MailKit:SMTP:SenderName"] ?? throw new InvalidOperationException();
});
var account = new Account(
    builder.Configuration["ExternalProviders:Cloudinary:CloudName"],
    builder.Configuration["ExternalProviders:Cloudinary:ApiKey"],
    builder.Configuration["ExternalProviders:Cloudinary:ApiSecret"]
);

#region Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
#endregion


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