package code {		import flash.display.MovieClip;
	import flash.display.*;	import flash.events.KeyboardEvent;	import flash.ui.Keyboard;	import flash.events.Event;	import flash.net.URLRequest;	import flash.display.Loader;	import flash.net.URLLoader;
	import flash.text.TextField;
	import flash.text.TextFormat;	import flash.geom.Rectangle;
	import flash.utils.Endian;
	import flash.events.MouseEvent;
		public class Document extends MovieClip {				// Variables
		public static var hasLegs: Boolean = false;
		private var hasWings: Boolean = false;		private var box: Box;		private var platforms:Array;		private var powerUps:Array;		private var levels:Array;
		private var demons:Array;		private var isRightPressed: Boolean = false;		private var isLeftPressed: Boolean = false;		private var isUpPressed: Boolean = false;
		private var isSpacePressed: Boolean = false;		private var upKeyHeld: Boolean = false;		private var keyFound: Boolean = false;		private var isDownPressed: Boolean = false;
		private var hasTorso: Boolean = false;
		private var hasArms: Boolean = false;
		private var gameComplete: Boolean = false;		private var ldr = new URLLoader();		private var level:Number;
		private var fader: Number = .0196;		private var currentLevel:Number = 0;
		private var pauseText:TextField;
		private var pauseShader: Shape;
		private var myFont: Font1;
		private var myFormat:TextFormat;
		private var messages: Array;
		private var bodyStatus: Number = 0;
		private var currentMessage: Number;
		private var frames: Number;
		private var gainPower: MovieClip;
		private var currWalkAni: MovieClip;
		var _textbox: TextField = new TextField;				public function Document() {			// constructor code			init();
			
		}
		public function init()
		{			platforms = new Array();			powerUps = new Array();
			messages = new Array();
			demons = new Array();			//add levels here, they are in order of appearance			levels = ["xml/level00.xml","xml/level01.xml","xml/level02.xml", "xml/ConfusionLevel.xml", "xml/PitLevel.xml", 
			"xml/Pit2Level.xml", "xml/LonelinessLevel.xml", "xml/DepressionLevel.xml", "xml/PressureLevel.xml", 
			"xml/AbuseLevel.xml", "xml/AnxietyLevel.xml", "xml/ViolenceLevel1.xml", "xml/ViolenceLevel2.xml",
			"xml/DemonsLevel.xml", "xml/Demons2Level.xml"];			//debug purpose: test one level.			//levels = ["xml/level00.xml"];			box = new Box(this);			box.x = 250;			box.y = 100;			
			//This block will set up the pause screen
			myFont = new Font1();
			//assemble the format
			myFormat = new TextFormat();
			myFormat.size = 25;
			myFormat.align = "center";
			myFormat.font = myFont.fontName;
			//assemble the text box
			pauseText = new TextField();
			pauseText.defaultTextFormat = myFormat;
			pauseText.textColor = 0xFFFFFF;
			pauseText.text = "Paused";
			//locate the textbox
			pauseText.width = 75;
			pauseText.height = 30;
			pauseText.x = (stage.stageWidth - pauseText.width) / 2;
			pauseText.y = (stage.stageHeight - pauseText.height) / 2;
			//create the screen shader for the pause screen
			pauseShader = new Shape();
			pauseShader.graphics.beginFill(0X000000, 0.7);
			pauseShader.graphics.drawRect(0, 0, stage.stageWidth, stage.stageHeight);
			pauseShader.graphics.endFill();
			
						ldr.addEventListener(Event.COMPLETE, xmlComplete);						stage.addEventListener(Event.ENTER_FRAME, onFrame);			stage.addEventListener(KeyboardEvent.KEY_DOWN, onKeyPressed);			stage.addEventListener(KeyboardEvent.KEY_UP, onKeyRelease);						//holds game at title screen						gotoAndStop("title");		}		private function levelCompleted() {			//make next level			currentLevel++;			if(currentLevel >= levels.length){				currentLevel = 0;
				gameComplete = true;			}			//remove everything so we may draw the next level			for (var i:int = this.numChildren-1; i >= 0; i--) {				this.removeChildAt (i);			}			
			if(!gameComplete)
			{				//loader for next level				var xmlPath = levels[currentLevel];				var xmlReq = new URLRequest(xmlPath);				ldr.addEventListener(Event.COMPLETE, xmlComplete);				ldr.load(xmlReq);
			}
			else
			{
				addChild(new End(this));
			}		}		public function levelReset() {			for (var i:int = this.numChildren-1; i >= 0; i--) {				this.removeChildAt (i);			}			//reload the level			var xmlPath = levels[currentLevel];			var xmlReq = new URLRequest(xmlPath);			ldr.addEventListener(Event.COMPLETE, xmlComplete);			ldr.load(xmlReq);		}		//think this is all set for when we test an xml file		private function xmlComplete(e:Event):void {			//resets the arrays and variables			platforms = new Array();			powerUps = new Array();
			messages = new Array();
			demons = new Array();
			currentMessage = 0;
			frames = 0;			keyFound = false;			//remove so complete only goes once			ldr.removeEventListener(Event.COMPLETE, xmlComplete);			var myXML:XML = new XML( e.target.data );			Depressed.RaiseHeight = 0;			Raiser.resetClicks();			// myXML is effectively references the <gallery> tag			for each (var platform:XML in myXML.platform) 			{				//first determine the type of platform				var _platform:Platform;				if(platform.type == "glass")				{					_platform = new Glass;					_platform.platformType = "glass";				}				else if(platform.type == "goal")				{					_platform = new Goal;					_platform.platformType = "goal";				}				else if(platform.type == "raiser")				{					_platform = new Raiser;					_platform.platformType = "raiser";				}				else if(platform.type == "depressed")				{					_platform = new Depressed;					_platform.platformType = "depressed";				}				else if(platform.type == "resetDepressed")				{					_platform = new Platform;					_platform.platformType = "resetDepressed";				}				else if(platform.type == "spike")				{					_platform = new Spikes;					_platform.platformType = "spikes";				}
				else if(platform.type == "beater")
				{
					_platform = new Beater(platform.startLeft == "true", platform.xMovable, platform.x);
					_platform.platformType = "beater";
				}				else if(platform.type == "fakeGoal")				{					_platform = new FakeGoal;					_platform.platformType = "fakeGoal";				}				else if(platform.type == "pressurePlatform")				{					_platform = new PressurePlatform(platform.direction);					_platform.platformType = "pressurePlatform";					trace("some pressure platforms");				}
				else if(platform.type == "destructible")
				{
					_platform = new Destructible();
					_platform.platformType = "destructible";
				}
				else if(platform.type == "legBreaker")
				{
					_platform = new Platform();
					_platform.platformType = "legBreaker";
				}
				else if(platform.type == "fallingRock")
				{
					_platform = new FallingRock();
					_platform.platformType = "fallingRock";
					trace("ammo");
				}				else				{					_platform = new Platform;					_platform.platformType = "platform";				}								//next set its location and sizes
				if (platform.x == "?") {
					_platform.x = (Math.random() * 500) + 80;
				}
				else {					_platform.x = platform.x;
				}				_platform.y = platform.y;				_platform.height = platform.height;				_platform.width = platform.width;								//add it to the platform array				platforms.push(_platform);			}			for each (var powerUp:XML in myXML.powerup) 			{				var _power:PowerUp;				if(powerUp.type == "key")				{					_power = new GoalKey();					_power.powerUpType = "key";					trace("Vegeta, Look! It's a KEY!");				}
				else if(powerUp.type == "legs")
				{
					_power = new Legs();
					_power.powerUpType = "legs";
					if(bodyStatus == 1)
					{
						removeBodyStatus();
					}
				}
				else if(powerUp.type == "torso")
				{
					_power = new Torso();
					_power.powerUpType = "torso";
					if(bodyStatus == 2)
					{
						removeBodyStatus();
					}
				}
				else if(powerUp.type == "arms")
				{
					_power = new Arms();
					_power.powerUpType = "arms";
					if(bodyStatus == 3)
					{
						removeBodyStatus();
					}
				}
				else if(powerUp.type == "wings")
				{
					_power = new Wings();
					_power.powerUpType = "wings";
					if(bodyStatus == 4)
					{
						removeBodyStatus();
					}
				}				else				{					_power = new PowerUp();					_power.powerUpType = "unknown";				}				//next set its location and sizes				_power.x = powerUp.x;				_power.y = powerUp.y;				_power.height = powerUp.height;				_power.width = powerUp.width;				_power.gotoAndStop(1);				powerUps.push(_power);			}			//add box because everything will be cleared after a level is done
			if(bodyStatus == 0)
			{				addChild(box);
			}
			else
			{
				bodyStatus--;
				addBodyStatus();
			}			//draw the platforms			for each (var p:Platform in platforms)			{				addChild(p);			}			for each (var pU:PowerUp in powerUps)			{				addChild(pU);			}
			for each (var villain:XML in myXML.enemy)
			{
				var _villain: Enemy;
				if(villain.type == "demon1")
				{
					_villain = new Demon1();
				}
				else if(villain.type == "demon2")
				{
					_villain = new Demon2();
				}
				else
				{
					_villain = new Demon3();
				}
				_villain.x = villain.x;
				_villain.y = villain.y;
				_villain.width = villain.width;
				_villain.height = villain.height;
				_villain.gotoAndPlay(1);
				demons.push(_villain);
				addChild(_villain);
				trace(demons);
			}
			for each (var textbox:XML in myXML.textbox)
			{
				_textbox = new TextField();
				_textbox.defaultTextFormat = myFormat;
				_textbox.background = true;
				_textbox.backgroundColor = 0x000000;
				_textbox.textColor = 0xFFFFFF;
				for each (var phrase:String in textbox.text)
				{
					messages.push(phrase);
				}
				_textbox.x = textbox.x;
				_textbox.y = textbox.y;
				_textbox.multiline = true;
				_textbox.wordWrap = true;
				_textbox.width = textbox.width;
				_textbox.height = textbox.height;
				addChild(_textbox);
			}
			box.x = myXML.startPosition.x;
			box.y = myXML.startPosition.y;
			
			//Had to be added because spikes
			box.xAccel = 0;
			box.yAccel = 0;		}					//determines what keys are pressed		public function onKeyPressed(ke: KeyboardEvent) {			if(ke.keyCode == Keyboard.RIGHT || ke.keyCode == Keyboard.D){				isRightPressed = true;
				if(currWalkAni != null)
				{
					currWalkAni.play();
					currWalkAni.scaleX = 1;
				}			}			if(ke.keyCode == Keyboard.LEFT || ke.keyCode == Keyboard.A){				isLeftPressed = true;
				if(currWalkAni != null)
				{
					currWalkAni.play();
					currWalkAni.scaleX = -1;
				}			}			if(ke.keyCode == Keyboard.UP || ke.keyCode == Keyboard.W){				isUpPressed = true;			}			if(ke.keyCode == Keyboard.DOWN){				isDownPressed = true;			}
			if(ke.keyCode == Keyboard.R){
				var deadMan: Dying = new Dying(this);
				deadMan.x = box.x;
				deadMan.y = box.y;
				addChild(deadMan);
				if(currWalkAni != null)
				{
					removeChild(currWalkAni);
				}
				else
				{
					removeChild(box);
				}
				box.y = 5000;
			}
			if(ke.keyCode == Keyboard.ESCAPE){
				var returnFromPause: Boolean = false;
				if(currentLabel == "pause")
				{
					gotoAndStop("game");
					returnFromPause = true;
					removeChild(pauseText);
					removeChild(pauseShader);
					if(gainPower != null)
					{
						gainPower.play();
					}
					if(currWalkAni != null)
					{
						currWalkAni.play();
					}
				}
				else if(currentLabel == "game")
				{
					if(!returnFromPause)
					{
						gotoAndStop("pause");
						addChild(pauseShader);
						addChild(pauseText);
						if(gainPower != null)
						{
							gainPower.stop();
						}
						if(currWalkAni != null && (isLeftPressed != false || isRightPressed != false))
						{
							currWalkAni.stop();
						}
					}				
				}
				else {}
			}
			if(ke.keyCode == Keyboard.SPACE){
				isSpacePressed = true;
			}
			if(currentLabel == "title")
			{
				gotoAndStop("game");
				
				//load game
				var xmlPath = levels[currentLevel];
				var xmlReq = new URLRequest(xmlPath);
				ldr.load(xmlReq);
			}		}					public function onKeyRelease(ke: KeyboardEvent){			//determines if keys are not pressed or stopped being pressed			if(ke.keyCode == Keyboard.RIGHT || ke.keyCode == Keyboard.D){				isRightPressed = false;
				if(currWalkAni != null)
				{
					currWalkAni.gotoAndStop(1);
				}			}			if(ke.keyCode == Keyboard.LEFT || ke.keyCode == Keyboard.A){				trace("got here");
				isLeftPressed = false;
				if(currWalkAni != null)
				{
					currWalkAni.gotoAndStop(1);
				}			}			if(ke.keyCode == Keyboard.UP|| ke.keyCode == Keyboard.W){				isUpPressed = false;				upKeyHeld = false;			}			if(ke.keyCode == Keyboard.DOWN){				isDownPressed = false;			}
			if(ke.keyCode == Keyboard.SPACE){
				isSpacePressed = false;
			}		}					public function onFrame(e: Event){
			if(currentLabel == "title")				
			{
				flasher_txt.alpha -= fader;
				if(flasher_txt.alpha <= 0.01 || flasher_txt.alpha >= 1)
				{
					if(flasher_txt.alpha > 1)
					{
						flasher_txt.alpha = 1;
					}
					else
					{
						flasher_txt.alpha = 0.01;
					}
					fader = -fader;
				}
			}
			if(currentLabel != "pause")
			{
				if(gainPower != null)
				{
					gainPower.x = box.x + (.5 * box.width);;
					gainPower.y = box.y + (.5 * box.height);
					if (gainPower.isPlaying == false)
					{
						addBodyStatus();
						removeChild(gainPower);
						gainPower = null;
					}
				}
				if(currWalkAni != null)
				{
					box.width = currWalkAni.width;
					box.height = currWalkAni.height;
					currWalkAni.x = box.x + (.5 * box.width);
					currWalkAni.y = box.y + (.5 * box.height);
				}
				if(currentMessage < messages.length)
				{
					_textbox.text = messages[currentMessage];
					frames++;
				}
				if(frames < 40)
				{
					_textbox.alpha = frames / 40;
				}
				if(frames > 120)
				{
					_textbox.alpha = (160 - frames) / 40;
				}				
				if (frames == 160)
				{
					currentMessage++;
					frames = 0;
					removeChild(_textbox);
					addChild(_textbox);
				}				//trace (Depressed.RaiseHeight);				//if the key is pressed, move object				if(isRightPressed) {					box.moveRight();				}				if(isLeftPressed) {					box.moveLeft();				}				if(isUpPressed){					if(!upKeyHeld)					{						box.moveUp();						upKeyHeld = true;					}				}
				if(isSpacePressed && hasWings){
					box.y -= 3;
					box.yAccel = -1;
					removeChild(currWalkAni);
					currWalkAni = new WingsJump();
					currWalkAni.x = box.x + (.5 * box.width);
					currWalkAni.y = box.y + (.5 * box.height);
					addChild(currWalkAni)
				}				if(isDownPressed){					box.moveDown();				}				box.movement();
				if (box.yAccel == 0 && hasWings){
					removeChild(currWalkAni);
					currWalkAni = new TorsoWalkingWings();
					addChild(currWalkAni);
				}							//move depressed blocks down if they aren't on the ground				if (Depressed.RaiseHeight >= .25)				{					Depressed.RaiseHeight -= .25;				}				else if (Depressed.RaiseHeight < .25)				{					Depressed.RaiseHeight = 0;					Raiser.resetClicks();				}
				for(var vCount:int = 0; vCount < demons.length; vCount++)
				{
					if(box.hitTestObject(demons[vCount]))
					{
						var deadMan: Dying = new Dying(this);
						deadMan.x = box.x;
						deadMan.y = box.y;
						addChild(deadMan);
						if(currWalkAni != null)
						{
							removeChild(currWalkAni);
						}
						else
						{
							removeChild(box);
						}
						box.y = 5000;
					}
				}				for(var pCount:int = 0; pCount < powerUps.length; pCount++)				{					if(box.hitTestObject(powerUps[pCount]))					{						if(powerUps[pCount].powerUpType == "key")						{							keyFound = true;							for each (var p: PressurePlatform in PressurePlatform.pressureList)							{								p.liftGate();							}						}
						else if(powerUps[pCount].powerUpType == "legs")
						{
							gainPower = new GetLegs();
							gainPower.gotoAndPlay(1);
							hasLegs = true;
							addChild(gainPower);
							removeChild(box);
						}
						else if(powerUps[pCount].powerUpType == "torso")
						{
							gainPower = new GetTorso();
							gainPower.gotoAndPlay(1);
							addChild(gainPower);
							removeChild(currWalkAni);
						}
						else if(powerUps[pCount].powerUpType == "arms")
						{
							gainPower = new GetArms();
							gainPower.gotoAndPlay(1);
							addChild(gainPower);
							removeChild(currWalkAni);
						}
						else if(powerUps[pCount].powerUpType == "wings")
						{
							gainPower = new GetWings();
							gainPower.gotoAndPlay(1);
							hasWings = true;
							addChild(gainPower);
							removeChild(currWalkAni);
						}						powerUps[pCount].parent.removeChild(powerUps[pCount]);						powerUps.splice(pCount);					}				}
				Destructible.idle();
			
			if (Destructible.revive())
			{
				platforms.push(Destructible.purgatory[0]);
				Destructible.purgatory.splice(0, 1);
			}				for(var i:uint = 0; i < platforms.length; i++)				{
					if(platforms[i].platformType == "pressurePlatform" && keyFound){
						PressurePlatform(platforms[i]).movePlatform();
					}
					
					if(platforms[i].platformType == "fallingRock"){
						FallingRock(platforms[i]).fall();
					}
						
					if(platforms[i].platformType == "destructible"){
						Destructible(platforms[i]).idle();
					}
					if(platforms[i].platformType == "beater"){
						Beater(platforms[i]).sense(box);
					}					checkCollision(platforms[i]);				}
			}		}		private function checkCollision(platform:Platform):void {			//distance from the center of the box (character) to the center of the platform			var dx:Number = (box.x + (box.width / 2)) - (platform.x + (platform.width / 2));			var dy:Number = (box.y + (box.height / 2)) - (platform.y + (platform.height / 2));						//is the box running into a platform?			if(box.hitTestObject(platform))			{				//add in the previous position to avoid shifting through platforms				dx += box.prevX - box.x;				dy += box.prevY - box.y;				//are you close to one of the sides, or to the top or bottom? (p.height / p.width) is a ratio for platform size				//Horizontal				if (Math.abs(dx) * (platform.height / platform.width) > Math.abs(dy)) {					//this qualifier allows to run on top of platforms placed right next to each other					if(box.prevY >= (platform.y - box.height + 2)) {						//left side						if (dx < 0) {							box.x = platform.x - box.width - .5;						}						//right side						else {							box.x = platform.x + platform.width;						}						box.xAccel = 0;					}				}				//Vertical				else {					//top					if (dy < 0) {						box.y = platform.y - box.height;						box.inAir = false;						box.doubleJump = false;					}					//bottom					else {						box.y = platform.y + platform.height + 1;					}					box.yAccel = 0;				}				//if we have run into the goal platform, go to the next level				if(platform.platformType == "goal")				{					trace("level done.");					levelCompleted();				}				//lets the game detect if a raiser button has been hit				else if(platform.platformType == "raiser")				{					//prevents repeating clicks					if (!Raiser(platform).Clicked)					{						Raiser(platform).raisePlatforms();					}				}				else if(platform.platformType == "spikes" || platform.platformType == "fakeGoal" ||
						platform.platformType == "fallingRock")				{
					if(dx - 5 < (platform.width / 2) && dy - 5 < (platform.height / 2)) {						var deadMan: Dying = new Dying(this);
						deadMan.x = box.x;
						deadMan.y = box.y;
						addChild(deadMan);
						if(currWalkAni != null)
						{
							removeChild(currWalkAni);
						}
						else
						{
							removeChild(box);
						}
						box.y = 5000;
					}				}
				else if(platform.platformType == "legBreaker")
				{
					if(bodyStatus == 1)
					{
						removeChild(currWalkAni);
						
						gainPower = new LegsBreak();
						gainPower.gotoAndPlay(1);
						hasLegs = false;
						addChild(gainPower);
						removeBodyStatus();
						bodyStatus--;
					}
				}				else if(platform.platformType == "resetDepressed")				{					Depressed.RaiseHeight = 0;					Raiser.resetClicks();				}				else if (platform.platformType == "destructible")				{
					trace("thingything");					Destructible(platform).touch();
					if (Destructible(platform).contact == 10)
					{
						removeThis(platform);
					}				}			}		}
		public function removeThis(platform:Platform)
		{
			for(var i:uint = 0; i < platforms.length; i++)
			{
				if (platform == platforms[i])
				{
					Destructible.purgatory.push(platforms[i]);
					platforms.splice(i, 1);
				}
			}
		}
		public function addBodyStatus()
		{
			bodyStatus++;
			if(bodyStatus <= 0){
				addChild(box);
			}
			else if(bodyStatus == 1){
				currWalkAni = new Walking();
			}
			else if(bodyStatus == 2){
				currWalkAni = new TorsoWalking();
			}
			else if(bodyStatus == 3){
				currWalkAni = new TorsoWalkingArms();
			}
			else{
				currWalkAni = new TorsoWalkingWings();
			}
			if(bodyStatus > 0){
				currWalkAni.stop();
				addChild(currWalkAni);
			}
		}
		public function removeBodyStatus()
		{
			trace("death");
			bodyStatus--;
			if(bodyStatus == 1){
				currWalkAni = new Walking();
			}
			else if(bodyStatus == 2){
				currWalkAni = new TorsoWalking();
			}
			else if(bodyStatus == 3){
				currWalkAni = new TorsoWalkingArms();
			}
			else{
				currWalkAni = null;
				box.width = 14;
				box.height = 14;
			}
		}
		public function reset(evt:MouseEvent)
		{
			trace("okay");
		}	}	}	