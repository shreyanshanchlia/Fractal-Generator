using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
	public Color errorColor;

	[Header("Background Color")]
	public RawImage renderImage;
	public Image selectedBackgroundColor;
	public TMP_InputField backgroundColorHex;
    public Color backgroundColor;

	[Header("Fractal Color")]
	public Image selectedFractalColor;
	public TMP_InputField fractalColorHex;
	public Color fractalColor;

	private void Start()
	{
		ColorPalette _colorPalette = new ColorPalette();
		_colorPalette.color = ColorPalette.ColorPaletteColors._FFFFFF;
		SetBackgroundColorButtons(_colorPalette);

		_colorPalette.color = ColorPalette.ColorPaletteColors._E8AF5A;
		SetFractalColorButtons(_colorPalette);
	}

	public void SetBackgroundColorButtons(ColorPalette colorPalette)
	{
		ColorPalette.ColorPaletteColors paletteColor = colorPalette.color;
		SetBackgroundColor(paletteColor.ToString().Substring(1));
	}
	public void SetBackgroundColor(string _colorHex)
	{
		Color _color;
		if (ColorUtility.TryParseHtmlString($"#{_colorHex}", out _color))
		{
			backgroundColorHex.text = _colorHex;
			backgroundColorHex.GetComponent<Image>().color = Color.white;
			backgroundColor = _color;
			selectedBackgroundColor.color = _color;
			renderImage.color = _color;
		}
		else //wrong input
		{
			backgroundColorHex.GetComponent<Image>().color = errorColor;
		}
	}
	public void SetFractalColorButtons(ColorPalette colorPalette)
	{
		ColorPalette.ColorPaletteColors paletteColor = colorPalette.color;
		SetFractalColor(paletteColor.ToString().Substring(1));
	}
	public void SetFractalColor(string _colorHex)
	{
		Color _color;
		if (ColorUtility.TryParseHtmlString($"#{_colorHex}", out _color))
		{
			fractalColorHex.text = _colorHex;
			fractalColorHex.GetComponent<Image>().color = Color.white;
			fractalColor = _color;
			selectedFractalColor.color = _color;
		}
		else //wrong input
		{
			fractalColorHex.GetComponent<Image>().color = errorColor;
		}
	}
}
