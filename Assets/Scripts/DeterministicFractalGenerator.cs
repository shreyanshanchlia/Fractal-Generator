using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DeterministicFractalGenerator : MonoBehaviour
{
    public SettingsManager settingsManager;

    public TextMeshProUGUI iterationCountText;

    public GameObject SquarePrefab;
    public GameObject renderHolder;

    int len;
    int timesIterated;

    public Queue<GameObject> R1;
    GameObject latestRender;
    Vector3 transformOperation;

    /// <summary> Initialize / Reset render. </summary>
    public void Start()
    {
        R1 = new Queue<GameObject>();
        ChangeBackgroundColor();

        GameObject _renderHolder = new GameObject("Render Holder");
        Destroy(renderHolder);
        renderHolder = _renderHolder;

        latestRender = Instantiate(SquarePrefab, renderHolder.transform);
        latestRender.GetComponent<SpriteRenderer>().color = settingsManager.fractalColor;
        latestRender.name = timesIterated.ToString();
        R1.Enqueue(latestRender);

        len = 1;

        timesIterated = 0;
        iterationCountText.text = $"Iterated {timesIterated} times!";

        settingsManager.StopRunning();
    }

    //Update is called once per frame.
    void Update()
    {
        //if allowed to render.
        if (settingsManager.isRunning && (timesIterated < settingsManager.totalIterations || settingsManager.totalIterations < 0))
        {
            int i = 0;
            while (i < len && i < settingsManager.iterationSpeed)
			{
                draw();
                i++;
			}
            len -= i;

            if (len <= 0)
            {
                len = R1.Count;
                timesIterated++;
                
                //update UI.
                iterationCountText.text = $"Iterated {timesIterated} times!";
            }
        }
    }
    float distance, delta;
    /// <summary>
    /// Deterministic Algorithm to render all iterations.
    /// </summary>
    void draw()
    {
        //draw image
        GameObject r = R1.Dequeue();

        foreach (float[] row in settingsManager.ifsCode)
        {
            latestRender = Instantiate(SquarePrefab, renderHolder.transform);
            latestRender.GetComponent<SpriteRenderer>().color = settingsManager.fractalColor;
            latestRender.name = timesIterated.ToString();

            if (row[0] != 0 || row[1] != 0)
            {
                distance = Mathf.Sqrt(row[0] * row[0] + row[1] * row[1]);
                delta = row[0] * row[3] - row[1] * row[2];

                //scale new
                transformOperation.x = r.transform.localScale.x * distance;
                transformOperation.y = r.transform.localScale.y * delta / distance;
                transformOperation.z = 1;
                latestRender.transform.localScale = transformOperation;

                //rotate
                transformOperation.x = transformOperation.y = 0;
                transformOperation.z = r.transform.localRotation.eulerAngles.z + row[1] / Mathf.Abs(row[1]) * Mathf.Acos(row[0] / distance) * Mathf.Rad2Deg;
                latestRender.transform.eulerAngles = transformOperation;
            }
			else
			{
                distance = Mathf.Sqrt(row[2] * row[2] + row[3] * row[3]);
                delta = row[0] * row[3] - row[1] * row[2];

                //scale new
                transformOperation.x = r.transform.localScale.x * delta / distance;
                transformOperation.y = r.transform.localScale.y * distance;
                transformOperation.z = 1;
                latestRender.transform.localScale = transformOperation;

                //rotate
                transformOperation.x = transformOperation.y = 0;
                transformOperation.z = r.transform.localRotation.eulerAngles.z + 90 - row[3] / Mathf.Abs(row[3]) * Mathf.Acos(row[2] / distance) * Mathf.Rad2Deg;
                latestRender.transform.eulerAngles = transformOperation;
            }
            //translate
            transformOperation.x = r.transform.position.x * row[0]
                + r.transform.position.y * row[1] + row[4];
            transformOperation.y = r.transform.position.x * row[2]
                + r.transform.position.y * row[3] + row[5];
            transformOperation.z = 0;
            latestRender.transform.position = transformOperation;
            R1.Enqueue(latestRender);
        }
        Destroy(r);
    }
    public void ChangeBackgroundColor()
	{
        StartCoroutine(ChangeBackgroundAfterFrame());
	}
    IEnumerator ChangeBackgroundAfterFrame()
    {
        yield return null;
        Camera.main.backgroundColor = settingsManager.backgroundColor;
    }
}
