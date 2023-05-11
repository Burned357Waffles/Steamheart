namespace Network.Response
{

    public class ResponseStartEventArgs : ExtendedEventArgs
    {
        private int _startingPlayerID;
        public ResponseStartEventArgs(int playerID)
        {
            event_id = Constants.SMSG_START_GAME;
            _startingPlayerID = playerID;
        }
    }
    public class ResponseStart : NetworkResponse
    {

        private int _startingPlayerID;
        
        public override void parse()
        {
            int id = DataReader.ReadInt(dataStream);
            _startingPlayerID = DataReader.ReadInt(dataStream);
        }

        public override ExtendedEventArgs process()
        {
            return new ResponseStartEventArgs(_startingPlayerID);
        }
    }
}