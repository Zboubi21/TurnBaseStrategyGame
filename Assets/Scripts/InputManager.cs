using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using GameDevStack.Patterns;

namespace TBSG
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        public static event Action OnPressLeftMouseButton;
        public static event Action OnPressRightMouseButton;
        public static event Action OnPressMiddleMouseButton;

        private void Update()
        {
            if (IsPointerOverUIObject(Input.mousePosition)) return;

            if (Input.GetMouseButtonDown(0)) OnPressLeftMouseButton?.Invoke();
            if (Input.GetMouseButtonDown(1)) OnPressRightMouseButton?.Invoke();
            if (Input.GetMouseButtonDown(2)) OnPressMiddleMouseButton?.Invoke();
        }

        public static bool IsPointerOverUIObject(Vector2 position)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = position
            };

            EventSystem s = EventSystem.current;
            if (s != null)
            {
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                return results.Count > 0;
            }
            else
                return false;
        }
    }
}