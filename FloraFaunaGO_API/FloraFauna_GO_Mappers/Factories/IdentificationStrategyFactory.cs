using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Mappers.Strategies;
using FloraFauna_GO_Mappers.Interfaces.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace FloraFauna_GO_Mappers.Factories;

/// <summary>
/// Factory for creating identification strategies based on species type.
/// This centralizes the strategy selection logic and follows the Factory pattern.
/// To add a new identification provider, only this class needs to be modified.
/// </summary>
public class IdentificationStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public IdentificationStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Creates the appropriate identification strategy based on the species type.
    /// </summary>
    /// <param name="type">The type of species to identify</param>
    /// <returns>The strategy instance for the given species type</returns>
    /// <exception cref="NotSupportedException">Thrown when the species type is not supported</exception>
    public IIdentificationStrategy CreateStrategy(EspeceType type)
    {
        return type switch
        {
            EspeceType.Plant => _serviceProvider.GetRequiredService<PlantNetStrategy>(),
            EspeceType.Insect => _serviceProvider.GetRequiredService<KindwiseStrategy>(),
            EspeceType.Animal => _serviceProvider.GetRequiredService<AnimalApiStrategy>(),
            _ => throw new NotSupportedException($"Species type '{type}' is not supported for identification.")
        };
    }
}