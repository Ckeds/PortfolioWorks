package code {
	
	import flash.display.MovieClip;
	
	public class Depressed extends Platform {
		
		private static var depressedList: Array = new Array();
		private static var raiseHeight: Number = 0;

		public function Depressed() {
			// constructor code
			depressedList.push(this);
		}
		
		public static function set RaiseHeight(value: Number){
			var diff: Number = value - raiseHeight;
			raiseHeight = value;
			for (var i: int = 0; i != depressedList.length; i++)
			{
				depressedList[i].y -= diff;
			}
		}
		
		public static function get RaiseHeight(){
			return raiseHeight;
		}

	}
	
}
