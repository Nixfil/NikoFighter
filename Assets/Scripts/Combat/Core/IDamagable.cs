using static UnityEngine.Analytics.IAnalytic;

public interface IDamageable
{
    void TakeHit(HitData hit);
}