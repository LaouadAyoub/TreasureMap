using Microsoft.Extensions.DependencyInjection;
using NLog;
using TreasureMap.Console.Application;
using TreasureMap.Core.Interfaces;
using TreasureMap.Core.Services;

namespace TreasureMap.Console;


public class Program
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    public static int Main()
    {
        Logger.Info("Application TreasureMap démarrée");
        try
        {
            Logger.Debug("Configuration des services...");
            var serviceProvider = ConfigureServices();

            Logger.Debug("Initialisation de l'application...");
            var app = serviceProvider.GetRequiredService<TreasureMapApplication>();

            Logger.Info("Démarrage de la simulation");
            var result = app.Run();

            Logger.Info($"Application terminée avec le code de retour : {result}");
            return result;
        }
        catch (Exception ex)
        {
            Logger.Fatal(ex, "Erreur fatale lors de l'exécution de l'application");
            System.Console.WriteLine($"Fatal error: {ex.Message}");

            #if DEBUG
                System.Console.WriteLine(ex.StackTrace);
            #endif

            return 1;
        }
    }

    private static ServiceProvider ConfigureServices()
    {
        Logger.Debug("Configuration du conteneur de dépendances");
        var services = new ServiceCollection();

        // Enregistrement des services avec leur durée de vie
        services.AddScoped<IMapParser, MapParser>();
        services.AddScoped<IMapWriter, MapWriter>();
        services.AddScoped<IGameEngine, GameEngine>();
        services.AddScoped<TreasureMapApplication>();

        Logger.Debug("Services enregistrés dans le conteneur");
        return services.BuildServiceProvider();
    }
}