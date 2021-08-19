using System.Collections;
using System.Linq;
using UnityEngine;

public class BarrierSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 _barrierSpawnPosition;
    [SerializeField] private GameObject _barrierPrefab;
    [SerializeField] private Vector3 _bigBarrierSpawnPosition;
    [SerializeField] private GameObject _bigBarrierPrefab;
    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;

    private void Start()
    {        
        FindObjectOfType<Player>().EpisodeBeginEvent += DestroyAllBarriers;
        StartSpawnCoroutine();
    }

    private void DestroyAllBarriers()
    {
        Debug.Log("Destroy all barriers");
        FindObjectsOfType<BigBarrier>().ToList()
            .ForEach(bigBarrier => Destroy(bigBarrier.gameObject));
        FindObjectsOfType<Barrier>().ToList()
            .ForEach(barrier => Destroy(barrier.gameObject));
    }

    private IEnumerator Spawn(float waitTime)
    {
        RandomInstantiate();
        yield return new WaitForSeconds(waitTime);
        StartSpawnCoroutine();
    }

    private Coroutine StartSpawnCoroutine()
    {
        return StartCoroutine(Spawn(GetRandomWaitTime()));
    }

    private float GetRandomWaitTime()
    {
        return Random.Range(_minWaitTime, _maxWaitTime);
    }

    private GameObject RandomInstantiate()
    {
        if(Random.Range(1, 6) == 1) return Instantiate(_bigBarrierPrefab, _bigBarrierSpawnPosition, Quaternion.identity);
        else return Instantiate(_barrierPrefab, _barrierSpawnPosition, Quaternion.identity);
    }
}
