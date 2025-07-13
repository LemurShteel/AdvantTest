using Leopotam.EcsLite;
using Providers;
using Saving;
using Systems;
using UI;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    private readonly GameStateSave _stateSave = new GameStateSave();
    
    [SerializeField]
    private UIBusinessesView _businessesView;
    [SerializeField]
    private DescriptionsProvider _descriptionsProvder;
    [SerializeField]
    private NamingProvider _namingProvider;
    
    private EcsWorld _world;
    private EcsSystems _systems;
    
    private void Awake()
    {
        _namingProvider.InitProvider();
        _descriptionsProvder.InitProvider();
    
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _systems.Add(new GameStartupSystem(_stateSave, _descriptionsProvder));
        _systems.Add(new IncomeTimerSystem());
        _systems.Add(new IncomeGenerationSystem(_descriptionsProvder));
        _systems.Add(new BusinessLevelUpSystem(_descriptionsProvder));
        _systems.Add(new BusinessUpgradeSystem(_descriptionsProvder));
        _systems.Add(new CreateBusinessViewLinkSystem(_descriptionsProvder));
        _systems.Add(new UpdateViewProgressSystem(_descriptionsProvder));
        _systems.Add(new SaveGameOnExitSystem(_stateSave));
    
        _businessesView.Init(_systems, _descriptionsProvder, _namingProvider);
        _systems.Init();
    }
    
    private void Update()
    {
        _systems.Run();
    }
    
    private void OnApplicationQuit()
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }
    
        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }
}