using Application.DTOs;
using Domain.Abstractions;

namespace Application.Interfaces;

public interface IPokemonService
{
    Task<Result<PokemonDto>> GetPokemonAsync(string name);
    Task<Result<List<PokemonDto>>> GetPokemonByTypeAsync(string type);
    Task<Result<string>> GetEvolutionAsync(string name);
    Task<Result<List<TypeDto>>> GetTypesAsync();
}
