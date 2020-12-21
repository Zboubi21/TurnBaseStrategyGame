using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

using Sirenix.OdinInspector;

namespace TBSG.Combat
{
    public class CharacterController : SerializedMonoBehaviour
    {
        public event Action OnLaunchSpell;

        protected Character m_Character; 
        public Character Character => m_Character;

        protected bool m_InMovementState = true;
        protected SpellParameters m_CurrentSpell;
        protected bool m_IsMyTurn = false;

        [Header("Debug")]
        [SerializeField] private Dictionary<SpellParameters, int> m_ThrowedPerTurnSpells = new Dictionary<SpellParameters, int>();
        [SerializeField] private Dictionary<SpellParameters, List<Entity>> m_TargetingPerOpponentSpells = new Dictionary<SpellParameters, List<Entity>>();
        [SerializeField] private Dictionary<SpellParameters, int> m_TurnsBetweenThrowsSpells = new Dictionary<SpellParameters, int>();

        // Getters
        public Dictionary<SpellParameters, int> ThrowedPerTurnSpells => m_ThrowedPerTurnSpells;
        public Dictionary<SpellParameters, List<Entity>> TargetingPerOpponentSpells => m_TargetingPerOpponentSpells;
        public Dictionary<SpellParameters, int> TurnsBetweenThrowsSpells => m_TurnsBetweenThrowsSpells;

        protected virtual void Awake()
        {
            m_Character = GetComponent<Character>();
            CombatManager.OnCombatEnd += OnCombatEnd;
        }

        public virtual void StartCharacterTurn()
        {
            m_InMovementState = true;
            m_Character.NewTurn();
            ResetThrowedPerTurnSpells();
            ResetTargetOpponentsSpells();
            ResetTurnsBetweenThrowsSpell();
            m_IsMyTurn = true;
        }
        public virtual void EndCharacterTurn()
        {
            m_Character.UnHighlightTiles();
            m_IsMyTurn = false;
        }

        private void OnCombatEnd(bool win)
        {
            if (win && m_IsMyTurn)
                EndCharacterTurn();
        }

        protected void MoveCharacter(GridTile targetTile)
        {
            if (m_Character.CanMoveToTile(targetTile))
                m_Character.MoveToTile(targetTile, OnCharacterReachedTargetPos); 
        }

        public virtual bool CanLaunchSpell(SpellParameters spell)
        {
            return CanThrowedPerTurnSpell(spell) && 
                CanThrowedTurnsBetweenThrowsSpell(spell) &&
                m_Character.HasEnoughActionPoints(spell) &&
                IsItInTheRightState(spell);
        }

        protected virtual bool IsItInTheRightState(SpellParameters spell) { return true; }

        protected bool CanLaunchSpellOnTile(SpellParameters spell, GridTile targetTile)
        {
            return m_Character.CanAttackTile(targetTile) && 
                CanTargetingOpponentWithSpell(spell, CombatManager.Instance.GetEntityOnGridTile(targetTile));
        }

        private bool CanThrowedPerTurnSpell(SpellParameters spell)
        {
            if (spell.ThrowsPerTurnNbr == 0) return true;

            if (!m_ThrowedPerTurnSpells.ContainsKey(spell))
            {
                m_ThrowedPerTurnSpells.Add(spell, 0);
                return true;
            }
            else
            {
                foreach (KeyValuePair<SpellParameters, int> item in m_ThrowedPerTurnSpells)
                    if (item.Key == spell && item.Value < item.Key.ThrowsPerTurnNbr)
                        return true;
            }
            return false;
        }

        private bool CanTargetingOpponentWithSpell(SpellParameters spell, Entity entity)
        {
            if (spell.TargetingPerOpponentNbr == 0) return true;

            if (!m_TargetingPerOpponentSpells.ContainsKey(spell))
            {
                m_TargetingPerOpponentSpells.Add(spell, new List<Entity>());
                return true;
            }
            else
            {
                foreach (KeyValuePair<SpellParameters, List<Entity>> item in m_TargetingPerOpponentSpells)
                    if (item.Key == spell && GetTargetNbrs(item.Value, entity) < item.Key.TargetingPerOpponentNbr)
                        return true;
            }
            return false;
        }

        private bool CanThrowedTurnsBetweenThrowsSpell(SpellParameters spell)
        {
            if (spell.TurnsBetweenThrowsNbr == 0) return true;

            if (!m_TurnsBetweenThrowsSpells.ContainsKey(spell))
            {
                m_TurnsBetweenThrowsSpells.Add(spell, 0);
                return true;
            }
            else
            {
                foreach (KeyValuePair<SpellParameters, int> item in m_TurnsBetweenThrowsSpells)
                    if (item.Key == spell && item.Value == 0)
                        return true;
            }
            return false;
        }

