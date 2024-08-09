import { Component, Types } from 'ecsy'

export class PositionComponent extends Component<PositionComponent> {}
PositionComponent.schema = {
  x: { type: Types.Number, default: 0 },
  y: { type: Types.Number, default: 0 }
}
