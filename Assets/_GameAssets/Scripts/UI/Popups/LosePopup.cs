using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MaskTransitions;
public class LosePopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TimerUI _timerUI;
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;
    private RectTransform _losePopupTransform;
    void OnEnable()
    {
        BackgroundMusic.Instance.PlayBackgroundMusic(false);//Stop bg Music
        _tryAgainButton.onClick.AddListener(TryAgainButtonClicked);
        _timerText.text = _timerUI.GetFinalTime();
        _mainMenuButton.onClick.AddListener(() =>
      {
          TransitionManager.Instance.LoadLevel(Consts.SceneNames.MENU_SCENE);
      });
    }
    private void TryAgainButtonClicked()
    {
        TransitionManager.Instance.LoadLevel(Consts.SceneNames.GAME_SCENE);

    }
}
