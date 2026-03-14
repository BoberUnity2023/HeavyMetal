using UnityEngine;

public class CarInput : MonoBehaviour, ICarInputable
{
    [SerializeField] private Hub _hub;

    private Vector2 _delta;
    private Vector2 _deltaKeyboard;
    private bool _isPressedLeft;
    private bool _isPressedRight;
    private bool _isPressedForce;
    private bool _isPressedBrake;
    private bool _isPressedHandbrake;
    private const float steeringSpeed = 2;
    private const float steeringBackSpeed = 4;

    public float Steer 
    { 
        get 
        { 
            return _delta.x; 
        } 
    }

    public float Force
    {
        get
        {
            return _delta.y;
        }
    }

    public float Brake
    {
        get
        {
            if (_hub.Level.Race.Car.CarControl.IsAccelerating)
                return 0;

            if (_delta.y >= 0)
            {
                if (_hub.Level.Race.Car.CarControl.Speed > 0)
                    return 0;

                return _delta.y;//Тормоза при нажатии Вперёд - заднем ходе
            }

            return -_delta.y;
        }
    }

    public float Handbrake
    {
        get
        {  
            return Input.GetKey(KeyCode.Space) || _isPressedHandbrake ? 1 : 0;
        }
    }

    public float Reverse
    {
        get
        {
            if (_delta.y >= 0)
                return 0;

            if (!_hub.Level.Race.Car.CarControl.IsAccelerating)
                return 0;

            return -_delta.y;
        }
    }    
    
    private void Update()
    {
        Update_KeyboardArrowcControl();
        //Update_JoystickControl();        
    } 
    
    private void Update_KeyboardArrowcControl()
    {
        Vector2 inputVector = _deltaKeyboard;// new Vector2(Delta.x, Delta.y);

        if (!Input.GetKey(KeyCode.UpArrow) && !_isPressedForce &&
            !Input.GetKey(KeyCode.DownArrow) && !_isPressedBrake)
        {
            inputVector.y = 0;
        }

        if (!Input.GetKey(KeyCode.RightArrow) && !_isPressedRight && 
            !Input.GetKey(KeyCode.LeftArrow) && !_isPressedLeft)
        {
            if (inputVector.x > 0)
                inputVector.x = Mathf.Max(0, inputVector.x - Time.deltaTime * steeringBackSpeed);

            if (inputVector.x < 0)
                inputVector.x = Mathf.Min(0, inputVector.x + Time.deltaTime * steeringBackSpeed);
        }

        if (Input.GetKey(KeyCode.UpArrow) || _isPressedForce)
            inputVector.y = 1;

        if (Input.GetKey(KeyCode.DownArrow) || _isPressedBrake)
            inputVector.y = -1; 
        

        if (Input.GetKey(KeyCode.LeftArrow) || _isPressedLeft)
            inputVector.x = Mathf.Max(inputVector.x - Time.deltaTime * steeringSpeed, -1);

        if (Input.GetKey(KeyCode.RightArrow) || _isPressedRight)
            inputVector.x = Mathf.Min(inputVector.x + Time.deltaTime * steeringSpeed, 1);

        _deltaKeyboard = inputVector;
        _delta = inputVector;
    }

    private void Update_JoystickControl()
    {
        if (!Input.GetKey(KeyCode.UpArrow) && !_isPressedForce &&
            !Input.GetKey(KeyCode.DownArrow) && !_isPressedBrake &&
            !Input.GetKey(KeyCode.RightArrow) && !_isPressedRight &&
            !Input.GetKey(KeyCode.LeftArrow) && !_isPressedLeft &&
            _deltaKeyboard == Vector2.zero)//TODO: Добавить плавное возвращение руля при отпускании джойстика
        {
            _delta.x = -_hub.Joistick.Delta.y;
            _delta.y = _hub.Joistick.Delta.x;
        }
    }

    public void PointerDownLeft()
    {
        _isPressedLeft = true;
    }

    public void PointerUpLeft()
    {
        _isPressedLeft = false;
    }

    public void PointerDownRight()
    {
        _isPressedRight = true;
    }

    public void PointerUpRight()
    {
        _isPressedRight = false;
    }

    public void PointerDownForce()
    {
        _isPressedForce = true;
    }

    public void PointerUpForce()
    {
        _isPressedForce = false;
    }

    public void PointerDownBrake()
    {
        _isPressedBrake = true;
    }

    public void PointerUpBrake()
    {
        _isPressedBrake = false;
    }

    public void PointerDownHandbrake()
    {
        _isPressedHandbrake = true;
    }

    public void PointerUpHandbrake()
    {
        _isPressedHandbrake = false;
    }
}
