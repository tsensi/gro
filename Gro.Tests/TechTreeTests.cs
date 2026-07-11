using Gro.EarthModel;
using Gro.ECS;
using Gro.Infection;
using Gro.Simulation;
using Gro.TechTree;
using Xunit;

namespace Gro.Tests;

public class TechTreeTests
{
    private static TechRegistry CreateTestRegistry()
    {
        var json = """
        [
          {"id": "infection_wave_1", "name": "Infection Wave I", "tier": 1, "prerequisite": null, "researchDays": 20, "diffusionRatePerDay": 0.02},
          {"id": "infection_wave_2", "name": "Infection Wave II", "tier": 2, "prerequisite": "infection_wave_1", "researchDays": 40, "diffusionRatePerDay": 0.015},
          {"id": "infection_wave_3", "name": "Infection Wave III", "tier": 3, "prerequisite": "infection_wave_2", "researchDays": 60, "diffusionRatePerDay": 0.01}
        ]
        """;
        return TechRegistry.LoadFromJson(json);
    }

    [Fact]
    public void Registry_LoadsAllTechs()
    {
        var registry = CreateTestRegistry();
        Assert.Equal(3, registry.All.Count);
    }

    [Fact]
    public void Registry_LooksUpById()
    {
        var registry = CreateTestRegistry();
        var tech = registry.Get("infection_wave_1");
        Assert.NotNull(tech);
        Assert.Equal("Infection Wave I", tech.Name);
        Assert.Equal(1, tech.Tier);
        Assert.Null(tech.Prerequisite);
        Assert.Equal(20, tech.ResearchDays);
    }

    [Fact]
    public void Registry_PrerequisiteChain()
    {
        var registry = CreateTestRegistry();
        var t2 = registry.Get("infection_wave_2")!;
        Assert.Equal("infection_wave_1", t2.Prerequisite);
        var t3 = registry.Get("infection_wave_3")!;
        Assert.Equal("infection_wave_2", t3.Prerequisite);
    }

    [Fact]
    public void Registry_UnknownReturnsNull()
    {
        var registry = CreateTestRegistry();
        Assert.Null(registry.Get("nonexistent"));
    }

    [Fact]
    public void ResearchComponent_StartsEmpty()
    {
        var research = new ResearchComponent();
        Assert.Equal(0.0, research.GetProgress("infection_wave_1"));
        Assert.False(research.IsFullyEstablished("infection_wave_1"));
        Assert.False(research.IsResearching("infection_wave_1"));
    }

    [Fact]
    public void ResearchComponent_StartResearch_TracksActive()
    {
        var research = new ResearchComponent();
        research.StartResearch("infection_wave_1");
        Assert.True(research.IsResearching("infection_wave_1"));
        Assert.Equal(0.0, research.GetProgress("infection_wave_1"));
    }

    [Fact]
    public void ResearchComponent_AddProgress_Accumulates()
    {
        var research = new ResearchComponent();
        research.StartResearch("infection_wave_1");
        research.AddProgress("infection_wave_1", 0.3);
        Assert.Equal(0.3, research.GetProgress("infection_wave_1"), precision: 10);
        research.AddProgress("infection_wave_1", 0.4);
        Assert.Equal(0.7, research.GetProgress("infection_wave_1"), precision: 10);
    }

    [Fact]
    public void ResearchComponent_AddProgress_CapsAt1()
    {
        var research = new ResearchComponent();
        research.StartResearch("infection_wave_1");
        research.AddProgress("infection_wave_1", 0.8);
        research.AddProgress("infection_wave_1", 0.5);
        Assert.Equal(1.0, research.GetProgress("infection_wave_1"));
        Assert.True(research.IsFullyEstablished("infection_wave_1"));
    }

    [Fact]
    public void ResearchComponent_NeverDecreases()
    {
        var research = new ResearchComponent();
        research.StartResearch("infection_wave_1");
        research.AddProgress("infection_wave_1", 0.5);
        research.AddProgress("infection_wave_1", -0.3);
        Assert.Equal(0.5, research.GetProgress("infection_wave_1"), precision: 10);
    }

