using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

	public SpriteRenderer sprite;
	public float speed = 0.1f;

	Vector2 offset = Vector2.zero;
	// Start is called before the first frame update
	void Start()
	{
		sprite.material.renderQueue = 0;
	}

	// Update is called once per frame
	void Update()
	{
		offset.x += Time.deltaTime * speed;
		Debug.Log(offset);
		sprite.material.mainTextureOffset = offset;
		//sprite.material.SetTextureOffset("_MainTex", offset);


	}


}
