using UnityEngine;

[RequireComponent(typeof(BarrierMovement))]
public class BigBarrier : MonoBehaviour
{
    private BarrierMovement _movement;

    private void Start()
    {
        _movement = GetComponent<BarrierMovement>();
    }

    private void FixedUpdate()
    {
        _movement.MoveToLeft();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bound>() != null || collision.GetComponent<Ammo>() != null)
        {
            Destroy(gameObject);
        }
    }
}
