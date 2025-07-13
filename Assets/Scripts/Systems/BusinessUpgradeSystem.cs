using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using Providers;
using Utils;

namespace Systems
{
    public class BusinessUpgradeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Dictionary<int, BusinessDescription> _businessesDescriptions;

        private EcsFilter _upgradeBusinessRequestFilter;

        private EcsPool<BusinessUpgradeRequestComponent> _upgradeRequestPool;
    
        private EcsPool<BusinessViewLinkComponent> _businessLinkPool;
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BusinessViewComponent> _businessViewPool;

        private EcsFilter _playerMoneyFilter;
        private EcsPool<PlayerMoneyComponent> _playerMoneyPool;
    
        private EcsFilter _playerMoneyViewFilter;
        private EcsPool<PlayerMoneyViewComponent> _playerMoneyViewPool;

        public BusinessUpgradeSystem(IDescriptionsProvider descriptionsProvider)
        {
            _businessesDescriptions = descriptionsProvider.GetBusinessDescriptions();
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _upgradeBusinessRequestFilter = world.Filter<BusinessUpgradeRequestComponent>().Inc<BusinessViewLinkComponent>().End();
            _upgradeRequestPool = world.GetPool<BusinessUpgradeRequestComponent>();

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
            if (_upgradeBusinessRequestFilter.GetEntitiesCount() <= 0)
            {
                return;
            }
        
            var playerMoneyEntity = _playerMoneyFilter.GetRawEntities()[0];
            ref var moneyComponent = ref _playerMoneyPool.Get(playerMoneyEntity);
        
            var playerMoneyViewEntity = _playerMoneyViewFilter.GetRawEntities()[0];
            ref var moneyViewComponent = ref _playerMoneyViewPool.Get(playerMoneyViewEntity);
        
            foreach (var viewEntity in _upgradeBusinessRequestFilter)
            {
                ref var link = ref _businessLinkPool.Get(viewEntity);
                ref var business = ref _businessPool.Get(link.BusinessEntity);
                ref var businessView = ref _businessViewPool.Get(viewEntity);
                ref var upgradeRequest = ref _upgradeRequestPool.Get(viewEntity);
            
                if (!_businessesDescriptions.TryGetValue(business.ID, out var description)
                    || (business.Upgrade & upgradeRequest.UpgradeType) != 0)
                {
                    _upgradeRequestPool.Del(viewEntity);
                    continue;
                }

                var isFirstUpgrade = (business.Upgrade & UpgradeType.FirstUpgrade) != 0;
                var cost = isFirstUpgrade ? description.FirstUpgradeCost : description.SecondUpgradeCost;
                if (moneyComponent.Money < cost)
                {
                    _upgradeRequestPool.Del(viewEntity);
                    continue;
                }

                MoneyUtils.ChangePlayerMoney(ref moneyComponent, ref moneyViewComponent, -cost);

                business.Upgrade |= business.Upgrade;
                businessView.TotalIncome.SetValue(MoneyUtils.GetIncome(ref business, description));

                var upgradeBinding = isFirstUpgrade ? businessView.FirstUpgradeBought : businessView.SecondUpgradeBought;
                upgradeBinding.SetValue(true);
                _upgradeRequestPool.Del(viewEntity);
            }
        }
    }
}

