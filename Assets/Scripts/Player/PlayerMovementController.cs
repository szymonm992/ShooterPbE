using UnityEngine;
using Elympics;

namespace ShooterPbE.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : ElympicsMonoBehaviour
    {
        private const float GROUND_CHECK_DIMENSIONS_HORIZONTAL = 0.3f;
        private const float GROUND_CHECK_DIMENSIONS_VERTICAL = 0.05f;
        private const float GROUND_CHECK_Y_OFFSET = 0.025f;

        [SerializeField] private float movementSpeed = 10.0f;
        [SerializeField] private float acceleration = 50.0f;
        [SerializeField] private float jumpForce = 10.0f;
        [SerializeField] private Rigidbody rig;
        [SerializeField] private CursorController cursorController;
        [SerializeField] private LayerMask groundMask;

        private bool IsGrounded => Physics.CheckBox(transform.position - Vector3.up * GROUND_CHECK_Y_OFFSET, 
            new Vector3(GROUND_CHECK_DIMENSIONS_HORIZONTAL, GROUND_CHECK_DIMENSIONS_VERTICAL, GROUND_CHECK_DIMENSIONS_HORIZONTAL), 
            Quaternion.identity, groundMask, QueryTriggerInteraction.Ignore);

        public void ProcessMovement(float forwardMovementValue, float rightMovementValue, bool isJumping, Vector3 lookAtPosition)
        {
            var inputVector = new Vector3(forwardMovementValue, 0, rightMovementValue);

            ApplyMovement(inputVector);
            ApplyRotation(lookAtPosition);
     
            if (isJumping && IsGrounded)
            {
                ApplyJump();
            }
        }

        private void ApplyMovement(Vector3 movementDirection)
        {
            var defaultVelocity = movementDirection * movementSpeed;
            var fixedVelocity = Vector3.MoveTowards(rig.velocity, defaultVelocity, Elympics.TickDuration * acceleration);

            rig.velocity = new Vector3(fixedVelocity.x, rig.velocity.y, fixedVelocity.z);
        }

        private void ApplyJump()
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void ApplyRotation(Vector3 lookAtPosition)
        {
            var desiredDirection = (lookAtPosition - rig.position);
            desiredDirection.y = 0;
            var desiredRotation = Quaternion.LookRotation(desiredDirection.normalized, Vector3.up);
            rig.MoveRotation(desiredRotation);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawCube(transform.position - (Vector3.up * GROUND_CHECK_Y_OFFSET),
            new Vector3(GROUND_CHECK_DIMENSIONS_HORIZONTAL, GROUND_CHECK_DIMENSIONS_VERTICAL, GROUND_CHECK_DIMENSIONS_HORIZONTAL));
        }
        #endif
    }
}

