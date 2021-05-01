using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace RPG.Scene
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(this);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            print("Scene Loaded");
            Destroy(this);
        }
    }
}