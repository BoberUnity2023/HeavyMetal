using UnityEngine;

public class BlowController : MonoBehaviour
{
    [SerializeField] private Car _car;    

    private void OnCollisionEnter(Collision collision)
    {
        if (!_car.IsVisible)
            return;

        if (collision.contacts.Length > 0)
        {
            float impulses = 0;
            bool isBorder = false;

            foreach (ContactPoint contact in collision.contacts)
            {
                float impulse = contact.impulse.magnitude;
                impulses += impulse;

                if (impulse > 500)
                {
                    GameObject prefab = SparkPrefab(impulse);
                    GameObject spark = Instantiate(prefab, contact.point, Quaternion.identity);
                    AudioSource sparkAudioSource = spark.GetComponent<AudioSource>();
                    sparkAudioSource.volume = Mathf.Min(impulse / 5000, 1);

                    if (!isBorder && _car.Hub.Game.IsEqualPhysicsMaterials(contact.otherCollider.material, _car.Hub.Game.GroundPropses[_car.Hub.Game.GroundPropses.Length - 1].PhysicMaterial))
                        isBorder = true;                    
                }
            }

            if (_car.Speed > _car.Config.DamageSpeed && impulses > _car.Config.DamageImpulse && isBorder)
            {
                if (!_car.IsAI)
                    Debug.Log("Damage from blow. Speed: " + (_car.Speed * 3.6f).ToString("f0") + " km/h; impulse: " + impulses.ToString("f0"));

                _car.DamageCounter.DamageAdd(34, false);
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