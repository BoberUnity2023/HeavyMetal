using TMPro;
using UnityEngine;

public class ReturnOnRoad : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private float _collapsTimePlayer;
    [SerializeField] private float _collapsTimeEnemy;
    private float _currentCollapsTime = 0;
    private bool isEffect = false;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //    MoveToNearestReturnPoint();
        if (!_car.Hub.Level.Race.IsStarted || 
            _car.IsFinished ||              
            _car.IsCrashed ||
            _car.Input.Handbrake > 0.01f)
            return;

        bool isTurned = transform.eulerAngles.z > 80 && transform.eulerAngles.z < 280;
        bool slippage = Mathf.Abs(_car.Speed) < 2 && Mathf.Abs(_car.Input.Force) > 0.5f;

        if ((slippage || isTurned) && CanReturn)
        {
            _currentCollapsTime += Time.deltaTime;
            if (_currentCollapsTime > 0.7f && !isEffect)
            {
                isEffect = true;
                ChangeOutlineColor();
            }

            if (_currentCollapsTime > CollapsTime)
            {
                MoveToNearestReturnPoint();
            }
        }
        else
        {
            _currentCollapsTime = 0;
            if (isEffect && _car.Speed > 3)
            {
                isEffect = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Dead")
        {
            _car.DamageCounter.DamageAdd(100, false);            
        }
    }

    public void MoveToNearestReturnPoint()
    {
        _car.Rigidbody.linearVelocity = Vector3.zero;
        _currentCollapsTime = 0;
        int currentPoint = Mathf.Max(0, _car.LapsCounter.CurrentPoint - 1);
        Transform wayPoint = _car.WayPath.Points[currentPoint];
        float y = PointPositionOnGround(wayPoint.position);
        transform.localPosition = new Vector3(wayPoint.position.x, y, wayPoint.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        Debug.Log(gameObject.name + " was returned on point " + currentPoint);
    }

    private void ChangeOutlineColor()
    {
        //meshRenderer.material.SetColor("_OutlineColor", isWhite ? Color.black : Color.white);
        //isWhite = !isWhite;
        //if (isEffect)
        //    Invoke("ChangeOutlineColor", whiteInterval);
        //else
        //    meshRenderer.material.SetColor("_OutlineColor", Color.black);
    }

    private float CollapsTime
    {
        get
        {
            return _car.IsAI ? _collapsTimeEnemy : _collapsTimePlayer;
        }
    }

    private bool CanReturn
    {
        get
        {
            if (!_car.IsAI)
                return true;

            return !_car.IsVisible;
        }
    }

    private float PointPositionOnGround(Vector3 position)
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(position + Vector3.up * 20, Vector3.down, 100.0F);

        for (int i = 0; i < hits.Length - 1; i++)//Without Blocker
        {
            RaycastHit hit = hits[i];
            if (_car.Hub.Game.IsMaterialGround(hit.collider.material))
            {
                return position.y + 20 - hit.distance + 2.5f;
            }
        }

        return position.y;
    }   
}
