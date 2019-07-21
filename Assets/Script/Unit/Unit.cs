using UnityEngine;
using UnityEditor;

public class Unit
{
    private static int increaseSeq;


    private int seq = increaseSeq++;
    private CoordDef pos;



    public Unit(int x, int y)
    {
        pos = new CoordDef(x, y);
    }

    public CoordDef Pos { get; set; }

    public int X { get { return pos.X; } set { pos.X = value; }}
    public int Y { get { return pos.Y; } set { pos.Y = value; }}


    public CoordDef getPos()
    {
        return pos;
    }

    public int getSeqence()
    {
        return seq;
    }


}