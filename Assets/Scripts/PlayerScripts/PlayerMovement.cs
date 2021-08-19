using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private AnimationCurve _yAnimation;
    [SerializeField] private float _duration;
    [SerializeField] private float _height;

    public Vector3 StartPosition { get; private set; }
    private bool _isJumped;

    private void Start()
    {
        GetComponent<Player>().EpisodeBeginEvent += StopJump;
        StartPosition = transform.position;
        _isJumped = false;
    }

    public void TryJump()
    {
        if (!_isJumped)
        {
            _isJumped = true;
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        float expiredTime = 0f;

        while (expiredTime < _duration)
        {
            expiredTime += Time.deltaTime;
            float progress = expiredTime / _duration;
            transform.position = StartPosition + new Vector3(0, _height * _yAnimation.Evaluate(progress), 0);

            yield return null;
        }
        _isJumped = false;
    }

    private void StopJump()
    {
        _isJumped = false;
        StopAllCoroutines();
        transform.position = StartPosition;
    }  
}