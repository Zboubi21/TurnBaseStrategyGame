using UnityEngine;
using GameDevStack.Patterns;

namespace TBSG
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void LoadMainMenu() => LoadingSceneManager.Instance.LoadMainMenu();
        public void LoadGame() => LoadingSceneManager.Instance.LoadGame();
    }
}