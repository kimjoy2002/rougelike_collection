using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using static TileType;
using static AstartUtil;

/// <summary>
/// 게임에 대한 로직이 우러지는 곳
/// 이 곳에서는 UI나 그래픽적인 일은 하지않아야 함
/// 
/// </summary>
public class LogicManager : MonoBehaviour
{
    public class TileWrraper
    {
        public TileType Tile { get; set; }

        public TileWrraper(TileType tile) => (Tile) = (tile);

        public bool HasFlag(Enum flag)
        {
           return Tile.Flags.HasFlag(flag);
        }
    }

    public IBoard Board { get; set; }

    public int columns = 16;
    public int rows = 16;
    public int waitMove = 0;

    private TileWrraper[,] mapTiles = null;

    private List<Unit> unitList = new List<Unit>();
    private Unit player = null;
    private Stack<CoordDef> will_move;

    /// <summary>
    /// 새로운 보드매니저로 갱신
    /// </summary>
    public void InitializeTile()
    {
        Board.Initialize(this, columns, rows);

        DebugLogger.Log("InitializeList. columns:" + columns + ", rows:" + rows);
        if (mapTiles != null)
        {
            mapTiles = null;
        }
        mapTiles = new TileWrraper[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                TileType tile = TileType.FLOOR;
                if (x == 0 || x == columns - 1 || y == 0 || y == rows - 1)
                {
                    tile = TileType.WALL;
                }

                if (UnityEngine.Random.Range(0, 2) < 1)
                {
                    tile = TileType.WALL;
                }

                if (x == 5 && y == 5)
                {
                    tile = TileType.FLOOR;
                }

                mapTiles[x, y] = new TileWrraper(tile);
                Board.SetTile(tile, x, y);
            }
        }
        player = CreateUnit(new Unit(5, 5));
    }

    public Unit CreateUnit(Unit unit)
    {
        unitList.Add(unit);
        Board.CreateUnit(unit.getSeqence(), unit.X, unit.Y);

        return unit;
    }

    public void ClickTile(int x, int y)
    {
        if (waitMove == 0)
        {
            DebugLogger.Log("ClickTile [{0},{1}]", x, y);

            if (isTileCanMove(x, y))
            {
                CoordDef newPos = new CoordDef(x, y);

                bool moveOk = AstartUtil.PathSearch(player.getPos(), newPos, ref will_move, mapTiles, columns, rows, MOVE_TYPE.NORMAL_MOVE);

                if (moveOk)
                {
                    EndTurn();
                }
            }
        }
    }

    public void EndTurn()
    {
        if ( will_move.Count > 0)
        {
            CoordDef pos = will_move.Pop();
            Board.MovingUnit(player.getSeqence(), pos.X, pos.Y);
            waitMove++;
            player.Pos = pos;
        }
    }

    public void ReturnCommand()
    {
        waitMove--;
        if (waitMove == 0)
        {
            EndTurn();
        }
    }


    private bool isTileOuter(int x, int y)
    {
        if(x < 0 || x >= columns || y < 0 || y >= rows)
        {
            return true;
        }
        return false;
    }

    private bool isTileCanMove(int x, int y)
    {
        if (isTileOuter(x, y))
            return false;

        if(mapTiles[x, y].HasFlag(TILE_FLAG.NONE_MOVE))
        {
            return false;
        }
        return true;
    }




    public void MouseDown(Vector2 pos)
    {
        Board.MouseDown(pos);
    }
    public void MouseDrag(Vector2 pos)
    {
        Board.MouseDrag(pos);
    }
    public void MouseUp(Vector2 pos)
    {
        Board.MouseUp(pos);
    }
}