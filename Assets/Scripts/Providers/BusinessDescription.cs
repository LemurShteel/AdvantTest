using System;
using UnityEngine;

namespace Providers
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Business", menuName = "ScriptableObjects/New Business", order = 1)]
    public class BusinessDescription : ScriptableObject
    {
        public string BusinessId => _businessId;
        public string BusinessNameKey => _businessNameKey;
        public float BaseIncomeDelaySeconds => _baseIncomeDelaySeconds;
        public int BaseCost => _baseCost;
        public float BaseIncome => _baseIncome;
        public int InitialLevel => _initialLevel;
        public string FirstUpgradeNameKey => _firstUpgradeNameKey;
        public int FirstUpgradeCost => _firstUpgradeCost;
        public int SecondUpgradeCost => _secondUpgradeCost;
        public string SecondUpgradeNameKey => _secondUpgradeNameKey;
        public float FirstUpgradeMultiplier => _firstUpgradeMultiplier;
        public float SecondUpgradeMultiplier => _secondUpgradeMultiplier;
    
        [SerializeField]
        private string _businessId = "business_id";
        [SerializeField]
        private string _businessNameKey = "business_id";
        [SerializeField]
        private float _baseIncomeDelaySeconds = 3;
        [SerializeField]
        private int _baseCost = 3;
        [SerializeField]
        private int _baseIncome = 3;
        [SerializeField]
        private int _initialLevel = 0;

        [Space]
        [SerializeField]
        private string _firstUpgradeNameKey = "first_upgrade";
        [SerializeField]
        private int _firstUpgradeCost = 50;
        [SerializeField]
        private float _firstUpgradeMultiplier = 0.5f;

        [Space]
        [SerializeField]
        private string _secondUpgradeNameKey = "second_upgrade";
        [SerializeField]
        private int _secondUpgradeCost = 400;
        [SerializeField]
        private float _secondUpgradeMultiplier = 1;
    }
}