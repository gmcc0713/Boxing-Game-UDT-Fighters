using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public void OpenPanel(GameObject panel)
	{
		if (panel)
		{
			panel.SetActive(true);
		}
	}
	public void ClosePanel(GameObject panel)
	{
		if (panel)
		{
			panel.SetActive(false);
		}
	}
}
