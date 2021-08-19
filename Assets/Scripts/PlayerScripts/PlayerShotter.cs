using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static Ammo;

public class PlayerShotter : MonoBehaviour
{
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private float _waitingTime;
    private bool readyToShoot = true;

    public void TryShoot(UnityAction<Collided> collideAction)
    {
        if (readyToShoot)
        {
            readyToShoot = false;
            var ammo = Instantiate(_ammoPrefab, transform.position, Quaternion.identity);
            ammo.GetComponent<Ammo>().DestroyEvent += collideAction;
            StartCoroutine(GetReadyToShoot());
        }
    }

    private IEnumerator GetReadyToShoot()
    {
        yield return new WaitForSeconds(_waitingTime);
        readyToShoot = true;
    }
}
