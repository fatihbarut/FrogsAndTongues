using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;

[BurstCompile]
public class Grape : MonoBehaviour
{
	public List<Transform> WayPoints = new List<Transform>(); // WayPoints listesi
	public bool goToFrog = false;
	private AudioSource audioSource;
	public AudioClip EatenSound;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		StartCoroutine(CheckForMovement());
	}

	private IEnumerator CheckForMovement()
	{
		while (true)
		{
			if (goToFrog && WayPoints.Count > 0)
			{
				MoveToWayPoints();
				yield break; // Hareket tamamland�ktan sonra dur
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void MoveToWayPoints()
	{
		// Sabit h�z (�rn. 5 birim/saniye)
		float moveSpeed = 5f;

		// DOTween Sequence olu�tur
		Sequence sequence = DOTween.Sequence();

		// �u anki pozisyon ba�lang�� WayPoint'i gibi ele al�n�yor
		Vector3 currentPosition = transform.position;

		// WayPoints boyunca hareket i�lemi
		for (int i = WayPoints.Count - 1; i >= 0; i--)
		{
			Transform waypoint = WayPoints[i];

			// �lgili WayPoint'e olan mesafeyi hesapla
			float distance = Vector3.Distance(currentPosition, waypoint.position);

			// Hareket s�resini mesafeye g�re hesapla (s�re = mesafe / h�z)
			float duration = distance / moveSpeed;

			// Hareketi s�raya ekle
			sequence.Append(transform.DOMove(waypoint.position, duration).SetEase(Ease.Linear));

			// Bir sonraki segment i�in g�ncel pozisyonu belirle
			currentPosition = waypoint.position;
		}

		// Hareket tamamland���nda ScaleDownAndDestroy fonksiyonunu �a��r
		sequence.OnComplete(() =>
		{
			ScaleDownAndDestroy();
		});
	}


	private void ScaleDownAndDestroy()
	{
		audioSource.PlayOneShot(EatenSound);
		transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
		{
			Destroy(gameObject);
		});
	}
}
