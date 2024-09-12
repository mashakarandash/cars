using System.Collections;
using UnityEngine;

public class CarBehavior : Vehicle
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
        _eventBus.MinusLifeAction.Invoke();
        gameObject.SetActive(false);

    }
}
