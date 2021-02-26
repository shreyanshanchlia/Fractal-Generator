using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdateCheck : MonoBehaviour
{
	public string updateURL;

	public Image updateBox;
	public TextMeshProUGUI updateResponse;

	string latestVersion;

	private void Start()
	{
		StartCoroutine(LoadVersion());
	}

	IEnumerator LoadVersion()
	{
		using (UnityWebRequest request = UnityWebRequest.Get(updateURL))
		{
			yield return new WaitForSeconds(2f);
			yield return request.SendWebRequest();

			if (request.isNetworkError) // Error
			{
				updateResponse.text = "Unable to check for updates.";
			}
			else // Success
			{
				string appVersion = Application.version;
				string requestedData = request.downloadHandler.text;

				if (Application.platform == RuntimePlatform.WindowsEditor)
				{
					int pos = requestedData.IndexOf("win: ");
					int versionLength = int.Parse(requestedData.Substring(pos + 5, 1));
					latestVersion = requestedData.Substring(pos + 6, versionLength);
					Debug.Log(latestVersion);
				}
				else if (Application.platform == RuntimePlatform.WindowsPlayer)
				{
					int pos = requestedData.IndexOf("win: ");
					int versionLength = int.Parse(requestedData.Substring(pos + 5, 1));
					latestVersion = requestedData.Substring(pos + 6, versionLength);
					Debug.Log(latestVersion);
				}
				else if (Application.platform == RuntimePlatform.OSXEditor)
				{
					int pos = requestedData.IndexOf("mac: ");
					int versionLength = int.Parse(requestedData.Substring(pos + 5, 1));
					latestVersion = requestedData.Substring(pos + 6, versionLength);
					Debug.Log(latestVersion);
				}
				else if (Application.platform == RuntimePlatform.LinuxPlayer)
				{
					int pos = requestedData.IndexOf("linux: ");
					int versionLength = int.Parse(requestedData.Substring(pos + 5, 1));
					latestVersion = requestedData.Substring(pos + 6, versionLength);
					Debug.Log(latestVersion);
				}
				else if (Application.platform == RuntimePlatform.WebGLPlayer)
				{
					int pos = requestedData.IndexOf("web: ");
					int versionLength = int.Parse(requestedData.Substring(pos + 5, 1));
					latestVersion = requestedData.Substring(pos + 6, versionLength);
					Debug.Log(latestVersion);
				}
				else if (Application.platform == RuntimePlatform.Android)
				{
					int pos = requestedData.IndexOf("android: ");
					int versionLength = int.Parse(requestedData.Substring(pos + 5, 1));
					latestVersion = requestedData.Substring(pos + 6, versionLength);
					Debug.Log(latestVersion);
				}
				if(appVersion == latestVersion)
				{
					updateResponse.text = "You are using the latest version!";
					updateBox.color = new Color(0, 0, 1, 30);
				}
				else
				{
					updateResponse.text = "Update Available!";
				}
			}
		}
	}
}
