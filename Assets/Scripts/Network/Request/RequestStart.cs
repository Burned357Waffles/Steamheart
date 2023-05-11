namespace Network.Request
{
    public class RequestStart : NetworkRequest
    {
        public RequestStart()
        {
            request_id = Constants.CMSG_START_GAME;
        }

        public void send()
        {
            packet = new GamePacket(request_id);
        }
    }
}