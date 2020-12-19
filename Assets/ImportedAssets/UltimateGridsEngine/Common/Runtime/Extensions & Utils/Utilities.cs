using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    // Manhattan distance used for rectangle grids
    public static float Heuristic(GridTile a, GridTile b)
    {
        return Mathf.Abs(a.m_GridPosition.x - b.m_GridPosition.x) + Mathf.Abs(a.m_GridPosition.y - b.m_GridPosition.y);
    }

    public static Color ColorFromRGB(int r, int g, int b)
    {
        return new Color((float)r / 256, (float)g / 256, (float)b / 256);
    }
}
