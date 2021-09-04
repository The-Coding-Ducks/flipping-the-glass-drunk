using Sandbox;
using Sandbox.UI;

namespace FlippingTheGlassDrunk.ui
{
	public class FlippingTheGlassDrunkHud : HudEntity<RootPanel>
	{
		public FlippingTheGlassDrunkHud()
		{
			if ( !IsClient ) return;

			RootPanel.AddChild<ChatBox>();
			RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
			RootPanel.AddChild<FilmOverlay>();
		}
	}
}
