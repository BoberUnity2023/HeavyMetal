using UnityEngine;

public class SteeringWheel : MonoBehaviour
{
    [SerializeField] private Car _car;    
    [SerializeField] private Animator _animator;    

    void Update()
    {
        Update_AnimationsTurn();
    }

    private void Update_AnimationsTurn()
    {
        AnimatorStateInfo stateTurn = _animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = _car.Hub.Input.PlayerInput.Steer / 2 + 0.5f;
        _animator.Play(stateTurn.fullPathHash, 0, normalizedTime);
    }
}
