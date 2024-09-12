using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TraficLight : MonoBehaviour
{
    [SerializeField] private MeshRenderer  _meshRenderer;
    [SerializeField] private int _greenTimer, _redTimer;
    [SerializeField] private bool _isTimeToGreenLight;
    [SerializeField] private List <Spawner> _spawner;
    public Dictionary<int, NavMeshAgent> CarsListCrossRoad = new Dictionary<int, NavMeshAgent>();
    public bool IsTimeToGreenLight => _isTimeToGreenLight;


    void Start()
    {
       if (_isTimeToGreenLight == true)
        {
            StartCoroutine(GreenLightCoroutine());
        }
       else
        {
            StartCoroutine(RedLightCoroutine());
        }
    }

    private IEnumerator GreenLightCoroutine()
    {
        _meshRenderer.material.color = Color.green;
        _isTimeToGreenLight = true;
        foreach (var spawner in _spawner)
        {
            spawner.SwiftTraficLightAction(true);

        }
        RestartMachine(true);

        yield return new WaitForSeconds(_greenTimer);
        StartCoroutine(RedLightCoroutine());
    }

    private IEnumerator RedLightCoroutine()
    {
        _meshRenderer.material.color = Color.red;
        _isTimeToGreenLight = false;
        foreach (var spawner in _spawner)
        {
            spawner.SwiftTraficLightAction(false);

        }

        yield return new WaitForSeconds(_redTimer);
        StartCoroutine(GreenLightCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("триггер активировался");
        if (other.TryGetComponent(out Vehicle car) && !_isTimeToGreenLight)
        {
            car.NavMeshAgent.speed = 0;
            Debug.Log(car.Index);
            if (!CarsListCrossRoad.ContainsKey(car.Index))
                CarsListCrossRoad.Add(car.Index, car.NavMeshAgent);
        }
        else if (other.TryGetComponent(out Vehicle car2) && _isTimeToGreenLight)
        {
            car2.NavMeshAgent.speed = 5;
            Debug.Log(car.Index);
            if (!CarsListCrossRoad.ContainsKey(car2.Index))
            CarsListCrossRoad.Add(car2.Index, car2.NavMeshAgent);
        }

    }

    private void RestartMachine(bool canMove)
    {
        if (canMove)
        {
            foreach (var car in CarsListCrossRoad)
            {
                car.Value.speed = 5;
                car.Value.gameObject.GetComponent<Vehicle>().GreenLightDetected = true;
            }

        }
    }




}
