using UnityEngine;
using System.Collections.Generic;

//The PlayerMotor controls all playerplayer movement.
//The player moves perpendicular to the ground normal.
//The incline multiplier multiplies the movement force to overcome gravity depending on the steepness if incline.

namespace RGUnityTools
{
    [RequireComponent(typeof(Rigidbody))]
    public class RGFirstPersonController : MonoBehaviour
    {
        #region SINGLETON
        public static RGFirstPersonController instance;

        private void Awake()
        {
            instance = this;
            speedMultipliers = new Dictionary<string, float>();
        }
        #endregion

        Rigidbody rb;

        [Header("Viewport")]
        [SerializeField] Transform viewport;
        [SerializeField] Transform standingView;
        [SerializeField] Transform sneakingView;
        Transform viewportTarget;

        [Header("Moving:")]
        [SerializeField] KeyCode moveForward = KeyCode.W;
        [SerializeField] KeyCode moveBackward = KeyCode.S;
        [SerializeField] KeyCode moveLeft = KeyCode.A;
        [SerializeField] KeyCode moveRight = KeyCode.D;
        [SerializeField] KeyCode sneak = KeyCode.LeftControl;
        [SerializeField] float minSpeedMultiplier = 0.25f;
        [SerializeField] float maxSpeedMultiplier = 2f;
        [SerializeField] float sneakSpeed = 0.15f;
        [SerializeField] float walkSpeed = 0.2f;
        [SerializeField] float runSpeed = 0.35f;
        [SerializeField] float aerialSpeed = 0.1f;
        [SerializeField] float runVelocityThreshold = 5f;
        [SerializeField] float jumpForce = 50f;
        [SerializeField] float heightOffGround = 0.1f;
        [Range(0.5f, 5f)]
        [SerializeField] float inclineMultiplier = 1f;
        [SerializeField] readonly float jumpCheckSpacing = 0.25f;
        Vector2 movementInput;
        Vector2 movementOutput;
        float speed;
        [HideInInspector]
        public Dictionary<string, float> speedMultipliers;
        float inclineFactor;
        bool running;
        bool sneaking;

        [Header("Looking:")]
        [Range(0.01f, 1f)]
        [SerializeField] float xSensitivity = 1f;
        [Range(0.01f, 1f)]
        [SerializeField] float ySensitivity = 1f;
        [Range(0.01f, 1f)]
        [SerializeField] float xInterpolation = 1f;
        [Range(0.01f, 1f)]
        [SerializeField] float yInterpolation = 1f;
        [Range(-90f, 0f)]
        [SerializeField] float minXRotation = -75f;
        [Range(0f, 90f)]
        [SerializeField] float maxXRotation = 85f;
        float xRotation;
        float yRotation;
        Quaternion xTarget;
        Quaternion yTarget;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            speed = walkSpeed;

            viewportTarget = standingView;
            yRotation = transform.localEulerAngles.y;
        }

        private void Update()
        {
            if (Vector3.Distance(viewport.position, viewportTarget.position) > 0.10)
            {
                viewport.position = Vector3.Lerp(viewport.position, viewportTarget.position, 0.05f);
            }

            MovementInput();
            RotationInput();
        }

        private void FixedUpdate()
        {
            MovementOutput();
            RotationOutput();
        }

        #region GENERAL
        bool OnGround()
        {
            Vector3[] origins = new Vector3[5];
            origins[0] = transform.position;
            origins[1] = transform.position + transform.forward * jumpCheckSpacing;
            origins[2] = transform.position - transform.right * jumpCheckSpacing;
            origins[3] = transform.position + transform.right * jumpCheckSpacing;
            origins[4] = transform.position - transform.forward * jumpCheckSpacing;


            foreach (Vector3 origin in origins)
            {
                Ray ray = new Ray(origin + Vector3.up, Vector3.down);
                RaycastHit hit;
                //Debug.DrawRay(origin + Vector3.up, Vector3.down);

                if (Physics.Raycast(ray, out hit, heightOffGround))
                {
                    if (hit.collider != null)
                        return true;
                }
            }

            return false;
        }

