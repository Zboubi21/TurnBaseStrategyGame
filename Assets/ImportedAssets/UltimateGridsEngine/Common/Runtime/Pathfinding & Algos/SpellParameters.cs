using System;
using UnityEngine;

[Serializable]
public class SpellParameters
{
    // Variables
    [SerializeField] private int m_ActionPoints = 1;
    [SerializeField] private int m_Damages = 1;
    [SerializeField] private int m_ThrowsPerTurnNbr = 1;
    [SerializeField] private int m_TargetingPerOpponentNbr = 0;
    [SerializeField] private int m_TurnsBetweenThrowsNbr = 0;
    [SerializeField] private GameObject m_ObjectToSpawn = null;
    [SerializeField] private bool m_WalkableTiles = true;
    [SerializeField] private bool m_FlyableTiles = false;
    [SerializeField] private bool m_InStraightLine = false;
    [SerializeField] private bool m_CanTargetMountain = false;

    // Getters
    public int ActionPoints => m_ActionPoints;
    public int Damages => m_Damages;
    public int ThrowsPerTurnNbr => m_ThrowsPerTurnNbr;
    public int TargetingPerOpponentNbr => m_TargetingPerOpponentNbr;
    public int TurnsBetweenThrowsNbr => m_TurnsBetweenThrowsNbr;
    public GameObject ObjectToSpawn => m_ObjectToSpawn;
    public bool WalkableTiles => m_WalkableTiles;
    public bool FlyableTiles => m_FlyableTiles;
    public bool InStraightLine => m_InStraightLine;
    public bool CanTargetMountain => m_CanTargetMountain;
}