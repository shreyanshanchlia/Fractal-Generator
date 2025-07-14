using System;
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
	public GameObject originMarker;

	[Header("Running Buttons")]
	public GameObject playButton;
	public GameObject pauseButton;

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
	public Vector2 fractalOffset;

	public uint fractalSize;
	public TMP_InputField sizeInputField;

	public uint iterationSpeed;
	public TMP_InputField speedInputField;

	public int totalIterations;
	public TMP_InputField totalIterationsInputField;

	[Header("IFS")]
	public int ifsRowLength;
	public List<float[]> ifsCode;
	public GameObject ifsInputTable;
	public GameObject ifsRowPrefab;

	float CameraSize;

	public Action OnStartRunning;

	private void Awake()
	{
		instance = this;
		CameraSize = Camera.main.orthographicSize;
		
		ColorPalette _colorPalette = new ColorPalette();
		_colorPalette.color = ColorPalette.ColorPaletteColors._FFFFFF;
		SetBackgroundColorButtons(_colorPalette);

		_colorPalette.color = ColorPalette.ColorPaletteColors._4D4D4D;
		SetFractalColorButtons(_colorPalette);
	}

	private void Start()
	{
		_camera = Camera.main;
		ifsCode = new List<float[]>();
		ifsCode.Add(new float[ifsRowLength]);

		fractalOffset = new Vector2(Screen.width / 2, Screen.height / 2);
	}
	public void ResetCamera()
	{
		_camera.transform.position = new Vector3(0, 0, -10);
		_camera.orthographicSize = CameraSize;
	}
	public void AdjustMarker()
	{
		Vector3 markerPosition = _camera.ScreenToWorldPoint(fractalOffset);
		markerPosition -= _camera.transform.position;
		originMarker.SetActive(true);
		originMarker.transform.position = markerPosition;
	}
	public void StartRunning()
	{
		isRunning = true;
		OnStartRunning?.Invoke();
		playButton.SetActive(false);
		pauseButton.SetActive(true);
	}
	public void StopRunning()
	{
		isRunning = false;
		playButton.SetActive(true);
		pauseButton.SetActive(false);
	}
	public void SetBackgroundColorButtons(ColorPalette colorPalette)
	{
		ColorPalette.ColorPaletteColors paletteColor = colorPalette.color;
		SetBackgroundColor(paletteColor.ToString().Substring(1));
	}
	public void SetBackgroundColor(string colorHex)
	{
		if (ColorUtility.TryParseHtmlString($"#{colorHex}", out var color))
		{
			backgroundColorHex.text = colorHex;
			backgroundColorHex.GetComponent<Image>().color = Color.white;
			backgroundColor = color;
			selectedBackgroundColor.color = color;
			if(canvasRenderImage) canvasRenderImage.color = color;
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
	public void SetFractalColor(string colorHex)
	{
		if (ColorUtility.TryParseHtmlString($"#{colorHex}", out var color))
		{
			fractalColor = color;
			selectedFractalColor.color = color;
			fractalColorHex.text = colorHex;
			fractalColorHex.GetComponent<Image>().color = Color.white;
		}
		else //wrong input
		{
			fractalColorHex.GetComponent<Image>().color = errorColor;
		}
	}
	public void SetFractalSize(string size)
	{
		if (uint.TryParse(size, out fractalSize))
		{
			fractalSize = uint.Parse(size);
			sizeInputField.GetComponent<Image>().color = Color.white;
		}
		else
		{
			sizeInputField.GetComponent<Image>().color = errorColor;
		}
	}
	public void SetIterationSpeed(string speed)
	{
		if(uint.TryParse(speed, out iterationSpeed))
		{
			iterationSpeed = uint.Parse(speed);
			speedInputField.GetComponent<Image>().color = Color.white;
		}
		else
		{
			speedInputField.GetComponent<Image>().color = errorColor;
		}
	}
	public void SetIterationCount(string count)
	{
		if (int.TryParse(count, out totalIterations))
		{
			totalIterations = int.Parse(count);
			totalIterationsInputField.GetComponent<Image>().color = Color.white;
		}
		else
		{
			totalIterationsInputField.GetComponent<Image>().color = errorColor;
		}
	}

	private GameObject _lastIfsRow;
	private Camera _camera;

	public void AddIFSRow()
	{
		GameObject newIfsRow = Instantiate(ifsRowPrefab, ifsInputTable.transform);
		ifsCode.Add(new float[ifsRowLength]);
		newIfsRow.name = $"F ({ifsCode.Count})";
		foreach (IFSElementID ifsElement in newIfsRow.transform.GetComponentsInChildren<IFSElementID>(true))
		{
			ifsElement.rowID = ifsCode.Count;
		}
		newIfsRow.transform.Find($"rowName").GetComponent<TMP_Text>().text = $"f{ifsCode.Count}";
		_lastIfsRow = newIfsRow;
	}
	public void IFSEdit(IFSElementID elementID)
	{
		ifsCode[elementID.rowID - 1][(int)elementID.columnType] = elementID.value;
	}

	[ContextMenu("Print IFS Matrix")]
	public void PrintIfsmAtrix()
	{
		string ifsmatrixString = "";
		foreach (float[] ifsRow in ifsCode)
		{
			if (ifsRow.Length == 7)
			{
				ifsmatrixString += $"{ifsRow[0]} {ifsRow[1]} {ifsRow[2]} {ifsRow[3]} {ifsRow[4]} {ifsRow[5]} {ifsRow[6]}\n";
			}
			else if (ifsRow.Length == 6)
			{
				ifsmatrixString += $"{ifsRow[0]} {ifsRow[1]} {ifsRow[2]} {ifsRow[3]} {ifsRow[4]} {ifsRow[5]}\n";
			}
		}
		print(ifsmatrixString);
	}
	public void CopyIFSMatrixToClipboard()
	{
		string ifsmatrixString = "";
		char newline = '\n';
		foreach (float[] ifsRow in ifsCode)
		{
			if (ifsRow.Length == 7)
			{
				ifsmatrixString += $"{ifsRow[0]} {ifsRow[1]} {ifsRow[2]} {ifsRow[3]} {ifsRow[4]} {ifsRow[5]} {ifsRow[6]}{newline}";
			}
			else if (ifsRow.Length == 6)
			{
				ifsmatrixString += $"{ifsRow[0]} {ifsRow[1]} {ifsRow[2]} {ifsRow[3]} {ifsRow[4]} {ifsRow[5]}{newline}";
			}
		}
		GUIUtility.systemCopyBuffer = ifsmatrixString;
	}
	public void PasteIFSMatrixFromClipboard()
	{
		string ifsmatrixString = GUIUtility.systemCopyBuffer;
		bool validClipboard = true;

		List<float[]> tempifsCode = new List<float[]>();
		try
		{
			string[] ifsmatrixStrings = ifsmatrixString.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
			int currentRowNumber = 0;
			foreach (string ifsRowClipBoard in ifsmatrixStrings)
			{
				float[] rowValues = System.Array.ConvertAll(ifsRowClipBoard.Split(new char[] { ' ', '\t', ',' }, System.StringSplitOptions.RemoveEmptyEntries), float.Parse);
				tempifsCode.Add(new float[ifsRowLength]);
				
				if (rowValues.Length == ifsRowLength || rowValues.Length == 0)
				{
					for (int i = 0; i < ifsRowLength; i++)
					{
						tempifsCode[currentRowNumber][i] = rowValues[i];
					}
				}
				else
				{
					validClipboard = false;
					Debug.LogError($"Invalid Clipboard length \n {GUIUtility.systemCopyBuffer}");
				}
				currentRowNumber++;
			}
		}
		catch (System.Exception e)
		{
			validClipboard = false;
			Debug.LogError($"Invalid Clipboard\n {GUIUtility.systemCopyBuffer}\n\n {e.Message}");
		}

		if (validClipboard)
		{
			ResetIFSTAble();
			int rowNumber = 0;
			foreach (float[] tempifsRow in tempifsCode)
			{
				AddIFSRow();
				GameObject newIfsRow = _lastIfsRow;
				for (int i = 0; i < tempifsRow.Length; i++)
				{
					ifsCode[rowNumber][i] = tempifsRow[i];
					newIfsRow.transform.GetChild(i + 1).GetComponent<TMP_InputField>().text = tempifsRow[i].ToString(CultureInfo.InvariantCulture);
				}
				rowNumber++;
			}
			ifsCode = tempifsCode;
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
