using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FlippingTheGlassDrunk.ui
{
	[Library("start-button")]
	public class StartButton : Panel
	{
		private Label Label { get; }

		public StartButton()
		{
			AddClass("title-font");
			AddClass("start-button-container");

			Label = Add.Label( "Flip The Glass!", "button" );

			AddEventListener( "onclick", () =>
			{
				FlippingTheGlassDrunk.FlipTheGlass();
			} );
		}
		
		[Event("ShowVictoryScreen")]
		public void OnShowVictoryScreen(bool showVictoryScreen, Client client)
		{
			SetClass("hidden", showVictoryScreen);
		}
	}
}
