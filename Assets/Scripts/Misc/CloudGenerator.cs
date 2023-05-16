using System.Collections;
using Misc;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] cloudArray;
    [SerializeField] float spawnInterval;
    [SerializeField] private GameObject endPoint;
    private Vector3 _startPosition;
    private float _startX;
    // Start is called before the first frame update
    void Start()
    {
        //_startPosition = new Vector3(0, 0, 0);
        //FirstCloudSpawn();
        _startPosition = transform.position;
        _startX = _startPosition.x;
        Invoke(nameof(AttemptCloudSpawn), spawnInterval);
    }

    private void SpawnCloud()
    {
        var cloudPrefab = cloudArray[Random.Range(0, cloudArray.Length - 1)];
        GameObject cloud = Instantiate(
            cloudPrefab,
            _startPosition, 
            cloudPrefab.transform.rotation,
            this.transform);
        float scale = Random.Range(0.25f, 1f);
        cloud.transform.localScale = new Vector3(scale, scale, scale);

        _startPosition.x = Random.Range(_startX - 50f, _startX);
        _startPosition.y = Random.Range(- 5f, + 5f);
        _startPosition.z = Random.Range(- 200f, 100f);
        
        float speed1 = Random.Range(1f, 1.5f);

        var position = endPoint.transform.position;
        cloud.GetComponent<CloudController>().StartFloating(speed1, position.x, position.z);
    }
    
    private void AttemptCloudSpawn()
    {
        SpawnCloud();
        Invoke(nameof(AttemptCloudSpawn), spawnInterval);
    }
    
    private void FirstCloudSpawn()
    {
        SpawnCloud();
        _startPosition = transform.position;
        _startPosition.x = -75;
        SpawnCloud();
    }
}
