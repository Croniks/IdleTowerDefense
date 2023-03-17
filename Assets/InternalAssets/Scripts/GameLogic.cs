using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private BaseLogic _baseLogic;
    [SerializeField] private EnemiesLogic _enemiesLogic;
    [SerializeField] private SettingsObject _settings;

    private void Awake()
    {
        _enemiesLogic.Setup(_settings, _baseLogic.transform.position);
        _baseLogic.Setup(_settings, _enemiesLogic.ShootingTargets);
    }
}