using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestReady : NetworkRequest
{
	public RequestReady()
	{
		request_id = Constants.CMSG_READY;
	}

	public void send()
	{
		packet = new GamePacket(request_id);
	}
}
