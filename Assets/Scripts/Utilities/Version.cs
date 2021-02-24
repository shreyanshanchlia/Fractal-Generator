using UnityEngine;
using TMPro;

public class Version : MonoBehaviour
{
    public TextMeshProUGUI productText;

	private void Awake()
	{
		string product = $"{Application.productName.ToUpper()} v{Application.version} ";
		if(Application.platform == RuntimePlatform.WindowsEditor)
		{
			product += "editor";
		}
		else if (Application.platform == RuntimePlatform.WindowsPlayer)
		{
			product += "win";
		}
		else if (Application.platform == RuntimePlatform.OSXEditor)
		{
			product += "mac";
		}
		else if (Application.platform == RuntimePlatform.LinuxPlayer)
		{
			product += "linux";
		}
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			product += "web";
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			product += "android";
		}
		productText.text = product;
	}
}
