using UnityEngine;

namespace UI
{
	public interface IGift
	{
		public void Collect();
		public void SetData(int value, Sprite giftIcon,Sprite unlockedIcon);
		public void AvailableToCollect();
		public void Disable();
	}
}