using System.Collections.Generic;
using FlippingTheGlassDrunk.player.controller;
using Sandbox;

namespace FlippingTheGlassDrunk.player
{
	public partial class DrunkenLad : Player
	{
		private Outfit Outfit { get; set; }
		[Net, OnChangedCallback] 
		public string Lad => "Alfred";

		private Dictionary<string, Color> LadColor => new()
		{
			{"Alfred", new Color( 1, 0, 0 )},
			{"Barklay", new Color( 0, 1, 0 )},
			{"Chad", new Color( 1, 0, 0 )},
			{"Duncan", new Color( 1, 1, 0 )}
		};

		public override void Respawn()
		{
			base.Respawn();
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
		}

		public void OnLadChanged()
		{
			GlowColor = LadColor[Lad];
		}
	}
}
