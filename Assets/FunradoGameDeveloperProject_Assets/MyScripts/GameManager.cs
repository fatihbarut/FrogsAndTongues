
using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public TMP_Text moveCountText;
	public GameObject victoryCanvas; // VictoryCanvas objesi buraya atanmalý

	void Awake()
	{
		int level = PlayerPrefs.GetInt("Level", 1); // Level deðerini al, varsayýlan olarak 0
		string targetName = "Level." + level; // Aranan isim

		foreach (Transform child in transform)
		{
			if (child.name != targetName)

				Destroy(child.gameObject); // Diðer nesneleri yok et
		}

		foreach (Transform child in transform)
		{
			if (child.name == targetName)
			{
				child.gameObject.SetActive(true); // Hedef nesneyi aktif et
			}

		}
	}
	public void Start()
	{
		StartCoroutine(UpdateMoveCountText());
		StartCoroutine(CheckVictory());
	}
	IEnumerator CheckVictory()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.21f); // Belirtilen süre beklenir.
			if (!GameObject.FindWithTag("Grape"))
			{
				// Eðer "Grape" nesnesi yoksa zafer ekranýný göster.
				victoryCanvas.SetActive(true);

				// Level'ý bir artýr ve kaydet.
				int currentLevel = PlayerPrefs.GetInt("Level", 1);
				if (currentLevel == 30) { currentLevel = 5; }
				PlayerPrefs.SetInt("Level", currentLevel + 1);
				PlayerPrefs.Save();

				yield break; // Coroutine'i sonlandýr.
			}
		}
	}

	public void Defeat()
	{
		Debug.Log("Defeat! No more moves left.");
	}

	IEnumerator UpdateMoveCountText()
	{
		yield return new WaitForSeconds(0.1f);
		Level level = FindFirstObjectByType<Level>();

		while (true)
		{
			yield return new WaitForSeconds(0.2f);
			moveCountText.text = "MOVE : " + level.MoveCount.ToString();
		}
	}
	public void ReloadCurrentScene()
	{
		// Mevcut sahnenin adýný al ve yeniden yükle
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}