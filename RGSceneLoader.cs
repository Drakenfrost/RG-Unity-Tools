using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

namespace RGUnityTools
{
    public class RGSceneLoader : MonoBehaviour
    {
        public int sceneIndexToLoad = 1;
        public Button button;

        private void Start()
        {
            button.onClick.AddListener(LoadScene);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(sceneIndexToLoad);
        }
    }
}
