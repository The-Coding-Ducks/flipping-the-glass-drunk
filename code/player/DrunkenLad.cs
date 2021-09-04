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
		public string Lad => "Chad";
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
				Outfit = new Outfit();
			}

			Outfit.ResetOutfit();
			Outfit.LoadOutfit("lad");

			GlowActive = true;
			GlowState = GlowStates.GlowStateOn;
			GlowColor = LadColor[Lad];

			ActiveChild = new Wand();
			ActiveChild.Owner = this;

			EnableDrawing = true;
			EnableAllCollisions = true;

			base.Respawn();
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
	}
}
