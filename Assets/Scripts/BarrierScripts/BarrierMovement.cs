using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    public void MoveToLeft()
    {
        transform.Translate(Vector3.left * Time.fixedDeltaTime * _speed);
    }
}
