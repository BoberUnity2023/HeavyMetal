using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z,
        Custom
    }

    [Header("Rotation Settings")]
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.Y;
    [SerializeField] private Vector3 customAxis = Vector3.up;

    [Header("Rotation Parameters")]
    [SerializeField] private float rotationSpeed = 90f; // Degrees per second
    [SerializeField] private float targetDegrees = 360f; // Total degrees to rotate

    [Header("Rotation State")]
    [SerializeField] private bool isRotating = false;
    [SerializeField] private bool loopRotation = false;

    private float currentRotation = 0f;
    private Vector3 actualRotationAxis;

    void Start()
    {
        UpdateRotationAxis();
    }

    void Update()
    {
        if (isRotating)
        {
            RotateObject();
        }
    }

    private void RotateObject()
    {
        // Calculate rotation for this frame
        float rotationThisFrame = rotationSpeed * Time.deltaTime;

        // Apply rotation
        transform.Rotate(actualRotationAxis, rotationThisFrame);

        // Track total rotation
        currentRotation += rotationThisFrame;

        // Check if target rotation is reached
        if (currentRotation >= targetDegrees)
        {
            if (loopRotation)
            {
                // Reset rotation tracking for looping
                currentRotation = 0f;
            }
            else
            {
                // Stop rotation
                isRotating = false;

                // Ensure we end exactly at the target rotation
                float overshoot = currentRotation - targetDegrees;
                transform.Rotate(actualRotationAxis, -overshoot);
                currentRotation = targetDegrees;
            }
        }
    }

    private void UpdateRotationAxis()
    {
        switch (rotationAxis)
        {
            case RotationAxis.X:
                actualRotationAxis = Vector3.right;
                break;
            case RotationAxis.Y:
                actualRotationAxis = Vector3.up;
                break;
            case RotationAxis.Z:
                actualRotationAxis = Vector3.forward;
                break;
            case RotationAxis.Custom:
                actualRotationAxis = customAxis.normalized;
                break;
        }
    }

    // Public methods to control rotation
    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    public void ToggleRotation()
    {
        isRotating = !isRotating;
    }

    public void ResetRotation()
    {
        currentRotation = 0f;
        isRotating = false;
        transform.rotation = Quaternion.identity;
    }

    public void SetRotationAxis(RotationAxis newAxis)
    {
        rotationAxis = newAxis;
        UpdateRotationAxis();
    }

    public void SetCustomAxis(Vector3 axis)
    {
        customAxis = axis;
        if (rotationAxis == RotationAxis.Custom)
        {
            UpdateRotationAxis();
        }
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = Mathf.Abs(speed); // Ensure positive speed
    }

    public void SetTargetDegrees(float degrees)
    {
        targetDegrees = Mathf.Abs(degrees); // Ensure positive degrees
    }

    // Properties for external access
    public bool IsRotating => isRotating;
    public float CurrentRotation => currentRotation;
    public float RotationProgress => Mathf.Clamp01(currentRotation / targetDegrees);
}