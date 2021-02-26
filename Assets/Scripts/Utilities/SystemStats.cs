using TMPro;
using UnityEngine;

public class SystemStats : MonoBehaviour
{
    public TextMeshProUGUI SystemStatsText;

	private void Start()
	{
		string systemInfo = "Running on ";
		systemInfo += SystemInfo.processorType;
		systemInfo += " and ";
		systemInfo += SystemInfo.graphicsDeviceName;

		SystemStatsText.text = systemInfo;
	}
}
