using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	
	#pragma warning disable 78
	
	public GameObject dirtPrefab; 
	public GameObject grassPrefab; 
	
	public int minX = -256;
	public int maxX = 256;
	public int minY = -13;
	public int maxY = 2;
	
	PerlinNoise noise;
	
	void Start () {
		noise = new PerlinNoise(Random.Range(1000000,10000000));
		Regenerate();
	}
	
	public void Regenerate(){
		
		float width = dirtPrefab.transform.lossyScale.x;
		float height = dirtPrefab.transform.lossyScale.y;
		
		for (int i = minX; i < maxX; i++){//columns (x values
			int columnHeight = 1 + noise.getNoise(i - minX, maxY - minY - 1);
			for(int j = minY; j < minY + columnHeight; j++){//rows (y values)
				GameObject block = (j == minY + columnHeight - 1)?grassPrefab:dirtPrefab;
				Instantiate(block, new Vector2(i * width, j * height), Quaternion.identity);
			}
		}
	}
}
