import { Schema, MapSchema, type } from '@colyseus/schema'

export class MyRoomState extends Schema {
  @type({ map: 'string' }) entities = new MapSchema()
}
