using TreasureMap.Core.Models;

namespace TreasureMap.Core.Interfaces
{
    public interface IGameEngine
    {
        void RunSimulation(Map map, List<Adventurer> adventurers);
    }

}
