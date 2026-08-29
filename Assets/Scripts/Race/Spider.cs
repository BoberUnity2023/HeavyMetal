using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Waypoint _waipoint;
    [SerializeField] private Animation _animation;
    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _crashed;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;    
    [SerializeField] private float _collisionForce;
    [SerializeField] private int _damage;
    [Range(0, 100)][SerializeField] private int _chanceActive;

    private const float _timeJump = 0.5f;
    private const float _timeAway = 3.5f;

    private int _currentPoint;
    private bool _isMovingToPath;    
    private Vector3 _directionTarget;
    private Vector3 _directionSmooth;
    private Vector3 _directionPrevious;

    public int CurrentPoint
    {
        get { return _currentPoint; }
        set { _currentPoint = value; }
    }

    public bool IsDead { get; private set; }

    private void Start()
    {
        if (Random.value * 100 > _chanceActive)
            Destroy(gameObject);

        _isMovingToPath = true; ;
    }

    private void FixedUpdate()
    {
        if (_isMovingToPath)
            MoveToPath();

        //TryAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hub.Level.IsLost || !_isMovingToPath)
            return;

        if (other.gameObject.tag == "Car")
        {
            Car car = other.GetComponentInParent<Car>();
            if (car == null || !car.IsVisible)
                return;

            if (car.Speed < 3)
            {
                Attack(other);
                return;
            }

            _isMovingToPath = false;
            Blast();

            if (car.IsAI)
            {
                Blast();
            }

            if (!car.IsAI)
            {
                if (Random.value < 0.35f)
                    JumpToCamera();
                else
                    Blast();
            }

            car.Rigidbody.AddForce(-car.Rigidbody.transform.forward * _collisionForce);
            car.DamageCounter.DamageAdd(_damage, false);
            return;
        }

        //if (_hub.Hero.IsHeroOrCakeOrPodnos(other.gameObject))
        //{
        //    Attack(other);
        //}
    }

    private void JumpToCamera()
    {
        _crashed.SetActive(true);
        _body.transform.SetParent(Camera.main.transform);

        float x = Random.Range(-0.3f, 0.3f);
        float y = Random.Range(-0.3f, 0.3f);
        Vector3 finish = new Vector3(x, y, 0.45f);
        _body.transform.DOLocalMove(finish, _timeJump);
        _body.transform.DOLocalRotate(Vector3.right * 90, _timeJump).OnComplete
            (
                () => _body.transform.DOLocalMove(-Vector3.right * 1.5f, _timeAway).OnComplete
                (
                    () => Dead()
                )
            );
    }

    private IEnumerator AfterOnTriggerEnter(Collider other, float time)
    {
        yield return new WaitForSeconds(time);
        //TODO: Wheel Ba-bah
        //_hub.Hero.ForceCakes(gameObject.name);        
    }

    private IEnumerator AfterAttack(float time)
    {
        yield return new WaitForSeconds(time);
        _isMovingToPath = true;
        _animation.Play("walk");
    }

    private void MoveToPath()
    {
        Vector3 _direction = _waipoint.Waipoints[_currentPoint].position - transform.position;
        if (_direction.magnitude < 0.2f)
        {
            //Debug.Log("Точка " + _currentPoint + " достигнута");
            if (_currentPoint == _waipoint.Waipoints.Length - 1)
            {
                Finish();
                return;
            }
            else
                _currentPoint++;
        }
        _directionTarget = _direction.normalized; 
        CalculateRotationSmooth();
        RotationForward(_directionSmooth);
        
        Move();
    }

    private void Attack(Collider other)
    {
        _isMovingToPath = false;

        Vector3 _direction = _hub.Level.Race.Car.transform.position - transform.position;
        _directionTarget = _direction.normalized;

        _animation.Play("Attack");
        _hub.Level.Race.Car.DamageCounter.DamageAdd(_damage, false);
        StartCoroutine(AfterOnTriggerEnter(other, _animation["Attack"].clip.length / 2));
        StartCoroutine(AfterAttack(_animation["Attack"].clip.length));
    }

    private void TryAttack()
    {
        if (Vector3.Distance(transform.position, _hub.Level.Race.Car.transform.position) < 0.3f)
        { 
            _animation.Play("Attack");
        }
    }

    void Finish()
    {
        //Debug.Log("Путь пройден");
        _currentPoint = 0;             
    }

    private void CalculateRotationSmooth()
    {
        _directionSmooth = Vector3.Lerp(_directionPrevious, _directionTarget, _rotationSpeed * Time.fixedDeltaTime);
        _directionPrevious = _directionSmooth;
    }

    private void Move()
    {        
        Vector3 _position = transform.position + _directionSmooth * _speed * Time.fixedDeltaTime;
        transform.position = _position;
    }

    private void RotationForward(Vector3 _direction)
    {
        _direction.y = 0;
        if (_direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_direction);
    }

    private void Dead()
    {
        _body.gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
        IsDead = true;
    }

    public void Blast()
    {
        _crashed.SetActive(true);
        _body.transform.DOScale(Vector3.zero, 1.0f).OnComplete(() => Dead());
    }
}
