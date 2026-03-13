using UnityEngine;

public class TimeLive : MonoBehaviour
{
    [SerializeField] private float time;

    void Start()
    {
        Destroy(gameObject, time);
    }
}
