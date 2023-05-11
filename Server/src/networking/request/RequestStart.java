package networking.request;

import core.GameServer;
import core.NetworkManager;
import networking.response.ResponseStart;

import java.io.IOException;

public class RequestStart extends GameRequest {
    @Override
    public void parse() throws IOException {

    }

    @Override
    public void doBusiness() throws Exception {
        GameServer server = GameServer.getInstance();
        server.isStarted = true;
        ResponseStart response = new ResponseStart(client.getPlayer().getID());
        responses.add(response);
        NetworkManager.addResponseForAllOnlinePlayers(client.getUserID(), response);
    }
}
