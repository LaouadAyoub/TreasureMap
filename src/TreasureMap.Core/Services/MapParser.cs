using NLog;
using TreasureMap.Core.Enums;
using TreasureMap.Core.Interfaces;
using TreasureMap.Core.Models;

namespace TreasureMap.Core.Services
{
    /// <summary>
    /// Service responsable de l'analyse du fichier d'entrée et de la création de l'état initial du jeu.
    /// Parse le fichier ligne par ligne pour créer la carte, placer les montagnes et les trésors,
    /// puis positionner les aventuriers dans leur configuration initiale.
    /// Le processus se fait en deux passes pour assurer que la carte est complètement configurée
    /// avant le placement des aventuriers.
    /// </summary>
    public class MapParser : IMapParser
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public (Map map, List<Adventurer> adventurers) ParseMap(string[] lines)
        {
            Logger.Info("Début de l'analyse du fichier d'entrée");
            Logger.Debug($"Nombre de lignes à analyser : {lines.Length}");

            Map map = null;
            var adventurers = new List<Adventurer>();

            try
            {
                // Première passe : configuration de la carte
                Logger.Debug("Première passe : création de la carte et placement des éléments statiques");
                foreach (var line in lines.Where(l => !l.StartsWith("#")))
                {
                    var parts = line.Split(" - ");
                    ProcessFirstPassLine(parts, ref map);
                }

                if (map == null)
                {
                    Logger.Error("Aucune définition de carte (C) trouvée dans le fichier");
                    throw new ArgumentException("Invalid map format: No map definition found");
                }

                Logger.Info($"Carte créée avec succès : {map.Width}x{map.Height}");

                // Deuxième passe : ajout des aventuriers
                Logger.Debug("Deuxième passe : placement des aventuriers");
                foreach (var line in lines.Where(l => !l.StartsWith("#") && l.StartsWith("A")))
                {
                    ProcessAdventurerLine(line, map, adventurers);
                }

                Logger.Info($"Parsing terminé : {adventurers.Count} aventuriers placés sur la carte");
                return (map, adventurers);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erreur lors du parsing de la carte");
                throw;
            }
        }

        private void ProcessFirstPassLine(string[] parts, ref Map map)
        {
            switch (parts[0])
            {
                case "C":
                    var width = int.Parse(parts[1]);
                    var height = int.Parse(parts[2]);
                    map = new Map(width, height);
                    Logger.Debug($"Création de la carte : {width}x{height}");
                    break;

                case "M":
                    if (map != null)
                    {
                        var mountainPosition = new Position(int.Parse(parts[1]), int.Parse(parts[2]));
                        map.AddMountain(mountainPosition);
                        Logger.Debug($"Ajout d'une montagne en ({mountainPosition.X},{mountainPosition.Y})");
                    }
                    break;

                case "T":
                    if (map != null)
                    {
                        var treasurePosition = new Position(int.Parse(parts[1]), int.Parse(parts[2]));
                        var treasureCount = int.Parse(parts[3]);
                        map.AddTreasures(treasurePosition, treasureCount);
                        Logger.Debug($"Ajout de {treasureCount} trésors en ({treasurePosition.X},{treasurePosition.Y})");
                    }
                    break;
            }
        }

        private void ProcessAdventurerLine(string line, Map map, List<Adventurer> adventurers)
        {
            var parts = line.Split(" - ");
            var name = parts[1];
            var position = new Position(int.Parse(parts[2]), int.Parse(parts[3]));
            var orientation = ParseOrientation(parts[4]);
            var movements = parts[5];

            var adventurer = new Adventurer(name, position, orientation, movements);
            adventurers.Add(adventurer);
            map.AddAdventurer(adventurer);

            Logger.Info($"Aventurier ajouté : {name} en position ({position.X},{position.Y}), " +
                       $"orientation {orientation}, séquence de mouvements : {movements}");
        }

        private static Orientation ParseOrientation(string orientation)
        {
            var result = orientation.ToUpper() switch
            {
                "N" => Orientation.North,
                "S" => Orientation.South,
                "E" => Orientation.East,
                "O" => Orientation.West,
                _ => throw new ArgumentException($"Invalid orientation: {orientation}")
            };

            Logger.Debug($"Orientation parsée : {orientation} -> {result}");
            return result;
        }
    }
}