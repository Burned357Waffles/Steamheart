public class Constants
{
	// Constants
	public static readonly string CLIENT_VERSION = "1.00";
	public static readonly string REMOTE_HOST = "localhost";
	public static readonly int REMOTE_PORT = 1729;
	
	// Request (1xx) + Response (2xx)
	public static readonly short CMSG_JOIN = 101;
	public static readonly short SMSG_JOIN = 201;
	public static readonly short CMSG_LEAVE = 102;
	public static readonly short SMSG_LEAVE = 202;
	public static readonly short CMSG_START_GAME = 103;
	public static readonly short SMSG_START_GAME = 203;
	public static readonly short CMSG_END_GAME = 104;
	public static readonly short SMSG_END_GAME = 204;
	public static readonly short CMSG_ADVANCE_TURN = 105;
	public static readonly short SMSG_ADVANCE_TURN = 205;
	
	public static readonly short CMSG_HEARTBEAT = 111;

	// public static readonly int USER_ID = -1;
	// public static readonly int OP_ID = -1;

}