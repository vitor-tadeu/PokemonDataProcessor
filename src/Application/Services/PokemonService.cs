using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Interfaces;

namespace Application.Services;

public class PokemonService : IPokemonService
{
    private readonly IPokeApiService _pokeApiService;

    public PokemonService(IPokeApiService pokeApiService)
    {
        _pokeApiService = pokeApiService;
    }

    public async Task<Result<PokemonDto>> GetPokemonAsync(string name)
    {
        var pokemon = await _pokeApiService.GetPokemonAsync(name);
        if (!pokemon.IsSuccess)
        {
            return Result<PokemonDto>.Failure(pokemon.ErrorMessage);
        }

        var formattedPokemon = new PokemonDto()
        {
            Name = StringHelper.CapitalizeFirstLetter(pokemon.Data!.Name),
            Height = pokemon.Data!.Height,
            Weight = pokemon.Data!.Weight,
            Abilities = string.Join(", ", pokemon.Data!.Abilities
                .Where(a => a.Ability?.Name is not null)
                .OrderBy(a => a.Ability!.Name)
                .Select(a => StringHelper.CapitalizeFirstLetter(a.Ability!.Name))
                .DefaultIfEmpty("No ability found!")
                .Aggregate((current, next) => $"{current}, {next}")
            ),
            Types = string.Join(", ", pokemon.Data!.Types
                .Where(a => a.Type?.Name is not null)
                .OrderBy(a => a.Type!.Name)
                .Select(a => StringHelper.CapitalizeFirstLetter(a.Type!.Name))
                .DefaultIfEmpty("No type found!")
                .Aggregate((current, next) => $"{current}, {next}")
            ),
            Stats = pokemon.Data!.Stats
                .Where(s => s.Stat is not null && !string.IsNullOrEmpty(s.Stat.Name))
                .Select(s => new PokemonStat
                {
                    BaseStat = s.Base_Stat,
                    Name = StringHelper.CapitalizeFirstLetter(s.Stat!.Name)
                }).ToList(),
            Sprites = !string.IsNullOrWhiteSpace(pokemon.Data!.Sprites?.Front_Default) ? pokemon.Data!.Sprites.Front_Default : "-"
        };

        return Result<PokemonDto>.Success(formattedPokemon);
    }

    public async Task<Result<List<PokemonDto>>> GetPokemonByTypeAsync(string type)
    {
        var pokemons = await _pokeApiService.GetPokemonByTypeAsync(type);
        if (!pokemons.IsSuccess)
        {
            return Result<List<PokemonDto>>.Failure(pokemons.ErrorMessage);
        }

        var formattedPokemons = pokemons.Data!.Select(p => new PokemonDto()
        {
            Name = StringHelper.CapitalizeFirstLetter(p.Name)
        }).ToList();

        return Result<List<PokemonDto>>.Success(formattedPokemons);
    }

    public async Task<Result<string>> GetEvolutionAsync(string name)
    {
        var pokemon = await _pokeApiService.GetPokemonAsync(name);
        if (!pokemon.IsSuccess)
        {
            return Result<string>.Failure(pokemon.ErrorMessage);
        }

        var evolutions = await _pokeApiService.GetEvolutionAsync(pokemon.Data!.Species.Url);
        if (!evolutions.IsSuccess)
        {
            return Result<string>.Failure(evolutions.ErrorMessage);
        }

        return Result<string>.Success($"Evolution: {string.Join(" → ", evolutions.Data!.Select(e => StringHelper.CapitalizeFirstLetter(e)))}");
    }

    public async Task<Result<List<TypeDto>>> GetTypesAsync()
    {
        var types = await _pokeApiService.GetTypesAsync();
        if (!types.IsSuccess)
        {
            return Result<List<TypeDto>>.Failure(types.ErrorMessage);
        }

        var formattedTypes = types.Data!.Select(p => new TypeDto()
        {
            Name = StringHelper.CapitalizeFirstLetter(p.Name)
        }).ToList();

        return Result<List<TypeDto>>.Success(formattedTypes);
    }
}
