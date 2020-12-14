using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Inventory
{
    public class PlayerResources : MonoBehaviour
    {
        private int ore;
        private int iron;
        private int gold;
        public int Ore
        {
            get => ore;
            private set
            {
                ore = value;
                oreText.text = value.ToString();
            }
        }
        public int Iron
        {
            get => iron;
            private set
            {
                iron = value;
                ironText.text = value.ToString();
            }
        }
        public int Gold {
            get => gold;
            private set
            {
                gold = value;
                goldText.text = value.ToString();
            }
        }

        [SerializeField] Text oreText;
        [SerializeField] Text ironText;
        [SerializeField] Text goldText;


        public void SetPlayerResources(int ore, int iron, int gold)
        {
            Ore = ore;
            Iron = iron;
            Gold = gold;
        }

        public bool CanPlayerCraft(int requiredOre, int requiredIron, int requiredGold)
        {
            if (Ore >= requiredOre && Iron >= requiredIron && Gold >= requiredGold)
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
