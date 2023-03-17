using System.Collections.Generic;

using EventBusSystem;

using UnityEngine;

public class BaseLogic : MonoBehaviour, IBaseDamageSubscriber
{
    [SerializeField] private Transform _damageSpriteTransform;

    private ISettingsGetter _settings;
    private IEnumerable<IShootingTarget> _shootingTargets;

    private float _maxHP;
    private float _currentHP;
    

    #region SetupLogic

    public void Setup(ISettingsGetter settings, IEnumerable<IShootingTarget> shootingTargets)
    {
        _settings = settings;
        _shootingTargets = shootingTargets;

        _maxHP = _currentHP = _settings.BaseMaxHP;
        _damageSpriteTransform.localScale = Vector3.zero;
    }
    
    #endregion

    #region UnityCalls

    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    #endregion

    #region PrivateMehtods

    #endregion
    
    public void HandleBaseDamage(float damage)
    {
        _currentHP -= damage;

        if(_currentHP <= 0f)
        {
            _damageSpriteTransform.localScale = Vector3.one;
            Debug.Log("База уничтожена !!!");
        }
        else
        {
            _damageSpriteTransform.localScale = (1f - (_currentHP / _maxHP)) * Vector3.one;
            Debug.Log($"База получила урон !!! Текущее здоровье: {_currentHP} !!!");
        }
    }
}