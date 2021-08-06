using UnityEngine;

[RequireComponent(typeof(BarrierMovement))]
public class Barrier : MonoBehaviour
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
        if(collision.GetComponent<Bound>() != null)
        {
            Destroy(gameObject);
        }
    }
}
