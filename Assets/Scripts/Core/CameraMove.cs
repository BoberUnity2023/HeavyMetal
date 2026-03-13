using UnityEngine;

public enum CameraState
{
    Game,
    Face,
    Finish
}
public class CameraMove : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    //[SerializeField] LayerMask _layerMask;
    [SerializeField] private float _speed;    
    [SerializeField] private float _distanceMin;
    [SerializeField] private float _distanceMax;
    [SerializeField] private float _distanceToTarget;
    [SerializeField] private float _distanceFaceState;
    [SerializeField] private float _distanceFinishState;    
    [SerializeField] private float _distanceScaler;
    [SerializeField] private float _distanceGame;
    //[SerializeField] private float _freeDist;
    [SerializeField] private Vector3 _offset;

    private Camera _camera;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _cameraPosition;
    private CameraState _state;

    public int TargetState { get; private set; }

    private Vector3 CameraPosition
    {
        get
        {
            //Vector3 heroToCamera = (_target.position - _hub.Hero.CameraTarget.position).normalized;
            //float dist = Mathf.Min(_distanceToTarget, _freeDist);
            return _cameraPosition.position;
        }
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
        //_hub.Level.OnLevelComplete += Level_OnLevelComplete;
        //_hub.Level.OnLevelLost += Level_OnLevelLost;
        //_hub.Screen.OnOrientationChanged += Screen_OnOrientationChanged;
        _distanceGame = _distanceToTarget;
        //SetState(CameraState.Game);
        //SetByTargetState();
        _offset = new Vector3(2, 2, -5);
        GameObject pos = new GameObject();
        pos.transform.position = _target.position + _offset;
        pos.transform.parent = _target;
        _cameraPosition = pos.transform;
    }

    private void OnDestroy()
    {
        //_hub.Level.OnLevelComplete -= Level_OnLevelComplete;
        //_hub.Level.OnLevelLost -= Level_OnLevelLost;
        //_hub.Screen.OnOrientationChanged -= Screen_OnOrientationChanged;
    }

    private void Update()
    {
        Update_ScrollZoom();
        //Update_FieldOfView();

        //_freeDist = FreeDistance;        
    }

    private void FixedUpdate()
    {
        FixedUpdate_TryMove();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetCanMove(bool value)
    {
        enabled = value;
    }

    public void SetState(CameraState state)
    {
        //Debug.Log("Camera.SetState(" + state.ToString() + ")");
        if (state == _state)
            return;

        _state = state;        
        if (state == CameraState.Game)
        {
            //SetTarget(_hub.Hero.CameraTarget);
            _distanceMin = _distanceFaceState;
            _distanceToTarget = _distanceGame;
        }

        if (_state == CameraState.Face)
        {
            _distanceGame = _distanceToTarget;
            //SetTarget(_hub.Hero.CameraTargetLevelComplete);
            _distanceMin = _distanceFaceState;
            _distanceToTarget = _distanceFaceState;
        }

        if (_state == CameraState.Finish)
        {
            _distanceGame = _distanceToTarget;
            //SetTarget(_hub.Hero.CameraTargetLevelComplete);
            _distanceMin = _distanceFinishState;
            _distanceToTarget = _distanceFinishState;
        }
    }

    public void Restart(Vector3 position)
    {
        transform.position = position;
        SetState(CameraState.Game);
    }

    public void FixedUpdate_TryMove()
    {
        if (_target == null)
            return;

        Vector3 a = transform.position;
        Vector3 b = CameraPosition;
        float t = Time.fixedDeltaTime * _speed;
        transform.position = Vector3.Lerp(a, b, t);        

        Quaternion qa = transform.rotation;
        Quaternion qb = _target.rotation;
        transform.rotation = Quaternion.Lerp(qa, qb, t);

        transform.LookAt(_target);
    }

    private void Update_ScrollZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _distanceToTarget = Mathf.Clamp(_distanceToTarget += scroll, _distanceMin, _distanceMax);
    }

    private void Update_FieldOfView()
    {
        float aspectRatio = _hub.Screen.AspectRatio;
        float angle = -50f * aspectRatio + 148;
        _camera.fieldOfView = Mathf.Clamp(angle, 60, 120);
    }

    private void Level_OnLevelLost()
    {
        SetState(CameraState.Face);
    }

    private void Level_OnLevelComplete(int cakes)
    {
        SetState(CameraState.Finish);
    }  

    private void Screen_OnOrientationChanged(ScreenOrientation orientation)
    {
        if (orientation == ScreenOrientation.Vertical)
        {
            _distanceToTarget *= _distanceScaler;
            _distanceMin *= _distanceScaler;
            _distanceMax *= _distanceScaler;            
        }

        if (orientation == ScreenOrientation.Horizontal)
        {
            _distanceToTarget /= _distanceScaler;
            _distanceMin /= _distanceScaler;
            _distanceMax /= _distanceScaler;            
        }
    }

    /*private float FreeDistance
    {
        get
        {
            RaycastHit hit;

            Vector3 direction = transform.position - _hub.Hero.CameraTarget.transform.position;
            if (Physics.Raycast(_hub.Hero.CameraTarget.transform.position, direction, out hit, 100, _layerMask))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);
                return hit.distance;
            }
            else
            {
                //Debug.DrawRay(_hub.Hero.CameraTarget.transform.position, direction * 100, Color.red);
                //Debug.LogWarning("Camera. Did not Hit");
                return 1000;
            }
        }
    }*/    

    public void SetNextTargetState()
    {
        //if (_state != CameraState.Game)
        //    return;
        TargetState++;

        if (TargetState > 3) 
            TargetState = 0;

        SetByTargetState();
    }

    public void SetByTargetState(int state)
    {
        
        TargetState = state;        
        SetByTargetState();
    }

    public void SetByTargetState()
    {
        if (TargetState == 0)
        {
            //if (_hub.Level.GameType == GameType.Podnos)
            //{
            //    _hub.Hero.CameraPosition.localPosition = new Vector3(4, 2, 3);
            //    _hub.Hero.CameraPosition.localRotation = Quaternion.Euler(15, -105, 0);
            //    _hub.Hero.CameraTarget.localPosition = new Vector3(0, 0.8f, 2.5f);
            //    _distanceToTarget = 2.5f;
            //    _distanceGame = 2.5f;
            //}

            //if (_hub.Level.GameType == GameType.Race)
            //{
            _target.localPosition = new Vector3(0, 2.5f, -2.5f);
            _target.localRotation = Quaternion.Euler(25, -3, 0);
            _target.localPosition = new Vector3(0, 0.5f, 0.5f);
            _distanceToTarget = 4.8f;
            _distanceGame = 7.5f;
            _speed = 3.2f;
            //}
        }

        if (TargetState == 1)
        {
            //if (_hub.Level.GameType == GameType.Podnos)
            //{
            //    _hub.Hero.CameraPosition.localPosition = new Vector3(5, 5, 4);
            //    _hub.Hero.CameraPosition.localRotation = Quaternion.Euler(40, -105, 0);
            //    _hub.Hero.CameraTarget.localPosition = new Vector3(0, 0.5f, 1.5f);
            //    _distanceToTarget = 2.8f;
            //    _distanceGame = 2.8f;
            //}

            //if (_hub.Level.GameType == GameType.Race)
            //{
            //    _hub.Hero.CameraPosition.localPosition = new Vector3(4, 2, 3);
            //    _hub.Hero.CameraPosition.localRotation = Quaternion.Euler(15, -105, 0);
            //    _hub.Hero.CameraTarget.localPosition = new Vector3(0, 0.8f, 2.5f);
            //    _distanceToTarget = 2.5f;
            //    _distanceGame = 2.5f;
            //}
        }

        //if (TargetState == 2)
        //{
        //    _hub.Hero.CameraPosition.localPosition = new Vector3(7, 7, 3);
        //    _hub.Hero.CameraPosition.localRotation = Quaternion.Euler(25, -105, 0);
        //    _hub.Hero.CameraTarget.localPosition = new Vector3(0, 0.2f, 1.5f);
        //    _distanceToTarget = 1.5f;
        //    _distanceGame = 1.5f;
        //}

        //if (TargetState == 3)
        //{
        //    _hub.Hero.CameraPosition.localPosition = new Vector3(20, 6, 6);
        //    _hub.Hero.CameraPosition.localRotation = Quaternion.Euler(15, -100, 0);
        //    _hub.Hero.CameraTarget.localPosition = new Vector3(0, 0.5f, 1.2f);
        //    _distanceToTarget = 3.0f;
        //    _distanceGame = 3.0f;
        //}
    }
}
