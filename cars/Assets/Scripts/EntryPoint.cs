using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private LevelGoal _levelGoal;
    [SerializeField] private ScriptableObjectPoolData _carPoolData;
    private ServiceLocator _serviceLocator;


    private EventBus _eventBus;
    
    void Awake()
    {
        _serviceLocator = new ServiceLocator();
        _carPoolData.Initialization();
        _eventBus = new EventBus();
        _serviceLocator.RegisterService(_levelGoal);
        _serviceLocator.RegisterService(_eventBus);
        _serviceLocator.RegisterService(_carPoolData);
    }
    
   
}
