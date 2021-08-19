using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Events;
using static Ammo;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShotter))]
public class Player : Agent
{
    public readonly UnityEvent _episodeBeginEvent = new UnityEvent();

    public event UnityAction EpisodeBeginEvent
    {
        add => _episodeBeginEvent.AddListener(value);
        remove => _episodeBeginEvent.RemoveListener(value);
    }

    [SerializeField] private LayerMask _barrierMask;
    private PlayerMovement _movement;
    private PlayerShotter _shotter;

    private readonly Dictionary<Collided, float> _rewardByCollided = new Dictionary<Collided, float>
        {
            {Collided.WithBigBarrier, 8f},
            {Collided.WithBarrier, -12f},
            {Collided.None, -10f}
        };


    public override void Initialize()
    {
        _movement = GetComponent<PlayerMovement>();
        _shotter = GetComponent<PlayerShotter>();
    }

    public override void OnEpisodeBegin()
    {
        _episodeBeginEvent?.Invoke();
    }

    public override void CollectObservations(VectorSensor sensor)
    {    
        RaycastHit2D hit = Physics2D.Raycast(_movement.StartPosition, Vector2.right, 200, _barrierMask);
        if (hit.collider != null)
        {
            sensor.AddObservation(hit.distance);
            sensor.AddObservation(hit.collider.gameObject.GetComponent<Barrier>() != null ? 1 : 2);
        } 
        else
        {
            sensor.AddObservation(0);
            sensor.AddObservation(0);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(Time.fixedDeltaTime);
        if (actions.DiscreteActions[0] == 1) _movement.TryJump();
        if (actions.DiscreteActions[1] == 1) _shotter.TryShoot(OnAmmoCollided);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        
        if (Input.GetKey(KeyCode.Space)) discreteActionsOut[0] = 1;
        if (Input.GetKey(KeyCode.S)) discreteActionsOut[1] = 1;
    }

    private void OnAmmoCollided(Collided collided)
    {
        Debug.Log(collided.ToString());
        AddReward(_rewardByCollided[collided]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Barrier>() != null || collision.GetComponent<BigBarrier>() != null)
        {
            Debug.Log("End episiode");
            AddReward(-20f);
            EndEpisode();
        }
    }
}
