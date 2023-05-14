using System.Runtime.CompilerServices;
using UnityEngine;

namespace Misc
{
    public class SpawnRandomLand : MonoBehaviour
    {

        public GameObject[] landPrefabs;

        private Animator _animator;
        
        // Start is called before the first frame update
        void Start()
        {
            var randomIndex = UnityEngine.Random.Range(0, landPrefabs.Length);
            var randomPrefab = landPrefabs[randomIndex];
            GameObject obj = Instantiate(randomPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            _animator = obj.transform.GetChild(0).GetComponent<Animator>();
            _animator.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
