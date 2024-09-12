using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furgon : Vehicle
{
    private EventBus _eventBus;

    private void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        _eventBus = ServiceLocator.Instance.GetRegisterService<EventBus>();
    }

    public override void OnMouseDown()
    {
        if (_eventBus.IsTimerActive == false)
        {
          _eventBus.DoubleScore.Invoke();
          gameObject.SetActive(false);
          _eventBus.IsTimerActive = true;
        }
        else
        {
            _eventBus.OnRestartTimer.Invoke();
            gameObject.SetActive(false);
        }
    }

    
}
