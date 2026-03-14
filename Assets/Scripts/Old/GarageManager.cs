using UnityEngine;

public class GarageManager : MonoBehaviour
{
    public static GarageManager Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    static public int leftSlotPrefabId()
    {
        return 0;
    }

    static public int leftSlotLevel()
    {
        return 1;
    }

    static public int rightSlotPrefabId()
    {
        return 0;
    }

    static public int rightSlotLevel()
    {
        return 1;
    }

    static public int backSlotPrefabId()
    {
        return 0;
    }

    static public int backSlotLevel()
    {
        return 1;
    }
}
