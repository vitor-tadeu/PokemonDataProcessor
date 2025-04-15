using System.Net.Http.Json;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.ExternalServices;

public class PokeApiService : IPokeApiService
{
    private static readonly string _baseURL = $"https://pokeapi.co/api/v2/";

    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public PokeApiService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<Result<Pokemon>> GetPokemonAsync(string name)
    {
        if (_cache.TryGetValue(name, out Pokemon? cachedPokemon))
        {
            return Result<Pokemon>.Success(cachedPokemon!);
        }

        try
        {
            var pokemon = await _httpClient.GetFromJsonAsync<Pokemon>($"{_baseURL}pokemon/{name}");
            if (pokemon is not null)
            {
                _cache.Set(name, pokemon, TimeSpan.FromMinutes(10));
                return Result<Pokemon>.Success(pokemon);
            }

            return Result<Pokemon>.Failure($"Pokémon {name} not found!");
        }
        catch (HttpRequestException ex)
        {
            return Result<Pokemon>.Failure($"HTTP request error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<Pokemon>.Failure($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result<List<PokemonInfo>>> GetPokemonByTypeAsync(string type)
    {
        if (_cache.TryGetValue(type, out List<PokemonInfo>? cachedTypes))
        {
            return Result<List<PokemonInfo>>.Success(cachedTypes!);
        }

        try
        {
            var response = await _httpClient.GetFromJsonAsync<PokemonTypeDetail>($"{_baseURL}type/{type}");
            if (response is null || response.Pokemon is null)
            {
                return Result<List<PokemonInfo>>.Failure($"No Pokémon found for {type}!");
            }

            var pokemon = response.Pokemon.Select(p => p.Pokemon).ToList();
            _cache.Set(type, pokemon, TimeSpan.FromMinutes(10));

            return Result<List<PokemonInfo>>.Success(pokemon);
        }
        catch (HttpRequestException ex)
        {
            return Result<List<PokemonInfo>>.Failure($"HTTP request error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<List<PokemonInfo>>.Failure($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result<List<string>>> GetEvolutionAsync(string url)
    {
        if (_cache.TryGetValue(url, out List<string>? cachedPokemon))
        {
            return Result<List<string>>.Success(cachedPokemon!);
        }

        try
        {
            var species = await _httpClient.GetFromJsonAsync<Species>(url);
            if (species is null)
            {
                return Result<List<string>>.Failure("Species data not found!");
            }

            var evolutionsChain = await _httpClient.GetFromJsonAsync<EvolutionChain>(species.Evolution_Chain.Url);
            if (evolutionsChain is null)
            {
                return Result<List<string>>.Failure("Evolution chain not found!");
            }

            var evolutions = new List<string>();
            var current = evolutionsChain.Chain;
            while (current is not null)
            {
                evolutions.Add(current.Species.Name);
                current = current.Evolves_To.FirstOrDefault();
            }

            if (!evolutions.Any())
            {
                return Result<List<string>>.Failure("No evolutions found!");
            }

            _cache.Set(url, evolutions, TimeSpan.FromMinutes(10));
            return Result<List<string>>.Success(evolutions);
        }
        catch (HttpRequestException ex)
        {
            return Result<List<string>>.Failure($"HTTP request error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<List<string>>.Failure($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<Result<List<TypeResult>>> GetTypesAsync()
    {
        if (_cache.TryGetValue("types", out List<TypeResult>? cachedTypesPokemon))
        {
            return Result<List<TypeResult>>.Success(cachedTypesPokemon!);
        }

        try
        {
            var response = await _httpClient.GetFromJsonAsync<PokemonTypeResult>($"{_baseURL}type");
            if (response is null || response.Results is null)
            {
                return Result<List<TypeResult>>.Failure("No Pokémon types found!");
            }

            var types = response.Results.ToList();
            _cache.Set("types", types, TimeSpan.FromMinutes(10));

            return Result<List<TypeResult>>.Success(types);
        }
        catch (HttpRequestException ex)
        {
            return Result<List<TypeResult>>.Failure($"HTTP request error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<List<TypeResult>>.Failure($"Unexpected error: {ex.Message}");
        }
    }
}
