namespace Gro.Services;

public sealed class ResourceService
{
    public double Biomass { get; set; }

    public void Add(double amount)
    {
        Biomass += amount;
    }

    public bool TrySpend(double amount)
    {
        if (Biomass >= amount)
        {
            Biomass -= amount;
            return true;
        }
        return false;
    }
}
