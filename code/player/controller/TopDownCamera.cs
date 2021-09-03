using Sandbox;

namespace FlippingTheGlassDrunk.player.controller
{
	public class TopDownCamera : Camera
	{
		protected virtual float CameraHeight => 500;
		protected virtual float CameraDistance => 250;

		public override void Update()
		{
			Entity target = Local.Pawn;
			Pos = target.Position + new Vector3( -CameraDistance, 0, CameraHeight );
			Rot = Rotation.From( (target.Position - Pos).EulerAngles );
			FieldOfView = 80;
			Viewer = null;
		}
	}
}
