
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


#if ApiConventions
using Microsoft.AspNetCore.Mvc;
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
#endif


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(NavSpatialData.Mapping.MappingConfig));

builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<IEnrouteWayPointRepository, EnrouteWayPointRepository>();
builder.Services.AddScoped<IVhfNavaidRepository, VhfNavaidRepository>();
builder.Services.AddScoped<IAirwaysRepository, AirwaysRepository>();
builder.Services.AddScoped<IFirUirRepository, FirUirRepository>();


builder.Services.AddDbContext<MenuDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", // Just for testing; refine for production
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });



// Add services to the container.


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new GeoJsonGeometryConverter());
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");


app.MapControllers();

app.Run();
