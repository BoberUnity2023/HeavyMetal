using UnityEngine;

public enum PushedType
{
    Horizontal,
    Vertical,
    Both
}

public class PushedObject : MonoBehaviour
{
    [SerializeField] private PushedType _type;    

    private void Awake()
    {
        gameObject.tag = "Pushed";
    }

    public Vector3 ForceDirection(Hub hub, ControllerColliderHit hit)
    {
        

        if (_type == PushedType.Vertical)
        { 
            return -Vector3.up * 50 * Time.fixedDeltaTime; 
        }

        //if (_type == PushedType.Both)
        //{
        //    Vector3 forceDirection = (transform.position - hub.Hero.transform.position);
        //    forceDirection.y = -1;
        //    forceDirection.Normalize();
        //    return forceDirection;
        //}

        return Vector3.zero;
    }
}
