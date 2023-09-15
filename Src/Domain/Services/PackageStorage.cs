using Domain.Entites;

namespace Domain;

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
    public VolatilePackageStorage(IEnumerable<Package> collection) => data = collection.ToList();
    public void Add(Package package) => data.Add(package);
    public Package Get(string id) => data.Where(p => p.KolliId == id).SingleOrDefault() ?? new Package();
    public IEnumerable<Package> GetAll() => data;
}