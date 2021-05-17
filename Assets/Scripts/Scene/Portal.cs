using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.Scene
{
    public class Portal : MonoBehaviour
    {

        enum DestinationIdentifier
        {
            Portal1, Portal2, Portal3, Portal4, Portal5, Portal6, Portal7, Portal8, Portal9, Portal10
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;

        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError(this.name + "'s sceneToLoad is not set");
                yield break;
            }

            DontDestroyOnLoad(this.gameObject);

            Fader fader = FindObjectOfType<Fader>();
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<PlayerControls>().SetCursor(CursorType.None);
            player.GetComponent<PlayerControls>().enabled = false;
            
            yield return fader.FadeOut(fadeOutTime);

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();

            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<PlayerControls>().SetCursor(CursorType.None);
            player.GetComponent<PlayerControls>().enabled = false;

            wrapper.Load();
            
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
            wrapper.Save();

            player.GetComponent<PlayerControls>().enabled = true;
            Destroy(this.gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }

            return null;
        }
    }
}