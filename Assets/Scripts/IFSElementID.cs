using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// To be put on IFS input elements for referencing.
/// </summary>
public class IFSElementID : MonoBehaviour
{
	/// <summary> color when invalid input is put.[OBSOLETE] </summary>
	public Color errorColor;

	// Element row and column index.
	[Header("Element ID")]
	public int rowID;
	public ColumnType columnType;

	/// <summary> Value of the element </summary>
	public float value;

    public enum ColumnType	{ a, b, c, d, e, f, p };

	/// <summary>
	/// Set value when new value is entered by user.
	/// </summary>
	/// <param name="_value">new value from input field</param>
	public void SetValue(string _value)
	{
		if (float.TryParse(_value, out value))
		{
			value = float.Parse(_value);
			this.gameObject.GetComponent<Image>().color = Color.white;
		}
		else //invalid string
		{
			this.gameObject.GetComponent<Image>().color = errorColor;
		}
		SetValueInTable();
	}
	/// <summary>
	/// Supply the updated value to Settings Manager.
	/// </summary>
	void SetValueInTable()
	{
		SettingsManager.instance.IFSEdit(this);
	}
}
