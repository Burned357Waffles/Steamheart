using System.Collections;
using Misc;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] cloudArray;
    [SerializeField] float spawnInterval;
    [SerializeField] private GameObject endPoint;
    private Vector3 _startPosition;
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        AttemptCloudSpawn();
        Invoke(nameof(AttemptCloudSpawn), spawnInterval);
    }

    private void SpawnCloud()
    {
        GameObject cloud = Instantiate(cloudArray[Random.Range(0, cloudArray.Length)]);
        cloud.transform.position = _startPosition;

        _startPosition.z = Random.Range(_startPosition.z - 30f, _startPosition.z + 30f);
        
        float speed = Random.Range(0.5f, 1f);
        cloud.GetComponent<CloudController>().StartFloating(speed, endPoint.transform.position.x);

    }
    
    private void AttemptCloudSpawn()
    {
        SpawnCloud();
        Invoke(nameof(AttemptCloudSpawn), spawnInterval);
    }
}
