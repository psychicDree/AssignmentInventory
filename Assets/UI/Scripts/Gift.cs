using System;
using DG.Tweening;
using UI.Scripts;
using Unity.VisualScripting;
using UnityEngine;

public class Gift : BaseGift
{
	
	public void OnEnable()
	{
		RackManager.OnSliderValueChanged += OnSliderValueChangedHandler;
	}

	private void OnDisable()
	{
		RackManager.OnSliderValueChanged -= OnSliderValueChangedHandler;
	}
	private void Start()
	{
		GiftSelectButton.onClick.AddListener(OnGiftCollectClicked);
		SetReceivedGift();
		if(!isRecieved) return;
		SetOpenedAndClosedGift();
		if(!isOpen )AvailableToCollect();
	}

	private void OnGiftCollectClicked()
	{
		Collect();
		Gift nextGift = RackManager.Gifts[transform.GetSiblingIndex() + 1];
		if (nextGift.isRecieved == true)
		{
			nextGift.GiftSelectButton.interactable = true;
		}
	}

	private void SetOpenedAndClosedGift()
	{
		int savedData = PlayerPrefs.GetInt(RackManager.OPEN_CARD_PROGRESS, 0);
		if ((savedData & cardBitIndex) != 0)
		{
			isOpen = true;
			isRecieved = false;
			GiftSelectButton.interactable = false;
			GiftImage.overrideSprite = GiftUnlockSprite;
			GlowImage.DOFade(0, 0.1f);
		}
		else
		{
			isOpen = false;
			GiftSelectButton.interactable = true;
		}
	}
	private void SetReceivedGift()
	{
		int savedData = PlayerPrefs.GetInt(RackManager.RECIEVED_CARD_PROGRESS, 0);

		if ((savedData & cardBitIndex) != 0) isRecieved = true;
	}
	private void OnSliderValueChangedHandler(int sliderValue)
	{
		if (!(sliderValue > giftUnlockValue)) return;
		AvailableToCollect();
		if(transform.GetSiblingIndex() != 0 &&RackManager.Gifts[transform.GetSiblingIndex() - 1].isOpen)GiftSelectButton.interactable = true;
		else if(transform.GetSiblingIndex() == 0)GiftSelectButton.interactable = true;
		isRecieved = true;
	}
}


