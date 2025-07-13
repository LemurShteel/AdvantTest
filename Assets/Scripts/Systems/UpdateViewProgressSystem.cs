using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using Providers;
using UnityEngine;

namespace Systems
{
    public class UpdateViewProgressSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Dictionary<int, BusinessDescription> _businessDescriptions;

        private EcsFilter _businessViewFilter;
        private EcsPool<BusinessViewComponent> _businessViewPool;
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BusinessViewLinkComponent> _viewLinksPool;

        public UpdateViewProgressSystem(IDescriptionsProvider descriptionsProvider)
        {
            _businessDescriptions = descriptionsProvider.GetBusinessDescriptions();
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _businessViewFilter = world.Filter<BusinessViewComponent>().Inc<BusinessViewLinkComponent>().End();
            _businessViewPool =  world.GetPool<BusinessViewComponent>();
            _businessPool = world.GetPool<BusinessComponent>();
            _viewLinksPool = world.GetPool<BusinessViewLinkComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var viewEntity in _businessViewFilter)
            {
                ref var link = ref _viewLinksPool.Get(viewEntity);
                ref var view = ref _businessViewPool.Get(viewEntity);
                if (!_businessPool.Has(link.BusinessEntity))
                {
                    continue;
                }

                ref var business = ref _businessPool.Get(link.BusinessEntity);
                if (business.Level < 1 || !_businessDescriptions.TryGetValue(business.ID, out var businessDescriptions))
                {
                    continue;
                }

                var incomeDelay = Mathf.Max(float.Epsilon, businessDescriptions.BaseIncomeDelaySeconds);
                view.Progress.SetValue(business.IncomeTimeProgress / incomeDelay);
            }
        }
    }
}
