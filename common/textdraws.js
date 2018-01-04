API.onUpdate.connect(function() {
    var res = API.getScreenResolutionMaintainRatio();
	API.drawText("Corso RP", (res.Width / 2) + 818.4375, (res.Height / 2) - 540, 0.69, 255, 255, 255, 255, 1, 1, true, false, 0); 
});