namespace Network.Response
{

    public class ResponseEndEventArgs : ExtendedEventArgs
    {
        private int _playerID;

        public ResponseEndEventArgs(int endingPlayerID)
        {
            event_id = Constants.SMSG_END_GAME;
            _playerID = endingPlayerID;
        }
    }
    
    public class ResponseEnd : NetworkResponse
    {
        private int _playerID;
        public override void parse()
        {
            _playerID = DataReader.ReadInt(dataStream);
        }

        public override ExtendedEventArgs process()
        {
            return new ResponseEndEventArgs(_playerID);
        }
    }
}