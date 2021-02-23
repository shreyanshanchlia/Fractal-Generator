using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DeterministicFractalGenerator : MonoBehaviour
{
    public SettingsManager settingsManager;

    public TextMeshProUGUI iterationCountText;

    public GameObject SquarePrefab;
    public GameObject renderHolder;

    int timesIterated;

    public Queue<GameObject> R1, R2;
    GameObject latestRender;
    Vector3 transformOperation;

    /// <summary> Initialize / Reset render. </summary>
    public void Start()
    {
        R1 = new Queue<GameObject>();
        R2 = new Queue<GameObject>();

        while (renderHolder.transform.childCount > 0)
        {
            Destroy(renderHolder.transform.GetChild(0).gameObject);
        }

        latestRender = Instantiate(SquarePrefab, renderHolder.transform);
        latestRender.name = timesIterated.ToString();
        R1.Enqueue(latestRender);

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
            int len = R1.Count;
			for (int i = 0; i < len; i++)
			{
                draw();
			}
            timesIterated++;
            //update UI.
        }
    }
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
            latestRender.name = timesIterated.ToString();
            R2.Enqueue(latestRender);

            //scale
            transformOperation.x = r.transform.localScale.x * row[0]
                + r.transform.localScale.y * row[1];
            transformOperation.y = r.transform.localScale.x * row[2]
                + r.transform.localScale.y * row[3];
            latestRender.transform.localScale = transformOperation;

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
        R2 = new Queue<GameObject>();
    }
}
