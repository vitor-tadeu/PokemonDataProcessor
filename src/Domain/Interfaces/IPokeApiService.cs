using Domain.Abstractions;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IPokeApiService
{
    Task<Result<Pokemon>> GetPokemonAsync(string name);
    Task<Result<List<PokemonInfo>>> GetPokemonByTypeAsync(string type);
    Task<Result<List<string>>> GetEvolutionAsync(string url);
    Task<Result<List<TypeResult>>> GetTypesAsync();
}
