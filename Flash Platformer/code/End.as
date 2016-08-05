package code {
	
	import flash.display.MovieClip;
	
	import flash.events.Event
	import flash.events.MouseEvent
	
	public class End extends MovieClip {
		
		var doc: Document;
		public function End(d: Document) {
			// constructor code
			doc = d;
			this.addEventListener(MouseEvent.CLICK, reset);
		}
		public function reset(evt:MouseEvent)
		{
			parent.removeChild(this);
			doc.init();
		}
	}
	
}
