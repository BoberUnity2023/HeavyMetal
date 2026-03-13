using UnityEngine;

public class ControllerOptimization : MonoBehaviour
{
    [SerializeField] private GameObject[] _hidenObjects;

    void Start()
    {
        Set();
    }

    public void Set()
    {
        int level = PlayerPrefs.GetInt("QualityLevel", 1);
        bool isVisible = level > 0;
        foreach (var obj in _hidenObjects)
        {
            if (obj == null) 
                Debug.LogError(gameObject.name + " Отсутствует ссылка на объект");

            obj.SetActive(isVisible);
        }
    }
}
