using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ControlsShowAndSwitch : MonoBehaviour
{
    [Header("Объекты")]
    [Tooltip("Весь Canvas (или родитель), который нужно скрыть через время")]
    public GameObject controlsCanvas;

    [Tooltip("Картинка для клавиатуры")]
    public GameObject keyImage;

    [Tooltip("Картинка для геймпада")]
    public GameObject joysImage;

    [Header("Настройки времени")]
    [Tooltip("Сколько секунд показывать подсказки")]
    public float displayDuration = 5f;

    private bool isUsingGamepad = false;
    private bool isHidden = false;

    void Start()
    {
        if (controlsCanvas == null)
        {
            Debug.LogWarning("ControlsShowAndSwitch: не назначен Controls Canvas!");
            return;
        }

        // Показываем Canvas
        controlsCanvas.SetActive(true);

        // Автоопределение устройства в начале
        DetectInitialDevice();

        // Запускаем таймер скрытия
        StartCoroutine(HideAfterDelay());
    }

    void Update()
    {
        if (isHidden) return;

        // === Клавиатура / мышь ===
        if (IsKeyboardOrMouseUsed())
        {
            if (isUsingGamepad)
            {
                isUsingGamepad = false;
                SwitchToKeyboard();
            }
        }
        // === Геймпад ===
        else if (IsGamepadUsed())
        {
            if (!isUsingGamepad)
            {
                isUsingGamepad = true;
                SwitchToGamepad();
            }
        }
    }

    // ================== Автоопределение в начале ==================
    void DetectInitialDevice()
    {
        // Если геймпад подключён — считаем, что игрок им пользуется
        if (Gamepad.current != null)
        {
            isUsingGamepad = true;
            SwitchToGamepad();
        }
        else
        {
            isUsingGamepad = false;
            SwitchToKeyboard();
        }
    }

    // ================== Проверки ввода ==================
    bool IsKeyboardOrMouseUsed()
    {
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
            return true;

        if (Mouse.current != null &&
            (Mouse.current.leftButton.wasPressedThisFrame ||
             Mouse.current.rightButton.wasPressedThisFrame ||
             Mouse.current.middleButton.wasPressedThisFrame))
            return true;

        return false;
    }

    bool IsGamepadUsed()
    {
        if (Gamepad.current == null)
            return false;

        var g = Gamepad.current;

        // Кнопки
        if (g.buttonSouth.wasPressedThisFrame ||
            g.buttonNorth.wasPressedThisFrame ||
            g.buttonEast.wasPressedThisFrame ||
            g.buttonWest.wasPressedThisFrame ||
            g.startButton.wasPressedThisFrame ||
            g.selectButton.wasPressedThisFrame ||
            g.leftShoulder.wasPressedThisFrame ||
            g.rightShoulder.wasPressedThisFrame ||
            g.leftTrigger.wasPressedThisFrame ||
            g.rightTrigger.wasPressedThisFrame ||
            g.leftStickButton.wasPressedThisFrame ||
            g.rightStickButton.wasPressedThisFrame ||
            g.dpad.up.wasPressedThisFrame ||
            g.dpad.down.wasPressedThisFrame ||
            g.dpad.left.wasPressedThisFrame ||
            g.dpad.right.wasPressedThisFrame)
        {
            return true;
        }

        // Стики
        if (g.leftStick.ReadValue().magnitude > 0.25f ||
            g.rightStick.ReadValue().magnitude > 0.25f)
        {
            return true;
        }

        return false;
    }

    // ================== Переключение картинок ==================
    void SwitchToKeyboard()
    {
        if (keyImage) keyImage.SetActive(true);
        if (joysImage) joysImage.SetActive(false);
    }

    void SwitchToGamepad()
    {
        if (keyImage) keyImage.SetActive(false);
        if (joysImage) joysImage.SetActive(true);
    }

    // ================== Скрытие через время ==================
    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        if (controlsCanvas != null)
            controlsCanvas.SetActive(false);

        isHidden = true;
    }
}