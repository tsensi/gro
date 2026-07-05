namespace Gro.UI;

public sealed class StateStore
{
    private readonly Dictionary<string, object?> _state = new();

    public T Get<T>(string key, T defaultValue)
    {
        if (_state.TryGetValue(key, out var val) && val is T typed)
            return typed;
        _state[key] = defaultValue;
        return defaultValue;
    }

    public void Set<T>(string key, T value)
    {
        _state[key] = value;
    }

    public bool Has(string key) => _state.ContainsKey(key);
}
