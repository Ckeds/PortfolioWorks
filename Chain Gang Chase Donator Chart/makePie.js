function getTotal(){
var myTotal = 0;
for (var i = 0; i < myData.length; i++) {
myTotal += myData[i];
}
return myTotal;
}

function plotData() {
var canvas;
var ctx;
var lastend = 0;
var myTotal = getTotal();
console.log(myTotal);

canvas = document.getElementById("canvas");
ctx = canvas.getContext("2d");
ctx.clearRect(0, 0, canvas.width, canvas.height);

for (var i = 0; i < myData.length; i++) {
ctx.fillStyle = myColor[i];
ctx.beginPath();
ctx.moveTo(200,150);
ctx.arc(200,150,150,lastend,lastend+
  (Math.PI*2*(myData[i]/myTotal)),false);
ctx.lineTo(200,150);
ctx.fill();
lastend += Math.PI*2*(myData[i]/myTotal);
}
}

