using UnityEngine;
using Elympics;

namespace ShooterPbE.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : ElympicsMonoBehaviour
    {
        private const float GROUND_CHECK_DIMENSIONS_HORIZONTAL = 0.3f;
        private const float GROUND_CHECK_DIMENSIONS_VERTICAL = 0.001f;
        private const float GROUND_CHECK_Y_OFFSET = 1.5f;

        [Header("Parameters:")]
        [SerializeField] private float movementSpeed = 10.0f;
        [SerializeField] private float acceleration = 50.0f;
        [SerializeField] private float jumpForce = 10.0f;
        [SerializeField] private Rigidbody rig;
        [SerializeField] private LayerMask groundMask;

        private bool IsGrounded => Physics.CheckBox(transform.position - Vector3.up * GROUND_CHECK_Y_OFFSET, 
            new Vector3(GROUND_CHECK_DIMENSIONS_HORIZONTAL, GROUND_CHECK_DIMENSIONS_VERTICAL, GROUND_CHECK_DIMENSIONS_HORIZONTAL), 
            Quaternion.identity, groundMask, QueryTriggerInteraction.Ignore);

        public void ProcessMovement(float forwardMovementValue, float rightMovementValue, bool isJumping)
        {
            Vector3 inputVector = new Vector3(forwardMovementValue, 0, rightMovementValue);
            Vector3 movementDirection = inputVector != Vector3.zero ? this.transform.TransformDirection(inputVector.normalized) : Vector3.zero;

            ApplyMovement(movementDirection);

            if (isJumping && IsGrounded)
            {
                ApplyJump();
            }
        }

        private void ApplyMovement(Vector3 movementDirection)
        {
            Vector3 defaultVelocity = movementDirection * movementSpeed;
            Vector3 fixedVelocity = Vector3.MoveTowards(rig.velocity, defaultVelocity, Elympics.TickDuration * acceleration);

            rig.velocity = new Vector3(fixedVelocity.x, rig.velocity.y, fixedVelocity.z);
        }

        private void ApplyJump()
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}

