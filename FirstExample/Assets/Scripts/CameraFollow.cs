using UnityEngine;

/// <summary>
/// Синхронизирует позицию камеры с позицией целевого объекта
/// </summary>
public class CameraFollow : MonoBehaviour
{
	/// <summary>
	/// Целевой объект, под объективом камеры
	/// </summary>
	public Transform target;

	/// <summary>
	/// Максимальная точка положения камеры
	/// </summary>
	public float topLimit = 10.0f;

	/// <summary>
	/// Минимальная точка положения камеры
	/// </summary>
	public float bottomLimit = -10.0f;

	/// <summary>
	/// Скорость следования камеры за объектом
	/// </summary>
	public float followSpeed = 0.5f;

	/// <summary>
	/// Определяет положение камеры после установки позиций всех объектов
	/// </summary>
    void LateUpdate()
    {
		//Если : целевой объект не определен
		if (target == null) return;

		//Позиция объекта
		Vector3 newPosition = transform.position;

		//Определение положения камеры
		newPosition.y = Mathf.Lerp(newPosition.y, target.position.y, followSpeed);

		//Ограничение выхода за границы
		newPosition.y = Mathf.Min(newPosition.y, topLimit);
		newPosition.y = Mathf.Max(newPosition.y, bottomLimit);

		//Обновить местоположение
		transform.position = newPosition;
    }

	/// <summary>
	/// При выборе камеры в редакторе, рисует линию между границами
	/// </summary>
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;

		var topPoint = new Vector3(transform.position.x, topLimit, transform.position.z);
		var bottomPoint = new Vector3(transform.position.x, bottomLimit, transform.position.z);

		Gizmos.DrawLine(topPoint, bottomPoint);
	}
}
