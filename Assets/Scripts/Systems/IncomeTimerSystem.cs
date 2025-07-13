using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class IncomeTimerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _businessFilter;
        private EcsPool<BusinessComponent> _businessPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _businessFilter = world.Filter<BusinessComponent>().End();
            _businessPool =  world.GetPool<BusinessComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var businessEntity in _businessFilter)
            {
                ref var business = ref _businessPool.Get(businessEntity);
                if (business.Level < 1)
                {
                    continue;
                }
                business.IncomeTimeProgress += Time.deltaTime;
            }
        }
    }
}
