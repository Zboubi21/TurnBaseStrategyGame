using System.Collections.Generic;
using UnityEngine;
using System;

namespace TBSG.Combat
{
    [RequireComponent(typeof(CharacterPathWalker))]
    [RequireComponent(typeof(GridObject))]
    public class Character : MonoBehaviour
    {
        [Header("Movement")]
        public RangeParameters m_MovementRangeParameters;

        private List<GridTile> m_CurrentMovementRange = new List<GridTile>();
        private CharacterPathWalker m_PathWalker;
        private GridObject m_GridObject;
        private GridMovement m_GridMovement;
        private int m_CurrentMouvementPoint;

        private void Awake()
        {
            m_GridObject = GetComponent<GridObject>();
            m_PathWalker = GetComponent<CharacterPathWalker>();
            m_GridMovement = GetComponent<GridMovement>();
        }

        public void CalculateMovementRange(bool andHighlight = false)
        {
            if (m_CurrentMouvementPoint <= 0) return;
            RangeParameters rangeParam = new RangeParameters(m_MovementRangeParameters);
            rangeParam.m_MaxReach = m_CurrentMouvementPoint;
            m_CurrentMovementRange = RangeAlgorithms.SearchByParameters(m_GridObject.m_CurrentGridTile, rangeParam, m_PathWalker.m_CanFly);
            if (andHighlight)
                HighlightMovementRange();
        }

        public void HighlightMovementRange()
        {
            if (m_CurrentMovementRange != null && m_CurrentMovementRange.Count > 0)
                HighlightManager.Instance.HighlighTiles(m_CurrentMovementRange);
        }

        // Wether or not this unit is able to move to a target tile (based on wether or not it is inside the range)
        public bool CanMoveToTile(GridTile targetTile)
        {
            return (m_CurrentMovementRange.Contains(targetTile) && targetTile != m_GridObject.m_CurrentGridTile);
        }

        public void MoveToTile(GridTile targetTile, Action onMovementEndCallback)
        {
            HighlightManager.Instance.UnHighlightTiles();
            m_PathWalker.DeterminePath(targetTile, true, () => { onMovementEndCallback(); });
            ClearMovementRange();
            m_CurrentMouvementPoint -= m_PathWalker.m_Path.Count;
        }

        public void ClearMovementRange()
        {
            m_CurrentMovementRange.Clear();
        }

        public void NewTurn()
        {
            m_CurrentMouvementPoint = m_MovementRangeParameters.m_MaxReach;
            CalculateMovementRange(true);
        }
    }
}