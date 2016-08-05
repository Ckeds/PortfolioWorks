// sound.js
"use strict";
// if app exists use the existing copy
// else create a new object literal
var app = app || {};

// define the .sound module and immediately invoke it in an IIFE
app.sound = (function(){
	console.log("sound.js module loaded");
	var bgAudio = undefined;
	var jumpAudio = undefined;
	var switchAudio = undefined;
	
	

	function init(){
		bgAudio = document.querySelector("#bgAudio");
		bgAudio.volume=0.2;
		jumpAudio = document.querySelector("#jumpAudio");
		jumpAudio.volume = 0.4;
		switchAudio = document.querySelector("#switchAudio");
		switchAudio.volume = 0.4;
	}
		
	function stopBGAudio(){
		bgAudio.pause();
		bgAudio.currentTime = 0;
	}
	
	function playJump(){
		jumpAudio.play();
	}
	function playSwitch(){
		switchAudio.play();
	}
	function playBGAudio(){
		bgAudio.play();
	}
		
	// export a public interface to this module
	return{
		init: init,
		playBGAudio: playBGAudio,
		stopBGAudio: stopBGAudio,
		playJump: playJump,
		playSwitch: playSwitch,
	};
}());