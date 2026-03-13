using UnityEngine;

public class PathIndicator : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _height;
    private ParticleSystem.EmissionModule _emissionModule;

    public void Init(Hub hub)
    {
        _hub = hub;
    }

    private void Start()
    {
        _emissionModule = _particleSystem.emission;
    }

    private void FixedUpdate()
    {
        if (_hub.Level.IsComplete ||
            _hub.Level.IsLost)
        { 
            Hide();
            return;
        }

        Vector3 delta = new Vector3(-_hub.Joistick.Delta.y, 0, _hub.Joistick.Delta.x - _hub.Joistick.Delta.y * 0.50f);
        //float groundHeight = _hub.Hero.Move.GroundHeight(delta);
        delta = new Vector3(delta.x, 0, delta.z);

        if (delta.magnitude > 0.15f)
        {
            //transform.position = _hub.Hero.transform.position + delta * 1.5f - Vector3.up * groundHeight;
            _emissionModule.enabled = true;
            DrawLine();            
        }
        else        
            Hide();        
    }

    private void DrawLine()
    {
        //_lineRenderer.enabled = true;
        //float x = _hub.Hero.transform.position.x;
        //float y = _hub.Hero.Move.GroundY;
        //float z = _hub.Hero.transform.position.z;
        //_lineRenderer.SetPosition(0, new Vector3(x, y, z) + Vector3.up * _height);
        //_lineRenderer.SetPosition(1, transform.position + Vector3.up * _height);
    }

    private void Hide()
    {
        _lineRenderer.enabled = false;
        _emissionModule.enabled = false;
    }
}
