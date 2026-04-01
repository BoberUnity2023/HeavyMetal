using UnityEngine;

public class CarRotate : MonoBehaviour
{    
    private void Update()
    {
        transform.Rotate(0, 30 * Time.deltaTime, 0);
    }
}
