using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestJoin : NetworkRequest
{
	public RequestJoin()
	{
		request_id = Constants.CMSG_JOIN;
	}

	public void send()
	{
		packet = new GamePacket(request_id);
	}
}
