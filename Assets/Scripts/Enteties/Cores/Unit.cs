using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Threading;

public class Unit : MonoBehaviour, IDamageable
{
    public event Action<Unit> OnUnitDestroyed;
    [SerializeField] private TriggerDetector attackRadius;
    [SerializeField] private List<Unit> targetsQueue = new();
    private Team myTeam;
    private Weapon weapon;
    private UnitDataStructure unitData;
    private bool isBusy = false;
    public void Init(UnitData unitData, Weapon weapon, Team team)
    {        
        this.unitData = unitData.GetUnitData();
        this.weapon = weapon;
        myTeam = team;
    }

    public bool TeamEquals(Team team)
    {
        return myTeam == team;
    }

    public void Takedamage()
    {
        throw new System.NotImplementedException();
    }
    private void OnEnemyNoticed(Unit target)
    {
        if (target.TeamEquals(myTeam) || target.gameObject.activeSelf == false)
            return;
        if (isBusy)
        {
            if (targetsQueue.Contains(target))
                return;
            targetsQueue.Add(target);
            target.OnUnitDestroyed += OnTargetInQueueDestroyed;
            return;
        }
        isBusy = true;
        weapon.isStopped = false;
        target.OnUnitDestroyed += OnTargetDestroyed;
        StartCoroutine(weapon.Rotate(target.transform, unitData.WeaponRotationSpeed));
    }

    private void OnTargetDestroyed(Unit unit)
    {
        weapon.ResetStates();
        weapon.isStopped = true;
        unit.OnUnitDestroyed -= OnTargetDestroyed;
        targetsQueue.Remove(unit);
        isBusy = false;
        var newTarget = CheckTargets();
        if (newTarget != null)
            OnEnemyNoticed(newTarget);
    }
    
    private void OnTargetInQueueDestroyed(Unit unit)
    {
        targetsQueue.Remove(unit);
    }
    private Unit CheckTargets()
    {
        if (targetsQueue.Count > 0)
        {
            var target = targetsQueue[0];
            targetsQueue.RemoveAt(0);
            target.OnUnitDestroyed -= OnTargetInQueueDestroyed;
            return target;
        }
        return null;
    }

    private async void OnEnable()
    {
        await UniTask.Delay(100);
        weapon.ResetStates();
        weapon.OnCanShoot += weapon.Shoot;
        attackRadius.OnTrigerEnter += (Collider2D colider) =>
        {
            if (colider.TryGetComponent(out TriggerDetector detector) == true)
                OnEnemyNoticed(detector.GetMyUnit());
        };
    }

    private void OnDisable()
    {
        weapon.isStopped = true;
        weapon.OnCanShoot -= weapon.Shoot;
        OnUnitDestroyed?.Invoke(this);
        isBusy = false;
        targetsQueue.Clear();
        attackRadius.ClearAllSubsribations();
    }

    
}

public enum Team
{
    firstTeam,
    secondTeam
}
