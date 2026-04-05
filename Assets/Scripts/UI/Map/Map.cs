using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject[] _maps;
    [SerializeField] private Vector2[] _offsets;

    private void Start()
    {
        ShowMap();
    }

    private void Update()
    {
        Transform player = _hub.Level.Race.Car.transform;
        Vector2 offset = _offsets[_hub.Game.CurrentLevel - 1];
        _player.localPosition = new Vector3(-player.position.x + offset.x, -player.position.z + offset.y, 0);
    }

    private void ShowMap()
    {
        for (int i = 0; i < _maps.Length; i++)
        {
            _maps[i].SetActive(i == _hub.Game.CurrentLevel - 1);
        }
    }
}
