using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseReadyEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request

	public ResponseReadyEventArgs()
	{
		event_id = Constants.SMSG_READY;
	}
}

public class ResponseReady : NetworkResponse
{
	private int user_id;

	public ResponseReady()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseReadyEventArgs args = new ResponseReadyEventArgs
		{
			user_id = user_id
		};

		return args;
	}
}
