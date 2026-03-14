using UnityEngine;

public class DestructibleCar : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Destruction Settings")]
    [SerializeField] private GameObject destroyedCarPrefab; // Optional: Destroyed version of car
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip destructionSound;

    [Header("Visual Feedback")]
    [SerializeField] private Material damagedMaterial; // Optional: Material when damaged
    [SerializeField] private int damageThresholdForVisual = 50;

    private MeshRenderer meshRenderer;
    private bool isDestroyed = false;

    void Start()
    {
        currentHealth = maxHealth;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed)
        {
            return;
        }

        currentHealth -= damage;

        // Apply damaged visual if health is below threshold
        if (currentHealth <= damageThresholdForVisual && currentHealth > 0 && damagedMaterial != null && meshRenderer != null)
        {
            meshRenderer.material = damagedMaterial;
        }

        // Destroy car if health reaches zero
        if (currentHealth <= 0)
        {
            DestroyCar();
        }
    }

    void DestroyCar()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        Debug.Log($"{gameObject.name} has been destroyed!");

        // Spawn explosion effect
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 3f);
        }

        // Play destruction sound
        if (destructionSound != null)
        {
            AudioSource.PlayClipAtPoint(destructionSound, transform.position);
        }

        // Replace with destroyed version if available
        if (destroyedCarPrefab != null)
        {
            GameObject destroyed = Instantiate(destroyedCarPrefab, transform.position, transform.rotation);

            // Copy velocity if original had rigidbody
            Rigidbody originalRb = GetComponent<Rigidbody>();
            Rigidbody destroyedRb = destroyed.GetComponent<Rigidbody>();
            if (originalRb != null && destroyedRb != null)
            {
                destroyedRb.linearVelocity = originalRb.linearVelocity;
                destroyedRb.angularVelocity = originalRb.angularVelocity;
            }

            Destroy(destroyed, 10f); // Clean up after 10 seconds
        }

        // Destroy this car
        Destroy(gameObject);
    }

    // Optional: Display health bar or health info
    void OnGUI()
    {
        if (!isDestroyed)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);
            if (screenPos.z > 0)
            {
                GUI.Label(new Rect(screenPos.x - 50, Screen.height - screenPos.y - 20, 100, 20),
                    $"HP: {currentHealth}/{maxHealth}");
            }
        }
    }
}