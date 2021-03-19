using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public static float speed = 10;

	bool score;

	private void OnEnable()
	{
		score = false;
	}

	void Update()
	{
		if (Player.isDead)
		{
			gameObject.SetActive(false);
			return;
		}

		transform.Translate(Vector2.left * speed * Time.deltaTime);
		Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

		if (!score && pos.x < 0.5f && !Player.isDead)
		{
			score = true;
			Player.Score++;
		}

		if (pos.x < -0.2f)
		{
			gameObject.SetActive(false);
			score = false;
		}
	}
}
