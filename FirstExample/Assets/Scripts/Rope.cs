using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Description("Веревка, состоящая из звеньев")]
public class Rope : MonoBehaviour
{
	[Header ("Тест заголовка")]
	[Description("Шаблон Rope Segment для создания новыйх звеньев")]
	public GameObject ropeSegmentPrefab;
	
	/// <summary>
	/// Список объектов Rope Segment
	/// </summary>
	List<GameObject> ropeSegments = new List<GameObject>();

	//[Description("Веревка удлиняется?")]
	public bool isIncreasing { get; set; }

	//[Description("Веревка укорачивается?")]
	public bool isDecreasing { get; set; }

	[Description("Объект твердого тела, к которому прикреплена веревка")]
	public Rigidbody2D connectedObject;

	[Description("Максимальная длина звена веревки | При превышении значения - добавляется новое звено")]
	public float maxRopeSegmentLength = 1.0f;
	
	[Space]
	[Description("Скорость генератции новых звеньев")]
	public float ropeSpeed = 4.0f;

	[Description("Минимальная длина звена веревки | При принижении значения - удаляется звено")]
	public static float MinRopeSegmentLength = 0.005f;

	/// <summary>
	/// Визуализатор LineRenderer, отображающий веревку
	/// </summary>
	LineRenderer lineRenderer;

	// Start is called before the first frame update
	void Start()
    {
		//Кэширование ссылки на визуализатор, 
		//решает проблему поиска визуализатора в каждом кадре
		lineRenderer = GetComponent<LineRenderer>();

		//Сброс состояния веревки
		ResetLength();
    }

	/// <summary>
	/// Удаляет все звенья и создает новое
	/// </summary>
	public void ResetLength()
	{
		foreach (GameObject segment in ropeSegments)
			Destroy(segment);

		ropeSegments = new List<GameObject>();

		isDecreasing = false;
		isIncreasing = false;

		CreateRopeSegment();
	}

	/// <summary>
	/// Доавляет новое звено веревки к верхнему концу
	/// </summary>
	private void CreateRopeSegment()
	{
		//Создать новое звено
		GameObject segment =
			(GameObject)Instantiate(ropeSegmentPrefab, this.transform.position, Quaternion.identity);

		//Сделать звено потомком объекта this и сохранить его мировые координаты
		ropeSegmentPrefab.transform.SetParent(this.transform, true);

		//Твердое тело звена
		Rigidbody2D segmentBody = segment.GetComponent<Rigidbody2D>();

		//Длина сочленения из звена
		SpringJoint2D segmentJoint = segment.GetComponent<SpringJoint2D>();

		//Ошибка, если шаблон звена не имеет твердого тела или пружинного сочленения
		if (segmentBody == null || segmentJoint == null)
		{
			Debug.LogError("Звено веревки не содержит твердое тело или сочленение");
			return;
		}

		//Добавление нового звена в начало списка звеньев
		ropeSegments.Insert(0, segment);

		//Если : веревка состоит из одного звена => соединить с гномиком
		if (ropeSegments.Count == 1)
		{
			SpringJoint2D connectedObjectJoint = connectedObject.GetComponent<SpringJoint2D>();

			connectedObjectJoint.connectedBody = segmentBody;

			connectedObjectJoint.distance = 0.1f;

			//Установка длины звена в максимум
			segmentJoint.distance = maxRopeSegmentLength;
		}
		//Иначе : звено не первое
		else
		{
			GameObject nextSegment = ropeSegments[1];

			SpringJoint2D nextSegmentJoint = nextSegment.GetComponent<SpringJoint2D>();

			//Присоединение сочленения к новому звену
			nextSegmentJoint.connectedBody = segmentBody;

			//Установка длины сочленения в 0 - она увеличится автоматически
			segmentJoint.distance = 0.0f;
		}

		//Соединение нового звена с опорой веревки
		segmentJoint.connectedBody = this.GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// Удаление звена
	/// </summary>
	private void RemoveRopeSegment()
	{
		//Если : звеньев меньше двух => выход
		if (ropeSegments.Count < 2) return;

		//Получаем верхнее и звено перед ним
		GameObject topSegment = ropeSegments[0];
		GameObject nextSegment = ropeSegments[1];

		//Соединяем второе звено с опорой
		SpringJoint2D nextSegmentJoint = nextSegment.GetComponent<SpringJoint2D>();

		nextSegmentJoint.connectedBody = this.GetComponent<Rigidbody2D>();

		//Удаление верхнего звена
		ropeSegments.RemoveAt(0);
		Destroy(topSegment);
	}


    /// <summary>
	/// При необходимости в каждом кадре длина веревки удлиняется/укорачивается
	/// </summary>
    void Update()
    {
		//Верхнее звено и сочленение
		GameObject topSegment = ropeSegments[0];

		SpringJoint2D topSegmentJoint = topSegment.GetComponent<SpringJoint2D>();

		if(isIncreasing)
		{//Удлиняется сочленние, либо добавляется новое звено
			if (topSegmentJoint.distance >= maxRopeSegmentLength)
			{
				CreateRopeSegment();
			}
			else
			{
				topSegmentJoint.distance += ropeSpeed * Time.deltaTime;
			}
		}

		if (isDecreasing)
		{//Сокращение сочленения, либо удаление звена
			if(topSegmentJoint.distance <= MinRopeSegmentLength)
			{
				RemoveRopeSegment();
			}
			else
			{
				topSegmentJoint.distance -= ropeSpeed * Time.deltaTime;
			}
		}

		if (lineRenderer != null)
		{
			//Визуализатор рисует линию по коллекции точек.
			//Эти точки соответствуют позициям звеньев
			//Число вершин, отображаемых визуализатором,
			//Равно числу звеньев плюс точки верхней и нижней опор
			lineRenderer.positionCount = ropeSegments.Count + 2;

			//Верхняя вершина вегда соответствует положению опоры
			lineRenderer.SetPosition(0, this.transform.position);

			//Передача визуализатору координат всех звеньев
			for(int i = 0; i < ropeSegments.Count; i++)
			{
				lineRenderer.SetPosition(i + 1, ropeSegments[i].transform.position);
			}

			//Последняя точка соотвествует точке привязки несущего объекта
			SpringJoint2D connectedObjectJoint = connectedObject.GetComponent<SpringJoint2D>();
			lineRenderer.SetPosition(ropeSegments.Count + 1,
				connectedObject.transform.TransformPoint(connectedObjectJoint.anchor));
		}
    }
}
/*
//Шпаргалка для VS о том, что здесь классы
public class SpringJoint2D { }
public class Rigidbody2D { }
public class GameObject { }
public class LineRenderer { }
*/
