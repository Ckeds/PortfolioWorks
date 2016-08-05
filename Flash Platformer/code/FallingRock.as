package code {
	
	import flash.display.MovieClip;
	
	
	public class FallingRock extends Platform {
		
		
		public function FallingRock() {
			// constructor code
		}
		public function fall()
		{
			this.y += (Math.random() * 3) + 8;
			this.rotation += 4.97;
			if(this.y > 550)
			{
				this.y = - 100;
				this.x = (Math.random() * 500) + 80;
			}
		}
	}
	
}
