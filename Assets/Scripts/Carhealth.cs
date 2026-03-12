using UnityEngine;
using UnityEngine.Events;

public class CarHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Visual Effects")]
    [SerializeField] private GameObject destructionEffect;
    [SerializeField] private ParticleSystem damageParticles;

    [Header("Events")]
    public UnityEvent onDamageTaken;
    public UnityEvent onDestroyed;

    private bool isDestroyed = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDestroyed) return;

        // Reduce health
        currentHealth -= damageAmount;

        // Play damage effects
        onDamageTaken?.Invoke();
        if (damageParticles != null)
        {
            damageParticles.Play();
        }

        // Check if car should be destroyed
        if (currentHealth <= 0)
        {
            DestroyCar();
        }
    }

    public void DestroyCar()
    {
        if (isDestroyed) return;

        isDestroyed = true;

        // Play destruction effects
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, transform.rotation);
        }

        // Trigger destruction event
        onDestroyed?.Invoke();

        // Disable car components instead of destroying immediately
        DisableCar();

        // Destroy the car object after a delay
        Destroy(gameObject, 3f);
    }

    private void DisableCar()
    {
        // Disable various car components
        MonoBehaviour[] components = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour component in components)
        {
            if (component != this)
            {
                component.enabled = false;
            }
        }

        // Disable colliders
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        // Disable rigidbody physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Disable mesh renderers after a short delay
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }
}