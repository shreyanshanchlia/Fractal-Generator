using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
	public static SettingsManager instance;

	[Header("App State")]
	public bool isRunning;

	[Header("UI Settings")]
	public Color errorColor;

	[Header("Background Color")]
	public RawImage canvasRenderImage;
	public Image selectedBackgroundColor;
	public TMP_InputField backgroundColorHex;
    public Color backgroundColor;

	[Header("Fractal Color")]
	public Image selectedFractalColor;
	public TMP_InputField fractalColorHex;
	public Color fractalColor;

	[Header("Fractal Properties")]
	public uint fractalSize;
	public TMP_InputField sizeInputField;

	public uint iterationSpeed;
	public TMP_InputField speedInputField;

	[Header("IFS")]
	public List<float[]> ifsCode;
	public GameObject ifsInputTable;
	public GameObject ifsRowPrefab;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		ifsCode = new List<float[]>();
		ifsCode.Add(new float[7]);

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
			canvasRenderImage.color = _color;
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
	public void SetFractalSize(string _fractalSize)
	{
		if (uint.TryParse(_fractalSize, out fractalSize))
		{
			fractalSize = uint.Parse(_fractalSize);
			sizeInputField.GetComponent<Image>().color = Color.white;
		}
		else
		{
			sizeInputField.GetComponent<Image>().color = errorColor;
		}
	}
	public void SetIterationSpeed(string _iterationSpeed)
	{
		if(uint.TryParse(_iterationSpeed, out iterationSpeed))
		{
			iterationSpeed = uint.Parse(_iterationSpeed);
			speedInputField.GetComponent<Image>().color = Color.white;
		}
		else
		{
			speedInputField.GetComponent<Image>().color = errorColor;
		}
	}
	GameObject lastIFSRow;
	public void AddIFSRow()
	{
		GameObject newIFSRow = Instantiate(ifsRowPrefab, ifsInputTable.transform);
		ifsCode.Add(new float[7]);
		newIFSRow.name = $"F ({ifsCode.Count})";
		foreach (IFSElementID ifsElement in newIFSRow.transform.GetComponentsInChildren<IFSElementID>(true))
		{
			ifsElement.rowID = ifsCode.Count;
		}
		newIFSRow.transform.Find($"rowName").GetComponent<TMP_Text>().text = $"f{ifsCode.Count}";
		lastIFSRow = newIFSRow;
	}
	public void IFSEdit(IFSElementID elementID)
	{
		ifsCode[elementID.rowID - 1][(int)elementID.columnType] = elementID.value;
	}
	[ContextMenu("Print IFS Matrix")]
	public void printIFSMAtrix()
	{
		string _ifsmatrixString = "";
		foreach (float[] ifsRow in ifsCode)
		{
			_ifsmatrixString = $"{ifsRow[0]} {ifsRow[1]} {ifsRow[2]} {ifsRow[3]} {ifsRow[4]} {ifsRow[5]} {ifsRow[6]}\n";
		}
		print(_ifsmatrixString);
	}
	public void CopyIFSMatrixToClipboard()
	{
		string _ifsmatrixString = "";
		foreach (float[] ifsRow in ifsCode)
		{
			_ifsmatrixString = $"{ifsRow[0]} {ifsRow[1]} {ifsRow[2]} {ifsRow[3]} {ifsRow[4]} {ifsRow[5]} {ifsRow[6]}\n";
		}
		GUIUtility.systemCopyBuffer = _ifsmatrixString;
	}
	public void PasteIFSMatrixFromClipboard()
	{
		string _ifsmatrixString = GUIUtility.systemCopyBuffer;
		bool validClipboard = true;

		List<float[]> tempifsCode = new List<float[]>();
		try
		{
			string[] _ifsmatrixStrings = _ifsmatrixString.Split('\n');
			int _currentRowNumber = 0;
			foreach (string ifsRowClipBoard in _ifsmatrixStrings)
			{
				print(ifsRowClipBoard);

				float[] rowValues = System.Array.ConvertAll(ifsRowClipBoard.Split(new char[] { ' ', '\t', ',' }, System.StringSplitOptions.RemoveEmptyEntries), float.Parse);
				tempifsCode.Add(new float[7]);
				
				if (rowValues.Length == 7 || rowValues.Length == 0)
				{
					for (int i = 0; i < 7; i++)
					{
						tempifsCode[_currentRowNumber][i] = rowValues[i];
					}
				}
				else
				{
					validClipboard = false;
					Debug.LogError($"Invalid Clipboard length \n {GUIUtility.systemCopyBuffer}");
				}
				_currentRowNumber++;
			}
		}
		catch
		{
			validClipboard = false;
			Debug.LogError($"Invalid Clipboard\n {GUIUtility.systemCopyBuffer}");
		}

		if (validClipboard)
		{
			ResetIFSTAble();
			int _rowNumber = 0;
			foreach (float[] _tempifsRow in tempifsCode)
			{
				AddIFSRow();
				GameObject newIFSRow = lastIFSRow;
				for (int i = 0; i < _tempifsRow.Length; i++)
				{
					ifsCode[_rowNumber][i] = _tempifsRow[i];
					newIFSRow.transform.GetChild(i + 1).GetComponent<TMP_InputField>().text = _tempifsRow[i].ToString();
				}
				_rowNumber++;
			}
		}
	}
	public void ResetIFSTAble()
	{
		ifsCode = new List<float[]>();
		int x = 500;
		while (ifsInputTable.transform.childCount > 0 && x-- > 0)
		{
			Destroy(ifsInputTable.transform.GetChild(0).gameObject);
		}
	}
}
