using Application.Models;
using Domain.Entites;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("[controller]")]
public class PackageController : ControllerBase
{
    private readonly ILogger<PackageController> logger;
    private readonly IPackageService packageService;

    public PackageController(ILogger<PackageController> logger, IPackageService packageService)
    {
        this.logger = logger;
        this.packageService = packageService;
    }

    [HttpGet("", Name = "GetPackages")]
    public IActionResult GetPackages()
    {
        var packages = packageService.GetAllPackages();
        logger.LogInformation($"Fetched {packages.Count()} packages");

        var packageModels = packages.Select(package => new PackageModel
        {
            KolliId = package.KolliId,
            Weight = package.Weight,
            Length = package.Length,
            Height = package.Height,
            Width = package.Width,
            IsValid = package.IsValid
        }).ToArray();

        return Ok(packageModels);
    }

    [HttpGet("{id?}", Name = "GetPackage")]
    public IActionResult GetPackage(string id)
    {
        var package = packageService.GetPackage(id);

        if (package.KolliId == string.Empty)
            return NotFound(string.Empty);

        logger.LogInformation($"Fetched package: {package.KolliId} (Valid: {package.IsValid})");
        if (!package.IsValid)
            foreach (var message in package.Errors)
                logger.LogInformation(message);

        var packageModel = new PackageModel
        {
            KolliId = package.KolliId,
            Weight = package.Weight,
            Length = package.Length,
            Height = package.Height,
            Width = package.Width,
            IsValid = package.IsValid
        };

        return Ok(packageModel);
    }

    [HttpPost(Name = "PostPackage")]
    public IActionResult PostPackage(PackageModel packageModel)
    {
        var package = new Package
        {
            KolliId = packageModel.KolliId,
            Weight = packageModel.Weight,
            Length = packageModel.Length,
            Height = packageModel.Height,
            Width = packageModel.Width
        };
        var result = packageService.AddPackage(package);
        logger.LogInformation($"Added package: {result.KolliId}");

        return Created($"/package/{result.KolliId}", result);
    }
}
