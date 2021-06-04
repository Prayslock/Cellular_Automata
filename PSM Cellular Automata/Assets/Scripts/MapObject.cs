using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject  {

    public enum Type
    {
        Empty,
        Mine,
        Num_1,
        MineNum_2,
        MineNum_3,
        MineNum_4,
        MineNum_5,
        MineNum_6,
        MineNum_7,
        MineNum_8,
    }



    private CraftGrid<MapObject> grid;
    private int x;
    private int y;
    private Type type;
    private bool isRevealed;
    private bool isFlagged;

    public MapObject(CraftGrid<MapObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        type = Type.Empty;
        isRevealed = false;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public Type GetGridType()
    {
        return type;
    }

    public void SetGridType(Type type)
    {
        this.type = type;
        grid.TriggerGridObjectChanged(x, y);
        if (grid.showDebug)
        {
            grid.debugTextArray[x, y].text = grid.gridArray[x, y].ToString();
        }
    }

    public void SetFlagged()
    {
        isFlagged = true;
        grid.TriggerGridObjectChanged(x, y);

    }

    public bool IsFlagged()
    {
        return isFlagged;
    }

    public void Reveal()
    {
        isRevealed = true;
        grid.TriggerGridObjectChanged(x, y);
    }

    public bool IsRevealed()
    {
        return isRevealed;
    }

    public override string ToString()
    {
        return type.ToString();
    }

}
