using UnityEngine;

namespace Misc
{
    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraTransform;

        public float normalSpeed;
        public float fastSpeed;
        public float movementSpeed;
        public float movementTime;
        public float rotationAmount;
        public float minZoom;
        public float maxZoom;
        public float worldBorderX;
        public float wordBorderZ;
        public Vector3 zoomAmount;

        public Vector3 newPosition;
        public Vector3 newZoom;
        public Quaternion newRotation;
        public Quaternion newAngle;

        public Vector3 dragStartPosition;
        public Vector3 dragCurrentPosition;
        public Vector3 rotateStartPosition;
        public Vector3 rotateCurrentPosition;
        private Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
            var transform1 = transform;
            newPosition = transform1.position;
            newRotation = transform1.rotation;
            newZoom = cameraTransform.localPosition;
        }

        public void Update()
        {
            HandleMovementInput();
            HandleMouseInput();
        }

        private void HandleMouseInput()
        {
            if(Input.mouseScrollDelta.y != 0)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount;
            }
            if(Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }
        
            if(Input.GetMouseButton(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                Ray ray = _camera!.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);
                    newPosition = transform.position + dragStartPosition - dragCurrentPosition; 
                }
            }
        
            if (Input.GetMouseButtonDown(2))
            {
                rotateStartPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                rotateCurrentPosition = Input.mousePosition;

                Vector3 difference = rotateStartPosition - rotateCurrentPosition;

                rotateStartPosition = rotateCurrentPosition;

                newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            }
        }

        private void HandleMovementInput()
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * movementSpeed);
            }

            if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (transform.forward * -movementSpeed);
            }

            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * movementSpeed);
            }
        
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (transform.right * -movementSpeed);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }

            if (Input.GetKey(KeyCode.E))
            {
                newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            if (Input.GetKey(KeyCode.R))
            {
                newZoom += zoomAmount;
            }

            if (Input.GetKey(KeyCode.F))
            {
                newZoom += -zoomAmount;
            }

            clampCamera();
            Transform transform1;
            (transform1 = transform).position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform1.rotation, newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);        
        }

        private void clampCamera()
        {
            // binds position to world border
            float clampedPositionX = Mathf.Clamp(newPosition.x, -worldBorderX, worldBorderX);
            float clampedPositionZ = Mathf.Clamp(newPosition.z, -wordBorderZ, wordBorderZ);
            newPosition = new Vector3(clampedPositionX, 0.1f, clampedPositionZ);

            // binds position to min and max zoom amount
            float clampedZoomY = Mathf.Clamp(newZoom.y, minZoom, maxZoom);
            float clampedZoomZ = Mathf.Clamp(newZoom.z, -maxZoom, -minZoom);
            newZoom = new Vector3(0, clampedZoomY, clampedZoomZ);
        }

    }
}