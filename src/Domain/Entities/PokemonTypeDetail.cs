namespace Domain.Entities;

public class PokemonTypeDetail
{
    public List<PokemonEntry> Pokemon { get; set; } = [];
}

public class PokemonEntry
{
    public PokemonInfo Pokemon { get; set; } = new();
}

public class PokemonInfo
{
    public string Name { get; set; } = string.Empty;
}
