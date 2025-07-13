using Bindings;
using Components;
using Leopotam.EcsLite;
using Providers;
using UnityEngine;

namespace UI
{
    public class UIBusinessesView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _businessUnitsContent;
        [SerializeField]
        private UIBusinessUnitView _businessUnitViewPrefab; 
    
        [SerializeField]
        private ValueBinder<double> _money;

        private int _entity;

        public void Init(IEcsSystems systems, IDescriptionsProvider descriptionsProvider, INamingProvider namingProvider)
        {
            var businessDescriptions = descriptionsProvider.GetBusinessDescriptions();
            foreach (var (businessId, businessDescription) in businessDescriptions)
            {
                Instantiate(_businessUnitViewPrefab, _businessUnitsContent).Init(systems, businessId, businessDescription, namingProvider);
            }

            var world = systems.GetWorld();

            _entity = world.NewEntity();
            var moneyViewPool = world.GetPool<PlayerMoneyViewComponent>();
            moneyViewPool.Add(_entity);
            ref var componentView = ref moneyViewPool.Get(_entity);
            componentView.Money = _money;
        }
    }
}
