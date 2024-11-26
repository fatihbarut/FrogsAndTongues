using System.Collections;

using Unity.Burst;

using UnityEngine;

[BurstCompile]
public class Arrow : MonoBehaviour
{
	
	private Coroutine rayCheckCoroutine;

	private void Start()
	{
		// Ray g�nderme i�lemini periyodik olarak ba�lat
		rayCheckCoroutine = StartCoroutine(CheckRaycast());
	}
	private IEnumerator CheckRaycast()
	{
		float detectionDistance = 0.2f; // Alg�lama mesafesi (�arp��ma olarak kabul edilen mesafe)

		while (true)
		{
			// Sahnedeki t�m "Grape" nesnelerini bul
			GameObject[] grapes = GameObject.FindGameObjectsWithTag("Grape");

			foreach (GameObject grape in grapes)
			{
				if (grape == null) continue; // Nesne null ise atla

				// Kare mesafeyi hesapla
				float distanceSquared = (transform.position - grape.transform.position).sqrMagnitude;

				// E�er kare mesafe alg�lama mesafesinden k���kse i�lem yap
				if (distanceSquared <= detectionDistance * detectionDistance)
				{
					Destroy(gameObject);//	InstentiateDestroyAfter();
					yield break; // Fonksiyonu durdur, ��nk� i�i tamamlad�k
				}
			}

			// Her 0.01 saniyede bir tekrar kontrol et
			yield return new WaitForSeconds(0.01f);
		}
	}

	private void OnDestroy()
	{
		// Obje yok edilirken Coroutine durdurulur
		if (rayCheckCoroutine != null)
		{
			StopCoroutine(rayCheckCoroutine);
		}
	}
}
