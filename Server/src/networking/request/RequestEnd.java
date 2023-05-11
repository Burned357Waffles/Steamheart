package networking.request;


import core.GameServer;
import core.NetworkManager;
import networking.response.GameResponse;
import networking.response.ResponseEnd;

import java.io.IOException;

public class RequestEnd extends GameRequest {
    @Override
    public void parse() throws IOException {

    }

    @Override
    public void doBusiness() throws Exception {
        int thisClientID = client.getUserID();
        ResponseEnd response = new ResponseEnd(thisClientID);
        responses.add(response);
        NetworkManager.addResponseForAllOnlinePlayers(thisClientID, response);
    }
}
