using UnityEngine;
using UnityEngine.UI;


public class LevelText : MonoBehaviour
{
	public Text levelText; // Text UI nesnesini buraya baðlanýr

	private void OnEnable()
	{
		// PlayerPrefs'teki Level deðerini al ve UI'da göster
		int currentLevel = PlayerPrefs.GetInt("Level", 1); // Varsayýlan olarak 1
		levelText.text = $"Level: {currentLevel}";
	}
}
