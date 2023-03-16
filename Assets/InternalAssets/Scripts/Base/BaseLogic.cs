using EventBusSystem;

using UnityEngine;

public class BaseLogic : MonoBehaviour, IBaseDamageSubscriber
{
    [SerializeField] private Transform _damageSpriteTransform;

    [SerializeField, Space] private float _maxHealth;

    private float _currentHealth;


    private void Awake()
    {
        _currentHealth = _maxHealth;
        _damageSpriteTransform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public void HandleBaseDamage(float damage)
    {
        _currentHealth -= damage;

        if(_currentHealth <= 0f)
        {
            _damageSpriteTransform.localScale = Vector3.one;
            Debug.Log("База уничтожена !!!");
        }
        else
        {
            _damageSpriteTransform.localScale = (1f - (_currentHealth / _maxHealth)) * Vector3.one;
            Debug.Log($"База получила урон !!! Текущее здоровье: {_currentHealth} !!!");
        }
    }
}