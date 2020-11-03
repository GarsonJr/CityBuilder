using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CityBuilder.View
{
	public class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] private float _maxClickDelta;
		[SerializeField] private float _maxClickTime;
		[SerializeField] private float _minDragDelta;

		public event Action EscapePressed;
		public event Action<Vector2> Click;
		public Vector2 Drag { get; private set; }

		public void OnPointerDown(PointerEventData eventData)
		{
			_isPointerDown = true;
			_pointerDownTime = Time.time;
			_pointerDownPosition = eventData.position;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (!_isDragging && (_pointerDownPosition - eventData.position).sqrMagnitude < _maxClickDelta * _maxClickDelta
			                 && Time.time - _pointerDownTime < _maxClickTime)
			{
				Click?.Invoke(_pointerDownPosition);
			}

			_isPointerDown = false;
			_isDragging = false;
		}

		private void Update()
		{
			if (Input.GetButton("Cancel"))
				EscapePressed?.Invoke();

			if (!_isPointerDown)
			{
				Drag = Vector2.zero;
				return;
			}

			var mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			_isDragging = _isDragging || (mousePos - _pointerDownPosition).sqrMagnitude >= _minDragDelta;
			if (_isDragging)
			{
				Drag = _pointerDownPosition - mousePos;
				_pointerDownPosition = mousePos;
			}
		}

		private bool _isPointerDown;
		private bool _isDragging;
		private float _pointerDownTime;
		private Vector2 _pointerDownPosition;
	}
}