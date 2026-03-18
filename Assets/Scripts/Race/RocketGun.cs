using UnityEngine;

public class RocketGun : MonoBehaviour
{
    [SerializeField] private float _d;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _d = RayDistance;
    }

    float RayDistance
    {
        get
        {
            RaycastHit hit;

            Vector3 direction = transform.forward;
            if (Physics.Raycast(transform.position, direction, out hit, 100))
            {
                Debug.DrawRay(transform.position, direction, Color.yellow);
                return hit.distance;
            }
            else
            {
                Debug.DrawRay(transform.position, direction * 100, Color.red);
                //Debug.LogWarning("Camera. Did not Hit");
                return 1000;
            }
        }
    }
}
