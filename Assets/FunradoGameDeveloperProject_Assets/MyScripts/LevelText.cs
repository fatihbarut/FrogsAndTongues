using UnityEngine;
using UnityEngine.UI;


public class LevelText : MonoBehaviour
{
	public Text levelText; // Text UI nesnesini buraya ba�lan�r

	private void OnEnable()
	{
		// PlayerPrefs'teki Level de�erini al ve UI'da g�ster
		int currentLevel = PlayerPrefs.GetInt("Level", 1); // Varsay�lan olarak 1
		levelText.text = $"Level: {currentLevel}";
	}
}
