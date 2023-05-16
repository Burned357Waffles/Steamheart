using System;
using UnityEngine;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject gear;

        private void Awake()
        {
            Application.targetFrameRate = 30;
        }

        // Update is called once per frame
        void Update()
        {
            gear.transform.Rotate(0, 0, -2, Space.Self);
        }
    }
}
