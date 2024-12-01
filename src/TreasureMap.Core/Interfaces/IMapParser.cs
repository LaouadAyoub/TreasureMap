using TreasureMap.Core.Models;

namespace TreasureMap.Core.Interfaces
{
    public interface IMapParser
    {
        (Map map, List<Adventurer> adventurers) ParseMap(string[] lines);
    }
}