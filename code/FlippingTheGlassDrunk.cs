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

		[ServerCmd]
		public static void FlipTheGlass()
		{
			((FlippingTheGlassDrunk)Current).StartGame();
		}
		
		public void StartGame()
		{
			Host.AssertServer();
			
			if ( IsGameRunning || Client.All.Count <= 1 ) return;
			
			IsGameRunning = true;

			RunGameStateChangedEvent(IsGameRunning);

			int index = 0;
			foreach ( Client client in Client.All.ToList() )
			{
				SetClientToDrunkenLad( client, Lads[index] );
				index++;
			}
		}

		[ClientRpc]
		public void ShowVictoryScreen( Client winner )
		{
			Event.Run("ShowVictoryScreen", true, winner);
		}

		public void EndGame( Client winner )
		{
			Host.AssertServer();
			
			IsGameRunning = false;
			
			RunGameStateChangedEvent(IsGameRunning);
			ShowVictoryScreen( winner );

			foreach ( Client client in Client.All.ToList() )
			{
				SetClientToSpectator(client);
			}
		}

		public void SetClientToSpectator( Client client )
		{
			client.Pawn?.Delete();
			Spectator spectator = new Spectator();
			spectator.Respawn();
			client.Pawn = spectator;
			
			RunGameStateChangedEvent( To.Single(client), false);
		}

		private void SetClientToDrunkenLad( Client client, string ladName )
		{
			client.Pawn?.Delete();
			DrunkenLad drunkenLad = new DrunkenLad();
			drunkenLad.Lad = ladName;
			drunkenLad.Respawn();
			client.Pawn = drunkenLad;
		}

		public override void OnKilled( Entity pawn )
		{
			base.OnKilled( pawn );
			
			CheckGameState(null);
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			base.ClientDisconnect( cl, reason );
			
			if ( IsServer )
			{
				CheckGameState(cl);
			}
		}

		private void CheckGameState(Client ignore)
		{
			var playersAlive = All.OfType<DrunkenLad>()
				.Where(lad => lad.Health > 0)
				.Where( lad => lad.GetClientOwner() != ignore)
				.ToArray();

			if ( playersAlive.Length <= 1 && IsGameRunning )
			{
				if ( playersAlive.Length < 1 )
				{
					EndGame( Client.All.First() );
					return;
				}

				EndGame(playersAlive.First().GetClientOwner());
			}
		}

		public override void MoveToSpawnpoint( Entity pawn )
		{
			if ( pawn.GetType() == typeof(DrunkenLad) )
			{
				var spawnpoints = All.OfType<SpawnPoint>()
					.ToArray();

				for ( int i = 0; i < Lads.Length; i++ )
				{
					if ( ((DrunkenLad)pawn).Lad == Lads[i] )
					{
						pawn.Transform = spawnpoints[i % spawnpoints.Length].Transform;
						break;
					}
				}

				return;
			}
			
			base.MoveToSpawnpoint(pawn);
		}
		
		
		public override void DoPlayerNoclip( Client player )
		{
		}

		public override void DoPlayerDevCam( Client player )
		{
		}
	}
}
