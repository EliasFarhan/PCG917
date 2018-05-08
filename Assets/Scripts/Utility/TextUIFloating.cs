using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUIFloating : MonoBehaviour
{
	[SerializeField]
	string beforeText = "";
	[SerializeField]
	string afterText = "";
	[SerializeField]
	FloatingValue number;

	Text textUI;
	// Use this for initialization
	void Start ()
	{
		textUI = GetComponent<Text>();
		textUI.text = beforeText + number.value + afterText;
	}
	
	// Update is called once per frame
	void Update ()
	{

		textUI.text = beforeText + number.value + afterText;
	}
}
