using UnityEngine;
using UnityEngine.UI;

public class IFSElementID : MonoBehaviour
{
	public Color errorColor;

	[Header("Element ID")]
	public int rowID;
	public ColumnType columnType;

	public float value;

    public enum ColumnType	{ a, b, c, d, e, f, p };

	public void SetValue(string _value)
	{
		if (float.TryParse(_value, out value))
		{
			value = float.Parse(_value);
			this.gameObject.GetComponent<Image>().color = Color.white;
		}
		else
		{
			this.gameObject.GetComponent<Image>().color = errorColor;
		}
		SetValueInTable();
	}
	void SetValueInTable()
	{
		SettingsManager.instance.IFSEdit(this);
	}
}
