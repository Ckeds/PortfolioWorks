﻿package code {
	import flash.display.*;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.utils.Endian;
	import flash.events.MouseEvent;
	
		public static var hasLegs: Boolean = false;
		private var hasWings: Boolean = false;
		private var demons:Array;
		private var isSpacePressed: Boolean = false;
		private var hasTorso: Boolean = false;
		private var hasArms: Boolean = false;
		private var gameComplete: Boolean = false;
		private var fader: Number = .0196;
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
		var _textbox: TextField = new TextField;
			
		}
		public function init()
		{
			messages = new Array();
			demons = new Array();
			"xml/Pit2Level.xml", "xml/LonelinessLevel.xml", "xml/DepressionLevel.xml", "xml/PressureLevel.xml", 
			"xml/AbuseLevel.xml", "xml/AnxietyLevel.xml", "xml/ViolenceLevel1.xml", "xml/ViolenceLevel2.xml",
			"xml/DemonsLevel.xml", "xml/Demons2Level.xml"];
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
			
			
				gameComplete = true;
			if(!gameComplete)
			{
			}
			else
			{
				addChild(new End(this));
			}
			messages = new Array();
			demons = new Array();
			currentMessage = 0;
			frames = 0;
				else if(platform.type == "beater")
				{
					_platform = new Beater(platform.startLeft == "true", platform.xMovable, platform.x);
					_platform.platformType = "beater";
				}
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
				}
				if (platform.x == "?") {
					_platform.x = (Math.random() * 500) + 80;
				}
				else {
				}
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
				}
			if(bodyStatus == 0)
			{
			}
			else
			{
				bodyStatus--;
				addBodyStatus();
			}
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
			box.yAccel = 0;
				if(currWalkAni != null)
				{
					currWalkAni.play();
					currWalkAni.scaleX = 1;
				}
				if(currWalkAni != null)
				{
					currWalkAni.play();
					currWalkAni.scaleX = -1;
				}
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
			}
				if(currWalkAni != null)
				{
					currWalkAni.gotoAndStop(1);
				}
				isLeftPressed = false;
				if(currWalkAni != null)
				{
					currWalkAni.gotoAndStop(1);
				}
			if(ke.keyCode == Keyboard.SPACE){
				isSpacePressed = false;
			}
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
				}
				if(isSpacePressed && hasWings){
					box.y -= 3;
					box.yAccel = -1;
					removeChild(currWalkAni);
					currWalkAni = new WingsJump();
					currWalkAni.x = box.x + (.5 * box.width);
					currWalkAni.y = box.y + (.5 * box.height);
					addChild(currWalkAni)
				}
				if (box.yAccel == 0 && hasWings){
					removeChild(currWalkAni);
					currWalkAni = new TorsoWalkingWings();
					addChild(currWalkAni);
				}
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
				}
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
						}
				Destructible.idle();
			
			if (Destructible.revive())
			{
				platforms.push(Destructible.purgatory[0]);
				Destructible.purgatory.splice(0, 1);
			}
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
					}
			}
						platform.platformType == "fallingRock")
					if(dx - 5 < (platform.width / 2) && dy - 5 < (platform.height / 2)) {
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
				}
					trace("thingything");
					if (Destructible(platform).contact == 10)
					{
						removeThis(platform);
					}
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
		}