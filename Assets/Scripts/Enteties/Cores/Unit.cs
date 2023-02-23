using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class Unit : Member
{
    public override event Action<Member> OnMemberDestroyed;
    [SerializeField] private TriggerDetector attackRadius;
    [SerializeField] private List<Detector> detectors;
    [SerializeField] private List<Member> targetsQueue = new();
    [SerializeField] private UnitMovement movement;    
    private bool isBusy = false;
    private bool isRegenerating = false;
    private bool isInitialized;
    private Team team;
    private Weapon weapon;
    private Entity entity;
    private UnitDataStructure unitData;

    private void Awake()
    {
        entity = transform.parent.GetComponent<Entity>();
        movement = transform.parent.GetComponent<UnitMovement>();
    }
    public void Init(UnitData unitData, WeaponData weaponData, ProjectileData projectileData, Team team, Transform enemyBase)
    {
        isInitialized = true;
        this.team = team;
        this.unitData = unitData.GetUnitData();

        weapon = entity.GetWeapon();       
        weapon.Init(weaponData, projectileData, this.team, detectors);
        weapon.OnCanShoot += weapon.Shoot;

        movement.SetDestination(enemyBase);
        
        attackRadius.OnColliderEnter += HandleColliderEnter;
    }
    private void OnDisable()
    {
        if (!isInitialized)
            return;

        weapon.isStopped = true;
        weapon.OnCanShoot -= weapon.Shoot;
        weapon.ResetStates();

        OnMemberDestroyed?.Invoke(this);
        isBusy = false;

        targetsQueue.Clear();
        attackRadius.ClearAllSubsribations();
    }
    private void Attack(Member member)
    {
        if (member.TeamEquals(team) || !member.gameObject.activeInHierarchy)
            return;
        if (isBusy)
        {
            if (targetsQueue.Contains(member))
                return;
            targetsQueue.Add(member);
            member.OnMemberDestroyed += MemberInQueueDestroyed;
            return;
        }
        movement.MoveToFightingState();
        isBusy = true;
        weapon.isStopped = false;
        member.OnMemberDestroyed += MemberDestroyed;
        StartCoroutine(weapon.Rotate(member.transform, unitData.WeaponRotationSpeed));
    }
    private void MemberDestroyed(Member member)
    {
        movement.MoveToRunningState();
        weapon.ResetStates(isStopped: true);

        member.OnMemberDestroyed -= MemberDestroyed;
        isBusy = false;

        if (TryGetTarget(out Member newMember))
            Attack(newMember);
    }
    private void MemberInQueueDestroyed(Member member)
    {
        targetsQueue.Remove(member);
        
    }
    private bool TryGetTarget(out Member member)
    {
        if (targetsQueue.Count == 0)
        {
            member = null;
            return false;
        }

        var nextTarget = targetsQueue[0];
        targetsQueue.RemoveAt(0);
        nextTarget.OnMemberDestroyed -= MemberInQueueDestroyed;

        member = nextTarget;

        return true;
    }
    private void HandleColliderEnter(Collider2D colider)
    {
        if (colider.TryGetComponent(out Detector detector))
        {
            if (detector.TryGetMember(out Member member) && member != this)
            {
                Attack(member);
            }
        }
    }
    public override void TakeDamage(int damage)
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
        StartCoroutine(StartRegeneraion());
    }
    public override bool TeamEquals(Team team)
    {
        return this.team == team;
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
