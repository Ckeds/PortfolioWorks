package code {		import flash.display.MovieClip;			public class PressurePlatform extends Platform {		
		static var pressureList: Array = new Array();
				private var direction: String;				private var xOri: Number = 0;				private var yOri: Number = 0;				public function PressurePlatform(direction:String) {			// constructor code
			pressureList.push(this);			this.direction = direction;		}				public function movePlatform(){			if(this.direction == "up" && this.y < yOri){				this.y += this.height / 500;			}			else if(this.direction == "down" && this.y > yOri){				this.y -= this.height / 250;			}
			else
			{
				this.y = yOri;
			}		}				public function liftGate(){			xOri = this.x;			yOri = this.y;			if(this.direction == "up"){				this.y -= this.height;			}			if(this.direction == "down"){				this.y += this.height;			}		}	}	}