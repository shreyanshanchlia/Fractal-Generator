using UnityEngine;

/// <summary>
/// enabled when change origin button is pressed.
/// </summary>
public class ChangeOrigin : MonoBehaviour
{
    public SettingsManager settingsManager;

	private void OnEnable()
	{
		settingsManager.originMarker.SetActive(true);
		settingsManager.ResetCamera();
	}
	private void Update()
	{
		if(Input.GetMouseButton(0))
		{
			settingsManager.fractalOffset = Input.mousePosition;
			settingsManager.AdjustMarker();
			this.enabled = false;
		}
	}
}
