using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Loading : MonoBehaviour
{
	public TMP_InputField nickname;

	public static void LoadScene(string sceneName)
	{
		SceneManager.LoadSceneAsync(sceneName);
	}

	public void OnSceneButton(string sceneName)
	{
		LoadScene(sceneName);
	}
	public void OnQuitButton()
	{
		Application.Quit();
	}

	public void NewGame()
	{
		string nick = nickname.text;
		if (nick.Length < 1)
		{
			//TODO: show error;
			return;
		}

		Player.NickName = nick;
		Hiscore.AddPlayer(nick);
		LoadScene("SampleScene");

	}
}
