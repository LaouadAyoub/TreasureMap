using NLog;
using TreasureMap.Core.Enums;
using TreasureMap.Core.Interfaces;
using TreasureMap.Core.Models;

namespace TreasureMap.Core.Services
{
    /// <summary>
    /// Moteur principal du jeu qui gère la simulation des mouvements des aventuriers.
    /// Implémente la logique de tour par tour où chaque aventurier effectue ses mouvements
    /// selon les règles définies, en gérant les collisions et la collecte des trésors.
    /// </summary>
    public class GameEngine : IGameEngine
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private Map? _map;
        private List<Adventurer>? _adventurers;
        private bool _isGameFinished;

        public void RunSimulation(Map map, List<Adventurer> adventurers)
        {
            Logger.Info("Démarrage de la simulation");
            Logger.Debug($"Initialisation avec une carte {map.Width}x{map.Height} et {adventurers.Count} aventuriers");

            _map = map;
            _adventurers = adventurers;
            _isGameFinished = false;

            int turnCounter = 1;
            while (!_isGameFinished)
            {
                Logger.Debug($"Début du tour {turnCounter}");
                ExecuteGameTurn();
                turnCounter++;
            }

            Logger.Info($"Simulation terminée après {turnCounter - 1} tours");
            LogFinalState();
        }

        private void ExecuteGameTurn()
        {
            bool anyMovementRemaining = false;

            foreach (var adventurer in _adventurers)
            {
                var nextMovement = adventurer.PeekNextMovement();
                if (nextMovement != null)
                {
                    Logger.Debug($"Tour de {adventurer.Name} - Position actuelle: ({adventurer.Position.X},{adventurer.Position.Y}), Orientation: {adventurer.Orientation}");
                    anyMovementRemaining = true;
                    ProcessMovement(adventurer, nextMovement.Value);
                    adventurer.ConsumeCurrentMovement();
                }
            }

            _isGameFinished = !anyMovementRemaining;
        }

        private void ProcessMovement(Adventurer adventurer, MovementType movement)
        {
            Logger.Debug($"{adventurer.Name} tente le mouvement: {movement}");

            switch (movement)
            {
                case MovementType.Forward:
                    TryMoveForward(adventurer);
                    break;
                case MovementType.TurnLeft:
                    adventurer.TurnLeft();
                    Logger.Debug($"{adventurer.Name} tourne à gauche - Nouvelle orientation: {adventurer.Orientation}");
                    break;
                case MovementType.TurnRight:
                    adventurer.TurnRight();
                    Logger.Debug($"{adventurer.Name} tourne à droite - Nouvelle orientation: {adventurer.Orientation}");
                    break;
            }
        }

        private void TryMoveForward(Adventurer adventurer)
        {
            var currentPosition = adventurer.Position;
            var nextPosition = adventurer.GetNextPosition();

            if (IsValidMove(nextPosition))
            {
                _map.MoveAdventurer(currentPosition, nextPosition);
                adventurer.MoveTo(nextPosition);
                Logger.Info($"{adventurer.Name} avance de ({currentPosition.X},{currentPosition.Y}) vers ({nextPosition.X},{nextPosition.Y})");
                TryCollectTreasure(adventurer);
            }
            else
            {
                Logger.Debug($"{adventurer.Name} ne peut pas avancer vers ({nextPosition.X},{nextPosition.Y}) - Mouvement bloqué");
            }
        }

        private bool IsValidMove(Position position)
        {
            if (position.X < 0 || position.X >= _map.Width ||
                position.Y < 0 || position.Y >= _map.Height)
            {
                Logger.Debug($"Position ({position.X},{position.Y}) hors des limites de la carte");
                return false;
            }

            var cell = _map.GetCell(position);
            return cell.Type != CellType.Mountain && cell.Adventurer == null;
        }

        private void TryCollectTreasure(Adventurer adventurer)
        {
            var cell = _map.GetCell(adventurer.Position);
            if (cell.Type == CellType.Treasure && cell.CollectTreasure())
            {
                adventurer.CollectTreasure();
                Logger.Info($"{adventurer.Name} collecte un trésor en ({adventurer.Position.X},{adventurer.Position.Y}) - Total: {adventurer.CollectedTreasures}");
            }
        }

        private void LogFinalState()
        {
            Logger.Info("État final de la simulation :");
            foreach (var adventurer in _adventurers)
            {
                Logger.Info($"Aventurier {adventurer.Name}: Position finale ({adventurer.Position.X},{adventurer.Position.Y}), " +
                          $"Orientation {adventurer.Orientation}, Trésors collectés: {adventurer.CollectedTreasures}");
            }
        }
    }
}