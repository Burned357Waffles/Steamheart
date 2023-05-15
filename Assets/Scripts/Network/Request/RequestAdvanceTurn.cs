namespace Network.Request
{
    public class RequestAdvanceTurn : NetworkRequest
    {
        public RequestAdvanceTurn()
        {
            request_id = Constants.CMSG_ADVANCE_TURN;
        }

        public void send()
        {
            packet = new GamePacket(request_id);
        }
        
    }
}