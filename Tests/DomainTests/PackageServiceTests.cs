using Domain.Services;

namespace DomainTests;

public class PackageServiceTests
{
    [Fact]
    public void GetPackage()
    {
        var packageId = "999111111111111111";
        var packageService = new PackageService();
        var expected = packageService.AddPackage(packageId, 20, 10, 10, 10);

        var actual = packageService.GetPackage(packageId);
        
        Assert.Equal(expected.KolliId, actual.KolliId);
    }

    [Fact]
    public void GetNonExistingPackage()
    {
        var packageService = new PackageService();

        var actual = packageService.GetPackage("999111111111111112");
        
        Assert.Equal(string.Empty, actual.KolliId);
    }

    [Fact]
    public void GetAllPackages()
    {
        var packageIdPrefix = "99911111111111111";
        var packageService = new PackageService();

        packageService.AddPackage(packageIdPrefix + "1", 20 * 1000, 10, 10, 10);
        packageService.AddPackage(packageIdPrefix + "2", 30 * 1000, 10, 10, 10);

        var all = packageService.GetAllPackages();
        
        Assert.Equal(2, all.Count());
        Assert.True(all.First().IsValid, "Expected package to be valid");
        Assert.False(all.Last().IsValid, "Expected package to not be valid");
    }

    [Fact]
    public void PackageTooHeavy()
    {
        var packageId = "999111111111111111";
        var packageService = new PackageService();
        packageService.AddPackage(packageId, 30 * 1000, 10, 10, 10);

        var package = packageService.GetPackage(packageId);
        
        Assert.False(package.IsValid);
    }

    [Fact]
    public void PackageTooLong()
    {
        var packageId = "999111111111111111";
        var packageService = new PackageService();
        packageService.AddPackage(packageId, 20 * 1000, 100, 10, 10);

        var package = packageService.GetPackage(packageId);
        
        Assert.False(package.IsValid);
    }

    [Fact]
    public void PackageTooHigh()
    {
        var packageId = "999111111111111111";
        var packageService = new PackageService();
        packageService.AddPackage(packageId, 20 * 1000, 10, 100, 10);

        var package = packageService.GetPackage(packageId);
        
        Assert.False(package.IsValid);
    }

    [Fact]
    public void PackageTooWide()
    {
        var packageId = "999111111111111111";
        var packageService = new PackageService();
        packageService.AddPackage(packageId, 20 * 1000, 10, 10, 100);

        var package = packageService.GetPackage(packageId);
        
        Assert.False(package.IsValid);
    }

    [Theory(Skip = "WIP")]
    [InlineData("999123", "length (must be 18)")]
    [InlineData("99912345678901234567890", "length (must be 18)")]
    [InlineData("999123456789ABCDEF", "format (only numbers allowed)")]
    [InlineData("666123456789012345", "prefix (must be 999)")]
    public void PackageIdValidation(string packageId, string failureReason)
    {
        var packageService = new PackageService();
        packageService.AddPackage(packageId, 20000, 10, 10, 10);

        var package = packageService.GetPackage(packageId);
        
        Assert.False(package.IsValid, $"Expected failure due to invalid {failureReason}");
    }
}