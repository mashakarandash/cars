using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoitingMan : MonoBehaviour
{
    [SerializeField] private ScriptableObjectPoolData _allCars;
    [SerializeField] private float _convertionDuration;

    private bool _isCarsConverted;

    private void OnMouseDown()
    {
        StartCoroutine(ConvertAllCarsInToYellowCars());
    }

    private IEnumerator ConvertAllCarsInToYellowCars()
    {
        if (_isCarsConverted)
        {
            yield return null;
        }
        else
        {
            _isCarsConverted = true;
            foreach (var car in _allCars.AllCars)
            {
                car.ConvertCarInToYellowCar();
            }

            yield return new WaitForSeconds(_convertionDuration);
            _isCarsConverted = false;

            foreach (var car in _allCars.AllCars)
            {
                car.ConvertCarInToDefault();
            }
        }
    }
}
