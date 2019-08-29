using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SignalOnTouch : MonoBehaviour
{
	//Оповещение о касании
	public UnityEvent onTouch;

	//Если тру то при касании проигрывается звук
	public bool playAudioOnTouch = true;

	// Вызов SendSignal при входе в олбасть взаимодействия
	void OnTriggerEnter2D(Collider2D collider)
	{
		SendSignal(collider.gameObject);
	}

	//Вызов, при касании с объектом
	void OnColliderEnter2D(Collision2D collision)
	{
		SendSignal(collision.gameObject);
	}

	//Проверка тэга игрока и вызов события
	void SendSignal(GameObject objectThatHit)
	{
		//Проверка игрок ли объект
		if (objectThatHit.CompareTag("Player"))
		{
			//Проверка звука
			if (playAudioOnTouch)
			{
				var audio = GetComponent<AudioSource>();

				if (audio && audio.gameObject.activeInHierarchy) audio.Play();
			}

			onTouch.Invoke();
		}
	}
}
