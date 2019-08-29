using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Смена спрайтов в граничных условиях
/// </summary>
public class SpriteSwapper : MonoBehaviour
{
	//Необходимый отобразиться спрайт
	public Sprite spriteToUse;

	//Визуализатор спрайта
	public SpriteRenderer spriteRenderer;

	//Отображаемый спрайт
	private Sprite originalSprite;

	// Смена спрайтов
	public void SwapSprite()
	{
		//Если требуется другой спрайт
		if (spriteToUse != spriteRenderer.sprite)
		{
			//Сохранение прошлого спрайта
			originalSprite = spriteRenderer.sprite;

			//Передача нового спрайта визуализатору
			spriteRenderer.sprite = spriteToUse;
		}
	}

	//Возвращает спрайт
    public void ResetSprite()
	{
		if (originalSprite != null)
		{
			spriteRenderer.sprite = originalSprite;
		}
	}
}
