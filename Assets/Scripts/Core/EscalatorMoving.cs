using UnityEngine;

public class EscalatorMoving : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;    
    private float _offset;

    private void Update()
    {
        Update_Movetexture();
    }

    private void Update_Movetexture()
    {
        _offset += Time.deltaTime;
        Material _material = _renderer.sharedMaterials[0];
        _material.SetTextureOffset("_BaseColorMap", new Vector2(_offset, 0));        
    }
}
