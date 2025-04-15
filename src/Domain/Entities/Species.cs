namespace Domain.Entities;
public class Species
{
    public EvolutionChainInfo Evolution_Chain { get; set; } = new();
    public ColorInfo Color { get; set; } = new();
    public HabitatInfo Habitat { get; set; } = new();
}

public class EvolutionChainInfo
{
    public string Url { get; set; } = string.Empty;
}

public class ColorInfo
{
    public string Name { get; set; } = string.Empty;
}

public class HabitatInfo
{
    public string Name { get; set; } = string.Empty;
}
