
using System;

public struct CoordDef
{
    public CoordDef(int x, int y) => (X, Y) = (x, y);


    public int X { get; set; }
    public int Y { get; set; }


    public static bool operator==(CoordDef lf, CoordDef rf)
    {
        return (lf.X == rf.X && lf.Y == rf.Y);
    }
    public static bool operator!=(CoordDef lf, CoordDef rf)
    {
        return (lf.X != rf.X || lf.Y != rf.Y);
    }

    public override bool Equals(Object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            CoordDef p = (CoordDef)obj;
            return (X == p.X) && (Y == p.Y);
        }
    }
    public override int GetHashCode()
    {
        return (X << 2) ^ Y;
    }

}