using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Defines
{
    //laser types
    public enum LaserTypes
    {
        Normal,
        Reflect
    }

    public enum UnitType
    {
        NormalMirror,
        HealerMirror,
        Troop_1,
        Troop_2,
        Jammer,
        none
    }

    //to check mirror types
    public static bool IsMirrorType(UnitType type)
    {
        return (type == UnitType.NormalMirror || type == UnitType.HealerMirror) ? true : false;
    }

}


