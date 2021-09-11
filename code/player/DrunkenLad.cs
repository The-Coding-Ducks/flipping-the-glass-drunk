using System.Collections.Generic;
using FlippingTheGlassDrunk.player.controller;
using FlippingTheGlassDrunk.weapons;
using Sandbox;

namespace FlippingTheGlassDrunk.player
{
	public partial class DrunkenLad : Player
	{
		private Outfit Outfit { get; set; }
		[Net, OnChangedCallback] 
		public string Lad { get; set; }
		public readonly Dictionary<string, Color> LadColor = new()
		{
			{"Alfred", new Color( 1, 0, 0 )},
			{"Barklay", new Color( 0, 1, 0 )},
			{"Chad", new Color( 0, 0, 1 )},
			{"Duncan", new Color( 1, 1, 0 )}
		};

		public override void Respawn()
		{
			SetModel("models/citizen/citizen.vmdl");

			Controller = new TopDownController();
			Animator = new TopDownAnimator();
			Camera = new TopDownCamera();

			if ( Outfit == null )
			{
				Outfit = new Outfit(this);
			}

			Outfit.LoadOutfit("lad");

			GlowActive = true;
			GlowState = GlowStates.GlowStateOn;
			GlowColor = LadColor[Lad];

			ActiveChild = new Wand();
			ActiveChild.Owner = this;
			ActiveChild.SetParent( this, true);

			EnableDrawing = true;
			EnableAllCollisions = true;

			base.Respawn();

			Health = 3;
			RunTakeDamageEvent();
		}

		[ClientRpc]
		public void RunTakeDamageEvent()
		{
			Event.Run("TookDamage");
		}
		
		public override void TakeDamage( DamageInfo info )
		{
			base.TakeDamage( info );
			
			RunTakeDamageEvent();
		}

		public override void OnKilled()
		{
			base.OnKilled();

			BecomeRagdollOnClient( new Vector3(), DamageFlags.Bullet, new Vector3(), new Vector3(), 0 );
			EnableDrawing = false;
			EnableAllCollisions = false;

			((FlippingTheGlassDrunk) Game.Current).SetClientToSpectator(GetClientOwner());
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			
			if ( Health > 0 )
			{
				SimulateActiveChild( cl, ActiveChild );
			}
		}

		public void OnLadChanged()
		{
			GlowColor = LadColor[Lad];
		}
		
		[ClientRpc]
		private void BecomeRagdollOnClient( Vector3 velocity, DamageFlags damageFlags, Vector3 forcePos, Vector3 force, int bone )
		{
			var ent = new ModelEntity();
			ent.Position = Position;
			ent.Rotation = Rotation;
			ent.Scale = Scale;
			ent.MoveType = MoveType.Physics;
			ent.UsePhysicsCollision = true;
			ent.EnableAllCollisions = true;
			ent.CollisionGroup = CollisionGroup.Debris;
			ent.SetModel( GetModelName() );
			ent.CopyBonesFrom( this );
			ent.CopyBodyGroups( this );
			ent.CopyMaterialGroup( this );
			ent.TakeDecalsFrom( this );
			ent.EnableHitboxes = true;
			ent.EnableAllCollisions = true;
			ent.SurroundingBoundsMode = SurroundingBoundsType.Physics;
			ent.RenderColor = RenderColor;
			ent.PhysicsGroup.Velocity = velocity;

			if ( Local.Pawn == this )
			{
				//ent.EnableDrawing = false; wtf
			}

			ent.SetInteractsAs( CollisionLayer.Debris );
			ent.SetInteractsWith( CollisionLayer.WORLD_GEOMETRY );
			ent.SetInteractsExclude( CollisionLayer.Player | CollisionLayer.Debris );

			foreach ( var child in Children )
			{
				if ( child is ModelEntity e )
				{
					var model = e.GetModelName();
					if ( model != null && !model.Contains( "clothes" ) )
						continue;

					var clothing = new ModelEntity();
					clothing.SetModel( model );
					clothing.SetParent( ent, true );
					clothing.RenderColor = e.RenderColor;

					if ( Local.Pawn == this )
					{
						//	clothing.EnableDrawing = false; wtf
					}
				}
			}

			if ( damageFlags.HasFlag( DamageFlags.Bullet ) ||
				 damageFlags.HasFlag( DamageFlags.PhysicsImpact ) )
			{
				PhysicsBody body = bone > 0 ? ent.GetBonePhysicsBody( bone ) : null;

				if ( body != null )
				{
					body.ApplyImpulseAt( forcePos, force * body.Mass );
				}
				else
				{
					ent.PhysicsGroup.ApplyImpulse( force );
				}
			}

			if ( damageFlags.HasFlag( DamageFlags.Blast ) )
			{
				if ( ent.PhysicsGroup != null )
				{
					ent.PhysicsGroup.AddVelocity( (Position - (forcePos + Vector3.Down * 100.0f)).Normal * (force.Length * 0.2f) );
					var angularDir = (Rotation.FromYaw( 90 ) * force.WithZ( 0 ).Normal).Normal;
					ent.PhysicsGroup.AddAngularVelocity( angularDir * (force.Length * 0.02f) );
				}
			}

			Corpse = ent;

			ent.DeleteAsync( 10.0f );
		}
	}
}
