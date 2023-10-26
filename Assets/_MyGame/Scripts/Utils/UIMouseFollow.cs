using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curio.Gameplay
{
    public class UIMouseFollow : MonoBehaviour
    {
        public Canvas TargetCanvas;
        protected Vector2 _newPosition;
        protected Vector2 _mousePosition;

        protected virtual void LateUpdate()
        {
#if !ENABLE_INPUT_SYSTEM || ENABLE_LEGACY_INPUT_MANAGER
            _mousePosition = Input.mousePosition;
#else
			_mousePosition = Mouse.current.position.ReadValue();
#endif
            RectTransformUtility.ScreenPointToLocalPointInRectangle(TargetCanvas.transform as RectTransform, _mousePosition, TargetCanvas.worldCamera, out _newPosition);
            transform.position = TargetCanvas.transform.TransformPoint(_newPosition);
        }

    }
}