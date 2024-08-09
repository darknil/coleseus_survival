// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.38
// 

using Colyseus.Schema;
using OS.PlayerSystem;

public partial class MyRoomState : Schema
{
	[Type(0, "map", typeof(MapSchema<PlayerData>))]
	public MapSchema<PlayerData> players = new();
}

