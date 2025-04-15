# 🏆 Pokémon Data Processor (Clean Architecture + .NET 9)

An interactive console project for fetching, visualizing, and exploring Pokémon information, using the [PokéAPI](https://pokeapi.co/) and implemented with **Clean Architecture** and **.NET 9**.

---

## 🚀 Main Features
```
✅ Search for Pokémon by name or type.
✅ View Pokémon evolution chain.
✅ Paginated list of Pokémon and types.
✅ Pokémon details submenu.
✅ View abilities, types, stats.
```

---

## 🛠️ Technologies
- .NET 9
- C# 12
- Clean Architecture
- Dependency Injection (DI)
- In-Memory Cache
- HttpClient (for API consumption)
- PokéAPI (public external API)
- Testing with xUnit + Moq
- Docker
- Main SOLID patterns
- Result Pattern
- Interactive console with menus
- ASCII Image Renderer

---

## 🧪 How to Run
```
Requirements: .NET 9 SDK installed.
git clone https://github.com/vitor-tadeu/PokemonDataProcessor.git
cd PokemonDataProcessor
dotnet run --project src/PokemonDataProcessor

## Run with Docker
1. Make sure Docker is installed and running.
2. In the terminal, run:

```bash
docker build -t pokemon-data-processor "full\path\to\the\Dockerfile\in\PokemonDataProcessor"
docker run --rm -it pokemon-data-processor
```

---

## 🎯 Final result
![Swagger](https://imgur.com/sx9ipKi.png)
