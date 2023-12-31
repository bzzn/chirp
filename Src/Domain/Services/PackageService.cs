using System.ComponentModel.DataAnnotations;
using Domain.Entites;

namespace Domain.Services;

public interface IPackageService
{
    Package AddPackage(string id, int weight, int length, int height, int width);
    Package GetPackage(string id);
    IEnumerable<Package> GetAllPackages();
}

public class PackageService : IPackageService
{
    private readonly IPackageStorage storage;

    public PackageService()
    {
        this.storage = new VolatilePackageStorage();
    }

    public PackageService(IPackageStorage storage)
    {
        this.storage = storage;
    }

    public Package AddPackage(string id, int weight, int length, int height, int width)
    {
        var existingPackage = storage.Get(id);
        if (existingPackage.KolliId != string.Empty)
            return new Package();

        var package = new Package
        {
            KolliId = id,
            Weight = weight,
            Length = length,
            Height = height,
            Width = width
        };
        storage.Add(package);
        return package;
    }

    public Package GetPackage(string id)
    {
        var package = storage.Get(id);
        return Validate(package);
    }

    public IEnumerable<Package> GetAllPackages()
    {
        return storage.GetAll().Select(Validate).ToList();
    }

    private Package Validate(Package package)
    {
        var validationContext = new ValidationContext(package);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(package, validationContext, results, true))
        {
            package.IsValid = false;
            foreach (var error in results)
                package.Errors.Append(error.ErrorMessage);
        }
        else
        {
            package.IsValid = true;
        }

        return package;
    }
}