using UnityEngine;

public class AudioListenerMovier : MonoBehaviour
{
    private bool _isInited;
    private Transform _target;

    public void Init(Transform target)
    {
        _isInited = true; 
        _target = target;
        enabled = true;        
    }
    
    void Update()
    {
        if (!_isInited || _target == null)
            return;

        transform.position = _target.position;
        transform.rotation = _target.rotation;
    }
}
