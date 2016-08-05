// The myKeys object will be in the global scope - it makes this script 
// really easy to reuse between projects

"use strict";

var app = app || {};

var myKeys = {};

myKeys.KEYBOARD = (function(){
	var keys = {
		"KEY_LEFT": 37, 
		"KEY_UP": 38, 
		"KEY_RIGHT": 39, 
		"KEY_DOWN": 40,
		"KEY_SPACE": 32,
		"KEY_SHIFT": 16
	}
	var letters = ['W','A','S','D','P','B','R','I','w','a','s','d','p','b','r','i'];
	letters.forEach( function(l) {
		keys['KEY_'+l] = l.charCodeAt(0);
	});
	Object.freeze(keys);
	return keys;
})();
console.log(myKeys.KEYBOARD);

// myKeys.keydown array to keep track of which keys are down
// this is called a "key daemon"
// main.js will "poll" this array every frame
// this works because JS has "sparse arrays" - not every language does
myKeys.keydown = [];
myKeys.active = [];

// event listeners
window.addEventListener("keydown",function(e){
	if(myKeys.keydown[e.keyCode] == true) return;
	myKeys.keydown[e.keyCode] = true;
	//app.main.keys = myKeys.keydown;
	console.log("WORK");
	switch(e.keyCode) {
		case myKeys.KEYBOARD.KEY_LEFT:
		case myKeys.KEYBOARD.KEY_A:
		case myKeys.KEYBOARD.KEY_a:
			myKeys.active['LEFT'] = true;
			break
		case myKeys.KEYBOARD.KEY_UP:
		case myKeys.KEYBOARD.KEY_W:
		case myKeys.KEYBOARD.KEY_w:
			myKeys.active['UP'] = true;
			break
		case myKeys.KEYBOARD.KEY_RIGHT:
		case myKeys.KEYBOARD.KEY_D:
		case myKeys.KEYBOARD.KEY_d:
			myKeys.active['RIGHT'] = true;
			break
		case myKeys.KEYBOARD.KEY_DOWN:
		case myKeys.KEYBOARD.KEY_S:
		case myKeys.KEYBOARD.KEY_s:
			myKeys.active['DOWN'] = true;
			break			
	}
	//restart
	if(app.main.gameState == app.main.GAME_STATE.END){
		app.main.reset(0);
		app.main.gameState = app.main.GAME_STATE.DEFAULT;
		app.main.level = 0;
		app.main.seconds = 0;
	}
	app.main.keys = myKeys.active;
});
	
window.addEventListener("keyup",function(e){
	myKeys.keydown[e.keyCode] = false;
	//app.main.keys = myKeys.keydown;
	console.log(e.keyCode);
	console.log(myKeys.KEYBOARD.KEY_I);
	switch(e.keyCode) {
		case myKeys.KEYBOARD.KEY_LEFT:
		case myKeys.KEYBOARD.KEY_A:
		case myKeys.KEYBOARD.KEY_a:
			myKeys.active['LEFT'] = false;
			break;
		case myKeys.KEYBOARD.KEY_UP:
		case myKeys.KEYBOARD.KEY_W:
		case myKeys.KEYBOARD.KEY_w:
			myKeys.active['UP'] = false;
			break;
		case myKeys.KEYBOARD.KEY_RIGHT:
		case myKeys.KEYBOARD.KEY_D:
		case myKeys.KEYBOARD.KEY_d:
			myKeys.active['RIGHT'] = false;
			break;
		case myKeys.KEYBOARD.KEY_DOWN:
		case myKeys.KEYBOARD.KEY_S:
		case myKeys.KEYBOARD.KEY_s:
			myKeys.active['DOWN'] = false;
			break;
		case myKeys.KEYBOARD.KEY_I:
		case myKeys.KEYBOARD.KEY_i:
			console.log("INSTRUCT");
			if (app.main.gameState == app.main.GAME_STATE.BEGIN) { 
				app.main.gameState = app.main.GAME_STATE.INSTRUCT;
			}
			break;
		//pause and resume
		case myKeys.KEYBOARD.KEY_P:
		case myKeys.KEYBOARD.KEY_p:
			if (app.main.paused){
				app.main.resumeGame();
			} else if (app.main.gameState == app.main.GAME_STATE.BEGIN || app.main.gameState == app.main.GAME_STATE.INSTRUCT) { 
				app.main.gameState = app.main.GAME_STATE.DEFAULT;
			} else{
				app.main.pauseGame();
			}
			break;
		// debug view
		case myKeys.KEYBOARD.KEY_B:
		case myKeys.KEYBOARD.KEY_b:
			app.main.toggleDebug();
			break;
		//restart game
		case myKeys.KEYBOARD.KEY_R:
		case myKeys.KEYBOARD.KEY_r:
			app.main.reset(app.main.level);
			break;
	}
	app.main.keys = myKeys.active;
});