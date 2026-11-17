using UnityEngine;
using TMPro;
using DG.Tweening;


public class EggCounterUI : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private TMP_Text _eggCountText;
    [Header("Settings")]
    [SerializeField] private Color _eggCouterColor = Color.white;
    [SerializeField] private float _colorDuration;
    [SerializeField] private float _scaleDuration;
    private RectTransform _eggCounterRectTransform;
    private void Awake()
    {
        _eggCounterRectTransform =_eggCountText.gameObject.GetComponent<RectTransform>();
    }
    public void SetEggCounterText(int counter,int max)
    {
        _eggCountText.text=counter.ToString()+"/"+max.ToString();
    }

    public void SetEggCompleted()
    {
        _eggCountText.DOColor(_eggCouterColor,_colorDuration);
        _eggCounterRectTransform.DOScale(1.2f,_scaleDuration).SetEase(Ease.OutBack);

    }

}