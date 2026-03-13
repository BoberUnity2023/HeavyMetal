using UnityEngine;


public class GamePushController : MonoBehaviour
{
    [SerializeField] private GameController _game;     

    
    public int Rank { get; private set; }

    public bool IsProductsFetched { get; private set; }
    public bool IsPlayersLBFetched { get; private set; }    

    private async void Start()
    {
        

        //await GP_Init.Ready;
        OnPluginReady();
    }

    private void OnDestroy()
    {
        
    }    

    private void OnPluginReady()
    {
        Debug.Log("GP Plugin is ready");
      
    }

    
    

    
}
