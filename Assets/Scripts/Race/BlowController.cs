using System;
using UnityEngine;

public class BlowController : MonoBehaviour
{
    [SerializeField] private Car _car;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Instantiate(_car.PrefabSparks, contact.point, Quaternion.identity);
            }
        }
    }
}
