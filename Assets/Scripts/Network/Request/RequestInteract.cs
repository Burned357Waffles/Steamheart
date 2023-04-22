using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestInteract : NetworkRequest
{
	public RequestInteract()
	{
		request_id = Constants.CMSG_INTERACT;
	}

	public void send(int pieceIndex, int targetIndex)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(pieceIndex);
		packet.addInt32(targetIndex);
	}
}
