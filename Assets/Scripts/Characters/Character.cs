using System.Collections.Generic;
using UnityEngine;
using System;

namespace TBSG.Combat
{
    [RequireComponent(typeof(CharacterPathWalker))]
    [RequireComponent(typeof(GridObject))]
    [RequireComponent(typeof(GridMovement))]
    public class Character : Entity
    {
        public static event Action<Character> OnCharacterDie;

        public event Action OnActionPointsChanged;
        public event Action OnMovementPointsChanged;
        
        [SerializeField] private int m_StartActionPoints = 6;
        [SerializeField] private RangeParameters m_MovementRangeParameters = null;
        [SerializeField] private CharacterTypes m_CharacterTypes = CharacterTypes.None;

        private List<GridTile> m_CurrentMovementRange = new List<GridTile>();
        private List<GridTile> m_CurrentAttackRange = new List<GridTile>();
        private CharacterPathWalker m_PathWalker;
        private GridObject m_GridObject;
        private GridMovement m_GridMovement;
        [NaughtyAttributes.ReadOnly, SerializeField] private int m_CurrentActionPoints, m_CurrentMouvementPoints;

        public int CurrentActionPoints => m_CurrentActionPoints;
        public int CurrentMouvementPoints => m_CurrentMouvementPoints;
        public CharacterTypes CharacterTypes => m_CharacterTypes;
        public CharacterPathWalker PathWalker => m_PathWalker;

        private void Awake()
        {
            Setup();
        }

        protected override void Start()
        {
            base.Start();
            m_GridObject = GetComponent<GridObject>();
            m_PathWalker = GetComponent<CharacterPathWalker>();
            m_GridMovement = GetComponent<GridMovement>();
        }

        private void Setup()
        {
            m_CurrentActionPoints = m_StartActionPoints;
            OnActionPointsChanged?.Invoke();
            m_CurrentMouvementPoints = m_MovementRangeParameters.MaxReach;
            OnMovementPointsChanged?.Invoke();
        }

        public void CalculateMovementRange(bool andHighlight = false)
        {
            if (m_CurrentMouvementPoints <= 0) return;
            RangeParameters rangeParam = new RangeParameters(m_MovementRangeParameters);
            rangeParam.MaxReach = m_CurrentMouvementPoints;
            m_CurrentMovementRange = RangeAlgorithms.SearchByMovement(m_GridObject.m_CurrentGridTile, rangeParam, m_PathWalker.m_CanFly);
            if (andHighlight)
                HighlightMovementRange(true);
        }
        public void CalculateAttackRange(Spell spell, bool andHighlight = false)
        {
            m_CurrentAttackRange = RangeAlgorithms.SearchBySpell(m_GridObject.m_CurrentGridTile, spell.Range, spell.SpellParameters);
            if (andHighlight)
                HighlightAttackRange(true);
        }

        public void HighlightMovementRange(bool unhighlightPrevious = false)
        {
            if (m_CurrentMovementRange != null && m_CurrentMovementRange.Count > 0)
                HighlightManager.Instance.HighlighTiles(m_CurrentMovementRange, 0, unhighlightPrevious);
        }
        public virtual void HighlightAttackRange(bool unhighlightPrevious = false)
        {
            if (m_CurrentAttackRange != null && m_CurrentAttackRange.Count > 0)
                HighlightManager.Instance.HighlighTiles(m_CurrentAttackRange, 1, unhighlightPrevious);
        }

        public void UnHighlightTiles()
        {
            HighlightManager.Instance.UnHighlightTiles();
        }

        public bool CanMoveToTile(GridTile targetTile)
        {
            return (m_CurrentMovementRange.Contains(targetTile) && targetTile != m_GridObject.m_CurrentGridTile);
        }
        public bool HasEnoughActionPoints(Spell spell)
        {
            return m_CurrentActionPoints >= spell.SpellParameters.ActionPoints;
        }
        public bool CanAttackTile(GridTile targetTile)
        {
            return (m_CurrentAttackRange.Contains(targetTile));
        }

        public void MoveToTile(GridTile targetTile, Action onMovementEndCallback)
        {
            m_PathWalker.DeterminePath(targetTile, true, () => { onMovementEndCallback(); });
            m_CurrentMouvementPoints -= m_PathWalker.m_Path.Count;
            ClearMovementRange();
            OnMovementPointsChanged?.Invoke();
        }
        public void TeleportOnTile(GridTile targetTile)
        {
            m_GridMovement.TryMoveTo(targetTile, false, false, false);
        }
        public void SwapPositionWithGridObject(GridObject targetObject)
        {
            m_GridMovement.SwapGridObjectsPositions(m_GridObject, targetObject);
        }
        public void SpendActionPoints(int ap)
        {
            m_CurrentActionPoints -= ap;
            OnActionPointsChanged?.Invoke();
        }

        public void ClearMovementRange()
        {
            HighlightManager.Instance.UnHighlightTiles();
            m_CurrentMovementRange.Clear();
        }
        public void ClearAttackRange()
        {
            HighlightManager.Instance.UnHighlightTiles();
            m_CurrentAttackRange.Clear();
        }

        public void NewTurn()
        {
            Setup();
            CalculateMovementRange(true);
        }

        protected override void OnTriggerDie()
        {
            base.OnTriggerDie();
            OnCharacterDie?.Invoke(this);
        }
    }
}