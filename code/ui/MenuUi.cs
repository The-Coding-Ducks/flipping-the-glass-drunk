using Sandbox;
using Sandbox.UI;

namespace FlippingTheGlassDrunk.ui
{
	[Library("menuui")]
	public class MenuUi : Panel
	{
		public MenuUi() {
			Host.AssertClient();
		
			AddClass("menuui");

			StyleSheet.Load( "/ui/MenuUi.styles.scss" );
			SetTemplate("/ui/MenuUi.html");
		
			Style.Display = DisplayMode.Flex;
			Style.Dirty();
		}

		[Event("GameStateChanged")]
		public void OnGameStateChanged(bool isGameRunning)
		{
			Style.Display = !isGameRunning ? DisplayMode.Flex : DisplayMode.None;
			Style.Dirty();
		}
	}
}
