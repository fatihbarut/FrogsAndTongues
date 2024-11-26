using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using static Cell;

#if UNITY_EDITOR
using Sirenix.OdinInspector;





[ExecuteInEditMode]

public class LevelDesigner : MonoBehaviour
{
	[Title("Selections", horizontalLine: true, bold: true)]

	[BoxGroup("Color Selection")]
	[ReadOnly, DisplayAsString, LabelText("Selected Color")]
	public Frog.Colors selectedColor;

	[HorizontalGroup("Color Selection/Buttons", Width = 80)]
	[GUIColor(0.0f, 0.5f, 1.0f)] // Mavi renk
	[Button("Blue")]
	private void SetBlue() => selectedColor = Frog.Colors.Blue;

	[HorizontalGroup("Color Selection/Buttons")]
	[GUIColor(1.0f, 1.0f, 0.0f)] // Sarý renk
	[Button("Yellow")]
	private void SetYellow() => selectedColor = Frog.Colors.Yellow;

	[HorizontalGroup("Color Selection/Buttons")]
	[GUIColor(0.0f, 1.0f, 0.0f)] // Yeþil renk
	[Button("Green")]
	private void SetGreen() => selectedColor = Frog.Colors.Green;

	[HorizontalGroup("Color Selection/Buttons")]
	[GUIColor(0.5f, 0.0f, 1.0f)] // Mor renk
	[Button("Purple")]
	private void SetPurple() => selectedColor = Frog.Colors.Purple;

	[HorizontalGroup("Color Selection/Buttons")]
	[GUIColor(1.0f, 0.0f, 0.0f)] // Kýrmýzý renk
	[Button("Red")]
	private void SetRed() => selectedColor = Frog.Colors.Red;

	[BoxGroup("Item Selection")]
	[ReadOnly, DisplayAsString, LabelText("Selected Item")]
	public Items selectedItem;

	[HorizontalGroup("Item Selection/Buttons", Width = 80)]
	[Button("Frog")]
	private void SetFrog() => selectedItem = Items.Frog;

	[HorizontalGroup("Item Selection/Buttons")]
	[Button("Arrow")]
	private void SetArrow() => selectedItem = Items.Arrow;

	[HorizontalGroup("Item Selection/Buttons")]
	[Button("Grape")]
	private void SetGrape() => selectedItem = Items.Grape;

	[BoxGroup("Direction Selection")]
	[ReadOnly, DisplayAsString, LabelText("Selected Direction")]
	public Directions selectedDirection;

	[HorizontalGroup("Direction Selection/Buttons", Width = 80)]
	[Button("Up")]
	private void SetUp() => selectedDirection = Directions.Up;

	[HorizontalGroup("Direction Selection/Buttons")]
	[Button("Down")]
	private void SetDown() => selectedDirection = Directions.Down;

	[HorizontalGroup("Direction Selection/Buttons")]
	[Button("Left")]
	private void SetLeft() => selectedDirection = Directions.Left;

	[HorizontalGroup("Direction Selection/Buttons")]
	[Button("Right")]
	private void SetRight() => selectedDirection = Directions.Right;

	[Title("Grid Settings", horizontalLine: true, bold: true)]
	[BoxGroup("Grid Settings")]
	[ValueDropdown("RowCountList"), LabelText("Row Count")]
	public int RowCount;

	[BoxGroup("Grid Settings")]
	[ValueDropdown("ColCountList"), LabelText("Column Count")]
	public int ColumnCount;

	[BoxGroup("Grid Settings")]
	[LabelText("Base Cell")]
	public GameObject BaseCell;

	[Title("Prefabs", horizontalLine: true, bold: true)]
	[FoldoutGroup("Prefabs", expanded: true)]
	[FoldoutGroup("Prefabs/Cell Prefabs")]
	[LabelText("Cell (Blue)")]
	public GameObject CellBlue;

	[FoldoutGroup("Prefabs/Cell Prefabs")]
	[LabelText("Cell (Yellow)")]
	public GameObject CellYellow;

