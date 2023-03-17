using System.Collections.Generic;

using EventBusSystem;

using UnityEngine;


public class BaseLogic : MonoBehaviour, IBaseDamageSubscriber
{

    [SerializeField] private Transform _damageSpriteTransform;

    [SerializeField, Space] private ProjectilesPool _projectilesPool;
    [SerializeField] private BaseShootingImprovementSystem _shootingImprovementSystem;
    [SerializeField] private AttackRangeCircle _attackRangeCircle;
    
    private ISettingsGetter _settings;
    private IEnumerable<IShootingTarget> _shootingTargets;
    private LinkedList<ProjectilePoolObject> _projectiles;
    private List<LinkedListNode<ProjectilePoolObject>> _projectilesNodesForRemove;

    private float _maxHP;
    private float _currentHP;
    private Vector3 _basePosition;

    private float _shotTime;
    private float _timeElapsedSinceLastShot = 0f;

    private float _attackRange;
    private float _attackDamage;

    #region SetupLogic

    public void Setup(ISettingsGetter settings, IEnumerable<IShootingTarget> shootingTargets)
    {
        _basePosition = transform.position;

        _settings = settings;
        _shootingTargets = shootingTargets;

        if (_projectiles != null)
        {
            foreach (var projectile in _projectiles)
            {
                projectile.ReturnToPool();
            }

            _projectiles.Clear();
        }
        _projectiles = new LinkedList<ProjectilePoolObject>();

        if (_projectilesNodesForRemove != null)
        {
            _projectilesNodesForRemove.Clear();
        }
        _projectilesNodesForRemove = new List<LinkedListNode<ProjectilePoolObject>>();
        
        _maxHP = _currentHP = _settings.BaseMaxHP;
        _damageSpriteTransform.localScale = Vector3.zero;

        _shootingImprovementSystem.Setup(_settings);

        ShootingRangeValueChangedHandler(_shootingImprovementSystem.ShootingRangeValue);
        ShotAmountPerSecondChangedHandler(_shootingImprovementSystem.ShotAmountPerSecondValue);
        DamageAmountValueChangedHandler(_shootingImprovementSystem.DamageAmountValue);
    }
    
    #endregion

    #region UnityCalls

    private void Update()
    {
        _timeElapsedSinceLastShot += Time.deltaTime;

        if(_timeElapsedSinceLastShot >= _shotTime)
        {
            float closestDistance = float.MaxValue;
            IShootingTarget currentTarget = null;
            float squareAttackRange = Mathf.Pow(_attackRange, 2);

            foreach (IShootingTarget target in _shootingTargets)
            {
                if (target.IsDestroyed == true)
                {
                    continue;
                }

                Vector3 targetPosition = target.GetTargetPosition();
                float distanceToEnemy = (targetPosition - _basePosition).sqrMagnitude;
                float deltaRange = squareAttackRange - distanceToEnemy;

                if (deltaRange >= 0f && distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    currentTarget = target;
                }
            }

            if(currentTarget != null)
            {
                ProjectilePoolObject projectile = _projectilesPool.Spawn();
                projectile.transform.position = _basePosition;
                TurnProjectileTowardsEnemy(projectile.transform, currentTarget.GetTargetPosition());

                LinkedListNode<ProjectilePoolObject> node = _projectiles.AddLast(projectile);

                projectile.Setup(node, _settings, currentTarget, _attackDamage);

                _timeElapsedSinceLastShot = 0f;
            }
        }

        foreach (var projectile in _projectiles)
        {
            if (projectile.enabled == true)
            {
                projectile.MoveToEnemy();
            }
            else
            {
                _projectilesNodesForRemove.Add(projectile.ProjectileNode);
                projectile.ReturnToPool();
            }
        }

        if(_projectilesNodesForRemove.Count > 0)
        {
            _projectilesNodesForRemove.ForEach(n => _projectiles.Remove(n));
            _projectilesNodesForRemove.Clear();
        }
    }

    private void OnEnable()
    {
        EventBus.Subscribe(this);

        _shootingImprovementSystem.DamageAmountValueChanged += DamageAmountValueChangedHandler;
        _shootingImprovementSystem.ShotAmountPerSecondValueChanged += ShotAmountPerSecondChangedHandler;
        _shootingImprovementSystem.ShootingRangeValueChanged += ShootingRangeValueChangedHandler;
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(this);

        _shootingImprovementSystem.DamageAmountValueChanged -= DamageAmountValueChangedHandler;
        _shootingImprovementSystem.ShotAmountPerSecondValueChanged -= ShotAmountPerSecondChangedHandler;
        _shootingImprovementSystem.ShootingRangeValueChanged -= ShootingRangeValueChangedHandler;
    }

    #endregion

    #region PrivateMethods

    private void TurnProjectileTowardsEnemy(Transform projectileTrans, Vector3 enemyPosition)
    {
        Vector3 direction = enemyPosition - projectileTrans.position;
        projectileTrans.rotation = Quaternion.LookRotation(direction, projectileTrans.up);
    }

    #endregion

    #region EventHandlers

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

    private void DamageAmountValueChangedHandler(float value)
    {
        _attackDamage = value;
    }

    private void ShotAmountPerSecondChangedHandler(float value)
    {
        _shotTime = 1f / value;
    }

    private void ShootingRangeValueChangedHandler(float value)
    {
        _attackRangeCircle.SetRange(value);
        _attackRange = _attackRangeCircle.GetDistanceToExtremePoint(_basePosition);
    }

    #endregion
}