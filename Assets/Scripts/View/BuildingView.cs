using System;
using CityBuilder.Config;
using UnityEngine;

namespace CityBuilder.View
{
	public class BuildingView : MonoBehaviour
	{
		[SerializeField] private Transform _root;
		[SerializeField] private Transform _meshTransform;
		[SerializeField] private MeshRenderer _mesh;
		[SerializeField] private Color _baseColor;
		[SerializeField] private Color _invalidPositionColor;
		[SerializeField, Range(0, 1)] private float _buildingModeAlpha;
		[SerializeField] private Material _baseMaterial;
		[SerializeField] private Material _transparentMaterial;

		public BuildingConfig Config => _config;
		public Vector3 Position { get; private set; }

		public void Init(BuildingConfig config, float cellSize)
		{
			_config = config;
			_meshTransform.localPosition = Vector3.up * config.Height / 2;
			_meshTransform.localScale = new Vector3(cellSize * config.Width, config.Height, cellSize * config.Length);
			_materialPropertyBlock = new MaterialPropertyBlock();
		}

		public void SetColorMode(ColorMode mode)
		{
			var transparent = mode != ColorMode.Base;
			if (_isTransparent != transparent)
			{
				var material = transparent ? _transparentMaterial : _baseMaterial;
				_mesh.materials = new[] {material};
				_isTransparent = transparent;
			}
			
			if (_isTransparent)
			{
				var color = mode == ColorMode.TransparentInvalid ? _invalidPositionColor : _baseColor;
				_mesh.GetPropertyBlock(_materialPropertyBlock);
				_materialPropertyBlock.SetColor("_Color", new Color(color.r, color.g, color.b, _buildingModeAlpha));
				_mesh.SetPropertyBlock(_materialPropertyBlock);
			}
		}

		public void SetPosition(Vector3 position)
		{
			_root.position = position;
			Position = position;
		}

		public enum ColorMode
		{
			Base,
			TransparentValid,
			TransparentInvalid
		}

		private BuildingConfig _config;
		private MaterialPropertyBlock _materialPropertyBlock;
		private bool _isTransparent;
	}
}