using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
	/// <summary>
	/// Screen ZOOM and PAN using mouse in game
	/// </summary>
	public class Navigator : MonoBehaviour
	{
		public bool zoomOverUI = false;
		[Header("Screen Zoom")]
		[SerializeField][Range(-1, -0.0001f)] float zoomSensitivity = -0.02f;
		[SerializeField] float minZoomLevel = 2, maxZoomLevel = 30;

		[Header("Screen Navigation")]
		[SerializeField][Range(0.1f, 25f)] float navigationSensitivity = 8.7f;
		[SerializeField][Range(0f, 1f)] float zoomToNavigationRelation = 0.1f;

		[Header("Hide Origin")]
		public GameObject originMarker;

		#region camera declaration
#pragma warning disable CS0108
		public Camera camera;
#pragma warning restore CS0108
		#endregion

		bool isFocused;
		float zoomLevel;
		float mouseScrollDelta;
		Vector3 prevMousePosition;
		Vector3 mouseMoveDelta;
		Vector3 moveWorldDelta;
		float screenCalibSensitivity;

		private void Awake()
		{
#if UNITY_STANDALONE
			Screen.fullScreen = false;
#endif
		}
		private void Start()
		{
			zoomLevel = camera.orthographicSize;

			prevMousePosition = Input.mousePosition;
			screenCalibSensitivity = Screen.width;
		}
		private void Update()
		{
			//mouse zoom
			mouseScrollDelta = Input.mouseScrollDelta.y;    //x axis is ignored

			if (mouseScrollDelta != 0f && (zoomOverUI || !EventSystem.current.IsPointerOverGameObject()))
			{
				zoomLevel = zoomLevel + mouseScrollDelta * zoomSensitivity * zoomLevel;
				SetZoomLevel(zoomLevel);
				if (Mathf.Abs(mouseScrollDelta) != 0)
				{
					originMarker.SetActive(false);
				}
			}

			//mouse navigation
			if (Input.GetMouseButton(0) && isFocused && (zoomOverUI || !EventSystem.current.IsPointerOverGameObject()))
			{
				mouseMoveDelta = prevMousePosition - Input.mousePosition;
				if (Mathf.Abs(mouseMoveDelta.x) + Mathf.Abs(mouseMoveDelta.y) != 0)
				{
					originMarker.SetActive(false);
				}
				MoveCamera();
			}
			isFocused = true;
			prevMousePosition = Input.mousePosition;
		}
		private void OnApplicationFocus(bool focus)
		{
			isFocused = false;
		}
		/// <summary>
		/// Sets the zoom level to a value specified
		/// </summary>
		/// <param name="_zoomLevel"> Changes camera orthographic projection size to this value. Value is constrained in between minZoomLevel(included) and MaxZoomLevel(included).</param>
		public void SetZoomLevel(float _zoomLevel)
		{
			zoomLevel = Mathf.Clamp(_zoomLevel, minZoomLevel, maxZoomLevel);
			camera.orthographicSize = zoomLevel;
		}
		private void MoveCamera()
		{
			moveWorldDelta = mouseMoveDelta * navigationSensitivity / screenCalibSensitivity;
			moveWorldDelta = moveWorldDelta * (1 + zoomToNavigationRelation * zoomLevel);
			camera.transform.position += moveWorldDelta;
		}
	}
}