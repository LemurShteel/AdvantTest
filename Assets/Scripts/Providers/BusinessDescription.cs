using System;
using UnityEngine;

namespace Providers
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Business", menuName = "ScriptableObjects/New Business", order = 1)]
    public class BusinessDescription : ScriptableObject
    {
        public string BusinessId => businessId;
        public string BusinessNameLocalizationKey => businessNameLocalizationKey;
        public float BaseIncomeDelaySeconds => baseIncomeDelaySeconds;
        public int BaseCost => baseCost;
        public float BaseIncome => baseIncome;
        public int InitialLevel => initialLevel;
        public string FirstUpgradeLocalizationKey => firstUpgradeLocalizationKey;
        public int FirstUpgradeCost => firstUpgradeCost;
        public int SecondUpgradeCost => secondUpgradeCost;
        public string SecondUpgradeLocalizationKey => secondUpgradeLocalizationKey;
        public float FirstUpgradeMultiplier => firstUpgradeMultiplier;
        public float SecondUpgradeMultiplier => secondUpgradeMultiplier;
    
        [SerializeField]
        private string businessId = "business_id";
        [SerializeField]
        private string businessNameLocalizationKey = "business_id";
        [SerializeField]
        private float baseIncomeDelaySeconds = 3;
        [SerializeField]
        private int baseCost = 3;
        [SerializeField]
        private int baseIncome = 3;
        [SerializeField]
        private int initialLevel = 0;

        [Space]
        [SerializeField]
        private string firstUpgradeLocalizationKey = "first_upgrade";
        [SerializeField]
        private int firstUpgradeCost = 50;
        [SerializeField]
        private float firstUpgradeMultiplier = 0.5f;

        [Space]
        [SerializeField]
        private string secondUpgradeLocalizationKey = "second_upgrade";
        [SerializeField]
        private int secondUpgradeCost = 400;
        [SerializeField]
        private float secondUpgradeMultiplier = 1;
    }
}