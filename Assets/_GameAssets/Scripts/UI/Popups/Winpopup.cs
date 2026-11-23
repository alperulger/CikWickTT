using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MaskTransitions;

public class Winpopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TimerUI _timerUI;
    [SerializeField] private Button _oneMoreButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;

    void OnEnable()
    {
        BackgroundMusic.Instance.PlayBackgroundMusic(false);//Stop bg Music
        _oneMoreButton.onClick.AddListener(OnOneMoreButtonClicked);
        _timerText.text = _timerUI.GetFinalTime();
        _mainMenuButton.onClick.AddListener(() =>
        {
            TransitionManager.Instance.LoadLevel(Consts.SceneNames.MENU_SCENE);
        });
    }
    private void OnOneMoreButtonClicked()
    {
        TransitionManager.Instance.LoadLevel(Consts.SceneNames.GAME_SCENE);

    }
}
