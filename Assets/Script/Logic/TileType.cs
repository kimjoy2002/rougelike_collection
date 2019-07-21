using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class TileType
{
    [Flags]
    public enum TILE_FLAG
    {
        NONE_MOVE = 1 << 0
    }

    public static readonly TileType WALL = new TileType("wall", TILE_FLAG.NONE_MOVE);
    public static readonly TileType FLOOR = new TileType("floor", 0);

    public static IEnumerable<TileType> Values
    {
        get
        {
            yield return WALL;
            yield return FLOOR;
        }
    }

    public string Name { get; private set; }
    public TILE_FLAG Flags { get; private set; }

    TileType(string name, TILE_FLAG flags) => (Name, Flags) = (name, flags);

    public override string ToString() => Name;
}