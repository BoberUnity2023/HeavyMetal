using UnityEngine;

public class FindMissingRuntime : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Update()
    {        
        // Находим вообще все живые объекты в игре на данный момент
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject go in allObjects)
        {
            Component[] components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.LogError($"[НАЙДЕНО] Потерян скрипт на объекте: '{go.name}' в сцене или префабе!", go);

                    go.hideFlags = HideFlags.None;

                    // Автоматически выделяем его в редакторе, чтобы вы его увидели
#if Unity_Editor
                        UnityEditor.Selection.activeGameObject = go;
#endif

                    // Опционально: автоматически очищаем его от сломанного скрипта
#if Unity_Editor
                        //UnityEditor.GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                        Debug.LogWarning($"Сломанный скрипт автоматически удален с объекта {go.name}!");
#endif
                }
            }
        }
    }
}
