using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Winpopup : MonoBehaviour
{
  [Header("References")]
  [SerializeField] TimerUI _timerUI;
  [SerializeField] private Button _oneMoreButton;
  [SerializeField] private Button _mainMenuButton;
  [SerializeField] private TMP_Text _timerText;
  
    void OnEnable()
    {
         _oneMoreButton.onClick.AddListener(OnOneMoreButtonClicked);
        _timerText.text=_timerUI.GetFinalTime();
    }
    private void OnOneMoreButtonClicked()
    {
        SceneManager.LoadScene(Consts.SceneNames.GAME_SCENE);
    }
}
