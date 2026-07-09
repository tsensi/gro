using Gro.ECS;
using Xunit;

namespace Gro.Tests;

public class ECSTests
{
    [Fact]
    public void Spawn_CreatesValidEntity()
    {
        var world = new World();
        var entity = world.Spawn();
        Assert.True(entity.IsValid);
        Assert.True(world.IsAlive(entity));
    }

    [Fact]
    public void Despawn_RemovesEntity()
    {
        var world = new World();
        var entity = world.Spawn();
        world.Despawn(entity);
        Assert.False(world.IsAlive(entity));
    }

    [Fact]
    public void Despawn_RemovesAllComponents()
    {
        var world = new World();
        var entity = world.Spawn();
        world.Set(entity, new ZoneLink { ZoneName = "France" });
        world.Despawn(entity);

        var revived = world.Spawn();
        Assert.False(world.Has<ZoneLink>(revived));
    }

    [Fact]
    public void SetAndGet_Component()
    {
        var world = new World();
        var entity = world.Spawn();
        world.Set(entity, new ZoneLink { ZoneName = "France" });

        var link = world.Get<ZoneLink>(entity);
        Assert.NotNull(link);
        Assert.Equal("France", link.ZoneName);
    }

    [Fact]
    public void Get_ReturnsDefault_WhenNoComponent()
    {
        var world = new World();
        var entity = world.Spawn();
        Assert.Null(world.Get<ZoneLink>(entity));
    }

    [Fact]
    public void Has_ReturnsFalse_WhenNoComponent()
    {
        var world = new World();
        var entity = world.Spawn();
        Assert.False(world.Has<ZoneLink>(entity));
    }

    [Fact]
    public void Remove_Component()
    {
        var world = new World();
        var entity = world.Spawn();
        world.Set(entity, new ZoneLink { ZoneName = "France" });
        world.Remove<ZoneLink>(entity);
        Assert.False(world.Has<ZoneLink>(entity));
    }

    [Fact]
    public void Query_ReturnsEntitiesWithComponent()
    {
        var world = new World();
        var e1 = world.Spawn();
        var e2 = world.Spawn();
        var e3 = world.Spawn();

        world.Set(e1, new ZoneLink { ZoneName = "France" });
        world.Set(e3, new ZoneLink { ZoneName = "Germany" });

        var entities = world.Query<ZoneLink>().ToList();
        Assert.Contains(e1, entities);
        Assert.Contains(e3, entities);
        Assert.DoesNotContain(e2, entities);
    }

    [Fact]
    public void SpawnInZone_AttachesZoneLink()
    {
        var world = new World();
        var entity = world.SpawnInZone("France");

        Assert.True(world.Has<ZoneLink>(entity));
        Assert.Equal("France", world.Get<ZoneLink>(entity)!.ZoneName);
    }

    [Fact]
    public void EntitiesInZone_FindsCorrectEntities()
    {
        var world = new World();
        var e1 = world.SpawnInZone("France");
        var e2 = world.SpawnInZone("Germany");
        var e3 = world.SpawnInZone("France");

        var inFrance = world.EntitiesInZone("France").ToList();
        Assert.Equal(2, inFrance.Count);
        Assert.Contains(e1, inFrance);
        Assert.Contains(e3, inFrance);
        Assert.DoesNotContain(e2, inFrance);
    }

    [Fact]
    public void EntitiesInZone_ReturnsEmpty_WhenNoneInZone()
    {
        var world = new World();
        world.SpawnInZone("France");
        Assert.Empty(world.EntitiesInZone("Germany"));
    }

    [Fact]
    public void MultipleComponentTypes_Independent()
    {
        var world = new World();
        var entity = world.Spawn();
        world.Set(entity, new ZoneLink { ZoneName = "France" });
        world.Set(entity, new TestTag { Label = "player" });

        Assert.Equal("France", world.Get<ZoneLink>(entity)!.ZoneName);
        Assert.Equal("player", world.Get<TestTag>(entity)!.Label);

        world.Remove<ZoneLink>(entity);
        Assert.False(world.Has<ZoneLink>(entity));
        Assert.True(world.Has<TestTag>(entity));
    }

    [Fact]
    public void EntityNone_IsInvalid()
    {
        Assert.False(Entity.None.IsValid);
    }

    private sealed class TestTag
    {
        public required string Label { get; init; }
    }
}
