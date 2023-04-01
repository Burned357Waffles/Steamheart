using System.Runtime.CompilerServices;
using UnityEngine;

namespace Misc
{
    public class SpawnRandomLand : MonoBehaviour
    {

        public GameObject[] landPrefabs;
        
        
        // Start is called before the first frame update
        void Start()
        {
            var randomIndex = UnityEngine.Random.Range(0, landPrefabs.Length);
            var randomPrefab = landPrefabs[randomIndex];
            Instantiate(randomPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
