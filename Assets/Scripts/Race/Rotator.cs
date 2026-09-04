using UnityEngine;

public class InfiniteRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = new Vector3(0f, 1f, 0f); // Ось вращения
    public float rotationSpeed = 90f; // Скорость вращения (градусы в секунду)

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
