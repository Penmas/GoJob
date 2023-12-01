using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	[SerializeField] private PlayerController playerContr;
	[SerializeField] private float cameraMoveSpeed;


	private Vector3 cameraPositionOffset;                           //ī�޶� ���� ��ġ
	Vector3 playerPos;
	private void Awake()
	{
		playerContr = FindObjectOfType<PlayerController>();
		cameraPositionOffset = transform.position;
	}

	private void Update()
	{
		CameraMove();
	}


	private void CameraMove()
	{
		playerPos = playerContr.gameObject.transform.position + cameraPositionOffset;

		gameObject.transform.position = Vector3.Lerp(transform.position, playerPos, cameraMoveSpeed * Time.deltaTime);
	}
}
