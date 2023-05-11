package networking.response;

import metadata.Constants;
import utility.GamePacket;

public class ResponseEnd extends GameResponse{

    int playerID;

    public ResponseEnd(int endingPlayerID) {
        responseCode = Constants.SMSG_END_GAME;
        playerID = endingPlayerID;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(playerID);
        return packet.getBytes();
    }
}
