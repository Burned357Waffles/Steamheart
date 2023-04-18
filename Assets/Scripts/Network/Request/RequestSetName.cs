using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestSetName : NetworkRequest
{
	public RequestSetName()
	{
		request_id = Constants.CMSG_SETNAME;
	}

	public void send(string name)
	{
		packet = new GamePacket(request_id);
		packet.addString(name);
	}
}
