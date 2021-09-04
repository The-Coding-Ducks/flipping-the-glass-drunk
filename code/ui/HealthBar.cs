using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FlippingTheGlassDrunk.ui
{
	[Library("healthbar")]
	public class HealthBar : Panel
	{
		private Label Label { get; set; }
		
		public HealthBar() {
			AddClass("game");
			
			Label = Add.Label( "", "health" );
		}

		[Event("TookDamage")]
		public void TookDamage()
		{
			Label.Text = "";

			if ( Local.Pawn == null ) return;
			
			for ( int i = 0; i < Local.Pawn.Health; i++ )
			{
				Label.Text += "🍺";
			}
		}
	}
}
