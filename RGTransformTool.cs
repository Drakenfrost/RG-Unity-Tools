using UnityEngine;

namespace RGUnityTools
{
    [ExecuteInEditMode]
    public class RGTransformTool : MonoBehaviour
    {
        [SerializeField] Transform targetTransform;

        [Header("Transform Tools")]
        [SerializeField] Vector3 positionOffset;
        [SerializeField] bool followTargetPosition;
        [Range(0.001f, 1f)]
        [SerializeField] float positionInterpolation = 1f;

        [Header("Rotation Tools")]
        [SerializeField] Vector3 rotationOffset;
        [SerializeField] bool followTargetRotation;
        [Range(0.001f, 1f)]
        [SerializeField] float rotationInterpolation = 1f;

        [SerializeField] Vector3 spawnSize;

        void Update()
        {
            if (targetTransform == null) return;

            if (followTargetPosition)
                FollowPosition(targetTransform.position + positionOffset, positionInterpolation);

            if (followTargetRotation)
                FollowRotation(targetTransform.rotation * Quaternion.Euler(rotationOffset), rotationInterpolation);
        }
        
        public void FollowPosition(Vector3 position, float interpolation)
        {
            if (positionInterpolation == 1f)
            {
                transform.position = position;
                return;
            }

            transform.position = Vector3.Lerp(transform.position, position, interpolation);
        }

        public void FollowRotation(Quaternion rotation, float interpolation)
        {
            if (rotationInterpolation == 1f)
            {
                transform.rotation = rotation;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, interpolation * Time.deltaTime);
        }
    }
}
