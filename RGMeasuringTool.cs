using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RGUnityTools
{
    public class RGMeasuringTool : MonoBehaviour
    {
        public bool drawSphere;
        public bool drawWireSphere;
        public float sphereRadius;
        public Color sphereColor;
        public bool drawCube;
        public bool drawWireCube;
        public float cubeSize;
        public Color cubeColor;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = sphereColor;
            if (drawSphere)
                Gizmos.DrawSphere(transform.position, sphereRadius);
            if (drawWireSphere)
                Gizmos.DrawWireSphere(transform.position, sphereRadius);
            Gizmos.color = cubeColor;
            if (drawCube)
                Gizmos.DrawCube(transform.position, new Vector3(cubeSize * 2, cubeSize * 2, cubeSize * 2));
            if (drawWireCube)
                Gizmos.DrawWireCube(transform.position, new Vector3(cubeSize * 2, cubeSize * 2, cubeSize * 2));
        }
    }
}
