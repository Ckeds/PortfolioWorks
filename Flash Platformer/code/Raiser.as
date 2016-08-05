package code {
	
	import flash.display.MovieClip;
	
	public class Raiser extends Platform {
		
		//array of all raisers used in reset
		private static var allRaisers: Array = new Array();
		
		private var clicked: Boolean = false;

		public function Raiser() {
			// constructor code
			allRaisers.push(this);
		}
		
		public function raisePlatforms() {
			Clicked = true;
			Depressed.RaiseHeight = 480 - this.y + this.height;
		}
		
		public function set Clicked (value: Boolean){
			if (clicked != value)
			{
				if(value)
				{
					resetClicks();
					height /= 2;
					y += height;
				}
				else
				{
					y -= height;
					height *= 2;
				}
				clicked = value;
			}
		}
		
		public function get Clicked (){
			return clicked;
		}
		
		public static function resetClicks(){
			for (var i: int = 0; i != allRaisers.length; i++)
			{
				allRaisers[i].Clicked = false;
			}
		}

	}
	
}
