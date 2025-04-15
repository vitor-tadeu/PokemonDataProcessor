using Infrastructure.ExternalServices;
using Microsoft.Extensions.Caching.Memory;

namespace IntegrationTests.Repositories
{
    public class PokemonRepositoryTests
    {
        [Fact]
        public async Task GetPokemonAsync_MustReturnValidPokemon()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://pokeapi.co/api/v2/")
            };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var repository = new PokeApiService(httpClient, memoryCache);

            var result = await repository.GetPokemonAsync("pikachu");
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("pikachu", result.Data!.Name);
        }

        [Fact]
        public async Task GetPokemonByTypeAsync_MustReturnPokemonsOfType()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v2/") };
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var repository = new PokeApiService(httpClient, memoryCache);

            var result = await repository.GetPokemonByTypeAsync("electric");
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Data!);
            Assert.Contains(result.Data!, p => p.Name.Contains("pikachu", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task GetEvolutionAsync_MustReturnPokemonEvolution()
        {
            var httpClient = new HttpClient();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var repository = new PokeApiService(httpClient, memoryCache);
            var speciesUrl = "https://pokeapi.co/api/v2/pokemon-species/1/"; // Bulbasaur

            var result = await repository.GetEvolutionAsync(speciesUrl);
            Assert.True(result.IsSuccess);
            Assert.Equal(new[] { "bulbasaur", "ivysaur", "venusaur" }, result.Data!);
        }

        [Fact]
        public async Task GetTypesAsync_MustReturnAllPokemonTypes()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://pokeapi.co/api/v2/") };
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var repository = new PokeApiService(httpClient, memoryCache);

            var result = await repository.GetTypesAsync();
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Data!);
            Assert.Contains(result.Data!, t => t.Name.Equals("fire", StringComparison.OrdinalIgnoreCase));
        }
    }
}
