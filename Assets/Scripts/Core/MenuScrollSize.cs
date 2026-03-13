using UnityEngine;

public class MenuScrollSize : MonoBehaviour
{
    [SerializeField] private ConfigLevels _configLevels;
    [SerializeField] private RectTransform _content;

    private void Update()
    {
        if (!_content.gameObject.activeSelf || !_content.gameObject.activeInHierarchy)
            return;

        int height = 270 * Lines + 200;
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, height);
        //Debug.Log("C: " + Columns + "; L: " + Lines + "; S: " + height);
    }

    private int Columns
    {
        get
        {
            ButtonLevel[] buttonLevels = _content.GetComponentsInChildren<ButtonLevel>();
            if (buttonLevels.Length < 1 ) 
                return 0;

            for (int i = 1; i < buttonLevels.Length; i++)
            {
                if (Mathf.Abs(buttonLevels[i].transform.position.x - buttonLevels[0].transform.position.x) < 1)
                    return i;
            }

            return 0;
        }
    }

    private int Lines
    {
        get
        {
            int remainder = _configLevels.Levels.Length % Columns;
            int addLine = remainder > 0 ? 1 : 0;
            return _configLevels.Levels.Length/ Columns + addLine;
        }
    }
}
