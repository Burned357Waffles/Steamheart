package networking.response;

import metadata.Constants;
import utility.GamePacket;

public class ResponseAdvanceTurn extends GameResponse {

    private int playerID;

    public ResponseAdvanceTurn(int callingPlayerID) {
        responseCode = Constants.SMSG_ADVANCE_TURN;
        playerID = callingPlayerID;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(playerID);
        return packet.getBytes();
    }
}

