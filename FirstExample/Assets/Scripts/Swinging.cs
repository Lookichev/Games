
using UnityEngine;

/// <summary>
/// Использует наклон девайса для отклонение гномика
/// </summary>
public class Swinging : MonoBehaviour
{
	/// <summary>
	/// Размер отклонения
	/// </summary>
	public float swingSensitivity = 100.0f;

    /// <summary>
	/// Упрощает работу с движком
	/// </summary>
    void FixedUpdate()
    {
		//Если : тело отсутствует
        if (GetComponent<Rigidbody2D>() == null)
		{
			Destroy(this);
			return;
		}

		//Приложение силы
		GetComponent<Rigidbody2D>().AddForce(
			new Vector2(InputManager.instance.sidewaysMotion * swingSensitivity, 0));
	}
}
