using Elympics;
using UnityEngine;
using UnityEngine.UI;

namespace ShooterPbE.Player
{
    public class CursorController : MonoBehaviour
    {
        public const float CURSOR_WORLD_POSITION_Y = 0.5f;

        public Vector3 WorldCursorPosition {get; private set;}

        [SerializeField] private RectTransform cursorImageTransform;
        [SerializeField] private Transform debugTrans;

        private Vector3 lastCursorPosition;

        public void ToggleCursorVisibility(bool visible)
        {
            if (visible)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }
        }

        private void Awake()
        {
            //ToggleCursorVisibility(false);
        }

        private void Update()
        {
            ProcessCursorCalculations();
        } 
        
        private void ProcessCursorCalculations()
        {
            var mouseRay = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Epsilon));
            lastCursorPosition = WorldCursorPosition;

            if (Physics.Raycast(mouseRay, out var hit))
            {
                WorldCursorPosition = new(hit.point.x, CURSOR_WORLD_POSITION_Y, hit.point.z);
            }
            else
            {
                WorldCursorPosition = lastCursorPosition;
            }

            debugTrans.position = WorldCursorPosition;
            cursorImageTransform.position = Input.mousePosition;
        }
    }
}
