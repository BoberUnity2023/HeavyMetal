using UnityEngine;
//Do not used
public class CakeLoader : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private GameObject _body;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Hero" || other.gameObject.name.Contains("PodnosFloor"))
        {
            _hub.Level.CreateCakes(_hub.Level.Level.NumOfCakesOnStart);
            Destroy(_body);
            Destroy(this);
        }
    }
}
