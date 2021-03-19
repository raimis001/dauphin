using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
	public static string NickName = "dummy";
	public static int Score = 0;
	public static int Death = 0;
	public static bool isDead = false;

	public float jumpForce = 20;
	public LayerMask groundLayer;
	public LayerMask obstacleLayer;

	public Transform body;
	public Transform ball;

	public TMP_Text scoreText;
	public TMP_Text deathText;
	public TMP_Text distanceText;
	public TMP_Text bestText;

	public RectTransform pretender;
	public RectTransform king;

	public GameObject restartButton;
	public GameObject resumeButton;

	public GameObject pausePanel;

	public AudioSource jumpSound;
	public AudioSource jumpEndSound;
	public AudioSource shotSound;
	public AudioSource shrinkSound;
	public AudioSource deathSound;
	public AudioSource explodeSound;

	public GameObject explodeEffect;

	float distance;
	float bestDistance;
	float lastDistance = 0;
	float oldDistance;

	float downTime = 0;
	float shotTime = 0;
	bool isJump = false;

	Rigidbody2D rigi;

	void Start()
	{
		//PlayerPrefs.SetFloat("BestDistance", 100);
		//PlayerPrefs.Save();

		rigi = GetComponent<Rigidbody2D>();

		bestDistance = PlayerPrefs.GetFloat(NickName, 100);
		bestText.text = bestDistance.ToString("0.0");
		oldDistance = bestDistance;

		Obstacle.speed = 10;
		isDead = false;
		restartButton.SetActive(false);
		pausePanel.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			pausePanel.SetActive(!pausePanel.activeInHierarchy);
		}

		if (pausePanel.activeInHierarchy)
		{
			Time.timeScale = 0;
			resumeButton.SetActive(true);
		}
		else
		{
			Time.timeScale = 1;
		}

		distance += Obstacle.speed * Time.deltaTime;
		distanceText.text = distance.ToString("0.0");

		if (distance > lastDistance + 100)
		{
			Obstacle.speed *= 1.1f;
			lastDistance = distance;
		}

		if (distance < bestDistance)
		{
			king.sizeDelta = new Vector2(120, 300);
			Vector2 size = new Vector2(120, 0);
			size.y = (distance / bestDistance) * 300;
			pretender.sizeDelta = size;
		}
		else
		{
			pretender.sizeDelta = new Vector2(120, 300);
			Vector2 size = new Vector2(120, 0);
			size.y = (bestDistance / distance) * 300;
			king.sizeDelta = size;

			PlayerPrefs.SetFloat(NickName, distance);
			PlayerPrefs.Save();
			
		}

		scoreText.text = Score.ToString();

		if (downTime > 0)
		{
			downTime -= Time.deltaTime;
			if (downTime <= 0)
			{
				body.localScale = new Vector3(1, 1, 1);
				downTime = 0;
			}
		}

		if (shotTime > 0)
		{
			shotTime -= Time.deltaTime;
			ball.Translate(Time.deltaTime * 15f, 0, 0);
			if (shotTime <= 0)
			{
				ball.gameObject.SetActive(false);
			}
			return;
		}

		RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.8f, Vector2.down, 0.2f, groundLayer);
		if (hit.collider == null)
		{
			isJump = true;
			return;
		}

		if (isJump)
		{
			isJump = false;
			jumpEndSound.Play();
		}

		if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
		{
			if (downTime > 0)
				return;
			if (shotTime > 0)
				return;

			shotTime = 1f;
			ball.localPosition = Vector3.zero;
			ball.gameObject.SetActive(true);
			shotSound.Play();
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.S))
		{
			body.localScale = new Vector3(1, 0.45f, 1);
			downTime = 1f;
			shrinkSound.Play();
		}

		if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.S))
		{
			body.localScale = new Vector3(1, 1, 1);
			downTime = 0;
		}

		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
		{
			if (body.localScale.y < 1)
				return;

			rigi.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			jumpSound.Play();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer != 12)
			return;


		if (collision.otherCollider.name == "Ball")
		{
			if (collision.gameObject.tag == "Shot")
			{
				ball.gameObject.SetActive(false);
				shotTime = 0;

				explodeEffect.transform.position = collision.transform.position;
				explodeEffect.SetActive(true);
				Invoke(nameof(ExplodeEnd), 1.5f);

				collision.gameObject.SetActive(false);
				explodeSound.Play();
			}
			return;
		}

		deathSound.Play();
		gameObject.SetActive(false);
		Death++;
		deathText.text = Death.ToString();
		//restartButton.SetActive(true);
		pausePanel.SetActive(true);
		resumeButton.SetActive(false);
		Time.timeScale = 0;
		isDead = true;
		if (bestDistance < distance)
		{
			oldDistance = bestDistance;
			bestDistance = distance;
			bestText.text = bestDistance.ToString("0.0");
		}

	}

	void ExplodeEnd()
	{
		explodeEffect.SetActive(false);
	}

	public void Restart()
	{
		Obstacle.speed = 10;
		distance = 0;
		isDead = false;
		pausePanel.SetActive(false);
		Time.timeScale = 1;
		gameObject.SetActive(true);
		restartButton.SetActive(false);
	}

}
