using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AmmoMovement))]
public class Ammo : MonoBehaviour
{
    public enum Collided
    {
        None,
        WithBarrier,
        WithBigBarrier
    }

    public readonly UnityEvent<Collided> DestroyEvent = new UnityEvent<Collided>();

    [SerializeField] private float _lifetime;
    private AmmoMovement _movement;

    private void Start()
    {
        _movement = GetComponent<AmmoMovement>();
        StartCoroutine(DestroyIfNoCollided());
    }

    private void FixedUpdate()
    {
        _movement.MoveToRight();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BigBarrier>() != null)
        {
            DestroyEvent?.Invoke(Collided.WithBigBarrier);
            Destroy(gameObject);
        }
        else if (collision.GetComponent<Barrier>() != null)
        {
            DestroyEvent?.Invoke(Collided.WithBarrier);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyIfNoCollided()
    {
        yield return new WaitForSeconds(_lifetime);

        DestroyEvent?.Invoke(Collided.None);
        Destroy(gameObject);
    }
}
