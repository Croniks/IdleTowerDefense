using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemiesLogic : MonoBehaviour
{
    [SerializeField] private EnemiesPool _enemiesPool;
    [SerializeField] private SettingsObject _settings;

    private float _circleRadiusOffset;
    private int _wavesCount;
    private float _wavesInterval;
    private int _enemiesCountPerWave;
    private float _enemiesInterval;
    
    private Vector3 _spawnCircleRadius;
    private List<EnemyPoolObject> _enemies;

    private Coroutine _spawnCoroutine;


    #region SetupLogic

    private void Awake()
    {
        ApplySettings(_settings);
        DefineSpawnCircleRadius();

        _enemies = new List<EnemyPoolObject>(_enemiesCountPerWave * _wavesCount);
    }

    private void Start()
    {
        _spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    private void ApplySettings(SettingsObject settings)
    {
        _circleRadiusOffset = settings.SpawnCircleRadiusOffset;
        _wavesCount = settings.WavesCount;
        _wavesInterval = settings.WavesInterval;
        _enemiesCountPerWave = settings.EnemiesCountPerWave;
        _enemiesInterval = settings.EnemiesInterval;
    }

    private void DefineSpawnCircleRadius()
    {
        Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1f));
        Vector3 downRightCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f + _circleRadiusOffset, 0f - _circleRadiusOffset, 1f));

        _spawnCircleRadius = downRightCorner - center;
    }

    #endregion

    private void Update()
    {
        _enemies.ForEach(e => { e.MoveToBase(); });
    }
    
    private IEnumerator SpawnEnemies()
    {
        for(int i = 0; i < _wavesCount; i++)
        {
            yield return new WaitForSeconds(_wavesInterval);

            for (int j = 0; j < _enemiesCountPerWave; j++)
            {
                yield return new WaitForSeconds(_enemiesInterval);

                Vector3 spawnPos = GetRandomSpawnPosition();

                EnemyPoolObject enemy = _enemiesPool.Spawn();
                enemy.transform.position = spawnPos;
                _enemies.Add(enemy);
                
                Vector3 destinationPos = _settings.DestinationPosition;
                float destinationTime = _settings.DestinationTime;
                float destinationPercent = _settings.DestinationPercent;
                
                enemy.Setup(spawnPos, destinationPos, destinationTime, destinationPercent);
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int randomAngle = UnityEngine.Random.Range(0, 359);
        Vector3 spawnPosition = Quaternion.AngleAxis(randomAngle, Vector3.forward) * _spawnCircleRadius;
        return spawnPosition;
    }
}