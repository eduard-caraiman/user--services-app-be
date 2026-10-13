using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using users_service.Database;
using users_service.Filters;
using users_service.Users.Repositories;
using users_service.Users.Requests;
using users_service.Users.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Users API", Version = "v1" });
});
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter<CreateUserRequest>>();
    options.Filters.Add<FluentValidationFilter<UpdateUserRequest>>();
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();


var connectionString = builder.Configuration.GetConnectionString("UsersDatabase")
                       ?? throw new InvalidOperationException("Connection string 'UsersDatabase' was not found.");
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlite(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();