    [Fact]
    public void ResearchSystem_AdvancesProgress()
    {
        var registry = CreateTestRegistry();
        var sim = new SimLoop();
        sim.AddSystem(new ResearchSystem(registry));

        var entity = sim.World.SpawnInZone("TestZone");
        sim.World.Set(entity, new InfectionComponent());
        var research = new ResearchComponent();
        research.StartResearch("infection_wave_1");
        sim.World.Set(entity, research);

        sim.Tick(10.0);

        Assert.Equal(0.5, research.GetProgress("infection_wave_1"), precision: 5);
    }

    [Fact]
    public void ResearchSystem_CompletesAtResearchDays()
    {
        var registry = CreateTestRegistry();
        var sim = new SimLoop();
        sim.AddSystem(new ResearchSystem(registry));

        var entity = sim.World.SpawnInZone("TestZone");
        sim.World.Set(entity, new InfectionComponent());
        var research = new ResearchComponent();
        research.StartResearch("infection_wave_1");
        sim.World.Set(entity, research);

        for (int i = 0; i < 200; i++)
            sim.Tick(0.1);

        Assert.True(research.IsFullyEstablished("infection_wave_1"));
    }

    [Fact]
    public void ResearchSystem_DoesNotAdvanceWithoutActiveResearch()
    {
        var registry = CreateTestRegistry();
        var sim = new SimLoop();
        sim.AddSystem(new ResearchSystem(registry));

        var entity = sim.World.SpawnInZone("TestZone");
        sim.World.Set(entity, new InfectionComponent());
        var research = new ResearchComponent();
        sim.World.Set(entity, research);

        sim.Tick(10.0);

        Assert.Equal(0.0, research.GetProgress("infection_wave_1"));
    }

    [Fact]
    public void ResearchSystem_MultipleZonesResearchIndependently()
    {
        var registry = CreateTestRegistry();
        var sim = new SimLoop();
        sim.AddSystem(new ResearchSystem(registry));

        var e1 = sim.World.SpawnInZone("Zone1");
        sim.World.Set(e1, new InfectionComponent());
        var r1 = new ResearchComponent();
        r1.StartResearch("infection_wave_1");
        sim.World.Set(e1, r1);

        var e2 = sim.World.SpawnInZone("Zone2");
        sim.World.Set(e2, new InfectionComponent());
        var r2 = new ResearchComponent();
        r2.StartResearch("infection_wave_1");
        sim.World.Set(e2, r2);

        sim.Tick(5.0);

        Assert.Equal(0.25, r1.GetProgress("infection_wave_1"), precision: 5);
        Assert.Equal(0.25, r2.GetProgress("infection_wave_1"), precision: 5);
    }

