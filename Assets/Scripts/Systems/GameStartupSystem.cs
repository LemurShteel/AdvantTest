using System.Collections.Generic;
using System.Linq;
using Components;
using Leopotam.EcsLite;
using Providers;
using Saving;
using Utils;

namespace Systems
{
    public class GameStartupSystem : IEcsInitSystem
    {
        private readonly GameStateSave _stateSave;
        private readonly Dictionary<int, BusinessDescription> _businessesDescriptions;

        public GameStartupSystem(GameStateSave stateSave, IDescriptionsProvider descriptionsProvider)
        {
            _stateSave = stateSave;
            _businessesDescriptions = descriptionsProvider.GetBusinessDescriptions();
        }

        public void Init(IEcsSystems systems)
        {
            _stateSave.LoadFromPrefs();

            var world = systems.GetWorld();
            var businessPool = world.GetPool<BusinessComponent>();
        
            var savedBusinesses = _stateSave.businessSaves.ToDictionary((e) => e.Id);
            foreach (var (businessId, description) in _businessesDescriptions)
            {
                var businessEntity = world.NewEntity();
                businessPool.Add(businessEntity);
                ref var business = ref businessPool.Get(businessEntity);
                business.ID = businessId;
            
                if (savedBusinesses.TryGetValue(businessId, out var savedBusiness))
                {
                    business.IncomeTimeProgress = savedBusiness.IncomeTimeProgress;
                    business.Level = savedBusiness.Level;
                    business.Upgrade = savedBusiness.Upgrade;
                }
                else
                {
                    business.Level = description.InitialLevel;
                }
            }
        
            var playerMoneyEntity = world.NewEntity();
            var playerMoneyPool = world.GetPool<PlayerMoneyComponent>();
            ref var playerMoney = ref playerMoneyPool.Add(playerMoneyEntity);
            
            var playerMoneyViewEntity = world.Filter<PlayerMoneyViewComponent>().End().GetRawEntities()[0];
            var playerMoneyViewPool = world.GetPool<PlayerMoneyViewComponent>();
            ref var playerMoneyView = ref playerMoneyViewPool.Get(playerMoneyViewEntity);
            
            MoneyUtils.ChangePlayerMoney(ref playerMoney, ref playerMoneyView, _stateSave.playerMoney);
        }
    }
}
