using UnityEngine;
using System.Collections;

public class CanvasSwitch : MonoBehaviour
{
    public CanvasGroup canvasToHide;
	[Range(1f, 30f)] public float cooldownTime;
	[Range(0f, 5f)] public float fadeDuration;

	Vector3 prevMousePos;

	private void Start()
	{
		prevMousePos = Input.mousePosition;
	}

	private void Update()
	{
		if(Input.anyKey || (Input.mousePosition - prevMousePos != Vector3.zero))
		{
			canvasToHide.alpha = 1;
			StopAllCoroutines();
			StartCoroutine(CoolDown());
		}
		prevMousePos = Input.mousePosition;
	}
	IEnumerator CoolDown()
	{
		yield return new WaitForSeconds(cooldownTime);
		canvasToHide.alpha = 0.5f;
		yield return new WaitForSeconds(fadeDuration);
		canvasToHide.alpha = 0f;
	}
}
