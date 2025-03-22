using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;
using ServerLibrary.Repositories.Implmentations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi("v1");
//https://stackoverflow.com/questions/79188513/addopenapi-adding-error-response-types-to-all-operations-net-9
//builder.Services.AddOpenApiCustom();

builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
var jwtSection = builder.Configuration.GetSection(nameof(JwtSection)).Get<JwtSection>();

//Starting
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
        throw new InvalidOperationException("Connection string not found"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSection!.Issuer,
        ValidAudience = jwtSection.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key!))
    };
});

builder.Services.AddScoped<IUserAccount, UserAccountRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm",
        builder => builder
        .WithOrigins("http://localhost:5229", "https://localhost:7015")
        .AllowAnyMethod()
        .AllowAnyHeader()
        //.SetIsOriginAllowed(origin => true) // allow any origin 
        .AllowCredentials());
});

var app = builder.Build();

app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseExceptionHandler("/error-development");
}

if (app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.UseExceptionHandler("/error");
}

//Configure Endpoint for Prod or Dev
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("SFL Services LLC")
        .WithDownloadButton(true)
        .WithTheme(ScalarTheme.Purple)
        .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
});

app.UseHttpsRedirection();

app.UseCors("AllowBlazorWasm");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
