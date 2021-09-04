using FlippingTheGlassDrunk.player.controller;
using Sandbox;

namespace FlippingTheGlassDrunk.player
{
	public class Spectator : Player
	{
		public override void Respawn()
		{
			base.Respawn();

			Camera = new TopDownCamera();
			Controller = new SpectatorController();

			Position = new Vector3( 0, 0, 0 );

			EnableAllCollisions = false;
		}
	}
}
