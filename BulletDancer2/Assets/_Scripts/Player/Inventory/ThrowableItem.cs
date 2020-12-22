using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Player.Inventory
{
    public interface ThrowableItem
    {
        void Throw(float speed, Vector2 itemDirection, Vector2 playerPosition);
        void SetButtonStatus(ThrowableItem throwableItem, bool isActive);
    }
}
