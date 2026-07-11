namespace Gro.TechTree;

public sealed class ResearchComponent
{
    private readonly Dictionary<string, double> _progress = new();
    private readonly HashSet<string> _activeResearch = new();

    public IReadOnlyDictionary<string, double> Progress => _progress;
    public IReadOnlySet<string> ActiveResearch => _activeResearch;

    public double GetProgress(string techId)
    {
        return _progress.TryGetValue(techId, out var p) ? p : 0.0;
    }

    public bool IsFullyEstablished(string techId)
    {
        return GetProgress(techId) >= 1.0;
    }

    public bool IsResearching(string techId)
    {
        return _activeResearch.Contains(techId);
    }

    public void StartResearch(string techId)
    {
        _activeResearch.Add(techId);
        if (!_progress.ContainsKey(techId))
            _progress[techId] = 0.0;
    }

    public void AddProgress(string techId, double amount)
    {
        if (amount <= 0) return;
        var current = GetProgress(techId);
        _progress[techId] = Math.Min(1.0, current + amount);
    }

    public bool HasAnyProgress(string techId)
    {
        return _progress.TryGetValue(techId, out var p) && p > 0.0;
    }
}
