using UnityEngine;

public class URL : MonoBehaviour
{
    public string url;

    public void OpenURL()
	{
		Application.OpenURL(url);
	}
}
