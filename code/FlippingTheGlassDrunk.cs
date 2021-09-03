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

		public void StartGame()
		{
			Host.AssertServer();
			
			if ( IsGameRunning ) return;
			
			IsGameRunning = true;

			Event.Run( "GameStateChanged", IsGameRunning );

			foreach ( Client client in Client.All.ToList() )
			{
				SetClientToDrunkenLad( client );
			}
		}

		public void EndGame( Client winner )
		{
			Host.AssertServer();
			
			IsGameRunning = false;
			
			Event.Run( "GameStateChanged", IsGameRunning );

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

		private void SetClientToDrunkenLad( Client client )
		{
			client.Pawn?.Delete();
			DrunkenLad drunkenLad = new DrunkenLad();
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
