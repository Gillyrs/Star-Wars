using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class Unit : MonoBehaviour, IDamageable
{
    public event Action<Unit> OnUnitDestroyed;
    [SerializeField] private TriggerDetector attackRadius;
    [SerializeField] private List<Unit> targetsQueue = new();
    private Team myTeam;
    private Weapon weapon;
    private UnitDataStructure unitData;
    [SerializeField] private bool isBusy = false;

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
        Debug.Log("Enemy noticed");
        if (target.TeamEquals(myTeam) || target.gameObject.activeSelf == false)
            return;
        if (isBusy)
        {
            targetsQueue.Add(target);
            return;
        }
        isBusy = true;
        target.OnUnitDestroyed += OnTargetDestroyed;
        StartCoroutine(weapon.Rotate(target.transform, unitData.WeaponRotationSpeed,
                                     async () =>
                                     { 
                                         while (isBusy == true && gameObject.activeSelf == true)
                                         {
                                             await weapon.Shoot();
                                             if (isBusy == false) break;
                                         }
                                             
                                     } ));
    }
    private void OnTargetDestroyed(Unit unit)
    {
        unit.OnUnitDestroyed -= OnTargetDestroyed;
        targetsQueue.Remove(unit);
        isBusy = false;
        var newTarget = CheckTargets();
        if (newTarget != null)
            OnEnemyNoticed(newTarget);
    }
    private Unit CheckTargets()
    {
        if (targetsQueue.Count > 0)
        {
            Debug.Log("From Queue");
            var target = targetsQueue[0];
            targetsQueue.RemoveAt(0);
            return target;
        }
        return null;
    }

    private async void OnEnable()
    {
        await UniTask.Delay(100);
        weapon.isStopped = false;
        attackRadius.OnTrigerEnter += (Collider2D colider) =>
        {
            if (colider.TryGetComponent(out TriggerDetector detector) == true)
                OnEnemyNoticed(detector.GetMyUnit());
        };
    }

    private void OnDisable()
    {
        weapon.isStopped = true;
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
