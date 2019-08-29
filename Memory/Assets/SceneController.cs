using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
	/// <summary>
	/// Кол-во строк размещения карт на столе
	/// </summary>
	public const int gridRows = 2;

	/// <summary>
	/// Кол-во столбцов размещения карт на столе
	/// </summary>
	public const int gridColumns = 4;

	/// <summary>
	/// Границы сетки размещения карт по горизонтале
	/// </summary>
	public const float offsetX = 2f;

	/// <summary>
	/// Границы сетки размещения карт по вертикале
	/// </summary>
	public const float offsetY = 2.5f;

	/// <summary>
	/// Можно-ли перевенуть еще одну карту
	/// </summary>
	public bool canReveal => _secondRevealed == null;


	/// <summary>
	/// Первая перевернутая карта
	/// </summary>
	private MemoryCard _firstRevealed;

	/// <summary>
	/// Вторая перевернутая карта
	/// </summary>
	private MemoryCard _secondRevealed;

	/// <summary>
	/// Карта в сцене
	/// </summary>
	[SerializeField]
	private MemoryCard originalCard;

	/// <summary>
	/// Коллекция рисунков карт
	/// </summary>
	[SerializeField]
	private Sprite[] images;

	/// <summary>
	/// Счет игрока
	/// </summary>
	[SerializeField]
	private TextMesh scoreLabel;

	/// <summary>
	/// Счет
	/// </summary>
	private int _score = 0;

	private List<MemoryCard> mas = new List<MemoryCard>(8);

	/// <summary>
	/// Обработка открытия карты
	/// </summary>
	/// <param name="card">Открытая карта</param>
	public void CardRevealed(MemoryCard card)
	{
		//Сохранение открытых карт
		if (_firstRevealed == null) _firstRevealed = card;
		else
		{
			_secondRevealed = card;
			StartCoroutine(CheckMatch());
		}
	}

	/// <summary>
	/// Перезапуск игры
	/// </summary>
	public void Restart()
	{
		Application.LoadLevel("Table");
	}

    void Start()
    {
		//Положение первой карты
		var startPoint = originalCard.transform.position;

		//Идентификационные пары карт
		int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };

		//Перемешивание массива
		numbers = ShuffleArray(numbers);

		//Проходка по всему столу
		for(var i = 0; i < gridColumns; i++)
			for(var j = 0; j < gridRows; j++)
			{
				MemoryCard card;
				
				//Если : первая карта - оригинал
				if (i == 0 && j == 0)
				{
					card = originalCard;
				}
				//Иначе : генераруем свою
				else
				{
					card = Instantiate(originalCard) as MemoryCard;
				}
				mas.Add(card);
				//Установка картинке карте
				var index = j * gridColumns + i;
				var id = numbers[index];
				card.SetCard(id, images[id]);

				//Задание координат карты на столе
				card.transform.position = 
					new Vector3((offsetX * i) + startPoint.x, -(offsetY * j) + startPoint.y, startPoint.z);
			}
    }

	/// <summary>
	/// Смешивание массива карт
	/// </summary>
	/// <param name="numbers">массив карт</param>
	/// <returns>перемешанный массив карт</returns>
	private int[] ShuffleArray(int[] numbers)
	{
		int[] newArray = numbers.Clone() as int[];

		for(var i = 0; i < newArray.Length; i++)
		{
			var tmp = newArray[i];
			var r = Random.Range(i, newArray.Length);
			newArray[i] = newArray[r];
			newArray[r] = tmp;
		}

		return newArray;
	}

	/// <summary>
	/// Сравнение открытых карт
	/// </summary>
	/// <returns></returns>
	private IEnumerator CheckMatch()
	{
		if (_firstRevealed.id == _secondRevealed.id)
		{
			_score++;
			scoreLabel.text = "Счет: " + _score;
		}
		else
		{
			yield return new WaitForSeconds(0.5f);

			_firstRevealed.Unreveal();
			_secondRevealed.Unreveal();
		}

		//Очистка переменных
		_firstRevealed = _secondRevealed = null;
	}
}