    [Fact]
    public void Diffusion_SpreadsTechFromEstablishedNeighbor()
    {
        var registry = CreateTestRegistry();

        var zoneA = new Zone
        {
            Name = "A", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(0, 0), new GeoCoord(0, 10), new GeoCoord(5, 10), new GeoCoord(5, 0) },
        };
        var zoneB = new Zone
        {
            Name = "B", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(5, 0), new GeoCoord(5, 10), new GeoCoord(10, 10), new GeoCoord(10, 0) },
        };
        var adjacency = AdjacencyMap.FromZones(new[] { zoneA, zoneB });

        var sim = new SimLoop();
        sim.AddSystem(new TechDiffusionSystem(registry, adjacency));

        var eA = sim.World.SpawnInZone("A");
        sim.World.Set(eA, new InfectionComponent());
        var rA = new ResearchComponent();
        rA.StartResearch("infection_wave_1");
        rA.AddProgress("infection_wave_1", 1.0);
        sim.World.Set(eA, rA);

        var eB = sim.World.SpawnInZone("B");
        sim.World.Set(eB, new InfectionComponent());
        var rB = new ResearchComponent();
        sim.World.Set(eB, rB);

        sim.Tick(10.0);

        Assert.True(rB.GetProgress("infection_wave_1") > 0.0);
        Assert.Equal(0.2, rB.GetProgress("infection_wave_1"), precision: 5);
    }

    [Fact]
    public void Diffusion_DoesNotSpreadFromPartialProgress()
    {
        var registry = CreateTestRegistry();

        var zoneA = new Zone
        {
            Name = "A", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(0, 0), new GeoCoord(0, 10), new GeoCoord(5, 10), new GeoCoord(5, 0) },
        };
        var zoneB = new Zone
        {
            Name = "B", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(5, 0), new GeoCoord(5, 10), new GeoCoord(10, 10), new GeoCoord(10, 0) },
        };
        var adjacency = AdjacencyMap.FromZones(new[] { zoneA, zoneB });

        var sim = new SimLoop();
        sim.AddSystem(new TechDiffusionSystem(registry, adjacency));

        var eA = sim.World.SpawnInZone("A");
        sim.World.Set(eA, new InfectionComponent());
        var rA = new ResearchComponent();
        rA.StartResearch("infection_wave_1");
        rA.AddProgress("infection_wave_1", 0.5);
        sim.World.Set(eA, rA);

        var eB = sim.World.SpawnInZone("B");
        sim.World.Set(eB, new InfectionComponent());
        var rB = new ResearchComponent();
        sim.World.Set(eB, rB);

        sim.Tick(10.0);

        Assert.Equal(0.0, rB.GetProgress("infection_wave_1"));
    }

    [Fact]
    public void Diffusion_DoesNotSpreadToNonInfectedZone()
    {
        var registry = CreateTestRegistry();

        var zoneA = new Zone
        {
            Name = "A", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(0, 0), new GeoCoord(0, 10), new GeoCoord(5, 10), new GeoCoord(5, 0) },
        };
        var zoneB = new Zone
        {
            Name = "B", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(5, 0), new GeoCoord(5, 10), new GeoCoord(10, 10), new GeoCoord(10, 0) },
        };
        var adjacency = AdjacencyMap.FromZones(new[] { zoneA, zoneB });

        var sim = new SimLoop();
        sim.AddSystem(new TechDiffusionSystem(registry, adjacency));

        var eA = sim.World.SpawnInZone("A");
        sim.World.Set(eA, new InfectionComponent());
        var rA = new ResearchComponent();
        rA.StartResearch("infection_wave_1");
        rA.AddProgress("infection_wave_1", 1.0);
        sim.World.Set(eA, rA);

        // B has research component but no InfectionComponent
        var eB = sim.World.SpawnInZone("B");
        var rB = new ResearchComponent();
        sim.World.Set(eB, rB);

        sim.Tick(10.0);

        Assert.Equal(0.0, rB.GetProgress("infection_wave_1"));
    }

    [Fact]
    public void Diffusion_ActiveResearchAddsToDiffusionInflux()
    {
        var registry = CreateTestRegistry();

        var zoneA = new Zone
        {
            Name = "A", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(0, 0), new GeoCoord(0, 10), new GeoCoord(5, 10), new GeoCoord(5, 0) },
        };
        var zoneB = new Zone
        {
            Name = "B", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(5, 0), new GeoCoord(5, 10), new GeoCoord(10, 10), new GeoCoord(10, 0) },
        };
        var adjacency = AdjacencyMap.FromZones(new[] { zoneA, zoneB });

        var sim = new SimLoop();
        sim.AddSystem(new ResearchSystem(registry));
        sim.AddSystem(new TechDiffusionSystem(registry, adjacency));

        var eA = sim.World.SpawnInZone("A");
        sim.World.Set(eA, new InfectionComponent());
        var rA = new ResearchComponent();
        rA.StartResearch("infection_wave_1");
        rA.AddProgress("infection_wave_1", 1.0);
        sim.World.Set(eA, rA);

        // B is researching AND receiving diffusion
        var eB = sim.World.SpawnInZone("B");
        sim.World.Set(eB, new InfectionComponent());
        var rB = new ResearchComponent();
        rB.StartResearch("infection_wave_1");
        sim.World.Set(eB, rB);

        sim.Tick(10.0);

        // Research alone: 10/20 = 0.5; Diffusion alone: 10*0.02 = 0.2; Combined >= 0.7
        double progress = rB.GetProgress("infection_wave_1");
        Assert.True(progress >= 0.7 - 0.01, $"Expected >= 0.69, got {progress}");
    }

    [Fact]
    public void Diffusion_NonAdjacentZonesDoNotSpread()
    {
        var registry = CreateTestRegistry();

        var zoneA = new Zone
        {
            Name = "A", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(0, 0), new GeoCoord(0, 10), new GeoCoord(5, 10), new GeoCoord(5, 0) },
        };
        var zoneC = new Zone
        {
            Name = "C", Type = ZoneType.Country, ParentName = "X",
            Boundary = new[] { new GeoCoord(50, 0), new GeoCoord(50, 10), new GeoCoord(55, 10), new GeoCoord(55, 0) },
        };
        var adjacency = AdjacencyMap.FromZones(new[] { zoneA, zoneC });

        var sim = new SimLoop();
        sim.AddSystem(new TechDiffusionSystem(registry, adjacency));

        var eA = sim.World.SpawnInZone("A");
        sim.World.Set(eA, new InfectionComponent());
        var rA = new ResearchComponent();
        rA.StartResearch("infection_wave_1");
        rA.AddProgress("infection_wave_1", 1.0);
        sim.World.Set(eA, rA);

        var eC = sim.World.SpawnInZone("C");
        sim.World.Set(eC, new InfectionComponent());
        var rC = new ResearchComponent();
        sim.World.Set(eC, rC);

        sim.Tick(10.0);

        Assert.Equal(0.0, rC.GetProgress("infection_wave_1"));
    }

    [Fact]
    public void Registry_LoadDefault_LoadsFromDataDirectory()
    {
        var registry = TechRegistry.LoadDefault();
        Assert.Equal(5, registry.All.Count);
        var t1 = registry.Get("infection_wave_1");
        Assert.NotNull(t1);
        Assert.Equal("Infection Wave I", t1.Name);
        var t5 = registry.Get("infection_wave_5");
        Assert.NotNull(t5);
        Assert.Equal(5, t5.Tier);
    }

    [Fact]
    public void Serialization_ExtractAndApply_RoundTrips()
    {
        var world = new World();
        var entity = world.SpawnInZone("France");
        world.Set(entity, new InfectionComponent());
        var research = new ResearchComponent();
        research.StartResearch("infection_wave_1");
        research.AddProgress("infection_wave_1", 0.6);
        research.StartResearch("infection_wave_2");
        research.AddProgress("infection_wave_2", 0.3);
        world.Set(entity, research);

        var saves = ResearchSerializer.Extract(world);
        Assert.Single(saves);
        Assert.Equal("France", saves[0].ZoneName);
        Assert.Equal(0.6, saves[0].Progress["infection_wave_1"], precision: 10);
        Assert.Equal(0.3, saves[0].Progress["infection_wave_2"], precision: 10);
        Assert.Contains("infection_wave_1", saves[0].ActiveResearch);
        Assert.Contains("infection_wave_2", saves[0].ActiveResearch);

        var newWorld = new World();
        var newEntity = newWorld.SpawnInZone("France");
        newWorld.Set(newEntity, new InfectionComponent());
        newWorld.Set(newEntity, new ResearchComponent());

        ResearchSerializer.Apply(newWorld, saves);

        var restored = newWorld.Get<ResearchComponent>(newEntity)!;
        Assert.Equal(0.6, restored.GetProgress("infection_wave_1"), precision: 10);
        Assert.Equal(0.3, restored.GetProgress("infection_wave_2"), precision: 10);
        Assert.True(restored.IsResearching("infection_wave_1"));
        Assert.True(restored.IsResearching("infection_wave_2"));
    }

    [Fact]
    public void Serialization_JsonRoundTrip()
    {
        var saves = new List<ResearchSaveData>
        {
            new()
            {
                ZoneName = "Germany",
                Progress = new Dictionary<string, double>
                {
                    ["infection_wave_1"] = 1.0,
                    ["infection_wave_2"] = 0.45,
                },
                ActiveResearch = new List<string> { "infection_wave_2" },
            }
        };

        var json = ResearchSerializer.ToJson(saves);
        var restored = ResearchSerializer.FromJson(json);

        Assert.Single(restored);
        Assert.Equal("Germany", restored[0].ZoneName);
        Assert.Equal(1.0, restored[0].Progress["infection_wave_1"]);
        Assert.Equal(0.45, restored[0].Progress["infection_wave_2"], precision: 10);
        Assert.Single(restored[0].ActiveResearch);
        Assert.Equal("infection_wave_2", restored[0].ActiveResearch[0]);
    }

    [Fact]
    public void Serialization_SkipsEmptyComponents()
    {
        var world = new World();
        var entity = world.SpawnInZone("Spain");
        world.Set(entity, new ResearchComponent());

        var saves = ResearchSerializer.Extract(world);
        Assert.Empty(saves);
    }

    [Fact]
    public void Serialization_MultipleZones()
    {
        var world = new World();

        var e1 = world.SpawnInZone("France");
        var r1 = new ResearchComponent();
        r1.StartResearch("infection_wave_1");
        r1.AddProgress("infection_wave_1", 1.0);
        world.Set(e1, r1);

        var e2 = world.SpawnInZone("Germany");
        var r2 = new ResearchComponent();
        r2.StartResearch("infection_wave_1");
        r2.AddProgress("infection_wave_1", 0.5);
        world.Set(e2, r2);

        var saves = ResearchSerializer.Extract(world);
        Assert.Equal(2, saves.Count);

        var france = saves.Find(s => s.ZoneName == "France")!;
        var germany = saves.Find(s => s.ZoneName == "Germany")!;
        Assert.Equal(1.0, france.Progress["infection_wave_1"]);
        Assert.Equal(0.5, germany.Progress["infection_wave_1"], precision: 10);
    }

    [Fact]
    public void Serialization_ApplyIgnoresMissingZones()
    {
        var world = new World();
        var entity = world.SpawnInZone("France");
        world.Set(entity, new ResearchComponent());

        var saves = new List<ResearchSaveData>
        {
            new()
            {
                ZoneName = "NonExistentZone",
                Progress = new Dictionary<string, double> { ["infection_wave_1"] = 0.5 },
                ActiveResearch = new List<string> { "infection_wave_1" },
            }
        };

        ResearchSerializer.Apply(world, saves);

        var research = world.Get<ResearchComponent>(entity)!;
        Assert.Equal(0.0, research.GetProgress("infection_wave_1"));
    }

    [Fact]
    public void Serialization_FileRoundTrip()
    {
        var world = new World();
        var entity = world.SpawnInZone("Italy");
        var research = new ResearchComponent();
        research.StartResearch("infection_wave_1");
        research.AddProgress("infection_wave_1", 0.75);
        world.Set(entity, research);

        var tempFile = Path.Combine(Path.GetTempPath(), $"gro_test_{Guid.NewGuid()}.json");
        try
        {
            ResearchSerializer.SaveToFile(world, tempFile);
            Assert.True(File.Exists(tempFile));

            var newWorld = new World();
            var newEntity = newWorld.SpawnInZone("Italy");
            newWorld.Set(newEntity, new ResearchComponent());

            ResearchSerializer.LoadFromFile(newWorld, tempFile);

            var restored = newWorld.Get<ResearchComponent>(newEntity)!;
            Assert.Equal(0.75, restored.GetProgress("infection_wave_1"), precision: 10);
            Assert.True(restored.IsResearching("infection_wave_1"));
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void Serialization_LoadFromFile_NonexistentFileIsNoOp()
    {
        var world = new World();
        var entity = world.SpawnInZone("Spain");
        world.Set(entity, new ResearchComponent());

        ResearchSerializer.LoadFromFile(world, "/tmp/nonexistent_gro_save.json");

        var research = world.Get<ResearchComponent>(entity)!;
        Assert.Equal(0.0, research.GetProgress("infection_wave_1"));
    }
}
