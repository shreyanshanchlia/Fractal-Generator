using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script uses values in Settings Manager to generate a fractal.
/// </summary>
public class RandomIterationFractalGenerator : MonoBehaviour
{
    /// <summary> Rendering Plane. </summary>
    RawImage rawImage;
    public RectTransform rawImageCanvas;
    
    public SettingsManager settingsManager;


    int timesIterated;
    float x = 0;
    float y = 0;
    Texture2D texture;

    /// <summary> Initialize / Reset plane </summary>
    public void Start()
    {
        rawImage = rawImageCanvas.GetComponent<RawImage>();
        rawImageCanvas.sizeDelta = new Vector2(Screen.width, Screen.height);
        texture = new Texture2D(width: Screen.width, height: Screen.height);
        
        rawImage.texture = texture;

        timesIterated = 0;
        settingsManager.StopRunning();
    }

    //Update is called once per frame.
    void Update()
    {
        //if allowed to render.
        if (settingsManager.isRunning && (timesIterated < settingsManager.totalIterations || settingsManager.totalIterations < 0))
        {
            //loop over multiple times, using iteration speed.
            for (int i = 0; i < settingsManager.iterationSpeed; i++)
            {
                drawPixel();
                timesIterated++;
            }
            //apply changes to the texture.
            texture.Apply();
        }
    }
    /// <summary>
    /// Random Iteration Algorithm to update x, y for next pixel to render.
    /// </summary>
    void drawPixel()
	{
        //draws pixel.
        texture.SetPixel(
            (int)(x * settingsManager.fractalSize + settingsManager.fractalOffset.x), 
            (int)(y * settingsManager.fractalSize + settingsManager.fractalOffset.y),
            settingsManager.fractalColor);

        //update x, y.
        float nextX = x, nextY = y;

        float r = Random.Range(0f, 1f);

		foreach (float[] ifsRow in settingsManager.ifsCode)
		{
            r -= ifsRow[6];
            if(r <= 0)
			{
                nextX = ifsRow[0] * x + ifsRow[1] * y + ifsRow[4];
                nextY = ifsRow[2] * x + ifsRow[3] * y + ifsRow[5];
                break;
            }
		}

        x = nextX;
        y = nextY;
    }
}
