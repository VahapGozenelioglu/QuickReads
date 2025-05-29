using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuickReads.Entities;
using QuickReads.Services.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Key"];

builder.Services.Configure<AuthenticationOptions>(options =>
{
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse(); // default 401'i bastır
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                var apiResponse = new ApiResponseModel<object>
                {
                    Error = true,
                    Message = "Unauthorized",
                    Result = null,
                    StatusCode = 401,
                    ErrorCode = 0
                };

                var json = JsonSerializer.Serialize(apiResponse);
                return context.Response.WriteAsync(json);
            }
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "JwtAuthApi", Version = "v1" });

    // JWT Bearer tanımı
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT token girin. Örnek: Bearer eyJhbGciOiJIUzI1..."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiResponseWrapperFilter>();
});
builder.Services.AddDbContext<QuickReads.Contexts.ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    response.ContentType = "application/json";

    var statusCode = response.StatusCode;

    var apiResponse = new ApiResponseModel<object>
    {
        Error = true,
        Message = statusCode == 401 ? "Unauthorized" :
            statusCode == 403 ? "Forbidden" :
            statusCode == 404 ? "Not Found" :
            "Error",
        Result = null,
        StatusCode = statusCode,
        ErrorCode = 0
    };

    var json = JsonSerializer.Serialize(apiResponse);
    await response.WriteAsync(json);
});
app.UseHttpsRedirection();
app.MapControllers();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var error = exceptionHandlerPathFeature?.Error;

        var response = new ApiResponseModel<object>
        {
            Error = true,
            Message = error?.Message ?? "An error occurred.",
            Result = null,
            StatusCode = 500,
            ErrorCode = 0
        };

        context.Response.StatusCode = response.StatusCode;
        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    });
});




app.Run();
