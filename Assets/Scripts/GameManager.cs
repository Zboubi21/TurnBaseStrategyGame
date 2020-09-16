using UnityEngine;
using GameDevStack.Patterns;

namespace TBSG
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private LoadingScene m_LoadingScene;

        protected override void Awake()
        {
            base.Awake();
            m_LoadingScene = GetComponent<LoadingScene>();
        }

        public void LoadMainMenu() => m_LoadingScene.LoadMainMenu();
        public void LoadGame() => m_LoadingScene.LoadGame();
    }
}