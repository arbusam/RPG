using System;
using GameDevTV.Utils;
using RPG.Scene;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        LazyValue<SavingWrapper> savingWrapper;

        [SerializeField] TMP_InputField newGameNameField;
        [SerializeField] Button newGameCreateButton;

        private void Awake()
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
            newGameCreateButton.onClick.AddListener(NewGame);
        }

        private void Update()
        {
            newGameCreateButton.interactable = newGameNameField.text != "";
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        public void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }

        public void NewGame()
        {
            savingWrapper.value.NewGame(newGameNameField.text);
        }
    }
}