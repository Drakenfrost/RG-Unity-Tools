using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RGUnityTools
{
    public class RGFreeViewport : MonoBehaviour
    {
        public Transform viewport;
        public Camera cam;

        //Rotation
        public bool allowRotation;
        public KeyCode rotateButton = KeyCode.Mouse1;
        public bool invertXRotation;
        public bool invertYRotation;
        [Range(0.1f, 5f)]
        public float xRotationSensitivity = 3f;
        [Range(0.1f, 5f)]
        public float yRotationSensitivity = 3f;

        public bool Rotating { get; private set; } = false;

        //Pan
        public bool allowPanning;
        public KeyCode panButton = KeyCode.Mouse2;
        public bool invertPan;
        [Range(0.1f, 5f)]
        public float panSensitivity = 3f;

        public bool Panning { get; private set; } = false;

        //Zoom
        [Tooltip("Use the mouse wheel to zoom.")]
        public bool allowZooming;
        [Range(1f, 5f)]
        public float zoomSensitivity = 1f;
        [Range(0.1f, 1f)]
        public float zoomInterpolation = 1f;
        [Min(0f)]
        public float minZoom = 20f;
        [Min(1f)]
        public float maxZoom = 100f;

        private float zoomTarget;

        private void Start()
        {
            if (cam == null)
                cam = viewport.GetComponent<Camera>();

            zoomTarget = minZoom;
        }

        private void Update()
        {
            UserInput();

            if (Rotating || Panning)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void FixedUpdate()
        {
            TranformOutput();
        }

        private void UserInput()
        {
            //Rotation input
            if (allowRotation)
            {
                if (Input.GetKeyDown(rotateButton))
                    Rotating = true;
                if (Input.GetKeyUp(rotateButton))
                    Rotating = false;
            }

            //Pan input
            if (allowPanning)
            {
                if (Input.GetKeyDown(panButton))
                    Panning = true;
                if (Input.GetKeyUp(panButton))
                    Panning = false;
            }

            //Zoom input
            if (allowZooming)
            {
                if (Input.mouseScrollDelta.y < 0 && zoomTarget < maxZoom)
                    zoomTarget -= Input.mouseScrollDelta.y * zoomSensitivity;
                if (Input.mouseScrollDelta.y > 0 && zoomTarget > minZoom)
                    zoomTarget -= Input.mouseScrollDelta.y * zoomSensitivity;
            }
        }

        private void TranformOutput()
        {
            //Rotation
            if (Rotating && !Panning)
            {
                float xMultiplier = xRotationSensitivity;
                if (invertXRotation)
                    xMultiplier = -xRotationSensitivity;

                float yMultiplier = -yRotationSensitivity;
                if (invertYRotation)
                    yMultiplier = yRotationSensitivity;

                transform.Rotate(Vector3.up, Input.GetAxisRaw("Mouse X") * xMultiplier, Space.World);
                transform.Rotate(viewport.right, Input.GetAxisRaw("Mouse Y") * yMultiplier, Space.World);
            }

            //Panning
            if (Panning && !Rotating)
            {
                float multiplier = -panSensitivity;
                if (invertPan)
                    multiplier = panSensitivity;

                transform.Translate(viewport.right * Input.GetAxisRaw("Mouse X") * multiplier, Space.World);
                transform.Translate(transform.forward * Input.GetAxisRaw("Mouse Y") * multiplier, Space.World);
            }

            //Zooming
            if (cam == null) return;
            if (cam.orthographicSize != zoomTarget)
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, zoomInterpolation);
        }
    }
}
