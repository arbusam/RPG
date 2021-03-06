using RPG.Control;
using RPG.Scene;
using UnityEngine;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        PlayerControls playerControls;

        private void Start()
        {
            playerControls = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        }

        private void OnEnable()
        {
            if (playerControls == null) return;
            Time.timeScale = 0;
            playerControls.SetCursor(CursorType.UI);
            playerControls.enabled = false;
        }

        private void OnDisable()
        {
            if (playerControls == null) return;
            Time.timeScale = 1;
            playerControls.enabled = true;
        }

        public void Save()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
        }

        public void SaveAndQuit()
        {
            Save();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.LoadMenu();
        }
    }
}