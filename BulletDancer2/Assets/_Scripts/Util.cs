using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts
{
    public static class Util
    {
        public static DoorDirection GetOppositeDirection(this DoorDirection direction) =>
            direction switch
            {
                DoorDirection.Top => DoorDirection.Down,
                DoorDirection.Down => DoorDirection.Top,
                DoorDirection.Left => DoorDirection.Right,
                DoorDirection.Right => DoorDirection.Left
            };
	}
}

