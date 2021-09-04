using FlippingTheGlassDrunk.entities;
using Sandbox;

namespace FlippingTheGlassDrunk.weapons
{
	[Library("weapon_ftg_wand", Title = "Wand", Spawnable = true)]
	public class Wand : BaseWeapon
	{
		public override float PrimaryRate => 1;

		public override void Spawn()
		{
			base.Spawn();
			
			SetModel("models/flipping_the_glass_drunk/wand/ftg_wand.vmdl");
		}

		public override async void AttackPrimary()
		{
			(Owner as AnimEntity)?.SetAnimBool( "b_attack", true );
			
			if ( IsServer )
			{
				await Task.Delay( 350 );
				if ( !Owner.IsValid() ) return;
				MagicOrb magicOrb = new MagicOrb();
				magicOrb.Position = Owner.EyePos + new Vector3( 45, -10, -25 ) * Owner.Rotation;
				magicOrb.Rotation = Owner.Rotation;
				magicOrb.Owner = Owner;
				magicOrb.Shoot(Owner.Rotation);
			}
		}

		public override void SimulateAnimator( PawnAnimator anim )
		{
			base.SimulateAnimator( anim );
			
			anim.SetParam( "holdtype", 4 );
		}
	}
}
