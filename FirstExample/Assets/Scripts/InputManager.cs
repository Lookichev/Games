
using UnityEngine;

/// <summary>
///  Преобразует данные, полученные от акселерометра, в информацию о боковом смещении
/// </summary>
public class InputManager : Singleton<InputManager>
{

	/// <summary>
	/// Возвращает величину смещения
	/// </summary>
	/// <remarks>-1 = max лево; 1 = max право</remarks>
	public float sidewaysMotion { get; private set; } = 0.0f;

	
	/// <summary>
	/// Величина отклонения сохраняется в каждом кадре
	/// </summary>
	void Update()
	{
		Vector3 accel = Input.acceleration;

		sidewaysMotion = accel.x;
	}
}
