using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Удаление объекта с задержкой
public class RemoveAfterDelay : MonoBehaviour
{
	//Задержка перед удалением в сек
	public float delay = 1.0f;

    void Start()
    {
		//Запуск сопрограммы
		StartCoroutine("Remove");
    }

	IEnumerator Remove()
	{
		//Завержка и удаление
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}
}
