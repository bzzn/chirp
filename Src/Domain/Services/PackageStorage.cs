using Microsoft.Extensions.Options;

using Domain.Entites;

namespace Domain;

public class VolatilePackageStorageOptions
{
    public List<Package> PreLoaded { get; set; } = new List<Package>();
}

public interface IPackageStorage
{
    void Add(Package package);
    Package Get(string id);
    IEnumerable<Package> GetAll();
}

public class VolatilePackageStorage : IPackageStorage
{
    private readonly List<Package> data;
    public VolatilePackageStorage() => data = new List<Package>();
    public VolatilePackageStorage(IOptions<VolatilePackageStorageOptions> options) => data = options.Value.PreLoaded;
    public void Add(Package package) => data.Add(package);
    public Package Get(string id) => data.Where(p => p.KolliId == id).SingleOrDefault() ?? new Package();
    public IEnumerable<Package> GetAll() => data;
}