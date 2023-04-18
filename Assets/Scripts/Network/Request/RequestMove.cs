using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestMove : NetworkRequest
{
	public RequestMove()
	{
		request_id = Constants.CMSG_MOVE;
	}

	public void send(int pieceIndex, int x, int y)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(pieceIndex);
		packet.addInt32(x);
		packet.addInt32(y);
	}
}
