using TreasureMap.Core.Models;

namespace TreasureMap.Core.Interfaces
{
    public interface IMapWriter
    {
        string GenerateOutput(Map map, List<Adventurer> adventurers);
    }
}
