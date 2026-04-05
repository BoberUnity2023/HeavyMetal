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
                float impulse = contact.impulse.magnitude;
                if (impulse > 500)
                {
                    GameObject spark = Instantiate(_car.PrefabSparks, contact.point, Quaternion.identity);
                    AudioSource sparkAudioSource = spark.GetComponent<AudioSource>();
                    sparkAudioSource.volume = Mathf.Min(impulse / 5000, 1);                    
                }           
            }
        }
    }
}
