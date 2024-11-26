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
		transform.DOScale(new Vector3(2f, 2f, 2f), 0.3f) // �lk b�y�me
			.SetEase(Ease.OutBounce) // Boing efekti
			.OnComplete(() => // B�y�me tamamlan�nca k���lmeye ba�la
			{
				transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f) // Eski boyuta d�n
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


		// Tongue nesnesindeki LineRenderer'� al
		LineRenderer lineRenderer = tongue.GetComponent<LineRenderer>();
		if (lineRenderer == null)
		{
			Debug.LogWarning("LineRenderer bulunamad�. Dil geri �ekilemiyor.");
			return;
		}

		int segmentCount = lineRenderer.positionCount;

		if (segmentCount <= 1)
		{
			Debug.LogWarning("LineRenderer'da geri �ekilecek yeterli segment yok.");
			return;
		}

		// �z�mleri s�rayla hareket ettir
		foreach (GameObject grape in GrapesToEat)
		{
			if (grape != null)
			{
				grape.GetComponent<Grape>().goToFrog = true;
			}
		}

		// Dil geri �ekilme animasyonu
		Sequence retractSequence = DOTween.Sequence();

		for (int i = segmentCount - 1; i > 0; i--)
		{
			int currentIndex = i;
			Vector3 previousSegment = lineRenderer.GetPosition(currentIndex - 1);

			retractSequence.Append(tongue.transform.DOMove(previousSegment, 1f).SetEase(Ease.Linear).OnComplete(() =>
			{
				lineRenderer.positionCount = currentIndex; // Segmenti kald�r
			}));
		}

		// Geri �ekilme tamamland���nda dil yok edilir
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
			// GrapesToEat listesindeki t�m elemanlar�n null olup olmad���n� kontrol et
			bool allNull = true;
			foreach (var grape in GrapesToEat)
			{
				if (grape != null) // E�er herhangi bir eleman null de�ilse
				{
					allNull = false;
					break; // Daha fazla kontrol etmeye gerek yok
				}
			}

			// E�er t�m elemanlar null ise nesneyi yok et
			if (allNull)
			{
				Destroy(gameObject);
				yield break; // Coroutine'i sonland�r
			}

			// Bir sonraki �er�eveye kadar bekle
			yield return null;
		}
	}

	public void DestroyAfter()
	{
		Destroy(gameObject);
	}

}
