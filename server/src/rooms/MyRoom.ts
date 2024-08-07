import { Room, Client } from "@colyseus/core";
import { MyRoomState, Player } from "./schema/MyRoomState";

export class MyRoom extends Room<MyRoomState> {
  maxClients = 4;

  onCreate (options: any) {
    console.log("Room created with options:", options);
    this.setState(new MyRoomState());
    

    this.onMessage('move', (client, payload) => {
      //
      // handle "type" message
      //
      const player = this.state.players.get(client.sessionId);
      if (player) {
          player.x += payload.x;
          player.y += payload.y;
      }
      console.log(client.sessionId, "moved", payload);
    });
  }

  update(deltaTime: number) {
    for (const player of this.state.players.values()) {
      player.x += Math.random() * 10;
      player.y += Math.random() * 10;
    }
  }
  onJoin (client: Client, options: any) {
    console.log(client.sessionId, "joined!");
    this.state.players.set(client.sessionId, new Player());
    const player = this.state.players.get(client.sessionId);
    // Send welcome message to the client.
    client.send("welcome", {player: player , message: "Welcome to the game!" });

  }

  onLeave (client: Client, consented: boolean) {
    console.log(client.sessionId, "left!");
    this.state.players.delete(client.sessionId);
    this.broadcast("playerLeft", { sessionId: client.sessionId });
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }

}
