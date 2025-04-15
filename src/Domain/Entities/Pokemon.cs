namespace Domain.Entities;

public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Height { get; set; }
    public int Weight { get; set; }
    public List<PokemonAbility> Abilities { get; set; } = [];
    public List<PokemonType> Types { get; set; } = [];
    public List<PokemonStat> Stats { get; set; } = [];
    public PokemonSpecies Species { get; set; } = new();
    public PokemonSprites? Sprites { get; set; }
}

public class PokemonAbility
{
    public AbilityInfo? Ability { get; set; }
}

public class AbilityInfo
{
    public string Name { get; set; } = string.Empty;
}

public class PokemonType
{
    public TypeInfo? Type { get; set; }
}

public class TypeInfo
{
    public string Name { get; set; } = string.Empty;
}

public class PokemonStat
{
    public int Base_Stat { get; set; }
    public StatInfo? Stat { get; set; }
}

public class StatInfo
{
    public string Name { get; set; } = string.Empty;
}

public class PokemonSpecies
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public class PokemonSprites
{
    public string Front_Default { get; set; } = string.Empty;
}
