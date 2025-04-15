namespace Domain.Entities;

public class PokemonTypeResult
{
    public List<TypeResult> Results { get; set; } = [];
}

public class TypeResult
{
    public string Name { get; set; } = string.Empty;
}
