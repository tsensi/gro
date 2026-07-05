namespace Gro.UI;

public enum EaseFunction { Linear, EaseIn, EaseOut, EaseInOut }

public sealed class Animation
{
    public float From { get; init; }
    public float To { get; init; }
    public float DurationMs { get; init; }
    public EaseFunction Ease { get; init; } = EaseFunction.EaseOut;
    public Action<float>? OnUpdate { get; init; }
    public Action? OnComplete { get; init; }

    internal float Elapsed { get; set; }
    internal bool Done => Elapsed >= DurationMs;

    internal float Value
    {
        get
        {
            float t = Math.Clamp(Elapsed / DurationMs, 0f, 1f);
            float eased = Ease switch
            {
                EaseFunction.Linear => t,
                EaseFunction.EaseIn => t * t,
                EaseFunction.EaseOut => 1f - (1f - t) * (1f - t),
                EaseFunction.EaseInOut => t < 0.5f ? 2f * t * t : 1f - (-2f * t + 2f) * (-2f * t + 2f) / 2f,
                _ => t,
            };
            return From + (To - From) * eased;
        }
    }
}

public sealed class Animator
{
    private readonly List<Animation> _active = new();

    public Animation Animate(float from, float to, float durationMs, EaseFunction ease = EaseFunction.EaseOut, Action<float>? onUpdate = null, Action? onComplete = null)
    {
        var anim = new Animation
        {
            From = from,
            To = to,
            DurationMs = durationMs,
            Ease = ease,
            OnUpdate = onUpdate,
            OnComplete = onComplete,
        };
        _active.Add(anim);
        return anim;
    }

    public void Tick(float deltaMs)
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            var anim = _active[i];
            anim.Elapsed += deltaMs;
            anim.OnUpdate?.Invoke(anim.Value);
            if (anim.Done)
            {
                anim.OnUpdate?.Invoke(anim.To);
                anim.OnComplete?.Invoke();
                _active.RemoveAt(i);
            }
        }
    }

    public bool HasActive => _active.Count > 0;
}
