using UnityEngine;
using UnityEngine.UI;

public class Joistick : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _borderSector;
    [SerializeField] private Image _borderSectorImage;
    [SerializeField] private float _size;
    [SerializeField] private Vector2 _delta;
    [SerializeField] private Vector2 _deltaKeyboard;
    [SerializeField] private Vector2 _d1;
    [SerializeField] private float _keyboardMoveForce;
    [SerializeField] private float _keyboardMoveBrake;
    private float _shiftSpeed;
    private bool _isTouched;
    
    string info;    

    public Vector2 Delta => _delta;
    public Vector2 DeltaKeyboard => _deltaKeyboard;

    private void Start()
    {
        SetSize();
        SetAlpha();
        Hide();
    }

    private void Update()
    {
        Update_MoveKeyboard();
    }

    private void LateUpdate()
    { 
        if (_hub.Control.Type != ControlType.Mobile)
            return;        

        if (_hub.Game.Device == Device.Mobile)
        {
            //Debug.Log("TouchCount: " + Input.touchCount);
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                //if (Input.touchCount == 1)
                //{
                    //Debug.Log("TouchPos: " + touch.position);
                //}

                //if (Input.touchCount == 2)
                //{
                    //Debug.Log("Touch1Pos: " + touch.position);
                    //Touch touch1 = Input.GetTouch(1);//Новый
                    //Debug.Log("Touch2Pos: " + touch1.position);
                //}                

                if (touch.phase == TouchPhase.Began)
                    OnTouchBegan(touch.position);

                if (touch.phase == TouchPhase.Moved)
                    OnTouchMoved(touch.position);

                if (touch.phase == TouchPhase.Ended)
                    OnTouchEnded();
            }
        }

        if (_hub.Game.Device == Device.Desktop)
        {
            if (Input.GetMouseButtonDown(0))
                OnTouchBegan(Input.mousePosition);

            if (Input.GetMouseButton(0))
                OnTouchMoved(Input.mousePosition);

            if (Input.GetMouseButtonUp(0))
                OnTouchEnded();
        }

        LateUpdate_BorderSectorRotate();
        LateUpdate_BorderSectorSetTransparancy();
    }

    public void SetSize()
    {
        _size = _size * Screen.height / 1080;        
    }

    public void SetAlpha()
    {        
        _canvasGroup.alpha = PlayerPrefs.GetFloat("JoystickAlpha", 0.75f);        
    }

    private void LateUpdate_BorderSectorRotate()
    {
        float _angle = -45;
        
        if (_delta != Vector2.zero)
            _angle = Vector2.Angle(Vector2.up, _delta);

        if (_delta.x > 0)
            _angle *= -1;
        
        _borderSector.rotation = Quaternion.Euler(0, 0, _angle);
    }

    private void LateUpdate_BorderSectorSetTransparancy()
    {
        float a = Mathf.Clamp((_delta.magnitude - 0.25f) * 2, 0, 1);
        _borderSectorImage.color = new Color(1, 1, 1, a);
    }

    public void OnTouchBegan(Vector2 touchPosition)
    {
        if (_hub.Control.IsOnUI(touchPosition))
            return;

        if (_hub.Level.IsLost || _hub.Level.IsComplete)
            return;

        _isTouched = true;
        transform.position = touchPosition;
        _center.transform.position = touchPosition;
        Show();

        //if ((Mathf.Abs(Input.mousePosition.x - transform.position.x) < _size) &&
        //    Mathf.Abs(Input.mousePosition.y - transform.position.y) < _size)
        //{
        //_isTouch = true;            
        //}        
    }

    public void OnTouchMoved(Vector2 touchPosition)
    {
        if (_hub.Control.IsOnUI(touchPosition))
            return;

        if (!_isTouched)
            return;

        float x = touchPosition.x - transform.position.x;
        float y = touchPosition.y - transform.position.y;

        Vector3 delta = new Vector2(x, y) / _size;
        _d1 = delta;
        float magnitude = delta.magnitude;
        delta = delta.normalized * Mathf.Min(magnitude, 1);

        _center.transform.position = transform.position + delta * _size;
        _delta = delta;
    }

    public void OnTouchEnded()
    {
        _isTouched = false;
        ResetCenter();
        Hide();
    }

    public void ResetCenter()
    {        
        _delta = new Vector2(0, 0);
        _center.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);        
    }

    public void ResetDelta()
    {        
        transform.position = _center.transform.position;
        _delta = new Vector2(0, 0);        
    }

    private void Show()
    {        
        transform.localScale = Vector3.one;
    }

    private void Hide()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update_MoveKeyboard()
    {
        float shiftSpeed = Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift) ? 1.6f : 1;
        _shiftSpeed = Mathf.Lerp(_shiftSpeed, shiftSpeed, 0.5f);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _deltaKeyboard.x = Mathf.Min(_deltaKeyboard.x + Time.deltaTime * _keyboardMoveForce, 0.5f * _shiftSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _deltaKeyboard.x = Mathf.Max(_deltaKeyboard.x - Time.deltaTime * _keyboardMoveForce, -0.4f * _shiftSpeed);
        }

        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            _deltaKeyboard.x = Mathf.Lerp(_deltaKeyboard.x, 0, Time.deltaTime * _keyboardMoveBrake);
        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            _deltaKeyboard.y = Mathf.Min(_deltaKeyboard.y + Time.deltaTime * _keyboardMoveForce, 0.4f * _shiftSpeed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            _deltaKeyboard.y = Mathf.Max(_deltaKeyboard.y - Time.deltaTime * _keyboardMoveForce, -0.4f * _shiftSpeed);
        }

        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            _deltaKeyboard.y = Mathf.Lerp(_deltaKeyboard.y, 0, Time.deltaTime * _keyboardMoveBrake);
        }
    }
}
