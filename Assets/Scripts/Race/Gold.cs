using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] private int _count;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            Car car = other.GetComponentInParent<Car>();
            gameObject.SetActive(false);
            if (!car.IsAI)
                car.Hub.Game.Saves.Coins += _count;
        }
    }
}
