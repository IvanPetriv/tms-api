using APIMain.Configuration.Objects;
using APIMain.Mapping;
using APIMain.Swagger;
using BackendDB.Models;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

#region Setup
var builder = WebApplication.CreateBuilder(args);
DotEnv.Load();

// Adds JWT authentication and authorization
JwtSettings jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
            ?? throw new NullReferenceException("appsettings.json does not have 'JwtSettings' property.");
string jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
            ?? throw new NullReferenceException("Environment does not have JWT key");
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

// Setups DB and models for API
string dbConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
            ?? throw new NullReferenceException("Environment does not have JWT key");
builder.Services.AddControllers();
builder.Services.AddDbContext<TmsMainContext>(options =>
    options.UseSqlServer(dbConnectionString));
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Setups Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1",
        Title = "TMS API",
        Description = "TMS REST API",
    });

    //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureOptions>();

#warning Is this the best fix?
builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});
#endregion

#region Use
var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion
