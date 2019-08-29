using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
	/// <summary>
	/// Адресат сообщения
	/// </summary>
	[SerializeField]
	private GameObject targetObject;

	/// <summary>
	/// Отправляемое сообщение
	/// </summary>
	[SerializeField]
	private string targetMessage;

	/// <summary>
	/// Цвет фона кнопки
	/// </summary>
	private Color highlightColor = Color.cyan;

	/// <summary>
	/// Изменение цвета кнопки при наведении мыши
	/// </summary>
	public void OnMouseOver()
	{
		var sprite = GetComponent<SpriteRenderer>();

		if(sprite != null) sprite.color = highlightColor;
	}

	/// <summary>
	/// Изменение цвета кнопки при отведении мыши
	/// </summary>
	public void OnMouseExit()
	{
		var sprite = GetComponent<SpriteRenderer>();

		if (sprite != null)	sprite.color = Color.white;
	}

	/// <summary>
	/// Увеличение размера кнопки при нажатии
	/// </summary>
	public void OnMouseDown()
	{
		transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
	}

	/// <summary>
	/// Возвращение к нормальному размеру при отпускании кнопки мыши
	/// </summary>
	public void OnMouseUp()
	{
		transform.localScale = Vector3.one;

		if (targetObject != null)
			targetObject.SendMessage(targetMessage);
	}
}
