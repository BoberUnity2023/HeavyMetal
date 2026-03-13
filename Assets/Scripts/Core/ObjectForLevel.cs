using UnityEngine;

public class ObjectForLevel : MonoBehaviour
{
    [SerializeField] private ControllerLevel _controllerLevel;
    [SerializeField] private GameObject[] _objects;

    private void Start()
    {
        foreach (var obj in _objects)
        {            
            bool active = _controllerLevel.Level.name.Contains(obj.name);
            obj.SetActive(active);
        }
    }
}
