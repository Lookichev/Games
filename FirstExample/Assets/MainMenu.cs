using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	// Имя сцены
	public string sceneToLoad;

	// Содержит текст "Loading..."
	public RectTransform loadingOverlay;

	// Фоновая загрузка сцены
	AsyncOperation sceneLoadingOperation;

	// При запуске начинает загрузку игры
    public void Start()
    {
		//Скрывает заставку 
		loadingOverlay.gameObject.SetActive(false);

		//Асинхронная загрузка сцены
		sceneLoadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);

		//Но не переключает на эту сцену до готовности
		sceneLoadingOperation.allowSceneActivation = false;
    }

	// Вызывается по запуску новой игры
    public void LoadScene()
	{
		//Делает заставку Loading  видимой
		loadingOverlay.gameObject.SetActive(true);

		//Сообщает о переключении сцен по готовности
		sceneLoadingOperation.allowSceneActivation = true;
	}
}
