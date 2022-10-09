using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UI.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

public class RackManager : MonoBehaviour
{
    private const string SAVED_PROGRESS = "SavedProgress";
    public const string OPEN_CARD_PROGRESS = "OpenedCardProgress";
    public const string RECIEVED_CARD_PROGRESS = "ReceivedCardProgress";
    public Transform GiftContainer;
    public GiftSO giftSO;

    private GameObject GiftPrefab;
    public Slider RackFillImage;
    
    public Button IncrementButton;
    public Button ResetButton;
    
    private float progress;
    private float maxValue;
    [SerializeField] private int increaseRate = 7;
    public delegate void OnSliderValueChangedEvent(int sliderValue);

    public static OnSliderValueChangedEvent OnSliderValueChanged;
    public static List<Gift> Gifts = new List<Gift>();
    private void Start()
    {
        progress = PlayerPrefs.GetFloat(SAVED_PROGRESS, 0);
        IncrementButton.onClick.AddListener(OnIncrementButtonClick);
        ResetButton.onClick.AddListener(OnResetButtonClick);
        RackFillImage.value = progress;
        GiftPrefab = Resources.Load("GiftPrefab") as GameObject;
        StartCoroutine(InitializeGifts());
    }

    private void OnResetButtonClick()
    {
        progress = 0;
        RackFillImage.value = 0;
        PlayerPrefs.SetFloat(SAVED_PROGRESS, progress);
        PlayerPrefs.SetInt(OPEN_CARD_PROGRESS, 0);
        PlayerPrefs.SetInt(RECIEVED_CARD_PROGRESS, 0);
        foreach (var gift in GiftContainer.GetComponentsInChildren<BaseGift>()) Destroy(gift.gameObject);
        StartCoroutine(InitializeGifts());
    }
    private void OnIncrementButtonClick()
    {
        progress += increaseRate;
        if (progress >= RackFillImage.maxValue * 0.9f)
        {
            RackFillImage.value = RackFillImage.maxValue;
            PlayerPrefs.SetFloat(SAVED_PROGRESS, progress);
            return;
        }
        RackFillImage.value = progress;
        PlayerPrefs.SetFloat(SAVED_PROGRESS, progress);
        OnSliderValueChanged.Invoke((int)progress);
    }
    public IEnumerator InitializeGifts()
    {
        for (int i = 0; i < giftSO.Gifts.Count; i++)
        {
            BaseGift go = Instantiate(GiftPrefab, GiftContainer).GetComponent<BaseGift>();
            go.SetData(giftSO.GetGiftValue(i), giftSO.GetGiftImage(i),giftSO.GetGiftUnlockImage(i));
            SetGiftUnlockValue(go,i);
            go.cardBitIndex = (int)Math.Pow(2,i);
        }
        yield return new WaitForSecondsRealtime(2);
        EnableFirstGiftToUnlock(GiftContainer);
    }
    private void EnableFirstGiftToUnlock(Transform GiftContainer)
    {
        Gift[] gifts = GiftContainer.GetComponentsInChildren<Gift>().ToArray();
        Gifts = gifts.ToList();
        List<Gift> giftList = gifts.Where(t => t.isRecieved).ToList();
        foreach (Gift gift in giftList) gift.GiftSelectButton.interactable = false;
        if(giftList.Any()) giftList.First().GiftSelectButton.interactable = true;
    }

    private void SetGiftUnlockValue(BaseGift gift, int index)
    {
        gift.giftUnlockValue = index switch
        {
            0 => RackFillImage.maxValue * 0.1f,
            1 => RackFillImage.maxValue * 0.3f,
            2 => RackFillImage.maxValue * 0.5f,
            3 => RackFillImage.maxValue * 0.7f,
            4 => RackFillImage.maxValue * 0.9f,
            _ => gift.giftUnlockValue
        };
    }
}
