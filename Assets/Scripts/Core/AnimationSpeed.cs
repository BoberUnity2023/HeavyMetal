using UnityEngine;

public class AnimationSpeed : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Start()
    {
        GetComponent<Animator>().speed = _speed;
    }
}
