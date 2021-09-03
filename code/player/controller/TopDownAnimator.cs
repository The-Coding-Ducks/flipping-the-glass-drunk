using Sandbox;
using System;

namespace FlippingTheGlassDrunk.player.controller
{
	public class TopDownAnimator : StandardPlayerAnimator
	{
		TimeSince TimeSinceFootShuffle = 60;
		private float duck;
		
		public override void Simulate()
		{
			// var idealRotation = Rotation.LookAt( Rotation.Forward, Vector3.Up );
			// var idealRotation = Rotation.LookAt(Trace.Ray(Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 1000).Run().EndPos);
			// idealRotation = Rotation.From( new Angles( idealRotation.x, idealRotation.y, idealRotation.z ) );
			//
			// DoRotation( Rotation.From( (Trace
			// 	.Ray( Input.Cursor.Origin,
			// 		Input.Cursor.Origin + Input.Cursor.Direction * 1000 ).Run().EndPos - Position).EulerAngles ) );
			DoWalk();

			//
			// Let the animation graph know some shit
			//
			bool sitting = HasTag( "sitting" );
			bool noclip = HasTag( "noclip" ) && !sitting;

			SetParam( "b_grounded", GroundEntity != null || noclip || sitting );
			SetParam( "b_noclip", noclip );
			SetParam( "b_sit", sitting );
			SetParam( "b_swim", Pawn.WaterLevel.Fraction > 0.5f && !sitting );

			// Vector3 aimPos = Position + Vector3.Forward * 1000;
			Vector3 aimPos = Trace.Ray(Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 1000).Run().EndPos;
			Vector3 lookPos = aimPos;

			//
			// Look in the direction what the player's input is facing
			//
			SetLookAt( "lookat_pos", lookPos ); // old
			SetLookAt( "aimat_pos", aimPos ); // old

			SetLookAt( "aim_eyes", lookPos );
			// SetLookAt( "aim_head", lookPos );
			// SetLookAt( "aim_body", aimPos );

			SetParam( "b_ducked", HasTag( "ducked" ) ); // old

			if ( HasTag( "ducked" ) ) duck = duck.LerpTo( 1.0f, Time.Delta * 10.0f );
			else duck = duck.LerpTo( 0.0f, Time.Delta * 5.0f );

			SetParam( "duck", duck );

			if ( Pawn.ActiveChild is BaseCarriable carry )
			{
				carry.SimulateAnimator( this );
			}
			else
			{
				SetParam( "holdtype", 0 );
				SetParam( "aimat_weight", 0.5f ); // old
				SetParam( "aim_body_weight", 0.5f );
			}

		}

		void DoWalk()
		{
			// Move Speed
			{
				var dir = Velocity;
				var forward = Rotation.Forward.Dot( dir );
				var sideward = Rotation.Right.Dot( dir );

				var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

				SetParam( "move_direction", angle );
				SetParam( "move_speed", Velocity.Length );
				SetParam( "move_groundspeed", Velocity.WithZ( 0 ).Length );
				SetParam( "move_y", sideward );
				SetParam( "move_x", forward );
			}

			// Wish Speed
			{
				var dir = WishVelocity;
				var forward = Rotation.Forward.Dot( dir );
				var sideward = Rotation.Right.Dot( dir );

				var angle = MathF.Atan2( sideward, forward ).RadianToDegree().NormalizeDegrees();

				SetParam( "wish_direction", angle );
				SetParam( "wish_speed", WishVelocity.Length );
				SetParam( "wish_groundspeed", WishVelocity.WithZ( 0 ).Length );
				SetParam( "wish_y", sideward );
				SetParam( "wish_x", forward );
			}
		}
	}
}
