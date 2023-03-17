using UnityEngine;

public interface IShootingTarget
{
    bool IsUnderAttack { get; set; }
    Vector3 GetTargetPosition();
    void TakeDamage(float damage);
}