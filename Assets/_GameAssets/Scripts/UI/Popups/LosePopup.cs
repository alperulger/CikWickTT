using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
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
         _tryAgainButton.onClick.AddListener(TryAgainButtonClicked);
        _timerText.text=_timerUI.GetFinalTime();
    }
    private void TryAgainButtonClicked()
    {
        SceneManager.LoadScene(Consts.SceneNames.GAME_SCENE);
    }
}
