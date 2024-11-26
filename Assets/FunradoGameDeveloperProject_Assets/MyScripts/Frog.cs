using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.Burst;

[BurstCompile]
public class Frog : MonoBehaviour
{
	public enum Colors
	{
		Blue,
		Green,
		Purple,
		Red,
		Yellow
	}

	private Level level;
	internal AudioSource audioSource; // AudioSource is internal

	public Colors color; // Frog's color
	public GameObject tonguePrefab; // Prefab for the tongue

	private LineRenderer lineRenderer; // LineRenderer reference
	public List<GameObject> GrapesToEat = new List<GameObject>();
	private Animator animator;


	private void Start()
	{
		level = Object.FindFirstObjectByType<Level>();
		audioSource = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
	}

	GameObject tongue;

	private void OnMouseDown()
	{


		if (level == null || level.MoveCount <= 0)
		{
			Object.FindFirstObjectByType<GameManager>().Defeat();
			return;
		}
		transform.DOScale(new Vector3(2f, 2f, 2f), 0.3f) // Ýlk büyüme
			.SetEase(Ease.OutBounce) // Boing efekti
			.OnComplete(() => // Büyüme tamamlanýnca küçülmeye baþla
			{
				transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f) // Eski boyuta dön
					.SetEase(Ease.OutBounce); // Boing efekti
			});

		animator.Play("FrogOpenMouth");
		level.MoveCount--;

		// Spawn tongue (sphere)
		tongue = Instantiate(tonguePrefab, transform.position, Quaternion.identity);
		Tongue tongueScript = tongue.GetComponent<Tongue>();

		// Set tongue properties
		tongueScript.frog = this;
		tongueScript.color = color;

		// Get LineRenderer from the tongue
		lineRenderer = tongue.GetComponent<LineRenderer>();
	}

	public void Eat(GameObject tongue)
	{


		// Tongue nesnesindeki LineRenderer'ý al
		LineRenderer lineRenderer = tongue.GetComponent<LineRenderer>();
		if (lineRenderer == null)
		{
			Debug.LogWarning("LineRenderer bulunamadý. Dil geri çekilemiyor.");
			return;
		}

		int segmentCount = lineRenderer.positionCount;

		if (segmentCount <= 1)
		{
			Debug.LogWarning("LineRenderer'da geri çekilecek yeterli segment yok.");
			return;
		}

		// Üzümleri sýrayla hareket ettir
		foreach (GameObject grape in GrapesToEat)
		{
			if (grape != null)
			{
				grape.GetComponent<Grape>().goToFrog = true;
			}
		}

		// Dil geri çekilme animasyonu
		Sequence retractSequence = DOTween.Sequence();

		for (int i = segmentCount - 1; i > 0; i--)
		{
			int currentIndex = i;
			Vector3 previousSegment = lineRenderer.GetPosition(currentIndex - 1);

			retractSequence.Append(tongue.transform.DOMove(previousSegment, 1f).SetEase(Ease.Linear).OnComplete(() =>
			{
				lineRenderer.positionCount = currentIndex; // Segmenti kaldýr
			}));
		}

		// Geri çekilme tamamlandýðýnda dil yok edilir
		retractSequence.OnComplete(() =>
		{

			Destroy(tongue);

			StartCoroutine(DestroyIfNoGrapeIsMoving());

			//	GrapesToEat.Clear(); // Listeyi temizle
		});
	}


	IEnumerator DestroyIfNoGrapeIsMoving()
	{
		while (true)
		{
			// GrapesToEat listesindeki tüm elemanlarýn null olup olmadýðýný kontrol et
			bool allNull = true;
			foreach (var grape in GrapesToEat)
			{
				if (grape != null) // Eðer herhangi bir eleman null deðilse
				{
					allNull = false;
					break; // Daha fazla kontrol etmeye gerek yok
				}
			}

			// Eðer tüm elemanlar null ise nesneyi yok et
			if (allNull)
			{
				Destroy(gameObject);
				yield break; // Coroutine'i sonlandýr
			}

			// Bir sonraki çerçeveye kadar bekle
			yield return null;
		}
	}

	public void DestroyAfter()
	{
		Destroy(gameObject);
	}

}
