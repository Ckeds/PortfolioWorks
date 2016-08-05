var app = app || {};

var Switch = function(x,y,w,h,on,sticks,action) {
	console.log(x+' '+y+' '+w+' '+h);
	this.x = x;
	this.y = y;
	this.width = w;
	this.height = h;
	this.color = '#ccc';
	this.on = on;
	this.staysOn = sticks;
	this.ontoggle = action;
	this.cooldown = 0;
}

Switch.prototype.draw = function(ctx) {
	app.main.switchEmit.updateAndDraw(ctx, {x:this.x, y:this.y});
	ctx.save();
	ctx.fillStyle = this.color;
	ctx.fillRect(this.x-this.width/2,this.y+this.height*.3,this.width,this.height*.2);
	//move to the middle of the switch part
	ctx.translate(this.x,this.y+this.height*.3);
	if(this.on) {
		ctx.rotate(Math.PI/6);
	}
	else {
		ctx.rotate(-Math.PI/6);
	}
	ctx.fillRect(-this.width*.1,-this.height*.8,this.width*.2,this.height*.8);
	ctx.restore();
	this.cooldown--;
};

Switch.prototype.toggle = function() {
	if(this.cooldown > 0) {
		return;
	}
	if(!this.staysOn || !this.on) {
		this.on = !this.on;
		this.ontoggle(this);
		this.cooldown = 10;
	}
};

app.Switch = Switch;