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
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(conf => conf.AddProfile<UserProfile>());

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ITemplatesRepository, TemplatesRepository>();

builder.Services.AddScoped<IUsersService, UsersService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
