package networking.response;

// Other Imports
import core.GameServer;
import metadata.Constants;
import model.Player;
import utility.GamePacket;
import utility.Log;
import java.util.List;
/**
 * The ResponseLogin class contains information about the authentication
 * process.
 */
public class ResponseLeave extends GameResponse {
    private Player player;

    public ResponseLeave() {
        responseCode = Constants.SMSG_LEAVE;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());

        GameServer.getInstance().removeActivePlayer(player.getID());

        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }
}