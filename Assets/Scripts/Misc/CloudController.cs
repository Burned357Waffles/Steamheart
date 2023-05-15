using System;
using UnityEngine;

namespace Misc
{
    public class CloudController : MonoBehaviour
    {
        private float _moveSpeed;
        private float _endPosX;
        private float _endPosZ;

        private void Update()
        {
            transform.Translate(Vector3.right * (Time.deltaTime * _moveSpeed));
            
            if (transform.position.x < _endPosX) Destroy(gameObject);
            //else if (transform.position.z > _endPosZ) Destroy(gameObject);
        }

        public void StartFloating(float speed, float endPosX, float endPosZ)
        {
            _moveSpeed = speed;
            _endPosX = endPosX;
            _endPosZ = endPosZ;
        }
    }
}