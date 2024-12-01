using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureMap.Core.Interfaces;
using TreasureMap.Core.Services;

namespace TreasureMap.Console.Application
{
    /// <summary>
    /// Application principale qui orchestre la simulation de la carte aux trésors.
    /// Cette classe gère le cycle de vie complet de la simulation, de la lecture du fichier
    /// d'entrée jusqu'à la génération des résultats.
    /// </summary>
    public class TreasureMapApplication
    {
        // Ajout du logger privé pour cette classe
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMapParser _mapParser;
        private readonly IMapWriter _mapWriter;
        private readonly IGameEngine _gameEngine;
        private readonly string _inputFile;
        private readonly string _outputFile;

        public TreasureMapApplication(
            IMapParser mapParser,
            IMapWriter mapWriter,
            IGameEngine gameEngine)
        {
            Logger.Debug("Initializing TreasureMapApplication");
            _mapParser = mapParser;
            _mapWriter = mapWriter;
            _gameEngine = gameEngine;

            // Définir les chemins des fichiers
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _inputFile = Path.Combine(baseDirectory, "Data", "input.txt");
            _outputFile = Path.Combine(baseDirectory, "Data", "output.txt");
            Logger.Info($"Input file path: {_inputFile}");
            Logger.Info($"Output file path: {_outputFile}");
        }

        public int Run()
        {
            Logger.Info("Starting TreasureMap application");
            try
            {
                if (!File.Exists(_inputFile))
                {
                    Logger.Error($"Input file not found at {_inputFile}");
                    System.Console.WriteLine($"Error: Input file not found at {_inputFile}");
                    return 1;
                }

                ExecuteSimulation();
                Logger.Info("Application completed successfully");
                return 0;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Fatal error during simulation");
                System.Console.WriteLine($"Error during simulation: {ex.Message}");
                return 1;
            }
        }

        private void ExecuteSimulation()
        {
            Logger.Info("Starting simulation execution");

            // Lecture et parsing
            Logger.Debug($"Reading input file: {_inputFile}");
            System.Console.WriteLine($"Reading input file: {_inputFile}");
            string[] inputLines = File.ReadAllLines(_inputFile);

            Logger.Debug("Parsing map and adventurers");
            System.Console.WriteLine("Parsing map and adventurers...");
            var (map, adventurers) = _mapParser.ParseMap(inputLines);
            Logger.Info($"Map parsed successfully. Size: {map.Width}x{map.Height}, Adventurers count: {adventurers.Count}");

            // Exécution
            Logger.Info("Starting game engine simulation");
            System.Console.WriteLine("Running simulation...");
            _gameEngine.RunSimulation(map, adventurers);
            Logger.Info("Simulation completed");

            // Génération du résultat
            Logger.Debug("Generating output");
            System.Console.WriteLine("Generating output...");
            string output = _mapWriter.GenerateOutput(map, adventurers);

            // Écriture du résultat
            Logger.Debug($"Writing results to: {_outputFile}");
            System.Console.WriteLine($"Writing results to: {_outputFile}");
            File.WriteAllText(_outputFile, output);

            Logger.Info("Simulation execution completed successfully");
            System.Console.WriteLine("Simulation completed successfully!");
        }
    }
}
