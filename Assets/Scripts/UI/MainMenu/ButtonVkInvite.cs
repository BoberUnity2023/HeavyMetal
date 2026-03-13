using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class ButtonVkInvite : MonoBehaviour
{
    [SerializeField] private MainMenuController _mainMenuController;

    private void Start()
    {
        bool state = _mainMenuController.Game.Platform == Platform.Vk;
        gameObject.SetActive(state);
        if (state)
        {
            GetComponent<Button>().onClick.AddListener(Press);
        }
    }

    private void Press()
    {
        //VKManager.Instance.ShowInvite();
    }
}
