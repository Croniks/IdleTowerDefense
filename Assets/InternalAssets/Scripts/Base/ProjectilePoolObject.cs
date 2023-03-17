using Spawners;

public class ProjectilePoolObject : PoolObject
{
    private IShootingTarget _target;
    private float _targetHitTime;
    private float _attackDamage;

    public void Setup
    (
        ISettingsGetter settings,
        IShootingTarget target,
        float attactDamage
    )
    {
        _target = target;
        _targetHitTime = settings.TargetHitTime;
        _attackDamage = attactDamage;
    }
}