using System;
using UnityEngine;
using UnityEngine.UI;

namespace CityBuilder.View
{
	public class LoadingBar : MonoBehaviour
	{
		[SerializeField] private Image _barForeground;

		public event Action LoadingCompleted;

		public void Subscribe(IProgress progress)
		{
			_progress = progress;
			enabled = true;
		}

		public void SetProgress(float progress) => _barForeground.fillAmount = progress;

		private void Update()
		{
			var progress = _progress.GetProgress();
			SetProgress(progress);
			if (progress >= 1)
				LoadingCompleted?.Invoke();
		}

		private IProgress _progress;
	}
}