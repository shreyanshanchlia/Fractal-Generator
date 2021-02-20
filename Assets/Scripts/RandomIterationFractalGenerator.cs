using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomIterationFractalGenerator : MonoBehaviour
{
    public RawImage rawImage;
    public Color color;

    float x = 0;
    float y = 0;
    Texture2D texture;

    void Start()
    {
        texture = new Texture2D(width: (int)(Screen.width * 0.96f), height: (int)(Screen.height * 0.96f));
        rawImage.texture = texture;
    }

    void Update()
    {
        drawPixel();
        texture.Apply();
    }
    void drawPixel()
	{
        texture.SetPixel((int)(x * 50 + Screen.width / 2), (int)(y * 50 + Screen.height / 3), color);

        float nextX, nextY;

        float r = Random.Range(0f, 1f);

        if (r < 0.01)
        {
            nextX = 0;
            nextY = 0.16f * y;
        }
        else if (r < 0.86)
        {
            nextX = 0.85f * x + 0.04f * y;
            nextY = -0.04f * x + 0.85f * y + 1.6f;
        }
        else if (r < 0.93)
        {
            nextX = 0.20f * x + -0.26f * y;
            nextY = 0.23f * x + 0.22f * y + 1.6f;
        }
        else
        {
            nextX = -0.15f * x + 0.28f * y;
            nextY = 0.26f * x + 0.24f * y + 0.44f;
        }

        x = nextX;
        y = nextY;
    }
}
