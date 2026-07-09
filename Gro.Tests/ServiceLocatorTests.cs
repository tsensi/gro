using Gro.Services;
using Xunit;

namespace Gro.Tests;

public class ServiceLocatorTests : IDisposable
{
    public ServiceLocatorTests()
    {
        ServiceLocator.Clear();
    }

    public void Dispose()
    {
        ServiceLocator.Clear();
    }

    [Fact]
    public void Register_And_Get()
    {
        var service = new ResourceService();
        ServiceLocator.Register(service);
        Assert.Same(service, ServiceLocator.Get<ResourceService>());
    }

    [Fact]
    public void Get_ThrowsWhenNotRegistered()
    {
        Assert.Throws<InvalidOperationException>(() => ServiceLocator.Get<ResourceService>());
    }

    [Fact]
    public void TryGet_ReturnsNullWhenNotRegistered()
    {
        Assert.Null(ServiceLocator.TryGet<ResourceService>());
    }

    [Fact]
    public void TryGet_ReturnsServiceWhenRegistered()
    {
        var service = new ResourceService();
        ServiceLocator.Register(service);
        Assert.Same(service, ServiceLocator.TryGet<ResourceService>());
    }

    [Fact]
    public void Clear_RemovesAllServices()
    {
        ServiceLocator.Register(new ResourceService());
        ServiceLocator.Clear();
        Assert.Null(ServiceLocator.TryGet<ResourceService>());
    }

    [Fact]
    public void ResourceService_Add()
    {
        var service = new ResourceService();
        service.Add(50.0);
        Assert.Equal(50.0, service.Biomass);
        service.Add(25.0);
        Assert.Equal(75.0, service.Biomass);
    }

    [Fact]
    public void ResourceService_TrySpend_Success()
    {
        var service = new ResourceService();
        service.Biomass = 100.0;
        Assert.True(service.TrySpend(60.0));
        Assert.Equal(40.0, service.Biomass);
    }

    [Fact]
    public void ResourceService_TrySpend_InsufficientFunds()
    {
        var service = new ResourceService();
        service.Biomass = 10.0;
        Assert.False(service.TrySpend(20.0));
        Assert.Equal(10.0, service.Biomass);
    }

    [Fact]
    public void ResourceService_TrySpend_ExactAmount()
    {
        var service = new ResourceService();
        service.Biomass = 50.0;
        Assert.True(service.TrySpend(50.0));
        Assert.Equal(0.0, service.Biomass);
    }
}
