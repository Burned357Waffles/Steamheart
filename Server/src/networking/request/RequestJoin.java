package networking.request;

// Java Imports
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

// Other Imports
import core.GameServer;
import core.NetworkManager;
import model.Player;
import networking.response.GameResponse;
import networking.response.ResponseJoin;
import utility.DataReader;
import utility.Log;

/**
 * The RequestLogin class authenticates the user information to log in. Other
 * tasks as part of the login process lies here as well.
 */

public class RequestJoin extends GameRequest {
    // Data
    private String username;

    // Responses


    @Override
    public void parse() throws IOException {
        username = DataReader.readString(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        GameServer server = GameServer.getInstance();
        int newID = server.getID();
        if (newID == 0) {
            responses.add(new ResponseJoin());
            return;
        }
        Player joiningPlayer = new Player(newID, username);
        server.setActivePlayer(joiningPlayer);
        joiningPlayer.setClient(client);
        client.setPlayer(joiningPlayer);

        ResponseJoin newPlayerResponse = new ResponseJoin(joiningPlayer);
        responses.add(newPlayerResponse);
        NetworkManager.addResponseForAllOnlinePlayers(newID, newPlayerResponse);
        for (Player player : server.getActivePlayers()) {
            if (player.getID() != newID) {
                responses.add(new ResponseJoin(player));
            }
        }
        System.out.println(joiningPlayer);

    }
}


//        GameServer gs = GameServer.getInstance();
//        int id = gs.getID();
//        if(id != 0) {
//            player = new Player(id, "Player " + id);
//            player.setID(id);
//            gs.setActivePlayer(player);
//
//            player.setClient(client);
//            // Pass Player reference into thread
//            client.setPlayer(player);
//            // Set response information
//            responseJoin.setStatus((short) 0); // Login is a success
//            responseJoin.setPlayer(player);
//            Log.printf("User '%s' has successfully logged in.", player.getName());
//
//            ResponseName responseName = new ResponseName();
//            responseName.setPlayer(player);
//            NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseName);
//        } else {
//            Log.printf("A user has tried to join, but failed to do so.");
//            responseJoin.setStatus((short) 1);
//        }
//    }

