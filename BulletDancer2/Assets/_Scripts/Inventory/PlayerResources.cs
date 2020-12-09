using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Inventory
{
    public class PlayerResources
    {
        public int Ore { get; private set; }
        public int Iron { get; private set; }
        public int Gold { get; private set; }

        public PlayerResources(int ore, int iron, int gold)
        {
            Ore = ore;
            Iron = iron;
            Gold = gold;
        }

        public bool CanPlayerCraft(int requiredOre, int requiredIron, int requiredGold)
        {
            if (Ore > requiredOre && Iron > requiredIron && Gold > requiredGold)
            {
                return true;
            }

            return false;
        }

        public void UseResources(int ore, int iron, int gold)
        {
            Ore -= ore;
            Iron -= iron;
            Gold -= gold;
        }
    }
}
