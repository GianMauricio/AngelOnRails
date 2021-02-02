using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChanger : MonoBehaviour {

	public List<Texture> textures;

	Material m;
	float t;
	float changeTime = 0.3f;

	float t2;
	float changeTime2 = 0.3f;

	int rindex = 0;
	int rindex2 = 0;

	// Use this for initialization
	void Start () {
		m = GetComponent<Renderer> ().material;
		changeTime = Random.Range (0.4f, 0.6f);
		changeTime2 = Random.Range (0.4f, 0.6f);
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		if (t>changeTime) {
			t = 0;
			changeTime = Random.Range (0.3f, 0.8f);
			rindex = Random.Range (0, textures.Count);
			for (int i = 0; i < 1000; i++) {
				rindex = Random.Range (0, textures.Count);
				if (rindex != rindex2) {
					break;
				}
			}
			m.SetTexture ("_Drops", textures [ rindex ] );
			m.SetTextureOffset ("_Drops", Random.insideUnitCircle);
		}

		t2 += Time.deltaTime;
		if (t2>changeTime2) {
			t2 = 0;
			changeTime2 = Random.Range (0.3f, 0.8f);
			for (int i = 0; i < 1000; i++) {
				rindex2 = Random.Range (0, textures.Count);
				if (rindex != rindex2) {
					break;
				}
			}

			m.SetTexture ("_Drops2", textures [ rindex2 ] );
			m.SetTextureOffset ("_Drops2", Random.insideUnitCircle);
		}

	}
}
