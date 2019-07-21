using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class UnityBoard : MonoBehaviour, IBoard
{
    private ClickBoard clickBoard;
    private int columns;
    private int rows;

    public GameObject[] defaultTiles;
    public GameObject[] wallTiles;
    public GameObject[] floorTiles;
    public GameObject[] unitTiles;
    public GameObject player;

    public GameObject onClickEff;

    private Transform boardHolder;
    private GameObject clickObject;

    private GameObject[,] mapTiles = null;

    private Dictionary<int, GameObject> unitTable = new Dictionary<int, GameObject>();

    private GameObject getGameObject(TileType tile)
    {
        if(tile == TileType.WALL)
        {
            return wallTiles[Random.Range(0, wallTiles.Length)];
        }
        else if (tile == TileType.FLOOR)
        {
            return floorTiles[Random.Range(0, floorTiles.Length)];
        }

        return defaultTiles[Random.Range(0, defaultTiles.Length)];
    }

    public void Initialize(LogicManager manager, int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        clickBoard = gameObject.AddComponent<ClickBoard>();
        clickBoard.Manager = manager;

        boardHolder = new GameObject("Borad").transform;
        mapTiles = new GameObject[columns, rows];

        if (onClickEff != null)
        {
            clickObject = Instantiate(onClickEff);
            clickObject.transform.SetParent(boardHolder);
        }

    }

    private GameObject newTileObject(UnityEngine.Object original, int x, int y)
    {
        GameObject obj = Instantiate(original, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
        /*TileActor actor = obj.GetComponent<TileActor>();
        if (actor != null)
        {
            actor.Column = x;
            actor.Row = y;
        } else
        {
            DebugLogger.Log("tile actor is null. maybe original has not script => " + original.ToString());
        }*/
        return obj;
    }

    /// <summary>
    /// 특정 타일을 해당하는 prefep으로 갱신
    /// 기존의 타일이 있으면 새로 만든다. (개선여지있음)
    /// </summary>
    public void SetTile(TileType tile, int x, int y)
    {
        GameObject tileObject = getGameObject(tile);
        if (x < 0 || x >= columns || y < 0 || y >= rows)
        {
            throw new Exception("setTile 에러. 범위이탈. x:" + x + ", y:" + y);
        }

        GameObject prevTile = mapTiles[x, y];
        if(prevTile == null)
        {
            prevTile = newTileObject(tileObject, x, y);
            mapTiles[x, y] = prevTile;
        }
        else if(prevTile != null)
        {
            Destroy(prevTile);
            prevTile = newTileObject(tileObject, x, y);
            mapTiles[x, y] = prevTile;
            //FIXME 추후에는 따로 Destroy하지않는 방법을 찾기
        }
        prevTile.transform.SetParent(boardHolder);
    }

    public void CreateUnit(int seq, int x, int y)
    {
        GameObject playerObj = Instantiate(player, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

        unitTable.Add(seq, playerObj);

        playerObj.transform.SetParent(boardHolder);
    }


    public void MovingUnit(int seq, int x, int y)
    {
        PlayerActor actor = unitTable[seq].GetComponent<PlayerActor>();

        if(actor != null)
        {
            actor.Move(x, y);
        }
        else
        {

            DebugLogger.Log("WARNING) no actor unit seq[{0}] pos[{1},{2}]", seq, x, y);
        }
    }


    public void MouseDown(Vector2 pos)
    {
        if (clickObject != null)
        {
            clickObject.transform.position = pos;
            ParticleSystem[] systems = clickObject.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem system in systems)
            {
                if(system.CompareTag("onClick"))
                {
                    system.Play();
                }
                else if (system.CompareTag("onDrag"))
                {
                    system.Play();
                }
            }
        }
    }

    public void MouseDrag(Vector2 pos)
    {
        if (clickObject != null)
        {
            clickObject.transform.position = pos;
        }
    }

    public void MouseUp(Vector2 pos)
    {
        if (clickObject != null)
        {
            ParticleSystem[] systems = clickObject.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem system in systems)
            {
                if (system.CompareTag("onDrag"))
                {
                    system.Stop();
                }
            }
        }
    }


}
