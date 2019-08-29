
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPart : MonoBehaviour
{

	// Для ApplyDamageSprite типа порез
	public Sprite detachedSprite;

	// Для ApplyDamageSprite типа ожог
	public Sprite burnedSprite;

	// Позиция, для отображения фонтана крови
	public Transform bloodFountainOrigin;

	// При true удаляется все содержимое объекта
	bool detached = false;

	/// <summary>
	/// Отделяет объект и устанавливает флаг на удаление физ свойств
	/// </summary>
	public void Detach()
	{
		detached = true;

		tag = "Untagged";

		transform.SetParent(null, true);
	}
	
    /// <summary>
	/// Реализация исчезновения объекта
	/// </summary>
    void Update()
    {
		//Если : часть тела не отделена ничего не предпринимать
		if (detached == false) return;

		//Твердое тело прекратило падение?
		var rigidbody = GetComponent<Rigidbody2D>();

		if (rigidbody.IsSleeping())
		{
			//Удаление всех сочленений
			foreach (Joint2D joint in GetComponentsInChildren<Joint2D>()) Destroy(joint);
			//Удаление всех твердых тел
			foreach (Rigidbody2D body in GetComponentsInChildren<Rigidbody2D>()) Destroy(body);
			//Удаление всех коллайдеров
			foreach (Collider2D collider in GetComponentsInChildren<Collider2D>()) Destroy(collider);
			//Удаление самого объекта
			Destroy(this);
		}
    }

	/// <summary>
	/// Изменяет спрайт по типу урона
	/// </summary>
	/// <param name="damageType">Тип урона</param>
	public void ApplyDamageSprite(Gnome.DamageType damageType)
	{
		Sprite spriteToUse = null;

		switch(damageType)
		{
			case Gnome.DamageType.Burning:
				spriteToUse = burnedSprite;
				break;
			case Gnome.DamageType.Slicing:
				spriteToUse = detachedSprite;
				break;
		}

		if(spriteToUse != null)
		{
			GetComponent<SpriteRenderer>().sprite = spriteToUse;
		}
	}
}
