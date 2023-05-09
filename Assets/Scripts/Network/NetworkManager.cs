using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Network
{
	public class NetworkManager : MonoBehaviour
	{
		private ConnectionManager cManager;
		private TMP_InputField _usernameInput;
		private GameObject _networkErrorMessage;

		void Awake()
		{
			DontDestroyOnLoad(gameObject);

			gameObject.AddComponent<MessageQueue>();
			gameObject.AddComponent<ConnectionManager>();

			_usernameInput = GameObject.Find("Name Input").GetComponent<TMP_InputField>();
			_networkErrorMessage = GameObject.Find("Network Error");
			_networkErrorMessage.SetActive(false);

			NetworkRequestTable.init();
			NetworkResponseTable.init();
		}

		// Start is called before the first frame update
		void Start()
		{
			cManager = GetComponent<ConnectionManager>();

			if (cManager)
			{
				try
				{
					cManager.setupSocket();

					StartCoroutine(RequestHeartbeat(0.1f));
				}
				catch (Exception e)
				{
					_networkErrorMessage.SetActive(true);
					Debug.Log("Socket error: " + e);
				}
			}
		}

		public void SendJoinRequest()
		{
			if (cManager && cManager.IsConnected())
			{
				string username = _usernameInput.text;
				RequestJoin request = new RequestJoin();
				request.send(username);
				cManager.send(request);
			}
			
		}

		public void SendLeaveRequest()
		{
			if (cManager && cManager.IsConnected())
			{
				RequestLeave request = new RequestLeave();
				request.send();
				cManager.send(request);
			}
		}

		// public bool SendSetNameRequest(string Name)
		// {
		// 	if (cManager && cManager.IsConnected())
		// 	{
		// 		RequestSetName request = new RequestSetName();
		// 		request.send(Name);
		// 		cManager.send(request);
		// 		return true;
		// 	}
		// 	return false;
		// }
		//
		// public bool SendReadyRequest()
		// {
		// 	if (cManager && cManager.IsConnected())
		// 	{
		// 		RequestReady request = new RequestReady();
		// 		request.send();
		// 		cManager.send(request);
		// 		return true;
		// 	}
		// 	return false;
		// }
		//
		// public bool SendMoveRequest(int pieceIndex, int x, int y)
		// {
		// 	if (cManager && cManager.IsConnected())
		// 	{
		// 		RequestMove request = new RequestMove();
		// 		request.send(pieceIndex, x, y);
		// 		cManager.send(request);
		// 		return true;
		// 	}
		// 	return false;
		// }
		//
		// public bool SendInteractRequest(int pieceIndex, int targetIndex)
		// {
		// 	if (cManager && cManager.IsConnected())
		// 	{
		// 		RequestInteract request = new RequestInteract();
		// 		request.send(pieceIndex, targetIndex);
		// 		cManager.send(request);
		// 		return true;
		// 	}
		// 	return false;
		// }

		public IEnumerator RequestHeartbeat(float time)
		{
			yield return new WaitForSeconds(time);

			if (cManager)
			{
				RequestHeartbeat request = new RequestHeartbeat();
				request.send();
				cManager.send(request);
			}

			StartCoroutine(RequestHeartbeat(time));
		}
	}
}
