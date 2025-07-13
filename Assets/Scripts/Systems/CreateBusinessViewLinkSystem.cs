using System;
using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using Providers;
using UnityEngine.Pool;

namespace Systems
{
    public class CreateBusinessViewLinkSystem : IEcsInitSystem
    {
        private readonly Dictionary<int, BusinessDescription> _descriptions;

        private EcsFilter _createViewLinksFilter;
        private EcsPool<BusinessViewComponent> _viewsPool;

        private EcsFilter _businessFilter;
        private EcsPool<BusinessComponent> _businessPool;
    
        private EcsPool<BusinessViewLinkComponent> _linksPool;
    
        public CreateBusinessViewLinkSystem(IDescriptionsProvider descriptionsProvider)
        {
            _descriptions = descriptionsProvider.GetBusinessDescriptions();
        }
    
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _createViewLinksFilter = world.Filter<BusinessViewComponent>().Exc<BusinessViewLinkComponent>().End();
            _viewsPool = world.GetPool<BusinessViewComponent>();
            _linksPool = world.GetPool<BusinessViewLinkComponent>();
        
            _businessFilter = world.Filter<BusinessComponent>().End();
            _businessPool = world.GetPool<BusinessComponent>();

            var businessIdEntityDictionary = DictionaryPool<int, int>.Get();
            foreach (var businessEntity in _businessFilter)
            {
                ref var business = ref _businessPool.Get(businessEntity);
                businessIdEntityDictionary.Add(business.ID, businessEntity);
            }
        
            foreach (var viewEntity in _createViewLinksFilter)
            {
                ref var view = ref _viewsPool.Get(viewEntity);
                if (businessIdEntityDictionary.TryGetValue(view.BusinessId, out var businessEntity))
                {
                    ref var linkComponent = ref _linksPool.Add(viewEntity);
                    linkComponent.BusinessEntity = businessEntity;
                }

                if (_descriptions.TryGetValue(view.BusinessId, out var description))
                {
                    ref var business = ref _businessPool.Get(businessEntity);
                    view.TotalIncome.SetValue(Utils.MoneyUtils.GetIncome(ref business, description));
                    view.Level.SetValue(business.Level);
                    view.Progress.SetValue(business.IncomeTimeProgress / MathF.Max(float.Epsilon, description.BaseIncomeDelaySeconds));
                    view.LevelUpCost.SetValue(Utils.MoneyUtils.GetLevelUpCost(ref business, description));
                    view.FirstUpgradeBought.SetValue((business.Upgrade & UpgradeType.FirstUpgrade) != 0);
                    view.SecondUpgradeBought.SetValue((business.Upgrade & UpgradeType.SecondUpgrade) != 0);
                }
            }
            businessIdEntityDictionary.Clear();
            DictionaryPool<int, int>.Release(businessIdEntityDictionary);
        }
    }
}
