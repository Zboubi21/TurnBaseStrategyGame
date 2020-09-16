using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace TBSG
{
    public class LoadingScene : MonoBehaviour
    {
        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}