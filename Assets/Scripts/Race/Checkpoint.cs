using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Hub _hub = null;
    [SerializeField] private Checkpoint _prevCheckpoint = null;
    [SerializeField] private Checkpoint _nextCheckpoint = null;
    [SerializeField] private Text _indicatorFinishPosition = null;
    [SerializeField] private AudioClip _lapSound = null;
    [SerializeField] private AudioClip _levelSucess = null;
    [SerializeField] private AudioClip _levelFailure = null;
    [SerializeField] private Image[] _starImages = null;
    [SerializeField] private Sprite _starOnSprite = null;
    [SerializeField] private bool _isDown = true;//На выезде z грузовика меньше z чекпоинта - правильное направление
    [SerializeField] private Transform _finishWindowButtons = null;
    [SerializeField] private Transform _finishWindowButtonsLosePosition = null;
    [SerializeField] private GameObject _nextLevelButton = null;
    private int _mesto = 1;
    

    private void OnTriggerExit(Collider other)
    {
        Debug.LogWarning("Checkpoint OnTriggerExit: " + other.gameObject.name);        
    }
}
