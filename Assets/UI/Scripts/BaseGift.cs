namespace UI.Scripts
{
	using TMPro;
	using UnityEngine;
	using Image = UnityEngine.UI.Image;
	using Button = UnityEngine.UI.Button;
	using System;
	using DG.Tweening;
	public class BaseGift : MonoBehaviour, IGift
	{
		public bool isOpen = false;
		public bool isRecieved;
		public Image GiftImage;
		public Image GlowImage;
		public TMP_Text GiftValue;
		public Sprite GiftUnlockSprite;
		internal float giftUnlockValue;
		public Button GiftSelectButton;
		public int cardBitIndex;
		private int SavedOpenedCardBitIndex;
		private int SavedReceivedCardBitIndex;
		public void Collect()
		{
			SavedOpenedCardBitIndex = PlayerPrefs.GetInt(RackManager.OPEN_CARD_PROGRESS,0);
			SavedOpenedCardBitIndex |= cardBitIndex;
			PlayerPrefs.SetInt(RackManager.OPEN_CARD_PROGRESS, SavedOpenedCardBitIndex);
			GiftImage.overrideSprite = GiftUnlockSprite;
			GlowImage.DOFade(0, 0.1f);
		}

		public void SetData(int value, Sprite giftIcon, Sprite unlockedIcon)
		{
			GiftValue.text = value.ToString();
			GiftImage.sprite = giftIcon;
			GiftUnlockSprite = unlockedIcon;
		}

		public void AvailableToCollect()
		{
			if(isOpen || isRecieved) return;
			GlowImage.DOFade(1, 1);
			GlowImage.transform.DOLocalRotate(new Vector3(0, 0, 360), 10, RotateMode.FastBeyond360)
				.SetRelative(true).SetEase(Ease.Linear).SetLoops(-1);			
			GlowImage.transform.DOScale(1.2f, 1f).SetEase(Ease.OutElastic).OnComplete(() =>
				GlowImage.transform.DOScale(1.1f, 1f).SetEase(Ease.Linear)).SetLoops(-1,LoopType.Yoyo);
			GiftImage.transform.DOScale(1.3f, 1f).SetEase(Ease.OutElastic).OnComplete(() =>
				GiftImage.transform.DOScale(1.1f, 0.5f).SetEase(Ease.InElastic));
			GiftImage.transform.DOShakeRotation(0.5f);
			SavedReceivedCardBitIndex = PlayerPrefs.GetInt(RackManager.RECIEVED_CARD_PROGRESS,0);
			SavedReceivedCardBitIndex |= cardBitIndex;
			PlayerPrefs.SetInt(RackManager.RECIEVED_CARD_PROGRESS, SavedReceivedCardBitIndex);
		}

		public void Disable()
		{
			GiftSelectButton.interactable = false;
		}
	}
}