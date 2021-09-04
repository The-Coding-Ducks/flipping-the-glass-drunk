using System;
using System.Collections.Generic;
using System.Linq;
using FlippingTheGlassDrunk.player;
using FlippingTheGlassDrunk.ui;
using Sandbox;

namespace FlippingTheGlassDrunk
{
	public partial class FlippingTheGlassDrunk : Game
	{
		[Net]
		public bool IsGameRunning { get; private set; }
		public string[] Lads = {"Alfred", "Barklay", "Chad", "Duncan"};

		public FlippingTheGlassDrunk() {
			IsGameRunning = false;

			if ( IsServer )
			{
				_ = new FlippingTheGlassDrunkHud();
			}
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );
			
			SetClientToSpectator(client);
		}

		[ClientRpc]
		public void RunGameStateChangedEvent(bool isGameRunning)
		{
			Event.Run( "GameStateChanged", isGameRunning );
		}
		
		public void StartGame()
		{
			Host.AssertServer();
			
			if ( IsGameRunning ) return;
			
			IsGameRunning = true;

			RunGameStateChangedEvent(IsGameRunning);

			int index = 0;
			foreach ( Client client in Client.All.ToList() )
			{
				SetClientToDrunkenLad( client, Lads[index] );
				index++;
			}
		}

		public void EndGame( Client winner )
		{
			Host.AssertServer();
			
			IsGameRunning = false;
			
			RunGameStateChangedEvent(IsGameRunning);

			foreach ( Client client in Client.All.ToList() )
			{
				SetClientToSpectator(client);
			}
		}

		private void SetClientToSpectator( Client client )
		{
			client.Pawn?.Delete();
			Spectator spectator = new Spectator();
			spectator.Respawn();
			client.Pawn = spectator;
		}

		private void SetClientToDrunkenLad( Client client, string ladName )
		{
			client.Pawn?.Delete();
			DrunkenLad drunkenLad = new DrunkenLad();
			drunkenLad.Lad = ladName;
			drunkenLad.Respawn();
			client.Pawn = drunkenLad;
		}

		[ServerCmd( "start_game" )]
		public static void Start()
		{
			((FlippingTheGlassDrunk) Current).StartGame();
		}
	}
}
