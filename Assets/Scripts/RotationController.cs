using UnityEngine;

public class RotationController : MonoBehaviour
{
    [Header("Object Rotator Reference")]
    [SerializeField] private ObjectRotator targetRotator;

    void Start()
    {
        // If no rotator is assigned, try to get it from the same GameObject
        if (targetRotator == null)
        {
            targetRotator = GetComponent<ObjectRotator>();
        }

        // If still null, try to find it in the scene
        if (targetRotator == null)
        {
            targetRotator = FindObjectOfType<ObjectRotator>();
        }

        if (targetRotator != null)
        {
            // Example: Configure the rotator
            targetRotator.SetRotationSpeed(120f);
            targetRotator.SetTargetDegrees(180f);
            targetRotator.StartRotation();
        }
        else
        {
            Debug.LogError("No ObjectRotator found in the scene!");
        }
    }

    void Update()
    {
        // Example: Control rotation with keyboard
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (targetRotator != null)
            {
                targetRotator.ToggleRotation();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (targetRotator != null)
            {
                targetRotator.ResetRotation();
            }
        }
    }

    // Public method to assign rotator at runtime
    public void SetTargetRotator(ObjectRotator rotator)
    {
        targetRotator = rotator;
    }
}