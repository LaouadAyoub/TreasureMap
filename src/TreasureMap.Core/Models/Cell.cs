using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreasureMap.Core.Enums;

namespace TreasureMap.Core.Models
{
    public class Cell
    {
        public CellType Type { get; private set; }
        public int TreasureCount { get; private set; }
        public Adventurer? Adventurer { get; private set; }  // Ajout de la référence à l'aventurier

        public Cell()
        {
            Type = CellType.Plain;
            TreasureCount = 0;
            Adventurer = null;
        }

        // Méthode pour ajouter un aventurier à la cellule
        public void SetAdventurer(Adventurer? adventurer)
        {
            if (Type == CellType.Mountain)
                throw new InvalidOperationException("Cannot place adventurer on a mountain");

            Adventurer = adventurer;
        }


        public void ConvertToMountain()
        {
            if (TreasureCount > 0)
                throw new InvalidOperationException("Cannot convert a cell with treasures to mountain");

            Type = CellType.Mountain;
        }

        public void AddTreasures(int count)
        {
            if (Type == CellType.Mountain)
                throw new InvalidOperationException("Cannot add treasures to a mountain");

            TreasureCount += count;
            if (TreasureCount > 0)
                Type = CellType.Treasure;
        }

        public bool CollectTreasure()
        {
            if (TreasureCount <= 0) return false;

            TreasureCount--;
            if (TreasureCount == 0)
                Type = CellType.Plain;  // La case redevient une plaine quand il n'y a plus de trésor

            return true;
        }
    }
}
