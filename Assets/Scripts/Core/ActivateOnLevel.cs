using UnityEngine;

public enum ActivateType
{
    Last,
    Before
}

public class ActivateOnLevel : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    [SerializeField] private ActivateType _activateType;
    [SerializeField] private int _level;

    private void Start()
    {
        if (_hub == null)
        { 
            _hub = FindObjectOfType<Hub>();
            Debug.LogWarning("Hub == null: " + gameObject.name);
        }
        
        if (_activateType == ActivateType.Last)   
            gameObject.SetActive(_hub.Game.CurrentLevel >= _level);

        if (_activateType == ActivateType.Before)
            gameObject.SetActive(_hub.Game.CurrentLevel < _level);
    }
}
