using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
using Random = UnityEngine.Random;
public class Projectile : MonoBehaviour
{
    public ProjectileDataStructure ProjectileData;
    public Action<Projectile> OnLifeTimeEnded;
    public Rigidbody2D Rb;
    [SerializeField] private TriggerDetector detector;
    [SerializeField] private List<Detector> myEntityDetectors;
    public void InitStats(ProjectileDataStructure projectileData, Member member, bool isReaching, int lifeTime)
    {
        ProjectileData = projectileData;
        this.myEntityDetectors = myEntityDetectors.ToList();
        if (isReaching)
            Activate(member, lifeTime);
        else
            Activate(null, ProjectileData.LifeTime);
    }

    public void ClearAllSubsribations()
    {
        OnLifeTimeEnded = null;
    }
    private async void Activate(Member member, int lifeTime)
    {
        if (member == null)
        {
            await UniTask.Delay(lifeTime);
            OnLifeTimeEnded?.Invoke(this);
            Debug.Log("MISS");
            return;
        }
        await UniTask.Delay(lifeTime);
        member.TakeDamage(ProjectileData.Damage);
        Debug.Log("DAMAGE");
        OnLifeTimeEnded?.Invoke(this);
    }
    private void OnDisable()
    {
        myEntityDetectors.Clear();
        Rb.velocity = Vector3.zero;
        Rb.angularVelocity = 0;
    }
}
