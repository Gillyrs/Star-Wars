using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
public class Projectile : MonoBehaviour
{
    public ProjectileDataStructure ProjectileData;
    public Action<Projectile> OnLifeTimeEnded;
    public Rigidbody2D Rb;
    [SerializeField] private TriggerDetector detector;
    [SerializeField] private List<Detector> myEntityDetectors;
    [SerializeField] private Team myTeam;
    public void InitStats(ProjectileData projectileData, bool isMissing, List<Detector> myEntityDetectors, Team team)
    {
        myTeam = team;
        ProjectileData = projectileData.GetProjectileData();
        this.myEntityDetectors = myEntityDetectors.ToList();        
        if (isMissing == false)
            detector.OnColliderEnter += Damage;  
        Activate();
    }

    public void ClearAllSubsribations()
    {
        OnLifeTimeEnded = null;
    }
    private async void Activate()
    {
        await UniTask.Delay(ProjectileData.LifeTime);
        OnLifeTimeEnded?.Invoke(this);
    }
    private void Damage(Collider2D collider)
    {
        if (!collider.TryGetComponent(out Detector detector))
            return;

        if (myEntityDetectors.Contains(detector))
            return;

        if (!(detector is CollisionDetector collisionDetector))
            return;

        if (!collisionDetector.TryGetMember(out Member member))
            return;

        if (member.TeamEquals(myTeam) || !gameObject.activeSelf)
            return;
        Debug.Log(gameObject.activeSelf);
        member.TakeDamage(ProjectileData.Damage);
        OnLifeTimeEnded?.Invoke(this);
    }

    private void OnDisable()
    {
        myEntityDetectors.Clear();
        Rb.velocity = Vector3.zero;
        Rb.angularVelocity = 0;
        detector.OnColliderEnter -= Damage;
    }
}
