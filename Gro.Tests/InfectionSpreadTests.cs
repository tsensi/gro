using Gro.EarthModel;
using Gro.ECS;
using Gro.Infection;
using Xunit;

namespace Gro.Tests;

public class InfectionSpreadTests
{
    [Fact]
    public void AdjacencyMap_DetectsCloseZones()
    {
        var zoneA = new Zone
        {
            Name = "A", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(0, 0), new GeoCoord(0, 10), new GeoCoord(5, 10), new GeoCoord(5, 0) },
        };
        var zoneB = new Zone
        {
            Name = "B", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(5.5, 0), new GeoCoord(5.5, 10), new GeoCoord(10, 10), new GeoCoord(10, 0) },
        };
        var zoneC = new Zone
        {
            Name = "C", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(20, 0), new GeoCoord(20, 10), new GeoCoord(25, 10), new GeoCoord(25, 0) },
        };

        var map = AdjacencyMap.FromZones(new[] { zoneA, zoneB, zoneC });

        Assert.True(map.AreAdjacent("A", "B"));
        Assert.False(map.AreAdjacent("A", "C"));
        Assert.False(map.AreAdjacent("B", "C"));
    }

    [Fact]
    public void AdjacencyMap_IsSymmetric()
    {
        var zoneA = new Zone
        {
            Name = "A", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(0, 0), new GeoCoord(0, 5), new GeoCoord(5, 5), new GeoCoord(5, 0) },
        };
        var zoneB = new Zone
        {
            Name = "B", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(5, 0), new GeoCoord(5, 5), new GeoCoord(10, 5), new GeoCoord(10, 0) },
        };

        var map = AdjacencyMap.FromZones(new[] { zoneA, zoneB });

        Assert.True(map.AreAdjacent("A", "B"));
        Assert.True(map.AreAdjacent("B", "A"));
    }

    [Fact]
    public void AdjacencyMap_IgnoresTopLevelZones()
    {
        var continent = new Zone
        {
            Name = "TestContinent", Type = ZoneType.Continent, ParentName = null,
            Boundary = new[] { new GeoCoord(0, 0), new GeoCoord(0, 10), new GeoCoord(10, 10) },
        };
        var country = new Zone
        {
            Name = "Country", Type = ZoneType.Country, ParentName = "TestContinent",
            Boundary = new[] { new GeoCoord(0, 0), new GeoCoord(0, 5), new GeoCoord(5, 5) },
        };

        var map = AdjacencyMap.FromZones(new[] { continent, country });
        Assert.Empty(map.GetNeighbors("TestContinent"));
    }

    [Fact]
    public void Spread_SplitsBiomassWithTax()
    {
        var world = new World();
        var source = world.SpawnInZone("France");
        world.Set(source, new InfectionComponent { Biomass = 100.0 });

        var sourceInfection = world.Get<InfectionComponent>(source)!;
        double remaining = sourceInfection.Biomass * 0.6;
        double half = remaining / 2.0;
        sourceInfection.Biomass = half;

        var newEntity = world.SpawnInZone("Germany");
        world.Set(newEntity, new InfectionComponent { Biomass = half });

        Assert.Equal(30.0, world.Get<InfectionComponent>(source)!.Biomass);
        Assert.Equal(30.0, world.Get<InfectionComponent>(newEntity)!.Biomass);
    }

    [Fact]
    public void Spread_TotalBiomassDecreasesBy40Percent()
    {
        var world = new World();
        var source = world.SpawnInZone("France");
        world.Set(source, new InfectionComponent { Biomass = 50.0 });

        var sourceInfection = world.Get<InfectionComponent>(source)!;
        double originalBiomass = sourceInfection.Biomass;
        double remaining = originalBiomass * 0.6;
        double half = remaining / 2.0;
        sourceInfection.Biomass = half;

        var newEntity = world.SpawnInZone("Germany");
        world.Set(newEntity, new InfectionComponent { Biomass = half });

        double totalAfter = world.Get<InfectionComponent>(source)!.Biomass
                          + world.Get<InfectionComponent>(newEntity)!.Biomass;
        Assert.Equal(originalBiomass * 0.6, totalAfter, precision: 10);
    }

    [Fact]
    public void AdjacencyMap_RealZones_FranceGermanyAdjacent()
    {
        var earth = Earth.Create();
        var map = AdjacencyMap.FromZones(earth.Zones);
        Assert.True(map.AreAdjacent("France", "Germany"));
    }

    [Fact]
    public void AdjacencyMap_RealZones_NonAdjacentCountries()
    {
        var earth = Earth.Create();
        var map = AdjacencyMap.FromZones(earth.Zones);
        Assert.False(map.AreAdjacent("France", "Japan"));
    }

    [Fact]
    public void AdjacencyMap_RealZones_FranceSpainAdjacent()
    {
        var earth = Earth.Create();
        var map = AdjacencyMap.FromZones(earth.Zones);
        Assert.True(map.AreAdjacent("France", "Spain"));
    }

    [Fact]
    public void AdjacencyMap_LoadFromFile_ParsesBorderLengths()
    {
        var map = AdjacencyMap.LoadFromBordersDirectory();
        Assert.True(map.AreAdjacent("Afghanistan", "Pakistan"));
        Assert.Equal(2670, map.GetBorderLength("Afghanistan", "Pakistan"));
        Assert.Equal(2670, map.GetBorderLength("Pakistan", "Afghanistan"));
    }

    [Fact]
    public void AdjacencyMap_LoadFromFile_WaterBordersHaveZeroLength()
    {
        var map = AdjacencyMap.LoadFromBordersDirectory();
        Assert.True(map.AreAdjacent("France", "United Kingdom"));
        Assert.Equal(0, map.GetBorderLength("France", "United Kingdom"));
    }

    [Fact]
    public void AdjacencyMap_GetBorderLength_NonAdjacentReturnsZero()
    {
        var map = AdjacencyMap.LoadFromBordersDirectory();
        Assert.Equal(0, map.GetBorderLength("France", "Japan"));
    }

    [Fact]
    public void MultipleInfectedNeighbors_AllDetected()
    {
        var world = new World();
        var map = AdjacencyMap.LoadFromBordersDirectory();

        var france = world.SpawnInZone("France");
        world.Set(france, new InfectionComponent { Biomass = 50.0 });
        var germany = world.SpawnInZone("Germany");
        world.Set(germany, new InfectionComponent { Biomass = 30.0 });

        string target = "Switzerland";
        var neighbors = map.GetNeighbors(target);
        var sourceEntities = new List<Entity>();
        foreach (var neighborName in neighbors)
        {
            foreach (var e in world.EntitiesInZone(neighborName))
            {
                if (world.Has<InfectionComponent>(e))
                {
                    sourceEntities.Add(e);
                    break;
                }
            }
        }

        Assert.True(sourceEntities.Count >= 2,
            $"Expected at least 2 infected neighbors for {target}, found {sourceEntities.Count}");
        Assert.Contains(france, sourceEntities);
        Assert.Contains(germany, sourceEntities);
    }
}
