using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using Providers;
using Utils;

namespace Systems
{
    public class IncomeGenerationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly Dictionary<int, BusinessDescription> _businessesDescriptions;

        private EcsFilter _businessFilter;
        private EcsPool<BusinessComponent> _businessPool;

        private EcsFilter _playerMoneyFilter;
        private EcsPool<PlayerMoneyComponent> _playerMoneyPool;

        private EcsFilter _playerMoneyViewFilter;
        private EcsPool<PlayerMoneyViewComponent> _playerMoneyViewPool;

        public IncomeGenerationSystem(IDescriptionsProvider descriptionsProvider)
        {
            _businessesDescriptions = descriptionsProvider.GetBusinessDescriptions();
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _businessFilter = world.Filter<BusinessComponent>().End();
            _businessPool =  world.GetPool<BusinessComponent>();
        
            _playerMoneyFilter = world.Filter<PlayerMoneyComponent>().End();
            _playerMoneyPool = world.GetPool<PlayerMoneyComponent>();

            _playerMoneyViewFilter = world.Filter<PlayerMoneyViewComponent>().End();
            _playerMoneyViewPool = world.GetPool<PlayerMoneyViewComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            var playerMoney = _playerMoneyFilter.GetRawEntities()[0];
            ref var moneyComponent = ref _playerMoneyPool.Get(playerMoney);
        
            var playerMoneyView = _playerMoneyViewFilter.GetRawEntities()[0];
            ref var moneyViewComponent = ref _playerMoneyViewPool.Get(playerMoneyView);

            foreach (var businessEntity in _businessFilter)
            {
                ref var business = ref _businessPool.Get(businessEntity);
                if (!_businessesDescriptions.TryGetValue(business.ID, out var description)
                    || business.Level < 1
                    || business.IncomeTimeProgress <= description.BaseIncomeDelaySeconds)
                {
                    continue;
                }

                var income = MoneyUtils.GetIncome(ref business, description);
                MoneyUtils.ChangePlayerMoney(ref moneyComponent, ref moneyViewComponent, income);
                business.IncomeTimeProgress = 0;
            }
        }
    }
}

