using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[System.Serializable]
	public class GiftItem
	{
		public int UnlockValue;
		public Sprite GiftSprite;
		public Sprite GiftUnlockSprite;
	}
	[CreateAssetMenu(menuName = "Gift Card", fileName = "Gift" )]
	public class GiftSO : ScriptableObject
	{
		public List<GiftItem> Gifts;
		public Sprite GetGiftImage(int index)=> Gifts[index].GiftSprite;
		public Sprite GetGiftUnlockImage(int index)=> Gifts[index].GiftUnlockSprite;
		public int GetGiftValue(int index)=> Gifts[index].UnlockValue;
	}
}