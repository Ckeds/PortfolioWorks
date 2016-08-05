var app = app || {};

app.main = {
	//  properties
	sound : undefined,
	gameState : undefined,
	keys : undefined,
	level : 0,
	paused: false,
	animationID: 0,
    WIDTH : 640, 
    HEIGHT: 480,
    canvas: undefined,
    Emitter: undefined,
    switchEmit: undefined,
    ctx: undefined,
    player: {},
    switches: [],
    platforms: [],
    levels: [
		"levels/level1.json",
		"levels/level2.json",
		"levels/level3.json",
		"levels/level4.json",
		"levels/level5.json",
		"levels/level6.json"
		],
	levelData: {},
   	lastTime: 0, // used by calculateDeltaTime() 
    debug: false,
    // gamestates
	GAME_STATE: Object.freeze({ // another fake enumeration
		BEGIN : 0,
		DEFAULT : 1,
		INSTRUCT : 2,
		ROUND_OVER : 3,
		REPEAT_LEVEL : 4,
		END : 5
	}),
	SPRITES: {
		walk: undefined,
		jump: undefined,
		idle: undefined,
		width: 64,
		height: 64,
		walkFrames: 6,
		jumpFrames: 1,
		idleFrames: 1
	},
	seconds: 0,
	timing: false,
	scores: [],
	
	//methods
	init : function() {
		console.log("app.main.init() called");
		// initialize properties
		this.canvas = document.querySelector('canvas');
		this.canvas.width = this.WIDTH;
		this.canvas.height = this.HEIGHT;
		this.ctx = this.canvas.getContext('2d');
		
		var image = new Image();
		image.src =  app.IMAGES['idle'];
		this.SPRITES.idle = image;
		
		image = new Image();
		image.src =  app.IMAGES['walk'];
		this.SPRITES.walk = image;
		
		console.log(this.Emitter);
		var emit = new this.Emitter();
		emit.numParticles = 100;
		emit.red = 255;
		
		this.switchEmit = emit;
		
		image = new Image();
		image.src =  app.IMAGES['jump'];
		this.SPRITES.jump = image;
		
		//local high scores
		if(localStorage.getItem('scores') != null) {
			this.scores = localStorage.getItem('scores').split(',');
			for(var i=0; i < this.scores.length; i++) {
				this.scores[i] = Number(this.scores[i]);
			}
			this.scores.sort(function(a, b) {
			  return a - b;
			});
		}
		console.log(this.scores);
		
		this.gameState = this.GAME_STATE.BEGIN;
		this.keys = [];
		
		this.reset(0);
		//this.reset();
		console.log("this.circles = " + this.circles);
		
		this.update();
	},
	reset: function(level){
		this.makePlayer();
		this.sound.playBGAudio();
		var request = new XMLHttpRequest();
   		request.open("GET", this.levels[level], false);
   		request.send(null)
   		console.log(request);
		this.levelData = JSON.parse(request.responseText);
		this.platforms = this.makePlatforms();
		this.switches = this.makeSwitches();
		this.timing = true;
	},
	pauseGame: function(){
		this.paused = true;
		cancelAnimationFrame(this.animationID);
		this.sound.stopBGAudio();
		this.update();
		this.timing = false;
	},
	resumeGame: function(){
		cancelAnimationFrame(this.animationID);
		this.paused = false;
		this.sound.playBGAudio();
		this.update();
		this.timing = true;
	},
	drawPauseScreen: function(ctx){
		ctx.save();
		ctx.fillStyle = "black";
		ctx.fillRect(0,0,this.WIDTH,this.HEIGHT);
		ctx.textAlign = "center";
		ctx.textBaseline = "middle";
		this.fillText(this.ctx,"...PAUSED...",this.WIDTH/2,this.HEIGHT/2, "40pt courier", "white");
		ctx.restore(); 
	},
	drawStartScreen: function(ctx){
		ctx.save();
		ctx.fillStyle = "black";
		ctx.fillRect(0,0,this.WIDTH,this.HEIGHT);
		ctx.textAlign = "center";
		ctx.textBaseline = "middle";
		this.fillText(this.ctx,"Project: Platform",this.WIDTH/2,this.HEIGHT/2.8, "40pt courier", "white");
		this.fillText(this.ctx,"Press 'P' to begin",this.WIDTH/2,this.HEIGHT/1.8, "20pt courier", "red");
		this.fillText(this.ctx,"Press 'I' for instructions",this.WIDTH/2,this.HEIGHT/1.4, "20pt courier", "blue");
		ctx.restore(); 
	},
	drawInstructScreen: function(ctx){
		ctx.save();
		ctx.fillStyle = "black";
		ctx.fillRect(0,0,this.WIDTH,this.HEIGHT);
		ctx.textAlign = "center";
		ctx.textBaseline = "middle";
		this.fillText(this.ctx,"Instructions",this.WIDTH/2,this.HEIGHT/3.8, "30pt courier", "white");
		this.fillText(this.ctx,"WASD/Arrow Keys to move",this.WIDTH/2,this.HEIGHT/2.4, "20pt courier", "gray");
		this.fillText(this.ctx,"Reach the goal as fast as possible",this.WIDTH/2,this.HEIGHT/2, "20pt courier", "gray");
		this.fillText(this.ctx,"Press 'P' to begin",this.WIDTH/2,this.HEIGHT/1.6, "20pt courier", "red");
	},
	update: function(){
		// 1) LOOP
		// schedule a call to update()
	 	this.animationID = requestAnimationFrame(this.update.bind(this));
	 	
	 	// 2) PAUSED?
	 	// if so, bail out of loop
	 	if(this.paused){
	 		this.drawPauseScreen(this.ctx);
	 		return;
	 	}
	 	if(this.gameState == this.GAME_STATE.BEGIN){
	 		this.drawStartScreen(this.ctx);
	 		return;
	 	}
	 	if(this.gameState == this.GAME_STATE.INSTRUCT){
	 		this.drawInstructScreen(this.ctx);
	 		return;
	 	}
		
	 	// 3) HOW MUCH TIME HAS GONE BY?
	 	var dt = this.calculateDeltaTime();
		if(this.timing && this.gameState != this.GAME_STATE.END) {
			this.seconds += dt;
		}
	 	//CLEAR
	 	this.ctx.fillStyle = "black"; 
		this.ctx.fillRect(0,0,this.WIDTH,this.HEIGHT); 
		
	 	if(this.gameState != this.GAME_STATE.END)
	 	{ 
		 	// 4) UPDATE
		 	// move
			this.movePlayer(dt);
			this.checkForCollisions();
	 		
			// 5) DRAW	
			// i) draw background

		
			// ii) draw
			this.ctx.globalAlpha = 1.0;
			this.player.draw(this.ctx);
			this.player.frame+=dt*6;
			
			this.drawPlatforms(this.ctx);
			
			//console.log(this.switches);
			for(var i=0; i < this.switches.length; i++ )
			{
				this.switches[i].draw(this.ctx);
			}
		}
		// iii) draw HUD
		this.ctx.globalAlpha = 1.0;
		this.drawHUD(this.ctx);
		
		// iv) draw debug info
		if (this.debug){
			// draw dt in bottom right corner
			this.fillText(this.ctx,"dt: " + dt.toFixed(3), this.WIDTH - 150, this.HEIGHT - 10, "18pt courier", "white");
		}
		
		//check if beat level
		if(this.gameState == this.GAME_STATE.ROUND_OVER) {
			this.level++;
			if(this.level < this.levels.length) {
				this.reset(this.level);
				this.gameState = this.GAME_STATE.DEFAULT;
			}
			else {
				console.log('You win!');
				this.gameState = this.GAME_STATE.END;
				this.timing = false;
				this.scores.push(this.seconds);
				this.scores.sort(function(a, b) {
				  return a - b;
				});
				console.log(this.scores + ' = ' + this.scores.slice(0,10).join(','));
				localStorage.setItem("scores", this.scores.slice(0,10).join(','));
			}
			return;
		}
	},
	drawPlatforms: function(ctx){
		for(var i=0;i<this.platforms.length; i++){
			this.platforms[i].draw(ctx);
		}
	},
	checkForCollisions: function(){
		for(var i = 0; i < this.platforms.length; i++)
		{
			if(Math.abs(this.player.x - this.platforms[i].x) < (this.player.width + this.platforms[i].width) / 2
				&& Math.abs(this.player.y - this.platforms[i].y) < (this.player.height + this.platforms[i].height) / 2){
				if(Math.abs(this.player.x - this.platforms[i].x) < (this.player.width / 1.2 + this.platforms[i].width) / 2)
				{
					if(this.player.y < this.platforms[i].y){
						this.player.yVel = 0;
						this.player.y = this.platforms[i].y - this.player.height / 2 - this.platforms[i].height / 2;
						this.player.jump = false;
					}
					else
					{
						if(this.player.yVel < 0){ this.player.yVel = 9.8; }
						this.player.y = this.platforms[i].y + this.player.height / 2 + this.platforms[i].height / 2;
					}
				}
				else
				{
					if(this.player.x < this.platforms[i].x){
						this.player.xVel = 0;
						this.player.x = this.platforms[i].x - this.player.width / 2 - this.platforms[i].width / 2;
					}
					else{
						this.player.xVel = 0;
						this.player.x = this.platforms[i].x + this.player.width / 2 + this.platforms[i].width / 2;
					}
				}
			}
		}
		for(var i = 0; i < this.switches.length; i++) {
			if(Math.abs(this.player.x - this.switches[i].x) < (this.player.width + this.switches[i].width) / 2
				&& Math.abs(this.player.y - this.switches[i].y) < (this.player.height + this.switches[i].height) / 2)
			{
				if(!this.switches[i].on){
					this.sound.playSwitch();
					this.switchEmit.createParticles({x:this.switches[i].x, y:this.switches[i].y});
				}
				this.switches[i].toggle();
				
				console.log(this.switchEmit);
			}
		}
	},
	movePlayer: function(dt){
		//move platforms
		for(var i=0;i<this.platforms.length; i++){
			this.platforms[i].x += this.platforms[i].xVel*dt;
			this.platforms[i].y += this.platforms[i].yVel*dt;
			if(this.platforms[i].xVel != 0 && this.platforms[i].goal) {
				if(this.platforms[i].x <= this.platforms[i].goal[0]) {
					this.platforms[i].xVel = 0;
				}
			}
			if(this.platforms[i].yVel != 0 && this.platforms[i].goal) {
				if(this.platforms[i].y <= this.platforms[i].goal[1]) {
					this.platforms[i].yVel = 0;
				}
			}
		}
		
		var xSpeed = 0;
		if(this.keys['UP']) {
			if(!this.player.jump){
				this.player.yVel = -300;
				this.player.jump = true;
				this.sound.playJump();
			}
		}
		if(this.keys['RIGHT'] && !this.keys['LEFT']) {
			xSpeed = this.player.xVel + 40;
			if(xSpeed >= 150)
			{
				xSpeed = 150;
				this.player.walk = true;
				this.player.facing = 1;
			}
		}
		else if(this.keys['LEFT']) {
			xSpeed = this.player.xVel - 40;
			if(xSpeed <= -150)
			{
				xSpeed = -150;
				this.player.walk = true;
				this.player.facing = -1;
			}
		}
		else {
			this.player.walk = false;
		}
		
		if(Math.abs(xSpeed) < 20)
		{
			xSpeed = 0;
		}
		else
		{
			if(xSpeed < 0)
			{
				xSpeed += 20;
			}
			else
			{
				xSpeed -= 20;
			}
		}
		if(this.player.x - this.player.width >= this.WIDTH && this.player.x <= this.WIDTH + this.player.width + 5) {
				this.gameState = this.GAME_STATE.ROUND_OVER;
			}
		this.player.yVel += 9.8;
		this.player.move(dt,xSpeed,this.player.yVel);
		if(this.player.y >= 400)
		{
			this.makePlayer();
		}
		if(this.player.x < 15) this.player.x = 15;
		//if(this.player.x > 625) this.player.x = 625;
		this.player.xVel = xSpeed;
	},
	toggleDebug: function(){
		this.debug = !this.debug;
	},
	fillText: function(ctx, string, x, y, css, color) {
		this.ctx.save();
		// https://developer.mozilla.org/en-US/docs/Web/CSS/font
		this.ctx.font = css;
		this.ctx.fillStyle = color;
		this.ctx.fillText(string, x, y);
		this.ctx.restore();
	},
	
	calculateDeltaTime: function(){
		// what's with (+ new Date) below?
		// + calls Date.valueOf(), which converts it from an object to a 	
		// primitive (number of milliseconds since January 1, 1970 local time)
		var now,fps;
		now = (+new Date); 
		fps = 1000 / (now - this.lastTime);
		fps = clamp(fps, 12, 60);
		this.lastTime = now; 
		return 1/fps;
	},
	makePlayer: function(){
		var playerDraw = function(ctx){
			var action;
			if(this.jump) {
				action = "jump";
			}
			else if(this.walk) {
				action = "walk";
			}
			else {
				action = "idle";
			}
			if(Math.floor(this.frame) >= this.IMAGES[action+'Frames']) {
				this.frame = 0;
			}
			this.image = this.IMAGES[action];
			//console.log(this.image);
			ctx.save();
			ctx.translate(this.x,this.y);
			var halfW = this.height/2;
			var halfH = this.height/2;
			if(!this.image){
				ctx.fillStyle = "red";
				ctx.fillRect(-halfW,-halfH, this.width, this.height);
				
			} else{
				if(this.facing < 0) {
					ctx.scale(-1,1);
				}
				//ctx.drawImage(this.image,-halfW, -halfH, this.width, this.height);
				// sprite animation will go here
				var col = Math.floor(this.frame);
				var imageX = col * this.IMAGES.width;
				ctx.drawImage( 
				this.image,        			// the image of the sprite sheet 
				imageX, 0, this.IMAGES.width, this.IMAGES.height, // source
				-halfW, -halfH, this.height, this.height  // destination
				); 
			}	
			ctx.restore();
		};
		var playerMove = function(dt, speedX, speedY){
			this.x += speedX * dt;
			this.y += speedY * dt;
		};
		this.player.x = 15;
		this.player.y = 350;
		this.player.width = 20;
		this.player.height = 40;
		this.player.xVel = 0;
		this.player.yVel = 0;
		this.player.jump = false;
		this.player.walk = false;
		this.player.facing = 1;
		this.player.frame = 0;
		this.player.image = undefined;
		this.player.IMAGES = this.SPRITES;
		this.player.draw = playerDraw;
		this.player.move = playerMove;
		
		Object.seal(this.player);
	},
	makePlatforms: function(){
		var platDraw = function(ctx){
			ctx.save();
			ctx.fillStyle = "white";
			ctx.fillRect(this.x - this.width / 2, this.y - this.height / 2, this.width, this.height);
			ctx.restore();
		};
		/*var platMove = function(dt, speedX, speedY){
			this.x += speedX * dt;
			this.y += speedY * dt;
		};*/
		var array = [];
		
		var levelData = this.levelData;
		for(var i = 0; i < levelData.platforms.length; i++){ 
			var p = {};
			p.x = levelData.platforms[i].x;
			p.y = levelData.platforms[i].y;
			p.width = levelData.platforms[i].width;
			p.height = levelData.platforms[i].height;
			p.xVel = 0;
			p.yVel = 0;
			p.draw = platDraw;
			if(levelData.platforms[i].goal){
				p.goal = levelData.platforms[i].goal;
			}
			Object.seal(p);
			array.push(p);
		}
		var p = {};
		p.x = 680;
		p.y = 370;
		p.width = 80;
		p.height = 10;
		p.xVel = 0;
		p.yVel = 0;
		p.draw = platDraw;
		Object.seal(p);
		array.push(p);
		console.log(array);
		return array;
	},
	makeSwitches: function() {
		console.log('here');
		var array = [];
		var that = this;
		
		var levelData = this.levelData;
		if(!levelData.switches) return [];
		console.log(levelData);
		for(var i = 0; i < levelData.switches.length; i++){
			var data = levelData.switches[i];
			var plat = that.platforms[data.platform];
			var s = new Switch(data.x,data.y,data.width,data.height,false,true,function(clicked){
				plat.xVel = (plat.goal[0]-plat.x)/5;
				plat.yVel = (plat.goal[1]-plat.y)/5;
				console.log('click! ('+plat.xVel+','+plat.yVel+')');
			});
			Object.seal(s);
			array.push(s);
		}
		console.log(array);
		return array;
	},
	drawHUD: function(ctx){
		
		ctx.save(); // NEW
		// draw time
      	// fillText(string, x, y, css, color)
		ctx.textAlign = 'left';
		this.fillText(this.ctx,this.formatTime(this.seconds), 20, 20, "14pt courier", "#ddd");
		//this.fillText(this.ctx,"Total Score: " + this.totalScore, this.WIDTH - 200, 20, "14pt courier", "#ddd");
		ctx.restore();
	
		if(this.gameState == this.GAME_STATE.REPEAT_LEVEL){
			ctx.save();
			ctx.textAlign = "center";
			ctx.textBaseline = "middle";
			this.fillText(this.ctx,"Round Failed", this.WIDTH/2, this.HEIGHT/2 - 20, "30pt courier", "red");
			this.fillText(this.ctx,"Click to Restart Level", this.WIDTH/2, this.HEIGHT/2 + 20, "30pt courier", "red");
			
		} // end if
		
		if(this.gameState == this.GAME_STATE.END){
			ctx.textAlign = "center";
			ctx.textBaseline = "middle";
			var str = "The Game is over";
			if(this.seconds == this.scores[0]) {
				str = "New High Score!";
			}
			this.fillText(this.ctx,str, this.WIDTH/2, 100, "30pt courier", "red");
			this.fillText(this.ctx,"High Scores", this.WIDTH/2, 140, "20pt courier", "#ddd");
		
			var bigNum = 360000-.01;
			for(var i = 0; i < 10; i++) {
				if(i >= this.scores.length) {
					this.scores.push(bigNum);
				}
				ctx.textAlign = 'left';
				this.fillText(this.ctx,"#"+(i+1)+":", this.WIDTH/2-75, 170+i*(this.HEIGHT-240)/10, "12pt courier", "#ddd");
				ctx.textAlign = 'right';
				this.fillText(this.ctx,this.formatTime(this.scores[i]), this.WIDTH/2+75, 170+i*(this.HEIGHT-240)/10, "12pt courier", "#ddd");
			}
			ctx.textAlign = 'center';
			this.fillText(this.ctx,"Press any key to begin again", this.WIDTH/2, this.HEIGHT-30, "20pt courier", "red");
		} // end if
		
		ctx.restore(); // NEW
	},
	formatTime: function(seconds) {
		var time = '';
		if(seconds >= 3600) {
			time += Math.floor(seconds/3600) + ':';
			if(Math.floor(seconds)%60 < 10) {
				time += '0';
			}
		}
		if( seconds >= 60 ) {
			time += Math.floor(seconds/60)%60 + ':';
			if(Math.floor(seconds)%60 < 10) {
				time += '0';
			}
		}
		time += Math.floor(seconds)%60+':';
		var milli = Math.floor(seconds*100)%100;
		if(milli < 10) {
			time += '0';
		}
		time += milli;
		return time;
	}		
}; // end app.main