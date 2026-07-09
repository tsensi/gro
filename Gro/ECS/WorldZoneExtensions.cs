namespace Gro.ECS;

public static class WorldZoneExtensions
{
    public static Entity SpawnInZone(this World world, string zoneName)
    {
        var entity = world.Spawn();
        world.Set(entity, new ZoneLink { ZoneName = zoneName });
        return entity;
    }

    public static IEnumerable<Entity> EntitiesInZone(this World world, string zoneName)
    {
        return world.Query<ZoneLink>()
            .Where(e => world.Get<ZoneLink>(e)?.ZoneName == zoneName);
    }
}
