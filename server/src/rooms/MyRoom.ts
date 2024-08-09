import { Room, Client } from '@colyseus/core'
import { World, Entity } from 'ecsy'
import { MyRoomState } from './schema/MyRoomState'
import { PositionComponent } from '../components/position.component'

type ECSWorld = World

export class MyRoom extends Room<MyRoomState> {
  world: ECSWorld
  ticker: NodeJS.Timeout
  entityMap: Map<string, Entity> = new Map()
  onCreate(options: any) {
    this.setState(new MyRoomState())
    this.world = new World()
    this.world.registerComponent(PositionComponent)

    this.ticker = setInterval(
      (deltaTime: number) => this.update(deltaTime),
      1000 / 60
    ) as unknown as NodeJS.Timeout
  }
  update(deltaTime: number) {
    this.world.execute()
    this.state.entities.forEach((value, key) => {
      const entity = this.entityMap.get(key)
      if (entity) {
        const position = entity.getComponent(PositionComponent)
        this.state.entities.set(
          key,
          JSON.stringify({ x: position.x, y: position.y })
        )
      }
    })
    this.clients.forEach((client) => {
      client.send('update', { entities: this.state.entities })
    })
  }

  onJoin(client: Client, options: any) {
    const entity = this.world.createEntity()
    entity.addComponent(PositionComponent, { x: 0, y: 0 })
    this.state.entities.set(client.sessionId, JSON.stringify({ x: 0, y: 0 }))
    client.send('position', { x: 0, y: 0 })
  }
  onLeave(client: Client, consented: boolean) {}

  onDispose() {
    clearInterval(this.ticker)
  }
}
