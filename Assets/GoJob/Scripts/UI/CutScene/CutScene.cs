using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CutSceneParameter
{
	public Image CutImage;
	public float FadeInTime;
	public float IdleTime;
	public float FadeOutTime;
}


public class CutScene : MonoBehaviour
{
	[SerializeField] private CutSceneParameter[] cut;
	[SerializeField] private string sceneName;

	private void Awake()
	{
		for(int i = 0; i < cut.Length; i++)
		{
			cut[i].CutImage.color = new Color(1, 1, 1, 0);
		}


		StartCoroutine(CutSceneCoroutine());
	}


	private void Update()
	{
		
	}


	private IEnumerator CutSceneCoroutine()
	{
		float time = 0;

		for (int i = 0; i < cut.Length; i++)
		{
			// ���̵� ��
			while(true)
			{
				time += Time.deltaTime;

				if (time >= cut[i].FadeInTime)
				{
					cut[i].CutImage.color = new Color(1, 1, 1, 1);
					break;
				}

				cut[i].CutImage.color = new Color(1, 1, 1, time / cut[i].FadeInTime);

				yield return null;
			}

			time = 0;

			// ��� �ð�
			while (true)
			{
				time += Time.deltaTime;

				if(time >= cut[i].IdleTime)
				{
					break;
				}
				
				yield return null;
			}

			time = cut[i].FadeOutTime;
			// ���̵� �ƿ�
			while (true)
			{
				time -= Time.deltaTime;
				if (time <= 0)
				{
					cut[i].CutImage.color = new Color(1, 1, 1, 0);
					break;
				}

				cut[i].CutImage.color = new Color(1, 1, 1, time / cut[i].FadeOutTime);

				yield return null;
			}


			cut[i].CutImage.gameObject.SetActive(false);
		}



		SceneManager.LoadScene(sceneName);
	}
} 
