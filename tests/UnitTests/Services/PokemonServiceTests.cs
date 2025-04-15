using Application.Services;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace UnitTests.Services;

public class PokemonServiceTests
{
    private readonly Mock<IPokeApiService> _pokeApiServiceMock;
    private readonly PokemonService _service;

    public PokemonServiceTests()
    {
        _pokeApiServiceMock = new Mock<IPokeApiService>();
        _service = new PokemonService(_pokeApiServiceMock.Object);
    }

    [Fact]
    public async Task GetPokemonAsync_MustReturnFormattedPokemon_WhenSuccess()
    {
        var fakePokemon = new Pokemon
        {
            Name = "pikachu",
            Height = 4,
            Weight = 60,
            Abilities = new List<PokemonAbility>
            {
                new() { Ability = new AbilityInfo { Name = "static" } },
                new() { Ability = new AbilityInfo { Name = "lightning-rod" } }
            },
            Types = new List<PokemonType>
            {
                new() { Type = new TypeInfo { Name = "electric" } }
            },
            Stats = new List<PokemonStat>
            {
                new() { Base_Stat = 35, Stat = new StatInfo { Name = "speed" } }
            },
            Sprites = new PokemonSprites { Front_Default = "https://..." }
        };

        _pokeApiServiceMock
            .Setup(repo => repo.GetPokemonAsync("pikachu"))
            .ReturnsAsync(Result<Pokemon>.Success(fakePokemon));

        var result = await _service.GetPokemonAsync("pikachu");
        Assert.True(result.IsSuccess);
        Assert.Equal("Pikachu", result.Data!.Name);
        Assert.Equal("Lightning-rod, Static", result.Data.Abilities);
        Assert.Equal("Electric", result.Data.Types);
        Assert.Equal("Speed", result.Data.Stats[0].Name);
        Assert.Equal("https://...", result.Data.Sprites);
    }

    [Fact]
    public async Task GetPokemonAsync_MustReturnFailure_WhenRepositoryFails()
    {
        _pokeApiServiceMock
            .Setup(repo => repo.GetPokemonAsync("non-existent"))
            .ReturnsAsync(Result<Pokemon>.Failure("Not found"));

        var result = await _service.GetPokemonAsync("non-existent");
        Assert.False(result.IsSuccess);
        Assert.Equal("Not found", result.ErrorMessage);
    }

    [Fact]
    public async Task GetPokemonByTypeAsync_MustReturnListWithCapitalizedNames_WhenSuccess()
    {
        var pokemons = new List<PokemonInfo>
        {
            new() { Name = "bulbasaur" },
            new() { Name = "ivysaur" },
            new() { Name = "venusaur" }
        };

        _pokeApiServiceMock
            .Setup(repo => repo.GetPokemonByTypeAsync("grass"))
            .ReturnsAsync(Result<List<PokemonInfo>>.Success(pokemons));

        var result = await _service.GetPokemonByTypeAsync("grass");
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Data!.Count);
        Assert.Equal("Bulbasaur", result.Data[0].Name);
        Assert.Equal("Ivysaur", result.Data[1].Name);
        Assert.Equal("Venusaur", result.Data[2].Name);
    }

    [Fact]
    public async Task GetPokemonByTypeAsync_MustReturnFailure_WhenRepositoryFails()
    {
        _pokeApiServiceMock
            .Setup(repo => repo.GetPokemonByTypeAsync("ghost"))
            .ReturnsAsync(Result<List<PokemonInfo>>.Failure("Type not found"));

        var result = await _service.GetPokemonByTypeAsync("ghost");
        Assert.False(result.IsSuccess);
        Assert.Equal("Type not found", result.ErrorMessage);
    }

    [Fact]
    public async Task GetEvolutionAsync_MustReturnFormattedEvolution_WhenSuccess()
    {
        // Arrange
        var speciesUrl = "https://pokeapi.co/api/v2/pokemon-species/1/";
        var pokemon = new Pokemon
        {
            Species = new PokemonSpecies { Url = speciesUrl }
        };

        _pokeApiServiceMock
            .Setup(r => r.GetPokemonAsync("bulbasaur"))
            .ReturnsAsync(Result<Pokemon>.Success(pokemon));

        _pokeApiServiceMock
            .Setup(r => r.GetEvolutionAsync(speciesUrl))
            .ReturnsAsync(Result<List<string>>.Success(new List<string> { "bulbasaur", "ivysaur", "venusaur" }));

        // Act
        var result = await _service.GetEvolutionAsync("bulbasaur");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Evolution: Bulbasaur → Ivysaur → Venusaur", result.Data);
    }

    [Fact]
    public async Task GetEvolutionAsync_MustReturnFailure_IfPokemonNotFound()
    {
        _pokeApiServiceMock
            .Setup(r => r.GetPokemonAsync("fake"))
            .ReturnsAsync(Result<Pokemon>.Failure("Not found"));

        var result = await _service.GetEvolutionAsync("fake");

        Assert.False(result.IsSuccess);
        Assert.Equal("Not found", result.ErrorMessage);
    }

    [Fact]
    public async Task GetEvolutionAsync_MustReturnFailure_IfEvolutionFails()
    {
        var speciesUrl = "url";
        var pokemon = new Pokemon
        {
            Species = new PokemonSpecies { Url = speciesUrl }
        };

        _pokeApiServiceMock
            .Setup(r => r.GetPokemonAsync("bulbasaur"))
            .ReturnsAsync(Result<Pokemon>.Success(pokemon));

        _pokeApiServiceMock
            .Setup(r => r.GetEvolutionAsync(speciesUrl))
            .ReturnsAsync(Result<List<string>>.Failure("Error in evolution"));

        var result = await _service.GetEvolutionAsync("bulbasaur");

        Assert.False(result.IsSuccess);
        Assert.Equal("Error in evolution", result.ErrorMessage);
    }

    [Fact]
    public async Task GetTypesAsync_MustReturnFormattedTypes_WhenSuccess()
    {
        var types = new List<TypeResult>
    {
        new() { Name = "fire" },
        new() { Name = "water" }
    };

        _pokeApiServiceMock
            .Setup(r => r.GetTypesAsync())
            .ReturnsAsync(Result<List<TypeResult>>.Success(types));

        var result = await _service.GetTypesAsync();

        Assert.True(result.IsSuccess);
        Assert.Equal("Fire", result.Data![0].Name);
        Assert.Equal("Water", result.Data[1].Name);
    }

    [Fact]
    public async Task GetTypesAsync_MustReturnFailure_WhenRepositoryFails()
    {
        _pokeApiServiceMock
            .Setup(r => r.GetTypesAsync())
            .ReturnsAsync(Result<List<TypeResult>>.Failure("Error fetching types"));

        var result = await _service.GetTypesAsync();
        Assert.False(result.IsSuccess);
        Assert.Equal("Error fetching types", result.ErrorMessage);
    }
}
