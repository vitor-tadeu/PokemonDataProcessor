namespace Domain.Entities;

public class EvolutionChain
{
    public EvolutionData Chain { get; set; } = new();
}

public class EvolutionData
{
    public SpeciesInfo Species { get; set; } = new();
    public List<EvolutionData> Evolves_To { get; set; } = new();
}

public class SpeciesInfo
{
    public string Name { get; set; } = string.Empty;
}
