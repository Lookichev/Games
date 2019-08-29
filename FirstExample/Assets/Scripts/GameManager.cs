using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	//Местоположение появления гномика
	public GameObject startingPoint;

	//Объект веревка
	public Rope rope;

	//Сценарий, управдяющий камерой
	public CameraFollow cameraFollow;

	//Текущий гномик
	Gnome currentGnome;

	// Шаблон гномика
	public GameObject gnomePrefab;

	//Компонент пользовательского интерфейса (перезапуск и продолжить)
	public RectTransform mainMenu;

	//Компонент пользовательского интерфейса (вверх, вниз, меню)
	public RectTransform gameplayMenu;

	//Компонент пользовательского интерфейса (Вы выиграли)
	public RectTransform gameOverMenu;

	//Режим бога для гномика
	public bool gnomeInvincible { get; set; }

	//Задержка перед созданием нового гномидзе
	public float delayAfterDeath = 1.0f;

	// Звук победы
	public AudioClip gameOverSound;

	// Звук гибели
	public AudioClip gnomeDiedSound;

	void Start()
    {
		//В момент запуска игры откат
		Reset();
    }

	//Сброс игры в исходное
	void Reset()
	{
		//Выключает меню, включает интерфейс игры
		if (gameOverMenu) gameOverMenu.gameObject.SetActive(false);

		if (mainMenu) mainMenu.gameObject.SetActive(false);

		if (gameplayMenu) gameplayMenu.gameObject.SetActive(true);

		//Находит все компоненты для сброски в исходное
		foreach (var reset in FindObjectsOfType<Resettable>()) reset.Reset();

		CreateNewGnome();

		//Прерыв паузы в игре
		Time.timeScale = 1.0f;
	}

	void CreateNewGnome()
	{
		//Удаление старого гнома
		RemoveGnome();

		//Создание нового
		GameObject newGnome = 
			Instantiate(gnomePrefab, startingPoint.transform.position, Quaternion.identity);

		//Добавление его в текущего
		currentGnome = newGnome.GetComponent<Gnome>();

		//Отображение веревки
		rope.gameObject.SetActive(true);

		//привязка веревки к гному
		rope.connectedObject = currentGnome.ropeBody;

		//Установка веревки в нуль
		rope.ResetLength();

		//Сообщение камеры, что надо следить за новым гномом
		cameraFollow.target = currentGnome.cameraFollowTarget;
	}

	void RemoveGnome()
	{
		//гномик неуязвим
		if (gnomeInvincible) return;

		//Веревка невидима
		rope.gameObject.SetActive(false);

		//Камера не следит за гномом
		cameraFollow.target = null;

		//Если гномик существует - делит
		if (currentGnome != null)
		{
			currentGnome.holdingTreasure = false;

			//Коллайдеры не сообщают о столкновении с гномом
			currentGnome.gameObject.tag = "Untagged";

			foreach (Transform child in currentGnome.transform)
				child.gameObject.tag = "Untagged";

			currentGnome = null;
		}
	}

	void KillGnome(Gnome.DamageType damageType)
	{
		//Если задан звук смерти гнома
		var audio = GetComponent<AudioSource>();
		if (audio) audio.PlayOneShot(gnomeDiedSound);

		//Демонстрация эффекта ловушки
		currentGnome.ShowDamageEffect(damageType);

		//Если гном уязвим
		if (!gnomeInvincible)
		{
			currentGnome.DestroyGnome(damageType);

			RemoveGnome();

			StartCoroutine(ResetAfterDelay());
		}
	}

	//В момент гибели гнома
	IEnumerator ResetAfterDelay()
	{
		//Ждать перед вызовом перезапуска
		yield return new WaitForSeconds(delayAfterDeath);
		Reset();
	}

	//Вызов при попадании на ножи
	public void TrapTouched()
	{
		KillGnome(Gnome.DamageType.Slicing);
	}

	//Вызов огненной ловушки
	public void FireTrapTouched()
	{
		KillGnome(Gnome.DamageType.Burning);
	}

	//Касание сокровищ
	public void TreasureCollected()
	{
		currentGnome.holdingTreasure = true;
	}

	//Гном касается выхода
	public void ExitReached()
	{
		//Если гном без сокровища находится наверху
		if (currentGnome != null && !currentGnome.holdingTreasure) return;

		//Завершение игры, если гном держит сокровище у выхода
		//if (currentGnome != null && currentGnome.holdingTreasure)
		{
			var audio = GetComponent<AudioSource>();
			if (audio) audio.PlayOneShot(gameOverSound);
		}

		//Приостановка игры
		Time.timeScale = 0.0f;

		if (gameOverMenu) gameOverMenu.gameObject.SetActive(true);
		if (gameplayMenu) gameplayMenu.gameObject.SetActive(false);
	}

	//Вызов паузы при касании кнопки меню или резума
	public void SetPaused (bool paused)
	{
		//Если игра на паузе - остановка, вкл меню
		if (paused)
		{
			Time.timeScale = 0.0f;

			mainMenu.gameObject.SetActive(true);
			gameplayMenu.gameObject.SetActive(false);
		}
		else
		{
			Time.timeScale = 1.0f;
			mainMenu.gameObject.SetActive(false);
			gameplayMenu.gameObject.SetActive(true);
		}
	}

	public void RestartGame()
	{
		Destroy(currentGnome.gameObject);
		currentGnome = null;
		Reset();
	}
}
