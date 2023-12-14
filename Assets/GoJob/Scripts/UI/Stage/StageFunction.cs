using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StageFunction : MonoBehaviour
{
	[Header("목표 지점")]
	[SerializeField] private GameObject playerCamera;
	[SerializeField] private GameObject goalCamera;
	[SerializeField] private GameObject goalUI;
	[SerializeField] private GameObject goalObject;

	[Space(10)]
	[Header("카운트다운")]
	[SerializeField] private GameObject countDownUI;
	[SerializeField] private Image countDownImage;
	[SerializeField] private Sprite[] numberSprite;

	[Space(10)]
	[Header("일시정지")]
	[SerializeField] private GameObject pauseUI;

	[Space(10)]
	[Header("게임 클리어")]
	[SerializeField] private GameObject flageObject;
	[SerializeField] private GameObject resultObject;
	[SerializeField] private float autoTime;
	[SerializeField] private bool autoNextStage;
	[SerializeField] private string nextStage;


	[Space(10)]
	[SerializeField] private TextMeshProUGUI[] textUIs;

	private bool uIOn;

	private Vector3 goalCameraDefaultPosition;
	private Vector3 goalPosition;


	private bool isStart;
	public bool UIOn
	{
		set => uIOn = value;
		get => uIOn;
	}

	private void Awake()
	{
		flageObject.SetActive(false);
		goalCameraDefaultPosition = goalCamera.transform.position;
		goalPosition = goalObject.transform.position + new Vector3(0, 2f, -3f);
		CameraView();


	}

	private void Update()
	{

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(uIOn)
			{
				PauseOff();
			}
			else
			{
				PauseOn();
			}
		}
	}


	public void StageMove(string name)
	{
		SceneManager.LoadScene(name);
	}

	public void CameraView()
	{
		goalCamera.SetActive(true);
		playerCamera.SetActive(false);

		Goalview();
	}

	public void Goalview()
	{
		StartCoroutine("GoalViewCoroutine");
	}


	public IEnumerator GoalViewCoroutine()
	{
		yield return new WaitForSeconds(1f);

		while(true)
		{
			goalCamera.transform.position = Vector3.Lerp(goalCamera.transform.position, goalPosition, 5f * Time.deltaTime);
			yield return null;

			if(Vector3.Distance(goalCamera.transform.position, goalPosition) <= 0.1f)
			{
				break;
			}
		}

		yield return new WaitForSeconds(2f);
		goalUI.SetActive(true);
		yield return new WaitForSeconds(3f);
		goalUI.SetActive(false);

		while (true)
		{
			goalCamera.transform.position = Vector3.Lerp(goalCamera.transform.position, goalCameraDefaultPosition, 5f * Time.deltaTime);
			yield return null;

			if (Vector3.Distance(goalCamera.transform.position, goalCameraDefaultPosition) <= 0.1f)
			{
				break;
			}
		}
		goalCamera.SetActive(false);
		playerCamera.SetActive(true);
		StartCountDown();
	}

	public void StartCountDown()
	{
		StartCoroutine(StartCountDownCoroutine());

	}

	public void PauseOn()
	{
		if(!isStart)
		{
			return;
		}

		isStart = false;
		uIOn = true;
		GameManager.Instance.GameSpeed = 0;
		pauseUI.SetActive(true);
	}

	public void PauseOff()
	{
		uIOn = false;
		pauseUI.SetActive(false);
		StartCountDown();
	}


	public void UIOff()
	{
		for(int i = 0; i < textUIs.Length; i++)
		{
			textUIs[i].gameObject.SetActive(false);
		}
	}


	private IEnumerator StartCountDownCoroutine()
	{
		// 3부터 시작
		countDownImage.sprite = numberSprite[0];
		countDownImage.rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		countDownUI.SetActive(true);

		// 크기 줄어들기
		for(int i = 0; i < numberSprite.Length; i++)
		{
			float size = 0.8f;
			countDownImage.rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			countDownImage.sprite = numberSprite[i];
			while (true)
			{
				size -= Time.deltaTime;

				if (size <= 0.5f)
				{
					size = 0.5f;
					countDownImage.rectTransform.localScale = new Vector3(size, size, size);
					break;
				}

				countDownImage.rectTransform.localScale = new Vector3(size, size, size);
				yield return null;
			}


			// 딜레이
			float time = 0;
			while(true)
			{
				time += Time.deltaTime;

				if(time >= 0.7f)
				{
					break;
				}
				yield return null;
			}

		}

		countDownUI.SetActive(false);
		GameManager.Instance.GameSpeed = 1;
		isStart = true;
		yield return null;
	}

	


	public void FlagsOn()
	{
		GameManager.Instance.GameSpeed = 0;
		flageObject.transform.position = goalObject.transform.position + new Vector3(0, 3f, 0);
		flageObject.SetActive(true);
		goalCamera.transform.position = flageObject.transform.position + new Vector3(0, 1f, -3f);
		goalCamera.transform.eulerAngles = new Vector3(0, 0, 0);


		StartCoroutine("FlagesCoroutine");
		
	}


	private IEnumerator FlagesCoroutine()
	{
		goalCamera.SetActive(true);

		float flagsAngle = 0;

		/*while(true)
		{
			flagsAngle += Time.deltaTime * 300f;
			if (flagsAngle > 360f)
			{
				flageObject.transform.eulerAngles = new Vector3(0, 0, 0);
				break;
			}
			flageObject.transform.eulerAngles = new Vector3(0, flagsAngle, 0);


			yield return null;
		}*/

		yield return new WaitForSeconds(1f);


		while (true)
		{
			flageObject.transform.position -= Vector3.up * Time.deltaTime * 4f;
			goalCamera.transform.position = flageObject.transform.position + new Vector3(0, 1f, -3f);

			if (flageObject.transform.position.y < 0)
			{
				flageObject.transform.position = new Vector3(goalObject.transform.position.x, 0f, goalObject.transform.position.z);
				goalCamera.transform.position = flageObject.transform.position + new Vector3(0, 1f, -3f);
				break;
			}

			yield return null;
		}


		resultObject.SetActive(true);


		float time = 0;

		while (true)
		{
			time += Time.deltaTime;
			if (autoNextStage)
			{
				if(time >= autoTime)
				{
					SceneManager.LoadScene(nextStage);
					break;
				}
			}
			else
			{
				if(Input.GetMouseButtonDown(0))
				{
					SceneManager.LoadScene(nextStage);
					break;
				}
			}

			yield return null;
		}
		
	}
	
}
