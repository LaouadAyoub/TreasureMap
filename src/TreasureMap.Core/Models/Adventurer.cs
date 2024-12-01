using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureMap.Core.Enums;

namespace TreasureMap.Core.Models
{
    public class Adventurer
    {
        public string Name { get; }
        public Position Position { get; private set; }
        public Orientation Orientation { get; private set; }
        public int CollectedTreasures { get; private set; }
        private Queue<MovementType> MovementSequence { get; }

        public Adventurer(string name, Position position, Orientation orientation, string movements)
        {
            Name = name;
            Position = position;
            Orientation = orientation;
            CollectedTreasures = 0;
            MovementSequence = ParseMovements(movements);
        }

        // Cette méthode regarde le prochain mouvement sans le retirer
        public MovementType? PeekNextMovement()
        {
            return MovementSequence.Count > 0 ? MovementSequence.Peek() : null;
        }

        // Cette méthode consomme explicitement le mouvement
        public void ConsumeCurrentMovement()
        {
            if (MovementSequence.Count > 0)
            {
                MovementSequence.Dequeue();
            }
        }
        // Méthode pour calculer la nouvelle position après un mouvement en avant
        public Position GetNextPosition()
        {
            return Orientation switch
            {
                Orientation.North => new Position(Position.X, Position.Y - 1),
                Orientation.South => new Position(Position.X, Position.Y + 1),
                Orientation.East => new Position(Position.X + 1, Position.Y),
                Orientation.West => new Position(Position.X - 1, Position.Y),
                _ => throw new InvalidOperationException("Invalid orientation")
            };
        }

    // Méthode pour tourner à gauche
    public void TurnLeft()
    {
        Orientation = Orientation switch
        {
            Orientation.North => Orientation.West,
            Orientation.West => Orientation.South,
            Orientation.South => Orientation.East,
            Orientation.East => Orientation.North,
            _ => throw new InvalidOperationException("Invalid orientation")
        };
    }

    // Méthode pour tourner à droite
    public void TurnRight()
    {
        Orientation = Orientation switch
        {
            Orientation.North => Orientation.East,
            Orientation.East => Orientation.South,
            Orientation.South => Orientation.West,
            Orientation.West => Orientation.North,
            _ => throw new InvalidOperationException("Invalid orientation")
        };
    }

    // Méthode pour se déplacer à une nouvelle position
    public void MoveTo(Position newPosition)
    {
        Position = newPosition;
    }

    // Méthode pour collecter un trésor
    public void CollectTreasure()
    {
        CollectedTreasures++;
    }

    // Méthode privée pour parser la séquence de mouvements
    private static Queue<MovementType> ParseMovements(string movements)
    {
        var sequence = new Queue<MovementType>();
        foreach (char movement in movements.ToUpper())
        {
            sequence.Enqueue(movement switch
            {
                'A' => MovementType.Forward,
                'G' => MovementType.TurnLeft,
                'D' => MovementType.TurnRight,
                _ => throw new ArgumentException($"Invalid movement character: {movement}")
            });
        }
        return sequence;
    }
}
}
