using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private BaseLogic _baseLogic;
    [SerializeField] private EnemiesLogic _enemiesLogic;
    [SerializeField] private SettingsObject _settingsObject;

    private void Awake()
    {
        
    }
}