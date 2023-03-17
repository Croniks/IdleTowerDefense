using System.Collections.Generic;

using UnityEngine;


public class BaseShootingImprovementSystem : MonoBehaviour
{
    private ISettingsGetter _settings;

    private Dictionary<int, ValueCostPair> _damageAmountGrades;
    public float DamageAmountValue { get => _damageAmountGrades[_currentDamageAmountGrade].value; }
    private int _currentDamageAmountGrade = 0;

    private Dictionary<int, ValueCostPair> _shotAmountPerSecondGrades;
    public float ShotAmountPerSecondValue { get => _shotAmountPerSecondGrades[_currentShotAmountPerSecondGrade].value; }
    private int _currentShotAmountPerSecondGrade = 0;

    private Dictionary<int, ValueCostPair> _shootingRangeGrades;
    public float ShootingRangeValue { get => _shootingRangeGrades[_currentShootingRangeGrade].value; }
    private int _currentShootingRangeGrade = 0;

    public void Setup(ISettingsGetter settings)
    {
        _settings = settings;

        _damageAmountGrades = SetupGradeCollection(_settings.BaseDamageSettings.DamageAmountPerShot);
        _shotAmountPerSecondGrades = SetupGradeCollection(_settings.BaseDamageSettings.ShotAmountPerSecond);
        _shootingRangeGrades = SetupGradeCollection(_settings.BaseDamageSettings.ShootingRange);
    }

    private Dictionary<int, ValueCostPair> SetupGradeCollection(IEnumerable<BaseDamageSettings.UpgradeValuePair> from)
    {
        Dictionary<int, ValueCostPair> to = new Dictionary<int, ValueCostPair>();

        foreach (var pair in from)
        {
            to.Add(pair.upgradeLvl, new ValueCostPair(pair.value, pair.cost));
        }

        return to;
    }
    
    private struct ValueCostPair
    {
        public float value;
        public int cost;

        public ValueCostPair(float value, int cost)
        {
            this.value = value;
            this.cost = cost;
        }
    }
}