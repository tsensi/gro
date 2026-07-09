namespace Gro.ECS;

internal interface IComponentPool
{
    void Remove(Entity entity);
}

internal sealed class ComponentPool<T> : IComponentPool
{
    private readonly Dictionary<int, T> _data = new();

    public void Set(Entity entity, T component)
    {
        _data[entity.Id] = component;
    }

    public T? TryGet(Entity entity)
    {
        return _data.TryGetValue(entity.Id, out var val) ? val : default;
    }

    public bool Has(Entity entity) => _data.ContainsKey(entity.Id);

    public void Remove(Entity entity) => _data.Remove(entity.Id);

    public IEnumerable<Entity> Entities => _data.Keys.Select(id => new Entity(id));
}
