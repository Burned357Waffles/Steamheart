using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseInteractEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public int piece_idx { get; set; } // The index of the piece that is initiating the interaction. Belongs to player with id user_id
	public int target_idx { get; set; } // The index of the piece that is being interacted with. Belongs to the opponent of the player with id user_id

	public ResponseInteractEventArgs()
	{
		event_id = Constants.SMSG_INTERACT;
	}
}

public class ResponseInteract : NetworkResponse
{
	private int user_id;
	private int piece_idx;
	private int target_idx;

	public ResponseInteract()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		piece_idx = DataReader.ReadInt(dataStream);
		target_idx = DataReader.ReadInt(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseInteractEventArgs args = new ResponseInteractEventArgs
		{
			user_id = user_id,
			piece_idx = piece_idx,
			target_idx = target_idx
		};

		return args;
	}
}
