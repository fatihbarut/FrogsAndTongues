using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Unity.Burst;
using System.Collections;

[BurstCompile]
public class Tongue : MonoBehaviour
{
	public Frog frog;
	public Frog.Colors color; // Reference the updated enum from Frog
	public List<Transform> WayPoints = new List<Transform>(); // WayPoints listesi

	private LineRenderer lineRenderer;
	private Vector3 direction; // Movement direction
	private bool isMoving = true;

	private AudioSource audioSource;
	public AudioClip MistakeSound;

	private float detectionDistance = 0.2f; // �arp��ma olarak kabul edilen mesafe
	private float moveSpeed = 5f; // Hareket h�z�

	private GameObject TheLastHitObject; // Son �arp�lan nesne

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		audioSource = GetComponent<AudioSource>();

		direction = frog.transform.forward;

		// Frog'un Transform'unu ba�lang��ta WayPoints listesine ekleriz
		WayPoints.Add(frog.transform);
		//	StartCoroutine(CheckForClosestObjectEnum());

		isMoving = true;
	}
	[BurstCompile]
	private void Update()
	{
		if (isMoving)
		{
			MoveTongue();
			UpdateLineRenderer();
			CheckForClosestObject();
		}
		else
		{
			Debug.LogWarning("Dil hareket etmiyor ��nk� isMoving false.");
		}
	}


	private void MoveTongue()
	{
		// Dilin pozisyonunu s�rekli olarak g�nceller
		
		transform.position += direction.normalized * moveSpeed * Time.deltaTime;
	}

	private void CheckForClosestObject()
	{
		float detectionDistanceSquared = detectionDistance * detectionDistance; // Kare mesafe
		GameObject closestObject = null;
		float closestDistanceSquared = float.MaxValue;

		// Kontrol edilecek tagler
		string[] tagsToCheck = { "Arrow", "Grape", "Frog" };

		// En yak�n nesneyi bul
		foreach (string tag in tagsToCheck)
		{
			GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
			foreach (GameObject obj in objectsWithTag)
			{
				if (obj == null) continue; // Null kontrol�

				// Kare mesafeyi hesapla
				float distanceSquared = (transform.position - obj.transform.position).sqrMagnitude;

				// En yak�n nesneyi belirle
				if (distanceSquared <= detectionDistanceSquared && distanceSquared < closestDistanceSquared)
				{
					closestDistanceSquared = distanceSquared;
					closestObject = obj;
				}
			}
		}

		// E�er en yak�n nesne bulunduysa
		if (closestObject != null)
		{
			HandleObjectCollision(closestObject.GetComponent<Collider>());

			// SphereCast ile ileri y�ndeki nesneleri kontrol et
			RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.5f, transform.forward, 1000);

			// SphereCast sonucunda en yak�n nesneden ba�ka nesne var m� kontrol et
			bool hasOtherObjects = false;
			foreach (RaycastHit hit in hits)
			{
				if (hit.collider.gameObject != closestObject)
				{
					hasOtherObjects = true;
					break;
				}
			}

			// E�er ba�ka nesne yoksa Eat �a��r
			if (!hasOtherObjects)
			{
				Debug.Log("eat");
				frog.Eat(this.gameObject);
			}
		}
	}



	private GameObject lastCollidedObject; // Son �arp�lan nesneyi tutacak de�i�ken

	private void HandleObjectCollision(Collider other)
	{
		// E�er son �arp�lan nesne ayn� ise i�leme girme
		if (lastCollidedObject == other.gameObject)
		{
			//Debug.Log($"Zaten bu nesneyle i�lem yap�ld�: {other.gameObject.name}");
			return;
		}

		// Yeni bir �arp��ma oldu�u i�in �arp�lan nesneyi kaydet
		lastCollidedObject = other.gameObject;

		
		if (other.CompareTag("Grape"))
		{
			HandleGrapeCollision(other);
		}
		else if (other.CompareTag("Arrow"))
		{
			HandleArrowCollision(other);
		}
		else if (other.CompareTag("Frog") && frog.gameObject != other.gameObject)
		{
			if (frog.GrapesToEat.Count<1)
			{
				Mistake();
			}
			else
			{
				frog.Eat(this.gameObject);
			}
			Debug.Log("frog'a �arpt�");
		}
	}

	private void HandleGrapeCollision(Collider other)
	{
		if (other.name.Contains(color.ToString()) && other.name.Contains("Grape"))
		{
			// Correct Grape
			other.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() =>
			{
				other.transform.DOScale(Vector3.one, 0.2f);
			if(other.GetComponent<AudioSource>().isPlaying==false)    other.GetComponent<AudioSource>().PlayOneShot(other.GetComponent<AudioSource>().clip);
			});
			

			if (!frog.GrapesToEat.Contains(other.gameObject))
			{
				frog.GrapesToEat.Add(other.gameObject);
			}

			// Grape'in WayPoints listesini g�ncelle
			Grape grape = other.GetComponent<Grape>();
			if (grape != null)
			{
				grape.WayPoints = new List<Transform>(WayPoints); // WayPoints listesini aktar
			}

			// Gitti�i y�nde ba�ka nesne olup olmad���n� kontrol et
			if (!CheckForRemainingObjects())
			{
				frog.Eat(this.gameObject);
			}
		}
		else
		{
			Mistake();
		}
	}

	private void HandleArrowCollision(Collider other)
	{
		if (other.name.Contains(color.ToString()))
		{
			// Hareketi durdur
			isMoving = false;

			// Arrow'u WayPoints listesine ekle
			WayPoints.Add(other.transform);

			// Dil Arrow'un konumuna gider
			transform.position = other.transform.position;

			// Arrow'un y�n�ne d�n
			direction = other.transform.forward; // Yeni y�n� belirle
			Quaternion targetRotation = Quaternion.LookRotation(direction);
			transform.rotation = targetRotation;

			// LineRenderer segment ekle
			AddLineRendererSegment(other.transform.position);

			isMoving = true;
		}
		else
		{
			Mistake();
		}
	}

	private void AddLineRendererSegment(Vector3 position)
	{
		// Yeni segment noktas� ekle
		int currentCount = lineRenderer.positionCount;
		lineRenderer.positionCount = currentCount + 1;
		lineRenderer.SetPosition(currentCount, position);
	}

	private void UpdateLineRenderer()
	{
		lineRenderer.SetPosition(0, frog.transform.position); // Frog'un pozisyonu
		lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position); // Tongue'un g�ncel pozisyonu
	}

	private bool CheckForRemainingObjects()
	{
		// Gitti�i y�nde 100m raycast ile �arpt��� t�m nesneleri al
		RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, 100f);

		// �arp�lan nesne d���nda ba�ka nesne var m� kontrol et
		foreach (var hit in hits)
		{
			if (hit.collider.gameObject != TheLastHitObject)
			{
				// E�er ba�ka bir nesne varsa devam et
				return true;
			}
		}

		// Ba�ka nesne yok, sona geldik
		return false;
	}

	private void Mistake()
	{

		frog.audioSource.PlayOneShot(MistakeSound);
		Destroy(gameObject);

	}


}
