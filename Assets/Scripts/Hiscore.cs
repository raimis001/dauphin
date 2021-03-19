using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hiscore : MonoBehaviour
{

	static Hiscore instance;
	static Dictionary<string, float> playerList = new Dictionary<string, float>();

	public Transform lineParent;
	public LineScore lineScore;

	private void Awake()
	{
		instance = this;

		string pList = PlayerPrefs.GetString("PlayerList");
		if (pList.Length == 0)
			return;

		string[] nameArray = pList.Split(';');

		foreach (string name in nameArray)
		{
			if (name == "")
				continue;

			float result = PlayerPrefs.GetFloat(name, 100);

			if (!playerList.ContainsKey(name))
			{
				playerList.Add(name, result);
			}
			else
			{
				playerList[name] = result;
			}
		}

	}

	public static void AddPlayer(string name)
	{
		if (playerList.ContainsKey(name))
			return;

		playerList.Add(name, 100);
		string pList = PlayerPrefs.GetString("PlayerList");
		pList = pList + name + ";";
		PlayerPrefs.SetString("PlayerList", pList);
		PlayerPrefs.Save();

		LineScore line = Instantiate(instance.lineScore, instance.lineParent);
		line.caption.text = name;
		line.score.text = "100.0";
		line.gameObject.SetActive(true);
	}

	private void Start()
	{

		foreach (KeyValuePair<string, float> data in playerList.OrderByDescending(key => key.Value))
		{
			LineScore line = Instantiate(lineScore, lineParent);
			line.caption.text = data.Key;
			line.score.text = data.Value.ToString("0.0");
			line.gameObject.SetActive(true);
		}
	}

	public void ClearScores()
	{
		PlayerPrefs.DeleteAll();
		for (int i = 1; i < lineParent.childCount; i++)
		{
			Destroy(lineParent.GetChild(i).gameObject, 0.01f);
		}
	}

}
