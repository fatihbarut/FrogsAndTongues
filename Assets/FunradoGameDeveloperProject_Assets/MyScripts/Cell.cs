using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class Cell : MonoBehaviour
{
	public Frog.Colors cellColor;
	public enum SpawnType
	{
		Grape,
		Frog,
		Arrow
	}
	public enum DirectionOfSpawn
	{
		Up,
		Down,
		Right,
		Left
	}

	// Renk  tür ve yön bilgilerini bir arada tutan sýnýf
	[System.Serializable]
	public class CellItem
	{
		[SerializeField]
		private Frog.Colors color; // Inspector'dan ayarlanabilir
		[SerializeField]
		private Cell.SpawnType type; // Inspector'dan ayarlanabilir
		[SerializeField]
		private Cell.DirectionOfSpawn spawnDirection; // Inspector'dan ayarlanabilir

		// Getter ve Setter'lar
		public Frog.Colors Color
		{
			get => color;
			set => color = value;
		}

		public Cell.SpawnType Type
		{
			get => type;
			set => type = value;
		}

		public Cell.DirectionOfSpawn SpawnDirection
		{
			get => spawnDirection;
			set => spawnDirection = value;
		}

		// Yapýcý Metod (Constructor)
		public CellItem(Frog.Colors color, Cell.SpawnType type, Cell.DirectionOfSpawn spawnDirection)
		{
			this.color = color;
			this.type = type;
			this.spawnDirection = spawnDirection;
		}
	}


	public List<CellItem> itemList;// Renk ve tür bilgileri
	public GameObject Tile;

	public GameObject TileBlue, TileGreen, TilePurple, TileRed, TileYellow;
	public GameObject GrapeBlue, GrapeGreen, GrapePurple, GrapeRed, GrapeYellow;
	public GameObject FrogBlue, FrogGreen, FrogPurple, FrogRed, FrogYellow;
	public GameObject ArrowBlue, ArrowGreen, ArrowPurple, ArrowRed, ArrowYellow;

	private void Start()
	{
		StartCoroutine(CheckIsEmpty());
	}

	private IEnumerator CheckIsEmpty()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.1f);

			// Grape, Frog ve Arrow için kontrol
			bool foundGrape = CheckForObject("Grape");
			bool foundFrog = CheckForObject("Frog");
			bool foundArrow = CheckForObject("Arrow");

			if (!foundGrape && !foundFrog && !foundArrow)
			{
				// Eðer ne Grape, ne Frog, ne de Arrow bulunamazsa iþlemi durdur ve fonksiyonu çaðýr
				//StopCoroutine(CheckIsEmpty());

				DestructionAndAfter();
				//	break;
			}
		}
	}

	// Belirli bir tag ve layer için tarama yapan yardýmcý fonksiyon
	private bool CheckForObject(string objectType)
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, .3f, LayerMask.GetMask(objectType));

		foreach (Collider collider in colliders)
		{
			if (collider.CompareTag(objectType))
			{
				return true; // Ýlgili obje bulundu
			}
		}
		return false; // Ýlgili obje bulunamadý
	}
	public void DestructionAndAfter()
	{
		if (itemList.Count < 1)
		{
			Tile.transform.DOScale(Vector3.zero, 0.2f);
			return;
		}

		Tile.transform.DOScale(Vector3.zero, 0.2f);

		// Listedeki ilk elemaný al
		CellItem currentItem = itemList[0];

		// Prefab referanslarýný bul
		GameObject tilePrefab = null;
		GameObject itemPrefab = null;

		switch (currentItem.Color)
		{
			case Frog.Colors.Blue:
				tilePrefab = TileBlue;
				itemPrefab = GetPrefabByType(currentItem.Type, GrapeBlue, FrogBlue, ArrowBlue);
				break;
			case Frog.Colors.Green:
				tilePrefab = TileGreen;
				itemPrefab = GetPrefabByType(currentItem.Type, GrapeGreen, FrogGreen, ArrowGreen);
				break;
			case Frog.Colors.Purple:
				tilePrefab = TilePurple;
				itemPrefab = GetPrefabByType(currentItem.Type, GrapePurple, FrogPurple, ArrowPurple);
				break;
			case Frog.Colors.Red:
				tilePrefab = TileRed;
				itemPrefab = GetPrefabByType(currentItem.Type, GrapeRed, FrogRed, ArrowRed);
				break;
			case Frog.Colors.Yellow:
				tilePrefab = TileYellow;
				itemPrefab = GetPrefabByType(currentItem.Type, GrapeYellow, FrogYellow, ArrowYellow);
				break;
		}

		if (tilePrefab != null && itemPrefab != null)
		{
			// Tile spawn et
			GameObject newTile = Instantiate(tilePrefab, Tile.transform.position, Quaternion.identity);
			newTile.transform.localScale = Vector3.one * 0.1f; // Baþlangýç boyutu
			newTile.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
			{
				Tile = newTile;
			});
			newTile.transform.SetParent(transform);

			// Item için yönü spawnDirection'a göre ayarla
			Quaternion spawnRotation = GetSpawnRotation(currentItem.SpawnDirection);
			GameObject newItem = Instantiate(itemPrefab, Tile.transform.position + new Vector3(0, 0.251f, 0), spawnRotation);
			newItem.transform.localScale = Vector3.one * 0.1f; // Baþlangýç boyutu
			newItem.transform.DOScale(Vector3.one, 0.5f);
			newItem.transform.SetParent(transform);
		}

		// Listedeki ilk elemaný kaldýr
		itemList.RemoveAt(0);
	}
	private Quaternion GetSpawnRotation(DirectionOfSpawn direction)
	{
		return direction switch
		{
			DirectionOfSpawn.Up => Quaternion.Euler(0, 0, 0),
			DirectionOfSpawn.Right => Quaternion.Euler(0, 90, 0),
			DirectionOfSpawn.Down => Quaternion.Euler(0, 180, 0),
			DirectionOfSpawn.Left => Quaternion.Euler(0, -90, 0),
			_ => Quaternion.identity, // Varsayýlan: Hiç döndürme yok
		};
	}
	private GameObject GetPrefabByType(SpawnType type, GameObject grapePrefab, GameObject frogPrefab, GameObject arrowPrefab)
	{
		return type switch
		{
			SpawnType.Grape => grapePrefab,
			SpawnType.Frog => frogPrefab,
			SpawnType.Arrow => arrowPrefab,
			_ => null,
		};
	}
}
