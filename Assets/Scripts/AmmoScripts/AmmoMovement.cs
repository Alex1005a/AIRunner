using UnityEngine;

public class AmmoMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    public void MoveToRight()
    {
        transform.Translate(Vector3.right * Time.fixedDeltaTime * _speed);
    }
}
