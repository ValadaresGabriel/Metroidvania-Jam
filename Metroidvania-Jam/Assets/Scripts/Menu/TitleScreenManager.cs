using UnityEngine;
using UnityEngine.UI;

namespace TS
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance { get; private set; }

        [Header("Menus")]
        [SerializeField] private GameObject optionPanel;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject titleScreenMainMenu;
        [SerializeField] private GameObject titleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] private Button mainMenuNewGameButton;
        [SerializeField] private Button mainMenuLoadGameButton;
        [SerializeField] private Button loadMenuReturnButton;
        [SerializeField] private Button noCharacterSlotsOkayButton;
        [SerializeField] private Button deleteCharacterPopUpConfirmButton;

        [Header("Pop Ups")]
        [SerializeField] private GameObject noCharacterSlotsPopUp;
        [SerializeField] private GameObject deleteCharacterSlotPopUp;

        [Header("Character Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void InstantiatePlayer()
        {
            Instantiate(playerPrefab);
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.Instance.AttemptToCreateNewGame();
        }

        public void OpenLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(true);
            titleScreenLoadMenu.SetActive(false);

            mainMenuLoadGameButton.Select();
        }

        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkayButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }

        #region Character Slots

        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }

        public void CloseDeleteCharacterSlot()
        {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(false);
                loadMenuReturnButton.Select();
            }
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.Instance.DeleteGame(currentSelectedSlot);

            // Disable and enable to update screen
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }

        #endregion

        public void ExitGame()
        {
            Application.Quit();
        }

        public void OpenOption()
        {
            optionPanel.SetActive(true);
        }

        public void CloseOption()
        {
            optionPanel.SetActive(false);
        }
    }
}