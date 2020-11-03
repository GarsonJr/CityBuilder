using UnityEngine;
using UnityEngine.UI;

namespace CityBuilder.View
{
	public class PowerView : MonoBehaviour
	{
		[SerializeField] private GameObject _rootObject;
		[SerializeField] private RectTransform _rootTransform;
		[SerializeField] private Text _text;

		public void SetPosition(Vector3 position) => _rootTransform.anchoredPosition = position;

		public void Show(int power)
		{
			_text.text = power.ToString();
			_rootObject.SetActive(true);
		}

		public void Hide() => _rootObject.SetActive(false);
	}
}