        Vector3 GroundNormal()
        {
            RaycastHit hit;
            float range = 0.5f;
            if (Physics.Raycast(transform.position, -transform.up, out hit, range, 10))
            {
                //VectorVisualizer.instance.DrawVector(transform.position, hit.normal * 4, Color.green , 0);
                return hit.normal.normalized;
            }

            return Vector3.up;
        }

        public void SetViewportRotation(Quaternion rotation)
        {
            xTarget.x = rotation.x;
            yTarget.y = rotation.y;
        }
        #endregion

        #region MOVEMENT
        void MovementInput()
        {
            if (Time.timeScale <= 0f) return;

            movementInput = Vector2.zero;

            //Get key presses:
            if (Input.GetKeyDown(sneak)) ToggleSneak();
            if (Input.GetKey(moveForward)) movementInput.y += 1;
            if (Input.GetKey(moveBackward)) movementInput.y -= 1;
            if (Input.GetKey(moveLeft)) movementInput.x -= 1;
            if (Input.GetKey(moveRight)) movementInput.x += 1;

            //Determine player speed:
            speed = walkSpeed;

            if (sneaking)
            {
                speed = sneakSpeed;
            }
            else 
            if (running)
            {
                speed = runSpeed;
            }

            if (!OnGround())
                speed = aerialSpeed;

            float speedMultiplier = 1f;
            foreach (KeyValuePair<string, float> m in speedMultipliers)
            {
                speedMultiplier *= m.Value;
            }
            if (speedMultiplier < minSpeedMultiplier)
                speedMultiplier = minSpeedMultiplier;
            else if (speedMultiplier > maxSpeedMultiplier)
                speedMultiplier = maxSpeedMultiplier;

            speed *= speedMultiplier;

            //Calculate incline factor
            inclineFactor = Mathf.Abs(Vector3.Dot(GroundNormal(), rb.velocity.normalized));

            movementOutput = speed * movementInput.normalized;
        }

        void MovementOutput()
        {
            //Move
            Vector3 movementVector = new Vector3(movementOutput.x, 0f, movementOutput.y);
            movementVector *= (1 + inclineFactor * inclineMultiplier);
            rb.AddRelativeForce(movementVector, ForceMode.VelocityChange);

            //Debug.Log("Incline" + inclineFactor * 90);
            //VectorVisualizer.instance.DrawVector(transform.position, rb.velocity, Color.magenta, 1);
        }
        #endregion

        #region ROTATION
        void RotationInput()
        {
            if (Time.timeScale <= 0f) return;

            //Process X rotation:
            xRotation -= Input.GetAxisRaw("Mouse Y") * xSensitivity;
            if (xRotation > maxXRotation) xRotation = maxXRotation;
            if (xRotation < minXRotation) xRotation = minXRotation;

            //Process Y rotation:
            yRotation += Input.GetAxisRaw("Mouse X") * ySensitivity;
            yRotation %= 360f;

            //Create a target rotation to spherically interpolate towards
            xTarget = Quaternion.Euler(xRotation, 0f, 0f);
            yTarget = Quaternion.Euler(0f, yRotation, 0f);

            //Debug.Log($"Movement input: {movementInput.x}:{movementInput.y}, Rotation input: {Input.mouseScrollDelta.x}:{Input.mouseScrollDelta.y}, Rotation: {xRotation}:{yRotation}");
        }

        void RotationOutput()
        {
            //Spherically interpolate from current rotation to the target rotation
            viewport.transform.localRotation = Quaternion.Slerp(viewport.transform.localRotation, xTarget, xInterpolation);
            transform.rotation = Quaternion.Slerp(transform.rotation, yTarget, yInterpolation);
        }
        #endregion

        #region RUN
        void StartRunning()
        {
            if (!OnGround()) return;
            running = true;
        }

        void StopRunning()
        {
            running = false;
        }
        #endregion

        #region SNEAK
        void ToggleSneak()
        {
            if (sneaking)
            {
                Stand();
                return;
            }

            Sneak();
        }

        void Sneak()
        {
            //if (!OnGround()) return;
            sneaking = true;
            viewportTarget = sneakingView;
        }

        void Stand()
        {
            sneaking = false;
            viewportTarget = standingView;
        }
        #endregion

        #region JUMP
        void Jump()
        {
            if (!OnGround()) return;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        #endregion
    }
}