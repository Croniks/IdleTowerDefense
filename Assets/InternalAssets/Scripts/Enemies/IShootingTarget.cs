using UnityEngine;

public interface IShootingTarget
{
    Vector3 GetTargetPosition();
    void TakeDamage(float damage);
}