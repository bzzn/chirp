using Microsoft.AspNetCore.Mvc;

using Application.Models;
using Domain.Entites;
using Domain.Services;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PackageModel>))]
    [Produces("application/json")]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PackageModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(InvalidPackageModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public IActionResult GetPackage([FromRoute]string id)
    {
        var correctLength = id.Length == 18;
        if (!correctLength)
            return BadRequest(new InvalidPackageModel { Message = $"Incorrect format, must be exactly 18 characters: {id}", KolliId = id });

        var allDigits = id.All(char.IsDigit);
        if (!allDigits)
            return BadRequest(new InvalidPackageModel { Message = $"Incorrect format, only digits allowed: {id}", KolliId = id });

        var correctPrefix = id.StartsWith("999");
        if (!correctPrefix)
            return BadRequest(new InvalidPackageModel { Message = $"Incorrect format, must start with 999: {id}", KolliId = id });

        var package = packageService.GetPackage(id);

        if (package.KolliId == string.Empty)
            return NotFound(new InvalidPackageModel { Message = $"No such package: {id}", KolliId = id });

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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PackageModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("application/json")]
    [Produces("application/json")]
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