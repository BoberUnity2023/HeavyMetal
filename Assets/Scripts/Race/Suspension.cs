using UnityEngine;
using System.Collections.Generic;

public class Suspension : MonoBehaviour
{
    [System.Serializable]
    public class WheelData
    {
        public string wheelName;
        public WheelCollider collider;
        public bool isFrontWheel;
        [HideInInspector] public bool isGroundedBefore;
    }

    [Header("Список всех колёс")]
    public List<WheelData> wheels = new List<WheelData>();

    [Header("Настройки порогов (в % от 0 до 1)")]
    [Range(0f, 1f)]
    [Tooltip("На сколько должна быть сжата подвеска при контакте, чтобы приземление считалось ЖЕСТКИМ")]
    public float hardLandingCompression = 0.8f;

    [Range(0f, 1f)]
    [Tooltip("Порог сжатия пружины для фиксации пробития на кочках (Bottom Out)")]
    public float heavyCompressionThreshold = 0.75f;

    [Header("Эффекты (Опционально)")]
    public AudioSource audioSource;
    //public AudioClip jumpSound;
    //public AudioClip softLandingSound;
    public AudioClip hardLandingSound;
    public ParticleSystem landingParticles;
    private Car _car;

    private bool isCarInAir;

    void FixedUpdate()
    {
        int wheelsOnGround = 0;
        float maxCompressionThisFrame = 0f;        

        foreach (var wheel in wheels)
        {
            if (wheel.collider == null) continue;

            WheelHit hit;
            bool isGrounded = wheel.collider.GetGroundHit(out hit);

            if (isGrounded)
            {
                wheelsOnGround++;

                // Вычисляем процент сжатия подвески (от 0.0 до 1.0)
                // hit.suspensionSlip не дает ход, поэтому считаем через положение hit.point
                float currentDistance = -wheel.collider.transform.InverseTransformPoint(hit.point).y - wheel.collider.radius;
                float totalSuspension = wheel.collider.suspensionDistance;

                // Нормализуем: 0 = разжата полностью, 1 = сжата в упор
                float compressionPercent = Mathf.Clamp01(1f - (currentDistance / totalSuspension));

                // 1. Ловим момент приземления конкретного колеса
                if (!wheel.isGroundedBefore)
                {                    
                    if (compressionPercent > maxCompressionThisFrame)
                    {
                        maxCompressionThisFrame = compressionPercent;
                    }

                    OnIndividualWheelLanding(wheel, compressionPercent);
                }

                // 2. Ловим критическое пробитие на кочках (Bottom Out)
                if (compressionPercent >= heavyCompressionThreshold)
                {
                    OnWheelBottomOut(wheel, compressionPercent);
                }
            }

            wheel.isGroundedBefore = isGrounded;
        }

        // --- ОБЩАЯ ЛОГИКА ДЛЯ ВСЕЙ МАШИНЫ ---

        if (!isCarInAir && wheelsOnGround == 0)
        {
            isCarInAir = true;
            OnCarTakeOff();
        }

        if (isCarInAir && wheelsOnGround > 0)
        {
            isCarInAir = false;
            OnCarLanding(maxCompressionThisFrame);
        }
    }

    public void Init(Car car)
    {
        _car = car;
        if (_car.Mode == Mode.Track)
            enabled = true;
    }

    private void OnCarTakeOff()
    {
        Debug.Log("🚀 Машина в воздухе!");
        //PlaySound(jumpSound);
    }

    private void OnCarLanding(float maxCompression)
    {
        Debug.Log($"🛬 Приземление. Макс. сжатие стойки: {maxCompression * 100f:F0}%");

        if (landingParticles != null) landingParticles.Play();

        // Если при приземлении стойка сжалась сильнее заданного процента (например, >85%)
        if (maxCompression >= hardLandingCompression)
        {
            PlaySound(hardLandingSound);
        }
        else
        {
            //PlaySound(softLandingSound);
        }
    }

    private void OnIndividualWheelLanding(WheelData wheel, float compression)
    {
        //string axis = wheel.isFrontWheel ? "Передняя" : "Задняя";
        //Debug.Log($"   Колесо [{wheel.wheelName}] ({axis}) приземлилось со сжатием {compression * 100f:F0}%");
    }

    private void OnWheelBottomOut(WheelData wheel, float compression)
    {
        // Сработает ОДИН РАЗ, только если выкрутить логгер или если реально пробило до упора
        //Debug.LogWarning($"💥 Жесткое пробитие подвески до упора на колесе [{wheel.wheelName}]! ({compression * 100f:F0}%)");
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.volume = _car.Hub.Game.Sound.VolumeSound;
            audioSource.PlayOneShot(clip);
        }
    }
}