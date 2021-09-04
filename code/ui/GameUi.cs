using Sandbox;
using Sandbox.UI;

namespace FlippingTheGlassDrunk.ui
{
	[Library("gameui")]
	public class GameUi : Panel
	{
		public GameUi() {
			Host.AssertClient();
			
			AddClass("gameui");

			StyleSheet.Load( "/ui/GameUi.styles.scss" );
			SetTemplate("/ui/GameUi.html");
			
			Style.Display = DisplayMode.None;
			Style.Dirty();
		}

		[Event("GameStateChanged")]
		public void OnGameStateChanged(bool isGameRunning)
		{
			Style.Display = isGameRunning ? DisplayMode.Flex : DisplayMode.None;
			Style.Dirty();
		}
	}
}
