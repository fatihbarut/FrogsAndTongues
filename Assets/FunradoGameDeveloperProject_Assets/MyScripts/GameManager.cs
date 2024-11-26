
using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public TMP_Text moveCountText;
	public GameObject victoryCanvas; // VictoryCanvas objesi buraya atanmal�

	void Awake()
	{
		int level = PlayerPrefs.GetInt("Level", 1); // Level de�erini al, varsay�lan olarak 0
		string targetName = "Level." + level; // Aranan isim

		foreach (Transform child in transform)
		{
			if (child.name != targetName)

				Destroy(child.gameObject); // Di�er nesneleri yok et
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
			yield return new WaitForSeconds(0.21f); // Belirtilen s�re beklenir.
			if (!GameObject.FindWithTag("Grape"))
			{
				// E�er "Grape" nesnesi yoksa zafer ekran�n� g�ster.
				victoryCanvas.SetActive(true);

				// Level'� bir art�r ve kaydet.
				int currentLevel = PlayerPrefs.GetInt("Level", 1);
				if (currentLevel == 30) { currentLevel = 5; }
				PlayerPrefs.SetInt("Level", currentLevel + 1);
				PlayerPrefs.Save();

				yield break; // Coroutine'i sonland�r.
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
		// Mevcut sahnenin ad�n� al ve yeniden y�kle
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}