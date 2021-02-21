using UnityEngine;
using UnityEngine.UI;

public class RandomIterationFractalGenerator : MonoBehaviour
{
    public RectTransform canvas;
    
    public SettingsManager settingsManager;

    RawImage rawImage;

    float x = 0;
    float y = 0;
    Texture2D texture;

    void Start()
    {
        rawImage = canvas.GetComponent<RawImage>();
        canvas.sizeDelta = new Vector2((int)(Screen.width * 0.95f), (int)(Screen.height * 0.95f));
        texture = new Texture2D(width: (int)(Screen.width * 0.95f), height: (int)(Screen.height * 0.95f));
        
        rawImage.texture = texture;
    }

    void Update()
    {
        if (settingsManager.isRunning)
        {
            for (int i = 0; i < settingsManager.iterationSpeed; i++)
            {
                drawPixel();
            }
            texture.Apply();
        }
    }
    void drawPixel()
	{
        texture.SetPixel((int)(x * settingsManager.fractalSize + Screen.width / 2), 
            (int)(y * settingsManager.fractalSize - 250 + Screen.height / 2), settingsManager.fractalColor);

        float nextX = x, nextY = y;

        float r = Random.Range(0f, 1f);

		foreach (float[] ifsRow in settingsManager.ifsCode)
		{
            if(ifsRow[6] <= r)
			{
                nextX = ifsRow[0] * x + ifsRow[1] * y + ifsRow[4];
                nextY = ifsRow[2] * x + ifsRow[3] * y + ifsRow[5];
            }
            r -= ifsRow[6];
		}

        x = nextX;
        y = nextY;
    }
}
