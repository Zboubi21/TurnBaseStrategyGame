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
        protected Spell m_CurrentSpell;
        protected bool m_IsMyTurn = false;

        [Header("Debug")]
        [SerializeField] private Dictionary<Spell, int> m_ThrowedPerTurnSpells = new Dictionary<Spell, int>();
        [SerializeField] private Dictionary<Spell, List<Entity>> m_TargetingPerOpponentSpells = new Dictionary<Spell, List<Entity>>();
        [SerializeField] private Dictionary<Spell, int> m_TurnsBetweenThrowsSpells = new Dictionary<Spell, int>();

        // Getters
        public Dictionary<Spell, int> ThrowedPerTurnSpells => m_ThrowedPerTurnSpells;
        public Dictionary<Spell, List<Entity>> TargetingPerOpponentSpells => m_TargetingPerOpponentSpells;
        public Dictionary<Spell, int> TurnsBetweenThrowsSpells => m_TurnsBetweenThrowsSpells;

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

        public virtual bool CanLaunchSpell(Spell spell)
        {
            return CanThrowedPerTurnSpell(spell) && 
                CanThrowedTurnsBetweenThrowsSpell(spell) &&
                m_Character.HasEnoughActionPoints(spell) &&
                IsItInTheRightState(spell);
        }

        protected virtual bool IsItInTheRightState(Spell spell) { return true; }

        protected bool CanLaunchSpellOnTile(Spell spell, GridTile targetTile)
        {
            return m_Character.CanAttackTile(targetTile) && 
                CanTargetingOpponentWithSpell(spell, CombatManager.Instance.GetEntityOnGridTile(targetTile));
        }

        private bool CanThrowedPerTurnSpell(Spell spell)
        {
            if (spell.SpellParameters.ThrowsPerTurnNbr == 0) return true;

            if (!m_ThrowedPerTurnSpells.ContainsKey(spell))
            {
                m_ThrowedPerTurnSpells.Add(spell, 0);
                return true;
            }
            else
            {
                foreach (KeyValuePair<Spell, int> item in m_ThrowedPerTurnSpells)
                    if (item.Key == spell && item.Value < item.Key.SpellParameters.ThrowsPerTurnNbr)
                        return true;
            }
            return false;
        }

        private bool CanTargetingOpponentWithSpell(Spell spell, Entity entity)
        {
            if (spell.SpellParameters.TargetingPerOpponentNbr == 0) return true;

            if (!m_TargetingPerOpponentSpells.ContainsKey(spell))
            {
                m_TargetingPerOpponentSpells.Add(spell, new List<Entity>());
                return true;
            }
            else
            {
                foreach (KeyValuePair<Spell, List<Entity>> item in m_TargetingPerOpponentSpells)
                    if (item.Key == spell && GetTargetNbrs(item.Value, entity) < item.Key.SpellParameters.TargetingPerOpponentNbr)
                        return true;
            }
            return false;
        }

        private bool CanThrowedTurnsBetweenThrowsSpell(Spell spell)
        {
            if (spell.SpellParameters.TurnsBetweenThrowsNbr == 0) return true;

            if (!m_TurnsBetweenThrowsSpells.ContainsKey(spell))
            {
                m_TurnsBetweenThrowsSpells.Add(spell, 0);
                return true;
            }
            else
            {
                foreach (KeyValuePair<Spell, int> item in m_TurnsBetweenThrowsSpells)
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

        protected virtual void LaunchSpell(Spell spell, GridTile gridTile)
        {
            switch (spell.AttackType)
            {
                case AttackType.Damage:
                    LaunchAttack(spell, gridTile);
                break;

                case AttackType.Invocation:
                    SpawnObjectOnTile(spell, spell.SpellParameters.ObjectToSpawn, gridTile);
                break;

                case AttackType.StateChange:
                break;
                
                case AttackType.Teleportation:
                    TeleportationSpellOnTile(spell, gridTile);
                break;
            }
        }

        private void AddThrowedPerTurnSpells(Spell spell)
        {
            if (m_ThrowedPerTurnSpells.ContainsKey(spell))
                m_ThrowedPerTurnSpells[spell] ++;
        }
        private void ResetThrowedPerTurnSpells()
        {
            foreach (KeyValuePair<Spell, int> item in m_ThrowedPerTurnSpells.ToList())
                m_ThrowedPerTurnSpells[item.Key] = 0;
        }

        private void AddTargetOpponentSpells(Spell spell, Entity target)
        {
            if (m_TargetingPerOpponentSpells.ContainsKey(spell))
                m_TargetingPerOpponentSpells[spell].Add(target);
        }
        private void ResetTargetOpponentsSpells()
        {
            foreach (KeyValuePair<Spell, List<Entity>> item in m_TargetingPerOpponentSpells.ToList())
                m_TargetingPerOpponentSpells[item.Key].Clear();
        }

        private void AddTurnsBetweenThrowsSpell(Spell spell)
        {
            if (m_TurnsBetweenThrowsSpells.ContainsKey(spell))
                m_TurnsBetweenThrowsSpells[spell] = spell.SpellParameters.TurnsBetweenThrowsNbr + 1;
        }
        private void ResetTurnsBetweenThrowsSpell()
        {
            foreach (KeyValuePair<Spell, int> item in m_TurnsBetweenThrowsSpells.ToList())
                if (item.Value > 0)
                    m_TurnsBetweenThrowsSpells[item.Key] --;
        }

        private void LaunchAttack(Spell spell, GridTile gridTile)
        {
            GridObject gridObject = GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition);
            if (gridObject && gridObject.TryGetComponent(out Entity entity))
            {
                entity.TakeDamage(spell.SpellParameters.Damages);
                AddTargetOpponentSpells(spell, entity);
            }
            OnLaunchedSpell(spell);
        }
        private void SpawnObjectOnTile(Spell spell, GridObject gridObj, GridTile gridTile)
        {
            if (!gridTile.m_IsTileFlyable)
            {
                GridObject targetPosGridObject = GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition);

                GridObject instantiatedGridObject = null;

                if (!targetPosGridObject)
                {
                    instantiatedGridObject = GridManager.Instance.InstantiateGridObject(gridObj, gridTile.m_GridPosition);
                }
                else
                {
                    instantiatedGridObject = Instantiate(gridObj, targetPosGridObject.transform.position, Quaternion.identity).GetComponent<GridObject>();
                    Entity targetEntity = CombatManager.Instance.GetEntityOnGridTile(gridTile);
                    if (targetEntity)
                        AddTargetOpponentSpells(spell, targetEntity);
                }
                instantiatedGridObject.GetComponent<Entity>().OnEntityInvoked(spell);
                OnLaunchedSpell(spell);
            }
        }
        private void TeleportationSpellOnTile(Spell spell, GridTile gridTile)
        {
            GridObject targetGridObject = GridManager.Instance.GetGridObjectAtPosition(gridTile.m_GridPosition);
            if (!targetGridObject)
                m_Character.TeleportOnTile(gridTile);
            else
                m_Character.SwapPositionWithGridObject(targetGridObject);
            OnLaunchedSpell(spell);
        }
        protected void OnLaunchedSpell(Spell spell)
        {
            m_Character.SpendActionPoints(spell.SpellParameters.ActionPoints);

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