namespace Gro.ECS;

public readonly record struct Entity(int Id)
{
    public static readonly Entity None = new(-1);
    public bool IsValid => Id >= 0;
}
