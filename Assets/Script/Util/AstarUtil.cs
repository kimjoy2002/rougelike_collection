using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using static LogicManager;
using static TileType;

public class AstartUtil
{
    private static int column = 0;
    private static int row = 0;
    private static Searchnode[,] astar_node;




    private class Searchnode
    {
        public CoordDef pos = new CoordDef(0, 0);
        public int cfs = 0; //cost from start
        public int ctg = 0; //cost to goal
        public int tc = 0; //토탈 코스트
        public int opcl = 0; //0은 없음 1은 오픈 2는 클로즈
        public Searchnode parent = null;
        public Searchnode()
        {
            this.pos = new CoordDef(0, 0);
            this.cfs = 0;
            this.ctg = 0;
            this.tc = 0;
            this.opcl = 0;
            this.parent = null;
        }

        public Searchnode(CoordDef pos, int cfs, int ctg, Searchnode parent)
        {
            this.pos = pos;
            this.cfs = cfs;
            this.ctg = ctg;
            this.tc = cfs + ctg;
            this.parent = parent;
        }

        public Searchnode Set(CoordDef pos, int cfs, int ctg, Searchnode parent)
        {
            this.pos = pos;
            this.cfs = cfs;
            this.ctg = ctg;
            this.tc = cfs + ctg;
            this.parent = parent;
            return this;
        }
    }



    [Flags]
    public enum MOVE_TYPE
    {
        NORMAL_MOVE = 1 << 0
    }


    public static bool PathSearch(CoordDef start, CoordDef goal, ref Stack<CoordDef> will_move, TileWrraper[,] map, int columns, int rows, MOVE_TYPE type)
    {
        if(column != columns || row != rows)
        {
            column = columns;
            row = rows;
            astar_node = new Searchnode[columns, rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    astar_node[x, y] = new Searchnode();
                }
            }
        }



        will_move = new Stack<CoordDef>();
        CoordDef ano_goal = start;
        bool is_move = !map[goal.X, goal.Y].HasFlag(TILE_FLAG.NONE_MOVE);
        if(!is_move)
        {
            return false;
        }

        int heuristic = 5;
        List<Searchnode> open = new List<Searchnode>();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                astar_node[x,y].pos = new CoordDef(0, 0);
                astar_node[x,y].cfs = 0;
                astar_node[x,y].ctg = 0;
                astar_node[x,y].tc = 0;
                astar_node[x, y].opcl = 0;
                astar_node[x,y].parent = null;
            }
        }
        astar_node.Initialize();

        priqueusPush(open, astar_node[start.X, start.Y].Set(start, 0, pathCost(start, goal), null));

        while(open.Count > 0)
        {
            Searchnode node = open[0];
            open.RemoveAt(0);
            node.opcl &= ~1;
            node.opcl |= 2;
            if(node.pos == goal)
            {
                Searchnode path = node;
                int i = 30;
                DebugLogger.Log("start position [{0},{1}]", start.X, start.Y);
                while (path != null && path.pos != start)
                {
                    if(path.parent != null)
                        DebugLogger.Log("test willmove [{0},{1}] parent[{2},{3}]", path.pos.X, path.pos.Y, path.parent.pos.X, path.parent.pos.Y);
                    else
                        DebugLogger.Log("test willmove [{0},{1}] ", path.pos.X, path.pos.Y);
                    will_move.Push(path.pos);
                    path = path.parent;
                    i--;
                    if (i <= 0)
                        break;
                }
                DebugLogger.Log("goal position [{0},{1}]", goal.X, goal.Y);
                return true;
            }
            else
            {
                //rect_iterator로 수정하기

                for (int x_ = -1; x_ <= 1; x_++)
                {
                    for (int y_ = -1; y_ <= 1; y_++)
                    {
                        if (x_ == 0 && y_ == 0)
                            continue;
                        CoordDef it = new CoordDef(node.pos.X + x_, node.pos.Y + y_);
                        Searchnode newnode = astar_node[it.X, it.Y];
                        int newcost = node.cfs + 1;
                        bool is_open = (newnode.opcl & 1) != 0;
                        bool is_close = (newnode.opcl & 2) != 0;
                        bool is_move_node = !map[it.X, it.Y].HasFlag(TILE_FLAG.NONE_MOVE);
                        DebugLogger.Log("newnode.cfs{0}, newcost{1}", newnode.cfs, newcost);
                        if (!is_move_node)
                        {
                            continue;
                        }
                        else if ((is_open || is_close) && (newnode.cfs <= newcost))
                        {
                            continue;
                        }

                        DebugLogger.Log("set [{0},{1}] cost[{2}] parent[{3},{4}]", it.X, it.Y, newcost, node.pos.X, node.pos.Y);
                        newnode.Set(new CoordDef(it.X, it.Y), newcost, heuristic * pathCost(it, goal), node);
                        priqueusPush(open, newnode);
                    }
                }
            }
        }
        return false;
    }


    private static void priqueusPush(List<Searchnode> queue, Searchnode data)
    {
        if(queue.Count != 0)
        {
            int i = 0;
            foreach (Searchnode node in queue)
            {
                if (node.tc > data.tc)
                {
                    queue.Insert(i, data);
                    data.opcl |= 1;
                    break;
                }
                i++;
            }
        }
        else
        {
            queue.Insert(0, data);
            data.opcl |= 1;
        }
    }


    private static int pathCost(CoordDef start, CoordDef goal)
    {
	    return Math.Abs(start.X - goal.X)+ Math.Abs(start.Y - goal.Y);
    }
}