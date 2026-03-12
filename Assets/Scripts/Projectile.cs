using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private int damage = 100;

    [Header("Audio")]
    [SerializeField] private AudioClip explosionSound;

    private bool hasExploded = false;
    private GameObject ownerCar; // The car that fired this rocket

    // Call this from RocketLauncher to set the owner
    public void SetOwner(GameObject owner)
    {
        ownerCar = owner;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ignore collision with owner car for a short time after spawn
        if (ownerCar != null && IsPartOfCar(collision.gameObject, ownerCar))
        {
            return;
        }

        Debug.Log($"Rocket hit: {collision.gameObject.name}");

        if (!hasExploded)
        {
            // Only damage the specific car we hit
            Explode(collision.contacts[0].point, collision.gameObject);
        }
    }

    // Check if a GameObject is part of a car (could be child object)
    bool IsPartOfCar(GameObject obj, GameObject car)
    {
        Transform current = obj.transform;
        while (current != null)
        {
            if (current.gameObject == car)
                return true;
            current = current.parent;
        }
        return false;
    }

    void Explode(Vector3 explosionPoint, GameObject hitObject)
    {
        hasExploded = true;

        // Spawn explosion effect
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, explosionPoint, Quaternion.identity);
            Destroy(effect, 3f);
        }

        // Play explosion sound
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, explosionPoint);
        }

        // Only damage the car that was hit
        DestructibleCar car = hitObject.GetComponent<DestructibleCar>();
        if (car != null)
        {
            Debug.Log($"Damaging {hitObject.name} with {damage} damage");
            car.TakeDamage(damage);
        }
        else
        {
            Debug.Log($"{hitObject.name} has no DestructibleCar component");
        }

        // Apply explosion force to nearby rigidbodies (optional visual effect)
        Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, explosionPoint, explosionRadius, 3f);
            }
        }

        // Destroy the rocket
        Destroy(gameObject);
    }

    // Visualize explosion radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}