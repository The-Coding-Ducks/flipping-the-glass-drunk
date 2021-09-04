using Sandbox;

namespace FlippingTheGlassDrunk.player.controller
{
	public class TopDownController : WalkController
	{
		public override void Simulate()
		{
			var angles =
				(Trace.Ray( Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 1000 ).Ignore(Pawn).Run().EndPos -
				 Position).EulerAngles;
			
			Rotation = Rotation.From( new Angles(0, angles.yaw, 0) );
			
			EyePosLocal = Vector3.Up * (EyeHeight * Pawn.Scale);
			UpdateBBox();

			EyePosLocal += TraceOffset;
			EyeRot = Rotation.LookAt( Trace
				.Ray( Input.Cursor.Origin, Input.Cursor.Origin + Input.Cursor.Direction * 1000 ).Run().EndPos );

			if ( Unstuck.TestAndFix() )
				return;

			CheckLadder();
			Swimming = Pawn.WaterLevel.Fraction > 0.6f;

			if ( !Swimming )
			{
				Velocity -= new Vector3( 0, 0, Gravity * 0.5f ) * Time.Delta;
				Velocity += new Vector3( 0, 0, BaseVelocity.z ) * Time.Delta;

				BaseVelocity = BaseVelocity.WithZ( 0 );
			}

			bool bStartOnGround = GroundEntity != null;
			if ( bStartOnGround )
			{
				Velocity = Velocity.WithZ( 0 );
				if ( GroundEntity != null )
				{
					ApplyFriction( GroundFriction * SurfaceFriction );
				}
			}

			WishVelocity = new Vector3( Input.Forward, Input.Left, 0 );
			var inSpeed = WishVelocity.Length.Clamp( 0, 1 );
			// WishVelocity *= Input.Rotation;

			if ( !Swimming )
			{
				WishVelocity = WishVelocity.WithZ( 0 );
			}

			WishVelocity = WishVelocity.Normal * inSpeed;
			WishVelocity *= GetWishSpeed();

			Duck.PreTick();

			bool bStayOnGround = false;
			if ( Swimming )
			{
				ApplyFriction( 1 );
				WaterMove();
			}
			else if ( GroundEntity != null )
			{
				bStayOnGround = true;
				WalkMove();
			}
			else
			{
				AirMove();
			}

			CategorizePosition( bStayOnGround );

			if ( !Swimming )
			{
				Velocity -= new Vector3( 0, 0, Gravity * 0.5f ) * Time.Delta;
			}


			if ( GroundEntity != null )
			{
				Velocity = Velocity.WithZ( 0 );
			}

			if ( Debug )
			{
				DebugOverlay.Box( Position + TraceOffset, mins, maxs, Color.Red );
				DebugOverlay.Box( Position, mins, maxs, Color.Blue );

				var lineOffset = 0;
				if ( Host.IsServer ) lineOffset = 10;

				DebugOverlay.ScreenText( lineOffset + 0, $"        Position: {Position}" );
				DebugOverlay.ScreenText( lineOffset + 1, $"        Velocity: {Velocity}" );
				DebugOverlay.ScreenText( lineOffset + 2, $"    BaseVelocity: {BaseVelocity}" );
				DebugOverlay.ScreenText( lineOffset + 3, $"    GroundEntity: {GroundEntity} [{GroundEntity?.Velocity}]" );
				DebugOverlay.ScreenText( lineOffset + 4, $" SurfaceFriction: {SurfaceFriction}" );
				DebugOverlay.ScreenText( lineOffset + 5, $"    WishVelocity: {WishVelocity}" );
			}

		}
	}
}
