using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SausageStage : MonoBehaviour
{

	public void StageMove(string name)
	{
		SceneManager.LoadScene(name);
	}
}
