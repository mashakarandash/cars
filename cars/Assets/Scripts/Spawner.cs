using System.Collections;
using UnityEngine;

[AddComponentMenu("My Game/Car")]
// [RequireComponent(typeof(Rigidbody2D))] - связывает указанный компонент с нашим классом и при добавлении нашего скрипта добавится указанный компонент и удалить его будет нельзя
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPosition;
        [Space(15)] //атрибуты, пространство межлду строками в инстепкторе
    [Header("Минимальное и максимальное значение для рандомайзера"), Space(5)]
    [SerializeField, Range(1, 10), Tooltip("это минимальное значение для рандомайзера")] private int _minValue; //Tooltip - подсказки
    [SerializeField, Range(1, 30)] private int _maxValue; // range - диапазон ввода значений
    [SerializeField, Range(1, 50)] private int _minSpeed;
    [SerializeField, Range(1, 50)] private int _maxSpeed;
    [SerializeField] private Transform _pointToMove;
    private ScriptableObjectPoolData _carPool;
    [SerializeField] private bool _canCreateCar = true;
    [SerializeField] private bool _shouldOvertake;
    [SerializeField] private TraficLight _traficLight;
    

    [HideInInspector] public float PublicField = 100; // убирает публичные поля внутри инспектора

    private bool _gameIsActive = true;
    private EventBus _eventBus;

    public DirectionToMove Directions;
    public static int CarIndex;

    private void Start()
    {
        Initialization();
        StartCoroutine(Waiting());
        _eventBus.StopGameAction += StopGame;
        _eventBus.RestartGameAction += RestartGame;
    }

    private void Initialization()
    {
        _eventBus = ServiceLocator.Instance.GetRegisterService<EventBus>();
        _carPool = ServiceLocator.Instance.GetRegisterService<ScriptableObjectPoolData>();
    }

    private void CreateCar()
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber >= 0 && randomNumber < 15) //указываем шанс выпадения. 0 - это включительно, 15 - не включительно.
        {
            var redCar = _carPool.RedCarPool.GetCar();
            DoCarSetting(redCar.gameObject);
        }
        if (randomNumber >= 15 && randomNumber < 30)
        {
            var blueCar = _carPool.BlueCarPool.GetCar();
            DoCarSetting(blueCar.gameObject);
        }
        if (randomNumber >= 30 && randomNumber < 45)
        {
            var greenCar = _carPool.GreenCarPool.GetCar();
            DoCarSetting(greenCar.gameObject);
        }
        if (randomNumber >= 45 && randomNumber < 70)
        {
            var yellowCar = _carPool.YellowCarPool.GetCar();
            DoCarSetting(yellowCar.gameObject);
        }
        if (randomNumber >= 70 && randomNumber < 80)
        {
            var policeCar = _carPool.PoliceCarPool.GetCar();
            DoCarSetting(policeCar.gameObject);
        }
        if (randomNumber >= 80 && randomNumber < 90)
        {
            var rainbowCar = _carPool.RainbowCarPool.GetCar();
            DoCarSetting(rainbowCar.gameObject);
        }
        if (randomNumber >= 90 && randomNumber < 100)
        {
            var furgon = _carPool.FurgonPool.GetCar();
            DoCarSetting(furgon.gameObject);
        }
    }

    private IEnumerator Waiting() //корутина 
    {

        while (_gameIsActive)
        {
           yield return new WaitForSeconds(GetRandomNumber());
            if (_gameIsActive && _canCreateCar)
            {
                CreateCar();
            }
        }
    }

    private int GetRandomNumber()
    {
        int number = Random.Range(_minValue, _maxValue);
        return number;
    }

    private void StopGame()
    {
        _gameIsActive = false;
    }

    private void RestartGame()
    {
        _gameIsActive = true;
        StartCoroutine(Waiting());
    }

    private void DoCarSetting(GameObject car) //передаем в метод игровой объект какой-то машинки, чтобы далее настроить ей скорость и местоположение (полосу)
    {
        int speed = Random.Range(_minSpeed, _maxSpeed); // скорости каждой машинки, диапазон
        car.TryGetComponent(out Vehicle vehicle);
        vehicle.Initialiaze(_traficLight);
        vehicle.SetSpeed(speed); //передаем скорость которую мы зарандомили
        //car.transform.position = _spawnPosition.transform.position; // выставляе  позицию спауна
        vehicle.TeleportTonewPosition(_spawnPosition.transform.position);

        vehicle.SetDestinationToPoint(_pointToMove.position, _shouldOvertake);
        Spawner.CarIndex++;
        vehicle.Index = CarIndex++;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    public void SwiftTraficLightAction(bool isGreenLight)
    {
        _canCreateCar = isGreenLight;
    }

  
}

public enum DirectionToMove
{
    left = 0,
    right = 1,
    up = 2,
    down = 3
}
