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

    private bool ValidateKolliId(string id, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (!(id.Length == 18))
            errorMessage = $"Incorrect format, must be exactly 18 characters: {id}";

        if (!id.All(char.IsDigit))
            errorMessage = $"Incorrect format, only digits allowed: {id}";

        if (!id.StartsWith("999"))
            errorMessage = $"Incorrect format, must start with 999: {id}";

        return errorMessage == string.Empty;
    }

    [HttpGet("{id?}", Name = "GetPackage")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PackageModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PackageModelInfo))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public IActionResult GetPackage([FromRoute]string id)
    {
        var packageValid = ValidateKolliId(id, out string? validationMessage);
        if (!packageValid)
            return BadRequest(new PackageModelInfo { Message = validationMessage, KolliId = id });

        var package = packageService.GetPackage(id);

        if (package.KolliId == string.Empty)
            return NotFound(new PackageModelInfo { Message = $"No such package: {id}", KolliId = id });

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
        var packageValid = ValidateKolliId(packageModel.KolliId, out string? validationMessage);
        if (!packageValid)
            return BadRequest(new PackageModelInfo { Message = validationMessage, KolliId = packageModel.KolliId });

        var existingPackage = packageService.GetPackage(packageModel.KolliId);
        if (existingPackage.KolliId != string.Empty)
        {
            return BadRequest(new PackageModelInfo
            {
                Message = $"Package already exists, kolli id: {packageModel.KolliId}",
                KolliId = packageModel.KolliId
            });
        }

        var result = packageService.AddPackage(
            packageModel.KolliId,
            packageModel.Weight,
            packageModel.Length,
            packageModel.Height,
            packageModel.Width
        );
        logger.LogInformation($"Added package: {result.KolliId}");

        return Created($"/package/{result.KolliId}",
            new PackageModelInfo
            {
                KolliId = result.KolliId,
                Message = $"Package created with kolli id: {result.KolliId}"
            }
        );
    }
}