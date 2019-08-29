
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	/// <summary>
	/// Единственный экземпляр класса
	/// </summary>
	private static T c_instance;

	/// <summary>
	/// Возвращает экземпляр
	/// </summary>
	public static T instance
	{
		get
		{
			if (c_instance == null)
			{
				c_instance = FindObjectOfType<T>();

				if (c_instance == null)
					Debug.LogError("Невозможно найти " + typeof(T));
			}

			return c_instance;
		}
	}
}
