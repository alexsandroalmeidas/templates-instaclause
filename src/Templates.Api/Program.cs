using Microsoft.EntityFrameworkCore;
using Templates.Api.Application.Mappings;
using Templates.Api.Application.Services;
using Templates.Api.Data;
using Templates.Api.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));

    options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);

    options.EnableSensitiveDataLogging();
});

builder.Services.AddAutoMapper(config =>
{
    config.AddMaps(typeof(UserProfile).Assembly);
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ITemplatesRepository, TemplatesRepository>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ITemplatesService, TemplatesService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
