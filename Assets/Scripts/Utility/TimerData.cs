using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimerData", menuName = "GD/TimerData", order = 1)]
public class TimerData : ScriptableObject
{
	public float period;
	public float time;
}
