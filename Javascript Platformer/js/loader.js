/*
loader.js
variable 'app' is in global scope - i.e. a property of window.
app is our single global object literal - all other functions and properties of 
the game will be properties of app.
*/
"use strict";

// if app exists use the existing copy
// else create a new empty object literal
var app = app || {};

//http://opengameart.org/content/side-scroller-sprite-base-animation
app.IMAGES = {
    idle: "img/idle.png",
    walk: "img/jog.png",
    jump: "img/jump.png"
 };

window.onload = function(){
	console.log("window.onload called");
	//app.main.keys = app.keys;
	console.log(app.main.keys);
	//audio
	app.sound.init();
	//emitter
	app.Emitter.utils = app.utils;
	app.main.sound = app.sound;
	app.main.Emitter = app.Emitter;
	console.log(app.main.Emitter);
	//imgs
	app.queue = new createjs.LoadQueue(false);
	app.queue.installPlugin(createjs.Sound);
	app.queue.on("complete", function(e){
		console.log("images loaded called");
		app.main.init();
	});

	app.queue.loadManifest([
     {id: "idle", src:app.IMAGES.idle},
     {id: "walk", src:app.IMAGES.walk},
     {id: "jump", src:app.IMAGES.jump}
	]);
}

window.onblur = function(){
	console.log("blur at " + Date());
	app.main.pauseGame();
};

window.onfocus = function(){
	console.log("focus at " + Date());
	app.main.resumeGame();
};