using System;
using UnityEngine;

namespace UI.HUD
{
    public class Billboard : MonoBehaviour
    {
        private Transform _camera;

        private void Start()
        {
            _camera = FindObjectOfType<Camera>().transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.forward);
        }
    }
}
