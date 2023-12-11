using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
	

	public void StageSelect(string name)
	{
		SceneManager.LoadScene(name);
	}
}
