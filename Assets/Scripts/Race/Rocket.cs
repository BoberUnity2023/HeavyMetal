using DG.Tweening;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject _prefabBlast;
    
    public void Init(Transform target)
    {
        transform.parent = target;
        Sequence tweener = transform.DOLocalJump(Vector3.zero, 2f, 1, 1.0f).OnComplete(() => Blast());
        /*tweener.OnUpdate(() => 
        {
            tweener.ChangeEndValue(target.position);
        });*/
    } 
    
    void Blast()
    {
        Instantiate(_prefabBlast, transform.position, Quaternion.identity);
    }
}
