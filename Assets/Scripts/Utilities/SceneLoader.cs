using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
	public void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}
}
