using DG.Tweening;
using System.Reflection.Emit;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private GameObject _butterfly;
    [SerializeField] private GameObject[] _walls;
    [Range(0, 100)][SerializeField] private int _chanceActive;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    
}
