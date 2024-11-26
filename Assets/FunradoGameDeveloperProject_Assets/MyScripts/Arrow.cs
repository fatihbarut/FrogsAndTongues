using System.Collections;

using Unity.Burst;

using UnityEngine;

[BurstCompile]
public class Arrow : MonoBehaviour
{
	
	private Coroutine rayCheckCoroutine;

	private void Start()
	{
		// Ray gönderme iþlemini periyodik olarak baþlat
		rayCheckCoroutine = StartCoroutine(CheckRaycast());
	}
	private IEnumerator CheckRaycast()
	{
		float detectionDistance = 0.2f; // Algýlama mesafesi (çarpýþma olarak kabul edilen mesafe)

		while (true)
		{
			// Sahnedeki tüm "Grape" nesnelerini bul
			GameObject[] grapes = GameObject.FindGameObjectsWithTag("Grape");

			foreach (GameObject grape in grapes)
			{
				if (grape == null) continue; // Nesne null ise atla

				// Kare mesafeyi hesapla
				float distanceSquared = (transform.position - grape.transform.position).sqrMagnitude;

				// Eðer kare mesafe algýlama mesafesinden küçükse iþlem yap
				if (distanceSquared <= detectionDistance * detectionDistance)
				{
					Destroy(gameObject);//	InstentiateDestroyAfter();
					yield break; // Fonksiyonu durdur, çünkü iþi tamamladýk
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
