using UnityEngine;
using System.Collections;

//refactor all the chrono I already have in PlayerCharacter
[System.Serializable]
public class Timer
{
	public float period;
	public float time;

	public Timer(float initialTime, float constPeriod)
	{
		period = constPeriod;
		time = initialTime;
	}
	public Timer(TimerData timerData)
	{
		period = timerData.period;
		time = timerData.time;
	}
	public void Update(float deltaTime)
	{
		if (time >= 0)
			time -= deltaTime;

	}
	public bool Over()
	{
		return time <= 0;
	}
	public void SetPeriod(float newPeriod)
	{
		if (newPeriod < time)
		{
			time = -1.0f;
		}
		period = newPeriod;
	}
	public void Reset()
	{
		time = period;
	}
	public float Current()
	{
		float current = (period - time) / period;
		if (current < 0)
			current = 0;
		if (current > 1)
			current = 1;
		return current;
	}
	public float CurrentTime()
	{
		return period - time;
	}
	public float Remain()
	{
		return time;
	}
}
