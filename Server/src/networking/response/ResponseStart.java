package networking.response;

import metadata.Constants;
import model.Player;
import utility.GamePacket;

public class ResponseStart extends GameResponse{

    public int playerID;

    public ResponseStart(int startingPlayerID) {
        responseCode = Constants.SMSG_START_GAME;
        playerID = startingPlayerID;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(playerID);
        return packet.getBytes();
    }
}
