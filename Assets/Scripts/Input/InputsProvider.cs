using ShooterPbE.Player;
using UnityEngine;

namespace ShooterPbE.Inputs
{
    public class InputsProvider : MonoBehaviour
    {
        public bool Jump { get; private set; }
        public bool Shoot { get; private set; }
        public Vector2 Movement { get; private set; }
        public Vector3 WorldCursorPosition { get; private set; }

        [SerializeField] private float mouseSensivity = 1.5f;
        [SerializeField] private CursorController cursorController;

        private void Update()
        {
            Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Jump = Input.GetButton("Jump");
            Shoot = Input.GetButton("Fire1");
            WorldCursorPosition = cursorController.WorldCursorPosition;
        }
    }
}
