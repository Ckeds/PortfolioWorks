package code {
	
	import flash.display.MovieClip;
	
	public class Destructible extends Platform {
		
		public var contact: Number;
		public var disable: Boolean;
		public var inContact: Boolean;
		
		public static var purgatory: Array = new Array();

		public function Destructible() {
			// constructor code
			contact = 0;
			disable = false;
			inContact = false;
		}
		
		public function touch()
		{
			if (disable)
			{
				return;
			}
			inContact = true;
			contact += 1;
			this.gotoAndStop(contact);
			if (contact == 10)
			{
				gotoAndPlay(11);
				disable = true;
			}
		}
		
		public function idle()
		{
			if (inContact)
			{
				inContact = false;
				return;
			}
			if (disable)
			{
				contact -= .14;
			}
			else
			{
				contact -= 1;
			}
			trace(contact);
			if (contact < 0)
			{
				contact = 0;
				if (disable)
				{
					this.gotoAndStop(0);
					disable = false;
				}
			}
			if (!disable)
			{
				this.gotoAndStop(contact);
			}
		}
		
		public static function idle()
		{
			for (var i:uint = 0; i < purgatory.length; i++)
			{
				purgatory[i].idle();
			}
		}
		
		public static function revive(): Boolean
		{
			if (purgatory.length > 0 && purgatory[0].contact < 1)
			{
				return true;
			}
			return false;
		}

	}
	
}
