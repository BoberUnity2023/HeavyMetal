using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Transform _pointPlayer;
    [SerializeField] private Transform[] _pointEnemies;
    [SerializeField] private GameObject[] _maps;
    [SerializeField] private Vector2[] _offsets;

    private void Start()
    {
        ShowMap();
    }

    private void Update()
    {        
        //Vector2 offset = _offsets[_hub.Game.CurrentLevel - 1];
        
        Car player = _hub.Level.Race.Car;
        SetPointToCar(_pointPlayer, player);       
        

        for (int i = 0; i < _hub.Level.Race.Enemies.Count; i++)
        {
            Car enemy = _hub.Level.Race.Enemies[i];
            SetPointToCar(_pointEnemies[i], enemy);
            //_pointEnemies[i].localPosition = new Vector3(-enemy.position.x + offset.x, -enemy.position.z + offset.y, 0);
        }
    }

    private void SetPointToCar(Transform point, Car car)
    {
        Vector2 offset = _offsets[_hub.Game.CurrentLevel - 1];
        point.localPosition = new Vector3(-car.transform.position.x + offset.x, -car.transform.position.z + offset.y, 0);
    }

    private void ShowMap()
    {
        for (int i = 0; i < _maps.Length; i++)
        {
            _maps[i].SetActive(i == _hub.Game.CurrentLevel - 1);
        }
    }
}
