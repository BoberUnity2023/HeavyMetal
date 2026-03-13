using UnityEngine;

public class ButtonLeaderboard : MonoBehaviour
{
    [SerializeField] private MainMenuController _mainMenuController;

    private void Start()
    {
        
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
