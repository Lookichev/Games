using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Используется для установки данного объекта в исходное положение
public class Resettable : MonoBehaviour
{
	public UnityEvent onReset;

	/// <summary>
	/// Вызывается диспетчером в момент сброса игры
	/// </summary>
    public void Reset()
	{
		//Порождает событие 
		onReset.Invoke();
	}
}
