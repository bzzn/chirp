using Domain;
using Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// In memory storage for packages hence instantiated as a Singleton
builder.Services.AddSingleton<IPackageStorage, VolatilePackageStorage>();
builder.Services.AddTransient<IPackageService, PackageService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();