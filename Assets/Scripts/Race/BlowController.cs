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
                    GameObject prefab = SparkPrefab(impulse);
                    GameObject spark = Instantiate(prefab, contact.point, Quaternion.identity);
                    AudioSource sparkAudioSource = spark.GetComponent<AudioSource>();
                    sparkAudioSource.volume = Mathf.Min(impulse / 5000, 1);                    
                }           
            }
        }
    }

    private GameObject SparkPrefab(float impulse)
    {
        if (impulse < 2000)
            return _car.PrefabsSparks[0];

        if (impulse < 5000)
            return _car.PrefabsSparks[1];

        return _car.PrefabsSparks[2];
    }
}
