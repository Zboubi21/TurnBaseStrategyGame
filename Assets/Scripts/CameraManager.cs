using UnityEngine;
using GameDevStack.Patterns;

namespace TBSG.Combat
{
    public class CameraManager : SingletonMonoBehaviour<CameraManager>
    {
        [SerializeField] private Camera m_MainCamera = null;

        public Camera GetCurrentCamera()
        {
            return m_MainCamera;
        }
    }
}