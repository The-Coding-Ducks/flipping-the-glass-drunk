using Sandbox;

namespace FlippingTheGlassDrunk.player
{
	public class Outfit
	{
		private ModelEntity Hat { get; set; }
		private ModelEntity Shirt { get; set; }
		private ModelEntity Trousers { get; set; }
		private ModelEntity Shoes { get; set; }

		private Player Owner { get; }

		public Outfit( Player owner )
		{
			Owner = owner;
		}

		public void ResetOutfit()
		{
			Hat?.Delete();
			Shirt?.Delete();
			Trousers?.Delete();
			Shoes?.Delete();

			Hat = null;
			Shirt = null;
			Trousers = null;
			Shoes = null;
		}

		public void SetHat(string model)
		{
			if ( Hat == null )
			{
				Hat = new ModelEntity();
			}
			Hat.SetModel( model );
			Hat.SetParent( Owner, true );
		}
		
		public void SetShirt(string model)
		{
			if ( Shirt == null )
			{
				Shirt = new ModelEntity();
			}
			Shirt.SetModel( model );
			Shirt.SetParent( Owner, true );
		}
		
		public void SetTrousers(string model)
		{
			if ( Trousers == null )
			{
				Trousers = new ModelEntity();
			}
			Trousers.SetModel( model );
			Trousers.SetParent( Owner, true );
		}
		
		public void SetShoes(string model)
		{
			if ( Shoes == null )
			{
				Shoes = new ModelEntity();
			}
			Shoes.SetModel( model );
			Shoes.SetParent( Owner, true );
		}

		public void LoadOutfit( string name )
		{
			ResetOutfit();
			switch ( name )
			{
				case "lad":
					SetHat( "models/citizen_clothes/hat/hat_leathercapnobadge.vmdl" );
					SetShirt( "models/citizen_clothes/jacket/suitjacket/suitjacketunbuttonedshirt.vmdl" );
					SetTrousers( "models/citizen_clothes/trousers/smarttrousers/smarttrousers.vmdl" );
					SetShoes( "models/citizen_clothes/shoes/smartshoes/smartshoes.vmdl" );
					break;
			}
		}
	}
}
