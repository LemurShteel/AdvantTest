using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using Providers;
using Utils;

namespace Systems
{
    public class BusinessLevelUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Dictionary<int, BusinessDescription> _businessesDescriptions;

        private EcsFilter _levelUpRequestFilter;

        private EcsPool<BusinessLevelUpRequestComponent> _levelUpRequestPool;

        private EcsPool<BusinessViewLinkComponent> _businessLinkPool;
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BusinessViewComponent> _businessViewPool;

        private EcsFilter _playerMoneyFilter;
        private EcsPool<PlayerMoneyComponent> _playerMoneyPool;

        private EcsFilter _playerMoneyViewFilter;
        private EcsPool<PlayerMoneyViewComponent> _playerMoneyViewPool;

        public BusinessLevelUpSystem(IDescriptionsProvider descriptionsProvider)
        {
            _businessesDescriptions = descriptionsProvider.GetBusinessDescriptions();
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _levelUpRequestFilter =
                world.Filter<BusinessLevelUpRequestComponent>().Inc<BusinessViewLinkComponent>().End();
            _levelUpRequestPool = world.GetPool<BusinessLevelUpRequestComponent>();

            _businessLinkPool = world.GetPool<BusinessViewLinkComponent>();
            _businessPool = world.GetPool<BusinessComponent>();
            _businessViewPool = world.GetPool<BusinessViewComponent>();

            _playerMoneyFilter = world.Filter<PlayerMoneyComponent>().End();
            _playerMoneyPool = world.GetPool<PlayerMoneyComponent>();

            _playerMoneyViewFilter = world.Filter<PlayerMoneyViewComponent>().End();
            _playerMoneyViewPool = world.GetPool<PlayerMoneyViewComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            if (_levelUpRequestFilter.GetEntitiesCount() <= 0)
            {
                return;
            }

            var playerMoney = _playerMoneyFilter.GetRawEntities()[0];
            ref var moneyComponent = ref _playerMoneyPool.Get(playerMoney);

            var playerMoneyView = _playerMoneyViewFilter.GetRawEntities()[0];
            ref var moneyViewComponent = ref _playerMoneyViewPool.Get(playerMoneyView);

            foreach (var viewEntity in _levelUpRequestFilter)
            {
                ref var link = ref _businessLinkPool.Get(viewEntity);
                ref var business = ref _businessPool.Get(link.BusinessEntity);
                ref var businessView = ref _businessViewPool.Get(viewEntity);

                if (!_businessesDescriptions.TryGetValue(business.ID, out var description))
                {
                    continue;
                }

                var upgradeCost = MoneyUtils.GetLevelUpCost(ref business, description);
                if (moneyComponent.Money >= upgradeCost)
                {
                    MoneyUtils.ChangePlayerMoney(ref moneyComponent, ref moneyViewComponent, -upgradeCost);
                    business.Level += 1;
                    businessView.Level.SetValue(business.Level);
                    businessView.LevelUpCost.SetValue(MoneyUtils.GetLevelUpCost(ref business, description));
                    businessView.TotalIncome.SetValue(MoneyUtils.GetIncome(ref business, description));
                }

                _levelUpRequestPool.Del(viewEntity);
            }
        }
    }
}