using Domain;
using Domain.Entites;
using Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Define a couple of packages to inject when starting the application
builder.Services.Configure<VolatilePackageStorageOptions>(options => {
    options.PreLoaded = new List<Package>
    {
        new() {
            KolliId = "999123456789000001",
            Weight = 20000,
            Length = 10,
            Height = 15,
            Width = 20
        },
        new() {
            KolliId = "999123456789000002",
            Weight = 50000,
            Length = 100,
            Height = 100,
            Width = 100
        },
    };
});

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