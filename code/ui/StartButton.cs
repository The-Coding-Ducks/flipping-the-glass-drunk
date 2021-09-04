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
			AddClass("start-button");

			Label = Add.Label( "Flip The Glass!" );

			AddEventListener( "onclick", () =>
			{
				FlippingTheGlassDrunk.FlipTheGlass();
			} );
		}
	}
}
