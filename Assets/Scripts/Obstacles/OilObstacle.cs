using UnityEngine;

public class OilObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            Car car = other.GetComponentInParent<Car>();
            car.Oil.OilStart(30);
        }
    }
}
