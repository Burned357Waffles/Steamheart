using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResponseJoinEventArgs : ExtendedEventArgs
{
	public short status { get; set; } // 0 = success
	public int user_id { get; set; } // 1 is first to join, 2 is second, anything else is not valid!
	public string username { get; set; }

	public string errorMessage;

	public ResponseJoinEventArgs()
	{
		event_id = Constants.SMSG_JOIN;
	}
	
}

public class ResponseJoin : NetworkResponse
{
	private short status;
	private int user_id;
	private string username;
	private string errorMessage;
	public ResponseJoin()
	{
	}

	public override void parse()
	{
		status = DataReader.ReadShort(dataStream);
		if (status == 0)
		{
			user_id = DataReader.ReadInt(dataStream);
			username = DataReader.ReadString(dataStream);
		}
		else
		{
			errorMessage = DataReader.ReadString(dataStream);
		}
	}

	public override ExtendedEventArgs process()
	{
		ResponseJoinEventArgs args = new ResponseJoinEventArgs
		{
			status = status
		};
		if (status == 0)
		{
			args.user_id = user_id;
			args.username = username;
		}
		else
		{
			args.errorMessage = errorMessage;
		}

		return args;
	}
}
