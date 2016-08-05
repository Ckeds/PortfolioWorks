package code {
	
	import flash.display.MovieClip;
	
	public class Beater extends Platform{
		
		private var startLeft: Boolean;
		private var xMovable: Number;
		private var startingX: Number;
		private var maxOffset: Boolean;

		public function Beater(AStartLeft: Boolean, AXMovable: Number, AX: Number) {
			// constructor code
			startLeft = AStartLeft;
			xMovable = AXMovable;
			startingX = AX;
			maxOffset = false;
		}
		
		public function move(moveOut: Boolean)
		{
			trace(startLeft);
			if (moveOut && !maxOffset)
			{
				if (startLeft)
				{
					x += 5;
					if (x >= xMovable + startingX)
					{
						x = xMovable + startingX;
						maxOffset = true;
					}
				}
				else
				{
					x -= 5;
					if (x <= startingX - xMovable)
					{
						x = startingX - xMovable;
						maxOffset = true;
					}
				}
			}
			else
			{
				if (startLeft)
				{
					x -= 5;
					if (x <= startingX)
					{
						trace(startingX);
						x = startingX;
						maxOffset = false;
					}
				}
				else
				{
					x += 5;
					if (x >= startingX)
					{
						trace(startingX);
						x = startingX;
						maxOffset = false;
					}
				}
			}
		}
		
		public function sense(player: Box)
		{
			//will move back then return if is not adjactent on y axis
			if (player.y < this.y)
			{
				if (player.y + player.height < this.y)
				{
					move(false);
					return;
				}
			}
			else
			{
				if (player.y > this.y + this.height)
				{
					move(false);
					return;
				}
			}
			
			if (startLeft)
			{
				if (player.x < this.x + this.width || player.x >= xMovable + startingX + this.width)
				{
					move(false);
					return;
				}
			}
			else
			{
				if (player.x + player.width > this.x || player.x + player.width < startingX - xMovable)
				{
					move(false);
					return;
				}
			}
			move(true);
		}

	}
	
}
