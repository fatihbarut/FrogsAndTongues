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
				yield break; // Hareket tamamlandýktan sonra dur
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void MoveToWayPoints()
	{
		// Sabit hýz (örn. 5 birim/saniye)
		float moveSpeed = 5f;

		// DOTween Sequence oluþtur
		Sequence sequence = DOTween.Sequence();

		// Þu anki pozisyon baþlangýç WayPoint'i gibi ele alýnýyor
		Vector3 currentPosition = transform.position;

		// WayPoints boyunca hareket iþlemi
		for (int i = WayPoints.Count - 1; i >= 0; i--)
		{
			Transform waypoint = WayPoints[i];

			// Ýlgili WayPoint'e olan mesafeyi hesapla
			float distance = Vector3.Distance(currentPosition, waypoint.position);

			// Hareket süresini mesafeye göre hesapla (süre = mesafe / hýz)
			float duration = distance / moveSpeed;

			// Hareketi sýraya ekle
			sequence.Append(transform.DOMove(waypoint.position, duration).SetEase(Ease.Linear));

			// Bir sonraki segment için güncel pozisyonu belirle
			currentPosition = waypoint.position;
		}

		// Hareket tamamlandýðýnda ScaleDownAndDestroy fonksiyonunu çaðýr
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
