using Application.DTOs;
using Application.Interfaces;
using Spectre.Console;

namespace Presentation.View;

public class PokemonConsole
{
    private readonly IPokemonService _pokemonService;

    public PokemonConsole(IPokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            var option = await AnsiConsole.PromptAsync(
                new SelectionPrompt<string>()
                    .Title("Choose an option:")
                    .AddChoices("Search for Pokémon", "Search for Type", "View Pokémon Evolution", "List of Types", "Exit")
            );

            switch (option)
            {
                case "Search for Pokémon":
                    Console.Write("Enter the Pokémon's name: ");
                    string? name = Console.ReadLine()?.Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Invalid name. Please try again.");
                        Console.ReadLine();
                        return;
                    }

                    await GetPokemonAsync(name);
                    break;

                case "Search for Type":
                    Console.Write("Enter the type (e.g. fire, water, electric): ");
                    string? type = Console.ReadLine()?.Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(type))
                    {
                        Console.WriteLine("Invalid type. Please try again.");
                        Console.ReadLine();
                        return;
                    }

                    await GetPokemonByTypeAsync(type);
                    break;

                case "View Pokémon Evolution":
                    Console.Write("Enter the Pokémon's name: ");
                    string? pokemonName = Console.ReadLine()?.Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(pokemonName))
                    {
                        Console.WriteLine("Invalid name. Please try again.");
                        Console.ReadLine();
                        return;
                    }

                    await GetEvolutionAsync(pokemonName);
                    break;

                case "List of Types":
                    await GetTypesAsync();
                    break;

                case "Exit":
                    Environment.Exit(0);
                    break;
            }
        }
    }

    #region Search for Pokémon
    public async Task GetPokemonAsync(string name)
    {
        AnsiConsole.MarkupLine($"[green]Searching for Pokémon {name}...[/]");

        var pokemon = await _pokemonService.GetPokemonAsync(name);
        if (!pokemon.IsSuccess)
        {
            AnsiConsole.MarkupLine($"[yellow]{pokemon.ErrorMessage}[/]");
            return;
        }

        ViewBasicDetailsPokemon(pokemon.Data!);
        ShowMoreDetails(pokemon.Data!);
    }

    private static void ViewBasicDetailsPokemon(PokemonDto pokemonDto)
    {
        var table = new Table
        {
            ShowRowSeparators = true,
            UseSafeBorder = true
        };

        table.AddColumn("Attribute");
        table.AddColumn("Value");

        table.AddRow("Name", pokemonDto.Name);
        table.AddRow("Height", pokemonDto.Height.ToString());
        table.AddRow("Weight", pokemonDto.Weight.ToString());
        table.AddRow("HP", pokemonDto.Stats.First(s => s.Name == "Hp").BaseStat.ToString());
        table.AddRow("Attack", pokemonDto.Stats.First(s => s.Name == "Attack").BaseStat.ToString());
        table.AddRow("Defense", pokemonDto.Stats.First(s => s.Name == "Defense").BaseStat.ToString());

        AnsiConsole.Write(table);
    }

    private static void ShowMoreDetails(PokemonDto pokemonDto)
    {
        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices("Show more details", "Back")
        );

        if (option == "Show more details")
        {
            ViewMoreDetailsPokemon(pokemonDto);
        }
    }

    private static void ViewMoreDetailsPokemon(PokemonDto pokemonDto)
    {
        var table = new Table
        {
            ShowRowSeparators = true,
            UseSafeBorder = true
        };

        table.AddColumn("Attribute");
        table.AddColumn("Value");

        table.AddRow("Abilities", pokemonDto.Abilities);
        table.AddRow("Types", pokemonDto.Types);

        string[] stats = ["hp", "attack", "defense"];
        foreach (var stat in pokemonDto.Stats)
        {
            if (stat.Name is not null && !stats.Contains(stat.Name, StringComparer.OrdinalIgnoreCase))
            {
                table.AddRow(stat.Name, stat.BaseStat.ToString());
            }
        }

        table.AddRow("Image URL", pokemonDto.Sprites);

        AnsiConsole.Write(table);
    }
    #endregion

    #region Search for Type
    private async Task GetPokemonByTypeAsync(string type)
    {
        AnsiConsole.MarkupLine($"[green]Listing Pokémon of type {type}...[/]");
        var pokemons = await _pokemonService.GetPokemonByTypeAsync(type);
        if (!pokemons.IsSuccess)
        {
            AnsiConsole.MarkupLine($"[yellow]{pokemons.ErrorMessage}[/]");
            return;
        }

        int page = 0;
        int pageSize = 10;

        while (true)
        {
            var paginatedPokemons = pokemons.Data!.Skip(page * pageSize).Take(pageSize).ToList();
            if (!paginatedPokemons.Any())
            {
                AnsiConsole.MarkupLine("[yellow]There are no more Pokémon to display.[/]");
                return;
            }

            var prompt = new SelectionPrompt<string>()
                .Title($"[green]Pokémon of type {type}[/]")
                .AddChoices(paginatedPokemons.Select(p => p.Name));

            if ((page + 1) * pageSize < pokemons.Data!.Count)
            {
                prompt.AddChoice("Next page");
            }

            if (page > 0)
            {
                prompt.AddChoice("Previous page");
            }

            prompt.AddChoice("Back");

            var option = await AnsiConsole.PromptAsync(prompt);
            switch (option)
            {
                case "Next page":
                    page++;
                    break;

                case "Previous page":
                    page--;
                    break;

                case "Back":
                    await RunAsync();
                    break;

                default:
                    await GetPokemonAsync(option);
                    break;
            }
        }
    }
    #endregion

    #region View Pokémon Evolution
    private async Task GetEvolutionAsync(string name)
    {
        AnsiConsole.MarkupLine($"[green]Searching for Pokémon evolution...[/]");

        var evolution = await _pokemonService.GetEvolutionAsync(name);
        if (!evolution.IsSuccess)
        {
            AnsiConsole.MarkupLine($"[yellow]{evolution.ErrorMessage}[/]");
            return;
        }

        Console.WriteLine(evolution.Data);
    }
    #endregion

    #region List of Types
    private async Task GetTypesAsync()
    {
        AnsiConsole.MarkupLine($"[green]Searching for Types...[/]");

        var types = await _pokemonService.GetTypesAsync();
        if (!types.IsSuccess)
        {
            AnsiConsole.MarkupLine($"[yellow]{types.ErrorMessage}[/]");
            return;
        }

        await ViewTypesDetailsAsync(types.Data!);
    }

    private async Task ViewTypesDetailsAsync(List<TypeDto> typeDto)
    {
        int page = 0;
        int pageSize = 10;

        while (true)
        {
            var paginatedTypes = typeDto.Skip(page * pageSize).Take(pageSize).ToList();

            if (!paginatedTypes.Any())
            {
                AnsiConsole.MarkupLine("[yellow]There are no more Types to display.[/]");
                return;
            }

            var prompt = new SelectionPrompt<string>()
                .Title($"Details for [yellow][/]")
                .AddChoices(paginatedTypes.Select(p => p.Name));

            if ((page + 1) * pageSize < typeDto.Count)
            {
                prompt.AddChoice("Next page");
            }

            if (page > 0)
            {
                prompt.AddChoice("Previous page");
            }

            prompt.AddChoice("Back");

            var option = await AnsiConsole.PromptAsync(prompt);
            switch (option)
            {
                case "Next page":
                    page++;
                    break;

                case "Previous page":
                    page--;
                    break;

                case "Back":
                    await RunAsync();
                    break;

                default:
                    await GetPokemonByTypeAsync(option);
                    break;
            }
        }
    }
    #endregion
}
