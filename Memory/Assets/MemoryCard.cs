using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
	[SerializeField]
	private GameObject cardBack;

	[SerializeField]
	private SceneController controller;

	private int _id;

	public int id => _id;

	/// <summary>
	/// Установка новой карты
	/// </summary>
	/// <param name="id">номер карты</param>
	/// <param name="image">картинка</param>
	public void SetCard(int id, Sprite image)
	{
		_id = id;
		GetComponent<SpriteRenderer>().sprite = image;
	}

	/// <summary>
	/// Вызывается при нажатии на объект
	/// </summary>
	public void OnMouseDown()
	{
		//Если : объект активен и можно открыть карту - деактивирует задник
		if (cardBack.activeSelf && controller.canReveal)
		{
			cardBack.SetActive(false);
			//Уведомление об открытие этой карты
			controller.CardRevealed(this);
		}
	}

	/// <summary>
	/// Сокрытие карты
	/// </summary>
	public void Unreveal()
	{
		cardBack.SetActive(true);
	}
}
