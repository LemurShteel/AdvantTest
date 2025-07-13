using Bindings;
using Components;
using Leopotam.EcsLite;
using Providers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIBusinessUnitView : MonoBehaviour
    {
        [SerializeField] 
        private Text _businessName;

        [SerializeField]
        private ValueBinder<int> _levelBinder;
        [SerializeField]
        private ValueBinder<double> _levelUpCostBinder;
        [SerializeField]
        private ValueBinder<double> _totalIncomeBinder;
        [SerializeField]
        private ValueBinder<float> _progressIncomeBinder;
    
        [Space]
        [SerializeField] 
        private Text _firstUpgradeName;
        [SerializeField]
        private ValueBinder<float> _firstUpgradeIncome;
        [SerializeField]
        private ValueBinder<int> _firstUpgradeCost;
        [SerializeField]
        private ValueBinder<bool> _firstUpgradeBought;

        [Space]
        [SerializeField] 
        private Text _secondUpgradeName;
        [SerializeField]
        private ValueBinder<float> _secondUpgradeIncome;
        [SerializeField]
        private ValueBinder<int> _secondUpgradeCost;
        [SerializeField]
        private ValueBinder<bool> _secondUpgradeBought;

        private int _entity;
        private EcsPool<BusinessViewComponent> _businessViewPool;
        private EcsPool<BusinessLevelUpRequestComponent> _businessLevelUpRequestPool;
        private EcsPool<BusinessUpgradeRequestComponent> _businessUpgradeRequestPool;
 
        public void Init(IEcsSystems systems, int businessId, BusinessDescription businessDescription, INamingProvider namingProvider)
        {
            _businessName.text = namingProvider.GetName(businessDescription.BusinessNameLocalizationKey);

            _firstUpgradeName.text = namingProvider.GetName(businessDescription.FirstUpgradeLocalizationKey);
            _firstUpgradeCost.SetValue(businessDescription.FirstUpgradeCost);
            _firstUpgradeIncome.SetValue(businessDescription.FirstUpgradeMultiplier * 100f);

            _secondUpgradeName.text = namingProvider.GetName(businessDescription.SecondUpgradeLocalizationKey);
            _secondUpgradeCost.SetValue(businessDescription.SecondUpgradeCost);
            _secondUpgradeIncome.SetValue(businessDescription.SecondUpgradeMultiplier * 100f);
        
            var world = systems.GetWorld();
            _entity = world.NewEntity();

            _businessViewPool = world.GetPool<BusinessViewComponent>();
        
            ref var componentView = ref _businessViewPool.Add(_entity);
            componentView.BusinessId = businessId;
            componentView.Level = _levelBinder;
            componentView.LevelUpCost = _levelUpCostBinder;
            componentView.TotalIncome = _totalIncomeBinder;
            componentView.FirstUpgradeBought = _firstUpgradeBought;
            componentView.SecondUpgradeBought = _secondUpgradeBought;
            componentView.Progress = _progressIncomeBinder;
        
            _businessLevelUpRequestPool = world.GetPool<BusinessLevelUpRequestComponent>();
            _businessUpgradeRequestPool = world.GetPool<BusinessUpgradeRequestComponent>();
        }

        public void OnLevelUp()
        {
            if (_businessLevelUpRequestPool.Has(_entity))
            {
                return;
            }
            _businessLevelUpRequestPool.Add(_entity);
        }

        public void OnBuyFirstUpgrade()
        {
            RequestUpgrade(UpgradeType.FirstUpgrade);
        }

        public void OnBuySecondUpgrade()
        {
            RequestUpgrade(UpgradeType.SecondUpgrade);
        }

        private void RequestUpgrade(UpgradeType upgradeType)
        {
            if (_businessUpgradeRequestPool.Has(_entity))
            {
                return;
            }
            ref var upgradeRequestComponent = ref _businessUpgradeRequestPool.Add(_entity);
            upgradeRequestComponent.UpgradeType = upgradeType;
        }
    }
}
