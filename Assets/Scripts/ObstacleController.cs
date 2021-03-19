using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
	public Transform[] obstacles;

	float currentTime = 0;

	void Start()
	{
		foreach (Transform t in obstacles)
		{
			t.gameObject.SetActive(false);
		}
	}

	void Update()
	{
		if (Player.isDead)
			return;

		if (currentTime <= 0)
		{
			Spawn();
			currentTime = Random.Range(3, 4) / (Obstacle.speed / 10);
			return;
		}

		currentTime -= Time.deltaTime;
	}

	void Spawn()
	{
		List<Transform> open = new List<Transform>();

		foreach (Transform t in obstacles)
		{
			if (t.gameObject.activeInHierarchy)
				continue;

			open.Add(t);
		}

		if (open.Count < 1)
			return;

		Transform tt = open[Random.Range(0, open.Count)];
	
		Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(1.2f, 0));
		Vector2 opos = tt.position;
		opos.x = pos.x;
		tt.position = opos;

		tt.gameObject.SetActive(true);
	}
}
