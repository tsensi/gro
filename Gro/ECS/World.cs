namespace Gro.ECS;

public sealed class World
{
    private int _nextId;
    private readonly HashSet<int> _alive = new();
    private readonly Dictionary<Type, IComponentPool> _pools = new();

    public Entity Spawn()
    {
        var entity = new Entity(_nextId++);
        _alive.Add(entity.Id);
        return entity;
    }

    public void Despawn(Entity entity)
    {
        if (!_alive.Remove(entity.Id))
            return;
        foreach (var pool in _pools.Values)
            pool.Remove(entity);
    }

    public bool IsAlive(Entity entity) => _alive.Contains(entity.Id);

    public void Set<T>(Entity entity, T component) where T : notnull
    {
        GetPool<T>().Set(entity, component);
    }

    public T? Get<T>(Entity entity)
    {
        return GetPool<T>().TryGet(entity);
    }

    public bool Has<T>(Entity entity)
    {
        return GetPool<T>().Has(entity);
    }

    public void Remove<T>(Entity entity)
    {
        GetPool<T>().Remove(entity);
    }

    public IEnumerable<Entity> Query<T>()
    {
        return GetPool<T>().Entities;
    }

    public IEnumerable<Entity> All => _alive.Select(id => new Entity(id));

    private ComponentPool<T> GetPool<T>()
    {
        var type = typeof(T);
        if (!_pools.TryGetValue(type, out var pool))
        {
            pool = new ComponentPool<T>();
            _pools[type] = pool;
        }
        return (ComponentPool<T>)pool;
    }
}
