using System.Collections.Immutable;
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
    private List<Package> storage;

    public PackageService()
    {
        storage = new List<Package>();
    }

    public Package AddPackage(string id, int weight, int length, int height, int width)
    {
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
        var package = storage.Where(p => p.KolliId == id).SingleOrDefault();
        return package ?? new Package();
    }

    public IEnumerable<Package> GetAllPackages()
    {
        return storage.ToImmutableList();
    }
}