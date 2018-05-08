using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRFoot : MonoBehaviour
{
	private int _contact = 0;

	public int Contact
	{
		get { return _contact; }
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			_contact++;
		}
	}
	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			_contact--;
		}
	}
}
