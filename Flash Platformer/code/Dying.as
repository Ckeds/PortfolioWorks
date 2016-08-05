package code {
	
	import flash.display.MovieClip;
	
	
	public class Dying extends MovieClip {
		
		private var restart: Document;
		public function Dying(r: Document) {
			// constructor code
			restart = r;
			this.gotoAndPlay(1);
		}
	}
	
}
