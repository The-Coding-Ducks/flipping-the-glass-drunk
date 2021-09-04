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
				if ( ((FlippingTheGlassDrunk)Game.Current).IsGameRunning )
				{
					ChatBox.AddChatEntry( "Conscience", "Flip a second glass are you crazy?!" );
				} else if ( Client.All.Count <= 1 )
				{
					ChatBox.AddChatEntry( "Conscience", "Who do you wanna fight, better wait for your lads!" );
				}
				else
				{
					FlippingTheGlassDrunk.FlipTheGlass();
				}
			} );
		}
		
		[Event("ShowVictoryScreen")]
		public void OnShowVictoryScreen(bool showVictoryScreen, Client client)
		{
			SetClass("hidden", showVictoryScreen);
		}
	}
}
