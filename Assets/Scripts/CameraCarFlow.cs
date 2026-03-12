using UnityEngine;

public class CarCameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera targetCamera;          // Камера, которая будет следовать
    public float followSpeed = 5f;       // Скорость плавности движения
    public float rotationSpeed = 5f;     // Скорость поворота камеры

    private Vector3 offset;              // Смещение камеры от машины

    void Start()
    {
        // Если камера не указана — берём основную
        if (targetCamera == null)
            targetCamera = Camera.main;

        // Вычисляем изначальное смещение между машиной и камерой
        offset = targetCamera.transform.position - transform.position;
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        // Целевая позиция камеры
        Vector3 desiredPosition = transform.position + offset;

        // Плавное перемещение камеры к цели
        targetCamera.transform.position = Vector3.Lerp(
            targetCamera.transform.position,
            desiredPosition,
            Time.deltaTime * followSpeed
        );

        // Плавный поворот камеры на машину
        Quaternion desiredRotation = Quaternion.LookRotation(transform.position - targetCamera.transform.position);
        targetCamera.transform.rotation = Quaternion.Slerp(
            targetCamera.transform.rotation,
            desiredRotation,
            Time.deltaTime * rotationSpeed
        );
    }
}
