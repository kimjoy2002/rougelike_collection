using UnityEngine;
using UnityEditor;

public interface IBoard
{
    void Initialize(LogicManager manager, int columns, int rows);

    //실제 게임 동작에 대한 API들
    void CreateUnit(int seq, int x, int y);
    void MovingUnit(int seq, int x, int y);
    void SetTile(TileType tile, int x, int y);

    //입력에 대한 API들
    void MouseDown(Vector2 pos);
    void MouseDrag(Vector2 pos);
    void MouseUp(Vector2 pos);
}