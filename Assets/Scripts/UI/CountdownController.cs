using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class CountdownController : MonoBehaviour
{

	public GameObject parentCanvas;
	public AudioClip boop;
	public AudioMixerGroup mixerGroup;
	Text text;
	GameGUI gameGUI;

	void Start()
	{
		text = GetComponent<Text>();
		gameGUI = GameObject.FindGameObjectWithTag("GameGUI").GetComponent<GameGUI>();
	}

	public void SetFinished()
	{
		gameGUI.StartGame();
		parentCanvas.SetActive(false);
	}

	public void SetNumber(int number)
	{
		text.text = number.ToString();
		SoundEffectManager.Instance.PlayClipOnce(boop, mixerGroup, Vector3.zero, 1, 1);
	}

}

