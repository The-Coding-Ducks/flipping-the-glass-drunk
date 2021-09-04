using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace FlippingTheGlassDrunk.ui
{
	[Library("victory-screen")]
	public partial class VictoryScreen : Panel
	{
		private static VictoryScreen Instance { get; set; }
		private Label WinnerMessage { get; }
		private Label GoBack { get; }
		
		public VictoryScreen() {
			AddClass("victory");
			AddClass("hidden");

			WinnerMessage = Add.Label( "", "title-font" );
			GoBack = Add.Label( "Go back to the bar", "title-font" );
			GoBack.AddEventListener("onclick", () =>
			{
				SetClass("active", false);
				Event.Run("ShowVictoryScreen", false, Client.All.First());
			});
			GoBack.AddClass("button");

			Instance = this;
		}

		public void SetWinner(Client client)
		{
			var name = client.Name;

			if ( name.Length > 15 )
			{
				name = name.Substring( 0, 15 ) + "...";
			}

			WinnerMessage.Text = name + " is the toughest lad in the pub!";
		}

		[Event("ShowVictoryScreen")]
		public void OnShowVictoryScreen(bool showVictoryScreen, Client client)
		{
			SetClass( "hidden", !showVictoryScreen );
			if ( client != null )
			{
				SetWinner(client);
			} 
		}
	}
}
