using UnityEngine;

namespace Misc
{
    public class MainMenuCameraRotation : MonoBehaviour
    {
        public float rotationSpeed = 0.25f;
        private readonly Vector3 _pivotPoint = Vector3.zero;
        private readonly float _distance = 6.0f;
        private readonly float _height = 3.0f;

        void LateUpdate()
        {
            // Calculate the position of the camera based on the pivot point, distance, and height
            Vector3 cameraPosition = new Vector3(
                _pivotPoint.x + Mathf.Cos(Time.time * rotationSpeed) * _distance,
                _pivotPoint.y + _height,
                _pivotPoint.z + Mathf.Sin(Time.time * rotationSpeed) * _distance
            );

            // Set the camera's position
            transform.position = cameraPosition;
            
            // Aim the camera at the pivot point
            transform.LookAt(_pivotPoint);
        }
    }

}
