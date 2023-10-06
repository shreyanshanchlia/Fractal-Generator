using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public RawImage gradientImage;
    public Slider colorSlider;
    public Image selectedColorImage;

    private Texture2D gradientTexture;
    private Color selectedColor = Color.white;

    private void Start()
    {
        // Load or generate your gradient texture and assign it to the RawImage.
        // Make sure the gradient texture is the same size as the RawImage.
        // Example:
        gradientTexture = Resources.Load<Texture2D>("GradientTexture");
        gradientImage.texture = gradientTexture;
    }

    private void Update()
    {
        // Calculate the position along the gradient based on the slider value.
        float gradientPosition = colorSlider.value;

        // Sample the color from the gradient texture at the calculated position.
        selectedColor = gradientTexture.GetPixel(Mathf.FloorToInt(gradientPosition * gradientTexture.width), 0);

        // Update the color displayed in the selected color image.
        selectedColorImage.color = selectedColor;
    }
}
