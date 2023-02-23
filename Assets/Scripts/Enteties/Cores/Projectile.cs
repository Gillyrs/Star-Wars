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
    public void InitStats(ProjectileDataStructure projectileData, bool isReaching, List<Detector> myEntityDetectors, Team team, int lifeTime)
    {
        myTeam = team;
        ProjectileData = projectileData;
        this.myEntityDetectors = myEntityDetectors.ToList();
        if (isReaching)
            Activate(lifeTime);//detector.OnColliderEnter += Damage;  
        else
            Activate(ProjectileData.LifeTime);
    }

    public void ClearAllSubsribations()
    {
        OnLifeTimeEnded = null;
    }
    private async void Activate(int lifeTime)
    {
        await UniTask.Delay(lifeTime);
        OnLifeTimeEnded?.Invoke(this);
    }
    /*private void Damage(Collider2D collider)
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
        Debug.Log("BULLET REACHED at " + DateTime.Now + ":" + DateTime.Now.Millisecond + " type PROJ");
        member.TakeDamage(ProjectileData.Damage);
        OnLifeTimeEnded?.Invoke(this);
    }*/

    private void OnDisable()
    {
        myEntityDetectors.Clear();
        Rb.velocity = Vector3.zero;
        Rb.angularVelocity = 0;
        //detector.OnColliderEnter -= Damage;
    }
}
