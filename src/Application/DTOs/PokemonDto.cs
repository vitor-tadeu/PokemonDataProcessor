namespace Application.DTOs;

public class PokemonDto
{
    public string Name { get; set; } = string.Empty;
    public int Height { get; set; }
    public int Weight { get; set; }
    public string Abilities { get; set; } = string.Empty;
    public string Types { get; set; } = string.Empty;
    public List<PokemonStat> Stats { get; set; } = [];
    public string Sprites { get; set; } = string.Empty;
}

public class PokemonStat
{
    public int BaseStat { get; set; }
    public string Name { get; set; } = string.Empty;
}
