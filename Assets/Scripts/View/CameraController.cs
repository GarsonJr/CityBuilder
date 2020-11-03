using CityBuilder.Config;
using UnityEngine;

namespace CityBuilder.View
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Transform _transform;
		[SerializeField] private Camera _camera;
		[SerializeField] private float _dragSpeed;
		[SerializeField] private float _distance;
		[SerializeField] private Vector3 _angle;

		public Camera Camera => _camera;

		public void Init(InputHandler input, GameConfig config)
		{
			_input = input;
			_maxX = config.CellSize * config.CityWidth / 2;
			_maxZ = config.CellSize * config.CityLength / 2;
		}

		private void LateUpdate()
		{
			_transform.rotation = Quaternion.LookRotation(_angle);

			var right = _transform.right;
			_planePos += (right * _input.Drag.x - Vector3.Cross(right, Vector3.down) * _input.Drag.y) * _dragSpeed;
			_planePos = new Vector3(Mathf.Clamp(_planePos.x, -_maxX, _maxX),
				0, Mathf.Clamp(_planePos.z, -_maxZ, _maxZ));

			_transform.position = _planePos - _distance * _angle;
		}

		private InputHandler _input;
		private Vector3 _planePos;
		private float _maxX;
		private float _maxZ;
	}
}