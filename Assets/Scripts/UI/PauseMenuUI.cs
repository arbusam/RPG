using RPG.Control;
using UnityEngine;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        PlayerControls playerControls;

        private void Awake()
        {
            playerControls = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
        }

        private void OnEnable()
        {
            Time.timeScale = 0;
            playerControls.SetCursor(CursorType.UI);
            playerControls.enabled = false;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
            playerControls.enabled = true;
        }
    }
}