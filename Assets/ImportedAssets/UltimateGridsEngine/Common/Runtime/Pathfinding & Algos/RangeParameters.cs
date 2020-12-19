using System;
using UnityEngine;
using System.Collections;
using NaughtyAttributes;

public enum RangeSearchType { RectangleByGridPosition, RectangleByMovement }

[Serializable]
public class RangeParameters
{
    public RangeParameters(RangeParameters param)
    {
        m_RangeSearchType = param.m_RangeSearchType;
        m_SquareRange = param.m_SquareRange;
        m_MinReach = param.m_MinReach;
        m_MaxReach = param.m_MaxReach;
        m_WalkableTiles = param.m_WalkableTiles;
        m_FlyableTiles = param.m_FlyableTiles;
        m_UnOccupiedTilesOnly = param.m_UnOccupiedTilesOnly;
        m_IgnoreTilesHeight = param.m_IgnoreTilesHeight;
        m_IncludeStartingTile = param.m_IncludeStartingTile;
    }

    [Header("Type")]
    public RangeSearchType m_RangeSearchType = RangeSearchType.RectangleByGridPosition;
    [ShowIf("SearchTypeIsRectByGridPosition")]
    public bool m_SquareRange = false;
    public bool SearchTypeIsRectByGridPosition() { return m_RangeSearchType == RangeSearchType.RectangleByGridPosition; }

    [Header("Reach")]
    public int m_MinReach = 0;
    public int m_MaxReach = 3;

    [Header("Tile Settings")]
    public bool m_WalkableTiles = true;
    public bool m_FlyableTiles = false;
    public bool m_UnOccupiedTilesOnly = true;
    public bool m_IgnoreTilesHeight = false;
    public bool m_IncludeStartingTile = false;
}