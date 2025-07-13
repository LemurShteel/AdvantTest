using Components;
using Leopotam.EcsLite;
using Saving;

namespace Systems
{
    public class SaveGameOnExitSystem : IEcsPostDestroySystem
    {
        private readonly GameStateSave _stateSave;

        public SaveGameOnExitSystem(GameStateSave stateSave)
        {
            _stateSave = stateSave;
        }

        public void PostDestroy(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var moneyFilter = world.Filter<PlayerMoneyComponent>().End();
            ref var money = ref world.GetPool<PlayerMoneyComponent>().Get(moneyFilter.GetRawEntities()[0]);
            _stateSave.playerMoney = money.Money;

            _stateSave.businessSaves.Clear();
            var businessFilter = world.Filter<BusinessComponent>().End();
            foreach (var businessEntity in businessFilter)
            {
                ref var business = ref world.GetPool<BusinessComponent>().Get(businessEntity);
                _stateSave.businessSaves.Add(new BusinessSave()
                {
                    Id = business.ID,
                    IncomeTimeProgress = business.IncomeTimeProgress,
                    Level = business.Level,
                    Upgrade = business.Upgrade
                });
            }
            _stateSave.SaveToPrefs();
        }
    }
}
