using UnityEngine;

namespace ShooterPbE.Inputs
{
    public class InputsProvider : MonoBehaviour
    {
        public bool Jump { get; private set; }
        public Vector2 Movement { get; private set; }

        private void Update()
        {
            Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Jump = Input.GetButton("Jump");
        }
    }
}
