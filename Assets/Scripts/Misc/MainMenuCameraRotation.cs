using UnityEngine;

namespace Misc
{
    public class MainMenuCameraRotation : MonoBehaviour
    {
        public float rotationSpeed = 0.05f;
        private Vector3 pivotPoint = Vector3.zero;
        private float distance = 6.0f;
        private float height = 3.0f;

        void LateUpdate()
        {
            // Calculate the position of the camera based on the pivot point, distance, and height
            Vector3 cameraPosition = new Vector3(
                pivotPoint.x + Mathf.Cos(Time.time * rotationSpeed) * distance,
                pivotPoint.y + height,
                pivotPoint.z + Mathf.Sin(Time.time * rotationSpeed) * distance
            );

            // Set the camera's position
            transform.position = cameraPosition;
            
            // Aim the camera at the pivot point
            transform.LookAt(pivotPoint);
        }
    }

}
