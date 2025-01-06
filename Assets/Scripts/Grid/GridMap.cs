using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    [HideInInspector]
    public int _Height;
    [HideInInspector]
    public int _Length;

    private bool[,] grid;

    public void init(int height,int length)
    {
        grid = new bool[height,length];
        this._Height = height; 
        this._Length = length;
    }

    public void Set(int height,int length,bool state)
    {
        if(CheckGridPosition(height, length) == false) { return; }
        grid[height,length] = state;
    }

    public bool Get(int height, int length) 
    {
        if (CheckGridPosition(height, length) == false) { return false; }
        return grid[height, length];
    }

    public bool CheckGridPosition(int height,int length)
    {
        if (height < 0 || height >= this._Height) { return false; }
        if (length < 0 || length >= this._Length) { return false; }
        return true;
    }
}
