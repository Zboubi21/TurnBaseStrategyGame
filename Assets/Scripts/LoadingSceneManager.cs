using UnityEngine;
using System.Collections;
using GameDevStack.Patterns;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TBSG
{
    public class LoadingSceneManager : SingletonMonoBehaviour<LoadingSceneManager>
    {
        private const string LOAD_ANIMATOR_NAME = "Load";
        private const float MIN_SCENE_LOADING_TIME = 1;

        [SerializeField] private Image m_LoadingBar = null;

        private AsyncOperation m_SceneLoadingOperation;
        private float m_LoadingProgress = 0;

        private CanvasGroup m_CanvasGroup;
        private Animator m_Animator;

        private enum SceneIndexes
        {
            MainMenu = 0,
            InGame = 1,
        }

        protected override void Awake()
        {
            base.Awake();
            m_CanvasGroup = GetComponent<CanvasGroup>();
            m_Animator = GetComponent<Animator>();
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadScene(SceneIndexes.MainMenu));
        }

        public void LoadGame()
        {
            StartCoroutine(LoadScene(SceneIndexes.InGame));
        }

        private IEnumerator LoadScene(SceneIndexes scene)
        {
            SetLoadingProgress(0);
            m_CanvasGroup.blocksRaycasts = true;
            m_Animator.SetBool(LOAD_ANIMATOR_NAME, true);

            yield return new WaitForSeconds(0.5f);

            m_SceneLoadingOperation = SceneManager.LoadSceneAsync((int)scene);
            m_SceneLoadingOperation.allowSceneActivation = false;

            float time = Time.time;
            while ((Time.time - time) <= MIN_SCENE_LOADING_TIME || m_SceneLoadingOperation.progress < 0.9f)
            {
                float fakeProgress = (Time.time - time) / MIN_SCENE_LOADING_TIME;
                float sceneLoadingProgress = m_SceneLoadingOperation.progress;
                m_LoadingProgress = Mathf.Min(fakeProgress, sceneLoadingProgress);
                SetLoadingProgress(m_LoadingProgress);
                yield return null;
            }
            m_SceneLoadingOperation.allowSceneActivation = true;

            yield return new WaitForSeconds(0.5f);

            m_Animator.SetBool(LOAD_ANIMATOR_NAME, false);
            m_CanvasGroup.blocksRaycasts = false;
        }

        private void SetLoadingProgress(float progress)
        {
            m_LoadingBar.fillAmount = progress;
        }
    }
}