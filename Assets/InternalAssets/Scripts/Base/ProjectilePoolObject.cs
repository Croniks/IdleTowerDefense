using Spawners;

public class ProjectilePoolObject : PoolObject
{
    private IShootingTarget _target;
    private float _targetHitTime;

    public void Setup
    (
        ISettingsGetter settings,
        IShootingTarget target
    )
    {
        _target = target;
        _targetHitTime = settings.TargetHitTime;
    }


}