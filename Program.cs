using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPIDemo2.Authentication;
using WebAPIDemo2.Interfaces;
using WebAPIDemo2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication("BasicAuthentication").
//            AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
//            ("BasicAuthentication", null);

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
           ValidAudience = builder.Configuration["Jwt:Issuer"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
       };
   });

builder.Services.AddControllers();
builder.Services.AddSingleton<IEmployee, EmployeeService>();
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

//http: --> https
app.UseHttpsRedirection();

//
//app.UseAuthorization(); // inbuilt middleware

app.UseAuthentication();
app.UseAuthorization();
//
app.MapControllers();
//app.UseMiddleware<BasicAuthetication>();
//app.UseBasicAuthetication();// custom middleware

app.Run();