	[FoldoutGroup("Prefabs/Cell Prefabs")]
	[LabelText("Cell (Green)")]
	public GameObject CellGreen;

	[FoldoutGroup("Prefabs/Cell Prefabs")]
	[LabelText("Cell (Purple)")]
	public GameObject CellPurple;

	[FoldoutGroup("Prefabs/Cell Prefabs")]
	[LabelText("Cell (Red)")]
	public GameObject CellRed;

	[FoldoutGroup("Prefabs/Frog Prefabs")]
	[LabelText("Frog (Blue)")]
	public GameObject FrogBlue;

	[FoldoutGroup("Prefabs/Frog Prefabs")]
	[LabelText("Frog (Yellow)")]
	public GameObject FrogYellow;

	[FoldoutGroup("Prefabs/Frog Prefabs")]
	[LabelText("Frog (Green)")]
	public GameObject FrogGreen;

	[FoldoutGroup("Prefabs/Frog Prefabs")]
	[LabelText("Frog (Purple)")]
	public GameObject FrogPurple;

	[FoldoutGroup("Prefabs/Frog Prefabs")]
	[LabelText("Frog (Red)")]
	public GameObject FrogRed;

	[FoldoutGroup("Prefabs/Grape Prefabs")]
	[LabelText("Grape (Blue)")]
	public GameObject GrapeBlue;

	[FoldoutGroup("Prefabs/Grape Prefabs")]
	[LabelText("Grape (Yellow)")]
	public GameObject GrapeYellow;

	[FoldoutGroup("Prefabs/Grape Prefabs")]
	[LabelText("Grape (Green)")]
	public GameObject GrapeGreen;

	[FoldoutGroup("Prefabs/Grape Prefabs")]
	[LabelText("Grape (Purple)")]
	public GameObject GrapePurple;

	[FoldoutGroup("Prefabs/Grape Prefabs")]
	[LabelText("Grape (Red)")]
	public GameObject GrapeRed;

	[FoldoutGroup("Prefabs/Arrow Prefabs")]
	[LabelText("Arrow (Blue)")]
	public GameObject ArrowBlue;

	[FoldoutGroup("Prefabs/Arrow Prefabs")]
	[LabelText("Arrow (Yellow)")]
	public GameObject ArrowYellow;

	[FoldoutGroup("Prefabs/Arrow Prefabs")]
	[LabelText("Arrow (Green)")]
	public GameObject ArrowGreen;

	[FoldoutGroup("Prefabs/Arrow Prefabs")]
	[LabelText("Arrow (Purple)")]
	public GameObject ArrowPurple;

	[FoldoutGroup("Prefabs/Arrow Prefabs")]
	[LabelText("Arrow (Red)")]
	public GameObject ArrowRed;

	private int[] RowCountList = new int[] { 4, 5, 6, 7 };
	private int[] ColCountList = new int[] { 4, 5, 6 };

	private Dictionary<string, GameObject> baseCellLookup;

	public enum FrogColors { Blue, Yellow, Green, Purple, Red }
	public enum Items { Frog, Arrow, Grape }
	public enum Directions { Up, Down, Left, Right }
	private void OnEnable()
	{
		SceneView.duringSceneGui -= OnScene;
		// add a listener
		SceneView.duringSceneGui += OnScene;
	}

	private void OnDisable()
	{
		SceneView.duringSceneGui -= OnScene;
	}
	public bool SpawnModeOn;

