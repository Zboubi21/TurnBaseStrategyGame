﻿using System;
using UnityEngine;
using NaughtyAttributes;

public enum RangeSearchType { RectangleByGridPosition, RectangleByMovement }

[Serializable]
public class RangeParameters
{
    // Constructor
    public RangeParameters(RangeParameters param)
    {
        m_RangeSearchType = param.m_RangeSearchType;
        m_SquareRange = param.m_SquareRange;
        m_MinReach = param.m_MinReach;
        m_MaxReach = param.m_MaxReach;
        m_UnOccupiedTilesOnly = param.m_UnOccupiedTilesOnly;
        m_IgnoreTilesHeight = param.m_IgnoreTilesHeight;
        m_IncludeStartingTile = param.m_IncludeStartingTile;
    }

    // Variables
    [Header("Type")]
    [SerializeField] private RangeSearchType m_RangeSearchType = RangeSearchType.RectangleByGridPosition;

    [ShowIf("SearchTypeIsRectByGridPosition")]
    [SerializeField] private bool m_SquareRange = false;

    [Header("Reach")]
    [SerializeField] private int m_MinReach = 0;
    [SerializeField] private int m_MaxReach = 3;

    [Header("Tile Settings")]
    [SerializeField] private bool m_UnOccupiedTilesOnly = true;
    [SerializeField] private bool m_IgnoreTilesHeight = false;
    [SerializeField] private bool m_IncludeStartingTile = false;

    // Getters
    public RangeSearchType RangeSearchType => m_RangeSearchType;
    public bool SquareRange => m_SquareRange;
    public int MinReach => m_MinReach;
    public int MaxReach { get => m_MaxReach; set => m_MaxReach = value; }
    public bool UnOccupiedTilesOnly => m_UnOccupiedTilesOnly;
    public bool IgnoreTilesHeight => m_IgnoreTilesHeight;
    public bool IncludeStartingTile => m_IncludeStartingTile;
}