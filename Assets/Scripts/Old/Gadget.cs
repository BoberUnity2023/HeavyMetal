using UnityEngine;

public class Gadget : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform muzzle;

    [SerializeField] private int shotsCount = 2;
    [SerializeField] private int chargesCount = 1;
    [SerializeField] private float shotDelay = 0.1f;

    [SerializeField] private float projectileSpeed = 1000f;
    [SerializeField] private float projectileLifetime = 5f;

    private int currentShots = 0;
    private float currentDelay = 0;

    private GameObject root;

    void Start()
    {
        root = GetCarRoot();
    }

    void Update()
    {
        if(currentShots > 0)
        {
            if(currentDelay <= 0.0f)
            {
                currentShots--;
                currentDelay = shotDelay;

                GameObject projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

                RocketProjectile rocketScript = projectile.GetComponent<RocketProjectile>();
                if (rocketScript != null)
                {
                    rocketScript.SetOwner(root);
                }

                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(muzzle.forward * projectileSpeed);
                }

                Destroy(projectile, projectileLifetime);
            }
            else
            {
                currentDelay -= Time.deltaTime;
            }
        }
    }

    public void setLevel(int level)
    {
        // \todo Implement leveling
    }

    public bool isReady()
    {
        return chargesCount > 0;
    }

    public void Fire()
    {
        if (projectilePrefab)
        {
            currentShots = shotsCount;
            currentDelay = shotDelay;
            chargesCount--;
        }
    }

    // Get the root car GameObject (in case RocketLauncher is on a child object)
    GameObject GetCarRoot()
    {
        CarController car = GetComponentInParent<CarController>();
        if (car != null)
        {
            return car.gameObject;
        }

        return gameObject;
    }
}