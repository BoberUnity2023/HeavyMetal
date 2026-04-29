using UnityEngine;

public class ObstacleLine : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _size;

    private void Start()
    {
        Vector3 position = transform.position + transform.right * Random.Range(-_size, _size);
        Instantiate(_prefab, position, transform.rotation, transform);        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < 5; i++)
        {
            Gizmos.DrawWireSphere(transform.position + transform.right * (_size * 0.5f * i - _size), 0.5f);            
        }

        Gizmos.DrawLine(transform.position - transform.right * _size, transform.position + transform.right * _size);
    }
}
