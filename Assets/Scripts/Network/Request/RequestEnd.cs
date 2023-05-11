namespace Network.Request
{
    public class RequestEnd : NetworkRequest
    {
        public RequestEnd()
        {
            request_id = Constants.CMSG_END_GAME;
        }

        public void send()
        {
            packet = new GamePacket(request_id);
        }
    }
}