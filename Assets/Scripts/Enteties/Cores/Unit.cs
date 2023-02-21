using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System.Threading;

public class Unit : Member
{
    public override event Action<Member> OnMemberDestroyed;

    [SerializeField] private TriggerDetector attackRadius;
    [SerializeField] private List<Member> targetsQueue = new();
    [SerializeField] private UnitMovement movement;
    [SerializeField] private Team myTeam;
    private Weapon weapon;
    [SerializeField] private UnitDataStructure unitData;
    private bool isBusy = false;
    private bool isRegenerating = false;
    private Entity entity;
    [SerializeField] private List<Detector> detectors;
    public void Init(Entity entity, 
                    UnitData unitData, WeaponData weaponData, ProjectileData projectileData,
                    Team team, Transform enemyBase)
    {
        this.entity = entity;
        this.unitData = unitData.GetUnitData();
        weapon = entity.GetWeapon();
        myTeam = team;
        weapon.InitStats(weaponData, projectileData, detectors, myTeam);        
        movement.SetDestination(enemyBase);
    }

    private void Update()
    {
        Debug.Log(unitData.Health);
    }
    private void Attack(Member member)
    {
        if (member.TeamEquals(myTeam) || member.gameObject.activeInHierarchy == false)
            return;
        if (isBusy)
        {
            if (targetsQueue.Contains(member))
                return;
            targetsQueue.Add(member);
            member.OnMemberDestroyed += OnTargetInQueueDestroyed;
            return;
        }
        movement.MoveToFightingState();
        isBusy = true;
        weapon.isStopped = false;
        member.OnMemberDestroyed += OnTargetDestroyed;
        StartCoroutine(weapon.Rotate(member.transform, unitData.WeaponRotationSpeed));
    }
    private void OnTargetDestroyed(Member member)
    {
        Debug.Log("Target Destroyed");
        movement.MoveToRunningState();
        weapon.ResetStates();
        weapon.isStopped = true;
        Debug.Log(weapon.isStopped);
        member.OnMemberDestroyed -= OnTargetDestroyed;
        isBusy = false;
        movement.MoveToRunningState();
        var newTarget = CheckTargets();
        if (newTarget != null)
            Attack(newTarget);
    }   
    private void OnTargetInQueueDestroyed(Member member)
    {
        targetsQueue.Remove(member);
        
    }
    private Member CheckTargets()
    {
        if (targetsQueue.Count > 0)
        {
            var target = targetsQueue[0];
            targetsQueue.RemoveAt(0);
            target.OnMemberDestroyed -= OnTargetInQueueDestroyed;
            return target;
        }
        return null;
    }
    private async void OnEnable()
    {
        await UniTask.Delay(100);
        weapon.ResetStates();
        weapon.OnCanShoot += weapon.Shoot;
        attackRadius.OnColliderEnter += (Collider2D colider) =>
        {
            if (colider.TryGetComponent(out TriggerDetector detector) == true)
            {
                if(detector.TryGetMember(out Member member) && member != this)
                    Attack(member);
            }
        };
    }
    private void OnDisable()
    {
        Debug.Log("I am disabled");
        weapon.isStopped = true;
        weapon.OnCanShoot -= weapon.Shoot;
        OnMemberDestroyed?.Invoke(this);
        isBusy = false;
        targetsQueue.Clear();
        attackRadius.ClearAllSubsribations();
    }

    public override async void TakeDamage(int damage)
    {
        unitData.Armor -= damage;
        int hasArmor = Convert.ToInt32(unitData.Armor >= 0);
        unitData.Health += unitData.Armor * (1 - hasArmor);
        unitData.Armor *= hasArmor;
        if (unitData.Health <= 0)
        {
            Die();
            return;
        }
        await UniTask.Delay(unitData.RegenerationCooldown);
        StartCoroutine(StartRegeneraion());
    }

    public override bool TeamEquals(Team team)
    {
        return myTeam == team;
    }

    private void Die()
    {   
        entity.gameObject.SetActive(false);
    }

    private IEnumerator StartRegeneraion()
    {
        while (unitData.Health != unitData.MaxHealth)
        {
            if (unitData.Health + unitData.RegenerationAmount >= unitData.MaxHealth)
            {
                unitData.Health = 100;
                StopCoroutine(StartRegeneraion());
            }
            else
                unitData.Health += unitData.RegenerationAmount;
            yield return new WaitForSeconds(unitData.RegenerationSpeed);
        }
    }
}

public enum Team
{
    FirstTeam,
    SecondTeam,
    None
}