        private int GetTargetNbrs(List<Entity> entities, Entity entity)
        {
            int entityNbr = 0;
            if (entities != null && entities.Count > 0)
                for (int i = 0, l = entities.Count; i < l; ++i)
                    if (entities[i] == entity)
                        entityNbr ++;
            return entityNbr;
        }

        protected virtual void LaunchSpell(SpellParameters spell, GridTile gridTile)
        {
            switch (spell.AttackType)
            {
                case AttackType.Damage:
                    LaunchAttack(spell, gridTile);
                break;

                case AttackType.Invocation:
                    SpawnObjectOnTile(spell, spell.ObjectToSpawn, gridTile);
                break;

                case AttackType.StateChange:
                break;
                
                case AttackType.Teleportation:
                    TeleportationSpellOnTile(spell, gridTile);
                break;
            }
        }

        private void AddThrowedPerTurnSpells(SpellParameters spell)
        {
            if (m_ThrowedPerTurnSpells.ContainsKey(spell))
                m_ThrowedPerTurnSpells[spell] ++;
        }
        private void ResetThrowedPerTurnSpells()
        {
            foreach (KeyValuePair<SpellParameters, int> item in m_ThrowedPerTurnSpells.ToList())
                m_ThrowedPerTurnSpells[item.Key] = 0;
        }

        private void AddTargetOpponentSpells(SpellParameters spell, Entity target)
        {
            if (m_TargetingPerOpponentSpells.ContainsKey(spell))
                m_TargetingPerOpponentSpells[spell].Add(target);
        }
        private void ResetTargetOpponentsSpells()
        {
            foreach (KeyValuePair<SpellParameters, List<Entity>> item in m_TargetingPerOpponentSpells.ToList())
                m_TargetingPerOpponentSpells[item.Key].Clear();
        }

        private void AddTurnsBetweenThrowsSpell(SpellParameters spell)
        {
            if (m_TurnsBetweenThrowsSpells.ContainsKey(spell))
                m_TurnsBetweenThrowsSpells[spell] = spell.TurnsBetweenThrowsNbr + 1;
        }
        private void ResetTurnsBetweenThrowsSpell()
        {
            foreach (KeyValuePair<SpellParameters, int> item in m_TurnsBetweenThrowsSpells.ToList())
                if (item.Value > 0)
                    m_TurnsBetweenThrowsSpells[item.Key] --;
        }

        private void LaunchAttack(SpellParameters spell, GridTile gridTile)
        {
            GridObject gridObject = GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition);
            if (gridObject && gridObject.TryGetComponent(out Entity entity))
            {
                entity.TakeDamage(spell.Damages);
                AddTargetOpponentSpells(spell, entity);
            }
            OnLaunchedSpell(spell);
        }
        private void SpawnObjectOnTile(SpellParameters spell, GameObject obj, GridTile gridTile)
        {
            if (!gridTile.m_IsTileFlyable)
            {
                GridObject targetPosGridObject = GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition);
                if (!targetPosGridObject)
                {
                    GridObject gridObject = obj.GetComponent<GridObject>();
                    GridManager.Instance.InstantiateGridObject(gridObject, gridTile.m_GridPosition);
                }
                else
                {
                    GridObject instantiatedGridObject = Instantiate(obj, targetPosGridObject.transform.position, Quaternion.identity).GetComponent<GridObject>();
                    Entity targetEntity = CombatManager.Instance.GetEntityOnGridTile(gridTile);
                    if (targetEntity)
                        AddTargetOpponentSpells(spell, targetEntity);
                }
                OnLaunchedSpell(spell);
            }
        }
        private void TeleportationSpellOnTile(SpellParameters spell, GridTile gridTile)
        {
            GridObject targetGridObject = GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition);
            if (!targetGridObject)
                m_Character.TeleportOnTile(gridTile);
            else
                m_Character.SwapPositionWithGridObject(targetGridObject);
            OnLaunchedSpell(spell);
        }
        protected void OnLaunchedSpell(SpellParameters spell)
        {
            m_Character.SpendActionPoints(spell.ActionPoints);

            OnResetSpell();

            AddThrowedPerTurnSpells(spell);
            AddTurnsBetweenThrowsSpell(spell);
            OnLaunchSpell?.Invoke();
        }

        protected void OnResetSpell()
        {
            m_InMovementState = true;
            m_Character.ClearAttackRange();
            m_Character.CalculateMovementRange(true);
            m_CurrentSpell = null;
        }

        protected virtual void OnCharacterReachedTargetPos()
        {
            m_Character.CalculateMovementRange(true);
        }
    }
}