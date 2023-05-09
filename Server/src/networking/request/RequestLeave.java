package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseLeave;
import core.NetworkManager;

public class RequestLeave extends GameRequest {
    // Responses

    @Override
    public void parse() throws IOException {

    }

    @Override
    public void doBusiness() throws Exception {
        ResponseLeave response = new ResponseLeave();
        Player player = client.getPlayer();
        response.setPlayer(player);
        responses.add(response);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), response);
    }
}