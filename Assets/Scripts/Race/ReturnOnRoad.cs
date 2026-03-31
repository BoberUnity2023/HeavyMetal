using UnityEngine;

public class ReturnOnRoad : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private float _collapsTime = 5;    
    private float _currentCollapsTime = 0;
    private bool isEffect = false;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //    MoveToNearestReturnPoint();


        if (!_car.IsFinished && _car.Input.Handbrake < 0.01f)
        {
            if (_car.Rigidbody.linearVelocity.magnitude < 2 && _car.Input.Force > 0.5f && CanReturn)
            {
                _currentCollapsTime += Time.deltaTime;
                if (_currentCollapsTime > 0.7f && !isEffect)
                {
                    isEffect = true;
                    ChangeOutlineColor();
                }

                if (_currentCollapsTime > _collapsTime)
                {
                    MoveToNearestReturnPoint();
                }
            }
            else
            {
                _currentCollapsTime = 0;
                if (isEffect && _car.Rigidbody.linearVelocity.magnitude > 3)
                {
                    isEffect = false;
                }
            }
        }
    }

    private void MoveToNearestReturnPoint()
    {
        _car.Rigidbody.linearVelocity = Vector3.zero;
        _currentCollapsTime = 0;
        int currentPoint = Mathf.Max(0, _car.LapsCounter.CurrentPoint - 1);
        Transform wayPoint = _car.WayPath.Points[currentPoint];
        transform.position = wayPoint.position;
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

    private bool CanReturn
    {
        get
        {
            if (!_car.IsAI)
                return true;

            return !_car.IsVisible;
        }
    }
}