	GameObject baseCell = null;
	private void OnScene(SceneView sceneview)
	{

		if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && SpawnModeOn == true)
		{

			Event.current.Use();
			//	Ray ray = sceneview.camera.ScreenPointToRay(Event.current.mousePosition);

			Vector3 mousePosition = Event.current.mousePosition;
			Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

			RaycastHit hit;
			int layer_mask = LayerMask.GetMask("BaseCell");
			if (Physics.Raycast(ray, out hit, 1000, layer_mask))
			{
				//	Debug.Log(hit.transform.name);
				Vector3 clickPosition = hit.point; // Týklama pozisyonu
				baseCell = hit.transform.gameObject;
				HandleSpawn(clickPosition);

			}


		}
	}

	private void HandleSpawn(Vector3 position)
	{
		// Ayný konumdaki Cell, Frog, Arrow ve Grape nesnelerini yok et
		string[] tags = { "Cell", "Frog", "Arrow", "Grape" };

		if (baseCell != null)
		{
			Vector3 baseCellPosition = baseCell.transform.position;

			foreach (string tag in tags)
			{
				foreach (var obj in GameObject.FindGameObjectsWithTag(tag))
				{
					if (Mathf.Approximately(obj.transform.position.x, baseCellPosition.x) &&
						Mathf.Approximately(obj.transform.position.z, baseCellPosition.z) &&
						obj.transform.position.y > baseCellPosition.y)
					{
						DestroyImmediate(obj);
					}
				}
			}
		}

		Transform parent = transform;

		// Yeni Cell oluþtur
#if UNITY_EDITOR
		GameObject newCell = PrefabUtility.InstantiatePrefab(GetSelectedCellPrefab(), parent) as GameObject;
		if (newCell != null)
		{
			newCell.transform.position = baseCell.transform.position + new Vector3(0, 0.1f, 0);
			newCell.transform.rotation = Quaternion.identity;
			newCell.name = "Cell" + selectedColor;
			newCell.GetComponent<Cell>().cellColor = selectedColor;
			// Komþu hücre renk kontrolü
			CheckAdjacentColors(newCell);
		}

		GameObject newItem = PrefabUtility.InstantiatePrefab(GetSelectedItemPrefab(), parent) as GameObject;
		if (newItem != null)
		{
			newItem.transform.position = baseCell.transform.position + new Vector3(0, 0.38f, 0);
			newItem.transform.rotation = Quaternion.identity;
			newItem.name = selectedItem + selectedColor.ToString();
		}
#else
    Debug.LogWarning("PrefabUtility can only be used in the Unity Editor!");
#endif

		// Yönünü ayarla
		if (newItem != null)
		{
			switch (selectedDirection)
			{
				case Directions.Up:
					newItem.transform.rotation = Quaternion.Euler(0, 0, 0);
					break;
				case Directions.Down:
					newItem.transform.rotation = Quaternion.Euler(0, 180, 0);
					break;
				case Directions.Left:
					newItem.transform.rotation = Quaternion.Euler(0, -90, 0);
					break;
				case Directions.Right:
					newItem.transform.rotation = Quaternion.Euler(0, 90, 0);
					break;
			}
		}
	}

	private GameObject FindNearestRight(GameObject obj)
	{
		// Sahnedeki "Cell" tag'ine sahip tüm nesneleri al
		GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Cell");
		GameObject nearest = null;
		float shortestDistance = Mathf.Infinity;

		Vector3 objPosition = obj.transform.position;

		foreach (GameObject other in allObjects)
		{
			// Kendisiyle veya kendisinin solunda kalan nesnelerle ilgilenme
			if (other == obj || other.transform.position.x <= objPosition.x)
				continue;

			// Hücreler arasýndaki mesafeyi hesapla
			float distance = Vector3.Distance(objPosition, other.transform.position);
			if (distance < shortestDistance)
			{
				// Daha kýsa mesafe bulunursa en yakýn olaný güncelle
				shortestDistance = distance;
				nearest = other;
			}
		}

		// En yakýn saðdaki hücreyi döndür
		return nearest;
	}

	private GameObject FindNearestLeft(GameObject obj)
	{
		// Sahnedeki "Cell" tag'ine sahip tüm nesneleri al
		GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Cell");
		GameObject nearest = null;
		float shortestDistance = Mathf.Infinity;

		Vector3 objPosition = obj.transform.position;

		foreach (GameObject other in allObjects)
		{
			// Kendisiyle veya kendisinin saðýnda kalan nesnelerle ilgilenme
			if (other == obj || other.transform.position.x >= objPosition.x)
				continue;

			// Hücreler arasýndaki mesafeyi hesapla
			float distance = Vector3.Distance(objPosition, other.transform.position);
			if (distance < shortestDistance)
			{
				// Daha kýsa mesafe bulunursa en yakýn olaný güncelle
				shortestDistance = distance;
				nearest = other;
			}
		}

		// En yakýn soldaki hücreyi döndür
		return nearest;
	}

	private void CheckAdjacentColors(GameObject currentCell)
	{
		// Hücrenin Cell.cs scriptine eriþ
		Cell currentCellScript = currentCell.GetComponent<Cell>();
		if (currentCellScript == null) return;

		// Saðdaki hücreyi bul ve renk bilgisine eriþ
		GameObject rightCell = FindNearestRight(currentCell);
		Frog.Colors? rightColor = rightCell != null ? rightCell.GetComponent<Cell>()?.cellColor : null;

		// Soldaki hücreyi bul ve renk bilgisine eriþ
		GameObject leftCell = FindNearestLeft(currentCell);
		Frog.Colors? leftColor = leftCell != null ? leftCell.GetComponent<Cell>()?.cellColor : null;

		// Sað ve sol renkleri kontrol et
		if (rightColor.HasValue && leftColor.HasValue && rightColor == leftColor)
		{
			// Sað ve sol renkler aynýysa yeni bir item ekle
			currentCellScript.itemList.Add(new Cell.CellItem(rightColor.Value, SpawnType.Grape, DirectionOfSpawn.Up));
			return;
		}

		// Sað ve sol renkler ayný deðilse üst ve altý kontrol et

		// Yukarýdaki hücreyi bul ve renk bilgisine eriþ
		GameObject topCell = FindNearestTop(currentCell);
		Frog.Colors? topColor = topCell != null ? topCell.GetComponent<Cell>()?.cellColor : null;

		// Aþaðýdaki hücreyi bul ve renk bilgisine eriþ
		GameObject bottomCell = FindNearestBottom(currentCell);
		Frog.Colors? bottomColor = bottomCell != null ? bottomCell.GetComponent<Cell>()?.cellColor : null;

		// Üst ve alt renkleri kontrol et
		if (topColor.HasValue && bottomColor.HasValue && topColor == bottomColor)
		{
			// Üst ve alt renkler aynýysa yeni bir item ekle
			currentCellScript.itemList.Add(new Cell.CellItem(topColor.Value, SpawnType.Grape, DirectionOfSpawn.Up));
		}
	}

	private GameObject FindNearestTop(GameObject obj)
	{
		// "Cell" tag'ine sahip hücreleri bul
		GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Cell");
		GameObject nearest = null;
		float shortestDistance = Mathf.Infinity;

		Vector3 objPosition = obj.transform.position;

		foreach (GameObject other in allObjects)
		{
			// Kendisiyle veya kendisinin altýnda kalan nesnelerle ilgilenme
			if (other == obj || other.transform.position.z <= objPosition.z)
				continue;

			// Hücreler arasýndaki mesafeyi hesapla
			float distance = Vector3.Distance(objPosition, other.transform.position);
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				nearest = other;
			}
		}

		// En yakýn üstteki hücreyi döndür
		return nearest;
	}

	private GameObject FindNearestBottom(GameObject obj)
	{
		// "Cell" tag'ine sahip hücreleri bul
		GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Cell");
		GameObject nearest = null;
		float shortestDistance = Mathf.Infinity;

		Vector3 objPosition = obj.transform.position;

		foreach (GameObject other in allObjects)
		{
			// Kendisiyle veya kendisinin üstünde kalan nesnelerle ilgilenme
			if (other == obj || other.transform.position.z >= objPosition.z)
				continue;

			// Hücreler arasýndaki mesafeyi hesapla
			float distance = Vector3.Distance(objPosition, other.transform.position);
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				nearest = other;
			}
		}

		// En yakýn alttaki hücreyi döndür
		return nearest;
	}






	// Belirtilen pozisyondaki hücrenin rengini bulma
	private Frog.Colors? GetCellColorAtPosition(Vector3 position)
	{
		// Pozisyondaki tüm hücreleri tarar
		Collider[] colliders = Physics.OverlapSphere(position, 0.1f, LayerMask.GetMask("Cell"));

		foreach (Collider collider in colliders)
		{
			Cell cellScript = collider.GetComponent<Cell>();
			if (cellScript != null && cellScript.itemList.Count > 0)
			{
				return cellScript.itemList[0].Color; // Ýlk item'in rengini döndür
			}
		}

		return null; // Renk bulunamadý
	}


	private GameObject GetSelectedCellPrefab()
	{
		return selectedColor switch
		{
			Frog.Colors.Blue => CellBlue,
			Frog.Colors.Yellow => CellYellow,
			Frog.Colors.Green => CellGreen,
			Frog.Colors.Purple => CellPurple,
			Frog.Colors.Red => CellRed,
			_ => null,
		};
	}

	private GameObject GetSelectedItemPrefab()
	{
		return (selectedItem, selectedColor) switch
		{
			(Items.Frog, Frog.Colors.Blue) => FrogBlue,
			(Items.Frog, Frog.Colors.Yellow) => FrogYellow,
			(Items.Frog, Frog.Colors.Green) => FrogGreen,
			(Items.Frog, Frog.Colors.Purple) => FrogPurple,
			(Items.Frog, Frog.Colors.Red) => FrogRed,

			(Items.Grape, Frog.Colors.Blue) => GrapeBlue,
			(Items.Grape, Frog.Colors.Yellow) => GrapeYellow,
			(Items.Grape, Frog.Colors.Green) => GrapeGreen,
			(Items.Grape, Frog.Colors.Purple) => GrapePurple,
			(Items.Grape, Frog.Colors.Red) => GrapeRed,

			(Items.Arrow, Frog.Colors.Blue) => ArrowBlue,
			(Items.Arrow, Frog.Colors.Yellow) => ArrowYellow,
			(Items.Arrow, Frog.Colors.Green) => ArrowGreen,
			(Items.Arrow, Frog.Colors.Purple) => ArrowPurple,
			(Items.Arrow, Frog.Colors.Red) => ArrowRed,

			_ => null,
		};
	}



	private void ClearChildren()
	{
		var children = new System.Collections.Generic.List<Transform>();
		foreach (Transform child in transform)
		{
			children.Add(child);
		}

		foreach (Transform child in children)
		{
			DestroyImmediate(child.gameObject);
		}
	}

	private void InitializeGrid()
	{
		baseCellLookup = new Dictionary<string, GameObject>();

		for (int y = 0; y < RowCount; y++)
		{
			for (int x = 0; x < ColumnCount; x++)
			{
				GameObject cellObj = PrefabUtility.InstantiatePrefab(BaseCell, transform) as GameObject;
				string cellName = $"BaseCell [{x},{y}]";
				cellObj.name = cellName;
				cellObj.transform.localPosition = new Vector3(x, 0, -y);

				baseCellLookup[cellName] = cellObj;
			}
		}
	}

	private GameObject GetBaseCellByName(int x, int y)
	{
		string cellName = $"BaseCell [{x},{y}]";
		if (baseCellLookup.TryGetValue(cellName, out GameObject baseCell))
		{
			return baseCell;
		}
		else
		{
			Debug.LogError($"BaseCell with name {cellName} not found!");
			return null;
		}
	}

	private void SpawnWithBaseCellCheck(int x, int y, GameObject cellPrefab, GameObject frogPrefab, GameObject grapePrefab, Frog.Colors selectedColor, bool isRow, bool spawnAtStart)
	{
		GameObject baseCellObject = GetBaseCellByName(x, y);
		if (baseCellObject != null)
		{
			BaseCell baseCellScript = baseCellObject.GetComponent<BaseCell>();
			if (baseCellScript != null)
			{
				if (baseCellScript.cell == null) // BaseCell boþ mu?
				{
					// Cell spawn et ve BaseCell'e ata
					GameObject spawnedCell = PrefabUtility.InstantiatePrefab(cellPrefab, transform) as GameObject;
					spawnedCell.transform.localPosition = baseCellObject.transform.localPosition + Vector3.up * 0.1f;
					spawnedCell.GetComponent<Cell>().cellColor = selectedColor;
					Cell newCell = spawnedCell.GetComponent<Cell>();
					baseCellScript.cell = newCell; // BaseCell'e yeni hücreyi ata
					

					// Frog spawn et (sadece baþlangýç veya bitiþ pozisyonlarýnda)
					if (frogPrefab != null && !baseCellScript.hasFrog)
					{
						GameObject spawnedFrog = PrefabUtility.InstantiatePrefab(frogPrefab, transform) as GameObject;
						spawnedFrog.transform.localPosition = baseCellObject.transform.localPosition + Vector3.up * 0.2f;

						// Frog yönü belirle (satýr veya sütuna göre)
						if (isRow)
						{
							spawnedFrog.transform.rotation = spawnAtStart ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);
						}
						else
						{
							spawnedFrog.transform.rotation = spawnAtStart ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
						}

						baseCellScript.hasFrog = true; // BaseCell'e kurbaða atandý
					//	Debug.Log($"Spawned Frog at BaseCell [{x},{y}] with correct rotation.");
					}

					// Grape spawn et
					if (grapePrefab != null)
					{
						GameObject spawnedGrape = PrefabUtility.InstantiatePrefab(grapePrefab, transform) as GameObject;
						spawnedGrape.transform.localPosition = baseCellObject.transform.localPosition + Vector3.up * 0.38f;
						// Debug.Log($"Spawned Grape at BaseCell [{x},{y}].");
					}
				}
				else
				{
					Cell.CellItem newItem = new Cell.CellItem((Frog.Colors)selectedColor, (Cell.SpawnType)selectedItem, (Cell.DirectionOfSpawn)selectedDirection);

					// Eðer itemList içinde ayný renk ve türde bir eleman yoksa ekle
					if (!baseCellScript.cell.itemList.Exists(item => item.Color == selectedColor && item.Type == (Cell.SpawnType)selectedItem))
					{
					//	Debug.Log($"BaseCell at [{x},{y}] is full. Adding color {selectedColor} and type {selectedItem} to itemList.");
						baseCellScript.cell.itemList.Add(newItem);
					}
					else
					{
				//		Debug.Log($"Color {selectedColor} and type {selectedItem} is already in itemList at BaseCell [{x},{y}].");
					}

				}
			}
		}
	}

	private void SpawnRandomColorColumn()
	{
		GameObject[] cellPrefabs = { CellBlue, CellYellow, CellGreen, CellPurple, CellRed };
		GameObject[] frogPrefabs = { FrogBlue, FrogYellow, FrogGreen, FrogPurple, FrogRed };
		GameObject[] grapePrefabs = { GrapeBlue, GrapeYellow, GrapeGreen, GrapePurple, GrapeRed };
		Frog.Colors[] frogColors = { Frog.Colors.Blue, Frog.Colors.Yellow, Frog.Colors.Green, Frog.Colors.Purple, Frog.Colors.Red };

		int randomIndex = Random.Range(0, cellPrefabs.Length);

		GameObject selectedCell = cellPrefabs[randomIndex];
		GameObject selectedFrog = frogPrefabs[randomIndex];
		GameObject selectedGrape = grapePrefabs[randomIndex];
		Frog.Colors selectedColor = frogColors[randomIndex];

		List<int> availableColumns = new List<int>();
		for (int column = 0; column < ColumnCount; column++)
		{
			bool isColumnFull = true;
			for (int row = 0; row < RowCount; row++)
			{
				GameObject baseCellObject = GetBaseCellByName(column, row);
				if (baseCellObject != null)
				{
					BaseCell baseCellScript = baseCellObject.GetComponent<BaseCell>();
					if (baseCellScript != null && baseCellScript.cell == null)
					{
						isColumnFull = false;
						break;
					}
				}
			}
			if (!isColumnFull)
			{
				availableColumns.Add(column);
			}
		}

		if (availableColumns.Count == 0)
		{
			Debug.Log("All columns are full. Cannot spawn.");
			return;
		}

		int randomColumn = availableColumns[Random.Range(0, availableColumns.Count)];

		bool spawnTop = Random.value > 0.5f;
		int frogRow = spawnTop ? 0 : RowCount - 1;

		SpawnWithBaseCellCheck(randomColumn, frogRow, selectedCell, selectedFrog, null, selectedColor, false, spawnTop);

		for (int y = 0; y < RowCount; y++)
		{
			if (y == frogRow) continue;

			SpawnWithBaseCellCheck(randomColumn, y, selectedCell, null, selectedGrape, selectedColor, false, false);
		}
	}

	private void SpawnRandomColorRow()
	{
		GameObject[] cellPrefabs = { CellBlue, CellYellow, CellGreen, CellPurple, CellRed };
		GameObject[] frogPrefabs = { FrogBlue, FrogYellow, FrogGreen, FrogPurple, FrogRed };
		GameObject[] grapePrefabs = { GrapeBlue, GrapeYellow, GrapeGreen, GrapePurple, GrapeRed };
		Frog.Colors[] frogColors = { Frog.Colors.Blue, Frog.Colors.Yellow, Frog.Colors.Green, Frog.Colors.Purple, Frog.Colors.Red };

		int randomIndex = Random.Range(0, cellPrefabs.Length);

		GameObject selectedCell = cellPrefabs[randomIndex];
		GameObject selectedFrog = frogPrefabs[randomIndex];
		GameObject selectedGrape = grapePrefabs[randomIndex];
		Frog.Colors selectedColor = frogColors[randomIndex];

		List<int> availableRows = new List<int>();
		for (int row = 0; row < RowCount; row++)
		{
			bool isRowFull = true;
			for (int column = 0; column < ColumnCount; column++)
			{
				GameObject baseCellObject = GetBaseCellByName(column, row);
				if (baseCellObject != null)
				{
					BaseCell baseCellScript = baseCellObject.GetComponent<BaseCell>();
					if (baseCellScript != null && baseCellScript.cell == null)
					{
						isRowFull = false;
						break;
					}
				}
			}
			if (!isRowFull)
			{
				availableRows.Add(row);
			}
		}

		if (availableRows.Count == 0)
		{
			Debug.Log("All rows are full. Cannot spawn.");
			return;
		}

		int randomRow = availableRows[Random.Range(0, availableRows.Count)];

		bool spawnLeft = Random.value > 0.5f;
		int frogColumn = spawnLeft ? 0 : ColumnCount - 1;

		SpawnWithBaseCellCheck(frogColumn, randomRow, selectedCell, selectedFrog, null, selectedColor, true, spawnLeft);

		for (int x = 0; x < ColumnCount; x++)
		{
			if (x == frogColumn) continue;

			SpawnWithBaseCellCheck(x, randomRow, selectedCell, null, selectedGrape, selectedColor, true, false);
		}
	}

	[Button("Generate Level")]
	private void GenerateLevel()
	{
		ClearChildren();
		InitializeGrid();
		if (Random.Range(0, 2) == 0)
			for (int x = 0; x < RowCount; x++)
			{

				SpawnRandomColorRow();


			}
		else
		{
			for (int x = 0; x < ColumnCount; x++)
			{

				SpawnRandomColorColumn();


			}
		}

	}

	[Button("Clear")]
	public void Clear()
	{
		ClearChildren();
	}
}
#endif