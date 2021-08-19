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

    private UnityEvent<Collided> _destroyEvent = new UnityEvent<Collided>();

    public event UnityAction<Collided> DestroyEvent
    {
        add => _destroyEvent.AddListener(value);
        remove => _destroyEvent.RemoveListener(value);
    }

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
            _destroyEvent?.Invoke(Collided.WithBigBarrier);
            Destroy(gameObject);
        }
        else if (collision.GetComponent<Barrier>() != null)
        {
            _destroyEvent?.Invoke(Collided.WithBarrier);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyIfNoCollided()
    {
        yield return new WaitForSeconds(_lifetime);

        _destroyEvent?.Invoke(Collided.None);
        Destroy(gameObject);
    }
}
