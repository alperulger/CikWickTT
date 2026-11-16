using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image[] _playerHealthImages;
    [Header("Sprites")]
    [SerializeField] private Sprite _playerHealthySprite;
    [SerializeField] private Sprite _playerUnHealthySprite;
    [Header("Settings")]
    [SerializeField] private float _scaleDuration = 0.5f;   
    private RectTransform[] _playerHealthImagesRectTransforms;
    void Awake()
    {
        _playerHealthImagesRectTransforms = new RectTransform[_playerHealthImages.Length];
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            _playerHealthImagesRectTransforms[i] = _playerHealthImages[i].GetComponent<RectTransform>();
        }
    }
    void Start()
    {

    }
    

    // Update is called once per frame
//For Testing
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.O))
        {
            AnimateDamage();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AnimateDamageForAll();
        }
    }
    public void AnimateDamage()
    {
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            if (_playerHealthImages[i].sprite == _playerHealthySprite)
            {
                AnimateDamageSprite(_playerHealthImages[i], _playerHealthImagesRectTransforms[i]);
                break;
            }
        }   
    }
    public void AnimateDamageForAll()
    {
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            AnimateDamageSprite(_playerHealthImages[i], _playerHealthImagesRectTransforms[i]);
        }
    }
    private void AnimateDamageSprite(Image activeImage,RectTransform activeImageRectTransform)
    {
        activeImageRectTransform.DOScale(0f,_scaleDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            activeImage.sprite = _playerUnHealthySprite;
            activeImageRectTransform.DOScale(1f, _scaleDuration).SetEase(Ease.OutBack);
        });
    }


    }
