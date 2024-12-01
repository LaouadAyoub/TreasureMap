using NLog;
using TreasureMap.Core.Enums;
using TreasureMap.Core.Interfaces;
using TreasureMap.Core.Models;

namespace TreasureMap.Core.Services
{
    /// <summary>
    /// Service responsable de la génération du fichier de sortie de la simulation.
    /// Convertit l'état final de la carte et des aventuriers en un format texte spécifique,
    /// incluant les dimensions de la carte, les positions des montagnes, les trésors restants,
    /// et l'état final des aventuriers.
    /// </summary>
    public class MapWriter : IMapWriter
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public string GenerateOutput(Map map, List<Adventurer> adventurers)
        {
            Logger.Info("Début de la génération du fichier de sortie");
            Logger.Debug($"Génération pour une carte de dimensions {map.Width}x{map.Height} avec {adventurers.Count} aventuriers");

            var lines = new List<string>();

            // Configuration de la carte
            lines.Add($"C - {map.Width} - {map.Height}");
            Logger.Debug("Dimensions de la carte écrites");

            // Écriture des montagnes
            Logger.Debug("Début de l'écriture des montagnes");
            int mountainCount = 0;
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var position = new Position(x, y);
                    var cell = map.GetCell(position);
                    if (cell.Type == CellType.Mountain)
                    {
                        lines.Add($"M - {x} - {y}");
                        mountainCount++;
                    }
                }
            }
            Logger.Debug($"{mountainCount} montagnes écrites");

            // Écriture des trésors restants
            Logger.Debug("Début de l'écriture des trésors");
            int treasureLocationsCount = 0;
            int totalTreasuresRemaining = 0;
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    var position = new Position(x, y);
                    var cell = map.GetCell(position);
                    if (cell.Type == CellType.Treasure && cell.TreasureCount > 0)
                    {
                        lines.Add($"T - {x} - {y} - {cell.TreasureCount}");
                        treasureLocationsCount++;
                        totalTreasuresRemaining += cell.TreasureCount;
                    }
                }
            }
            Logger.Debug($"{totalTreasuresRemaining} trésors restants sur {treasureLocationsCount} emplacements");

            // État final des aventuriers
            Logger.Debug("Écriture de l'état final des aventuriers");
            foreach (var adventurer in adventurers)
            {
                string adventurerLine = $"A - {adventurer.Name} - " +
                                      $"{adventurer.Position.X} - {adventurer.Position.Y} - " +
                                      $"{ConvertOrientationToString(adventurer.Orientation)} - " +
                                      $"{adventurer.CollectedTreasures}";
                lines.Add(adventurerLine);
                Logger.Info($"État final de {adventurer.Name}: Position({adventurer.Position.X},{adventurer.Position.Y}), " +
                          $"Orientation: {adventurer.Orientation}, Trésors collectés: {adventurer.CollectedTreasures}");
            }

            var output = string.Join(Environment.NewLine, lines);
            Logger.Info($"Génération du fichier terminée : {lines.Count} lignes générées");
            return output;
        }

        private static string ConvertOrientationToString(Orientation orientation)
        {
            var result = orientation switch
            {
                Orientation.North => "N",
                Orientation.South => "S",
                Orientation.East => "E",
                Orientation.West => "O",
                _ => throw new ArgumentException($"Invalid orientation: {orientation}")
            };
            Logger.Debug($"Conversion d'orientation : {orientation} -> {result}");
            return result;
        }
    }
}