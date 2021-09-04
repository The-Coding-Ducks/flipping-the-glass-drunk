using FlippingTheGlassDrunk.player;
using Sandbox;

namespace FlippingTheGlassDrunk.entities
{
	public partial class MagicOrb : BasePhysics
	{
		private Particles Particle { get; set; }
		private PointLightEntity PointLightEntity { get; set; }
		public override void Spawn()
		{
			base.Spawn();
			
			SetModel( "models/citizen_props/beachball.vmdl" );
			RenderAlpha = 0;
			DeleteAsync( 5 );
		}

		public void Shoot( Rotation ownerRotation )
		{
			SpawnBlobParticles();
			Velocity = new Vector3( 750, 0, 0 ) * ownerRotation;
			PhysicsBody.GravityEnabled = false;
			
			PointLightEntity = new PointLightEntity
			{
				// Color = new Color( 0.859375f, 0.078125f, 0.234375f ),
				Color = ((DrunkenLad) Owner).LadColor[((DrunkenLad) Owner).Lad],
				Brightness = 0.1f,
				Position = Position,
				Parent = this
			};
		}

		[ClientRpc]
		public void SpawnImpactParticles(Vector3 position)
		{
			if ( Owner == null ) return;
			
			Particles.Create( "particles/flipping_the_glass_drunk/magic_impact_" + ((DrunkenLad)Owner).Lad.ToLower() + ".vpcf", position );
		}

		[ClientRpc]
		public void SpawnBlobParticles()
		{
			Particle = Particles.Create( "particles/flipping_the_glass_drunk/magic_blob.vpcf" );
			Particle.SetPosition(0, Position);
			Particle.SetEntity(0, this, new Vector3(0,0,CollisionBounds.Center.z));
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			Particle?.Destroy( true );
		}

		protected override void OnPhysicsCollision( CollisionEventData eventData )
		{
			var damageInfo = new DamageInfo();
			damageInfo.Attacker = Owner;
			damageInfo.Damage = 1f;

			if ( Owner != eventData.Entity )
			{
				eventData.Entity.TakeDamage(damageInfo);
			}
			
			SpawnImpactParticles(eventData.Pos);
			Delete();
		}
	}
}
