using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreasureMap.Core.Models
{
    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        private Cell[,] Grid { get; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new Cell[width, height];

            // Initialiser toutes les cases
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Grid[x, y] = new Cell();
        }

        public void AddMountain(Position position)
        {
            ValidatePosition(position);
            Grid[position.X, position.Y].ConvertToMountain();
        }

        public void AddTreasures(Position position, int count)
        {
            ValidatePosition(position);
            Grid[position.X, position.Y].AddTreasures(count);
        }

        public Cell GetCell(Position position)
        {
            ValidatePosition(position);
            return Grid[position.X, position.Y];
        }

        private void ValidatePosition(Position position)
        {
            if (position.X < 0 || position.X >= Width ||
                position.Y < 0 || position.Y >= Height)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(position),
                    "Position is outside the map boundaries"
                );
            }
        }

        public void AddAdventurer(Adventurer adventurer)
        {
            ValidatePosition(adventurer.Position);
            var cell = GetCell(adventurer.Position);
            cell.SetAdventurer(adventurer);
        }

        public void MoveAdventurer(Position from, Position to)
        {
            ValidatePosition(from);
            ValidatePosition(to);

            var adventurer = GetCell(from).Adventurer;
            if (adventurer == null)
                throw new InvalidOperationException("No adventurer at source position");

            GetCell(from).SetAdventurer(null);      // Libérer l'ancienne position
            GetCell(to).SetAdventurer(adventurer);  // Occuper la nouvelle position
        }

        public bool IsPositionOccupied(Position position)
        {
            return GetCell(position).Adventurer != null;
        }
    }
}
