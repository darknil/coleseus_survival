import { Room, Client } from "@colyseus/core";
import { MyRoomState, Player } from "./schema/MyRoomState";

export type PositionMessage = {
  x: number,
  y: number
}

export class MyRoom extends Room<MyRoomState> {
  maxClients = 4;

  onCreate (options: any) {
    this.setState(new MyRoomState());

    this.onMessage('move', (client, payload) => {
      //
      // handle "type" message
      //
      const player = this.state.players.get(client.sessionId);
      const velocity = 2;

      if (payload.left) {
        player.x -= velocity;

      } else if (payload.right) {
        player.x += velocity;
      }

      if (payload.up) {
        player.y -= velocity;

      } else if (payload.down) {
        player.y += velocity;
      }
      console.log(client.sessionId, "moved", payload);
    });

    this.setSimulationInterval((deltaTime) => this.update(deltaTime));
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

    // Send welcome message to the client.
    client.send("welcomeMessage", "Welcome to Colyseus!");

    // Listen to position changes from the client.
    this.onMessage("position", (client, position: PositionMessage) => {
      console.log({position})
      const player = this.state.players.get(client.sessionId);
      player.x = position.x;
      player.y = position.y;
      console.log({position})
      client.send("welcomeMessage", `position: ${position.x}, ${position.y}`);
    });
  }

  onLeave (client: Client, consented: boolean) {
    console.log(client.sessionId, "left!");
    this.state.players.delete(client.sessionId);
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }

}
