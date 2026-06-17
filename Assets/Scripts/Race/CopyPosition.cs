using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] private Transform target = null;

    private void FixedUpdate()
    {
        transform.position = target.position;
        //transform.rotation = target.rotation;
    }
}
