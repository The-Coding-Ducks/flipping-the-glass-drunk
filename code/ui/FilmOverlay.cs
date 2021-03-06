using Sandbox;
using Sandbox.UI;

namespace FlippingTheGlassDrunk.ui
{
	public class FilmOverlay : Panel
	{
		public FilmOverlay() {
			Host.AssertClient();

			StyleSheet.Load( "/ui/FilmUi.styles.scss" );
			SetTemplate("/ui/FilmUi.html");
		}
		
		[Event("GameStateChanged")]
		public void OnGameStateChanged(bool isGameRunning)
		{
			SetClass("gameactive", isGameRunning);
		}
	}
}
