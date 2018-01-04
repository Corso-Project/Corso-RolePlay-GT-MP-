var resMR = API.getScreenResolutionMaintainRatio();
var moneyText = null;
var moneyNegative = false;

var changeText = null;
var changeNegative = false;
var changeTime = null;

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

API.onServerEventTrigger.connect(function (name, args) {
    if (name == "UpdateMoneyHUD")
    {
        var money = parseInt(args[0]); // disgusting hack for long support

        if (money < 0) {
            money = Math.abs(money);

            moneyNegative = true;
            moneyText = "-$" + numberWithCommas(money);
        } else {
            moneyNegative = false;
            moneyText = "$" + numberWithCommas(money);	
        }

        // money difference if received
        if(args.Length > 1)
        {
            var diff = parseInt(args[1]); // disgusting hack for long support

            if (diff < 0) {
                diff = Math.abs(diff);

                changeNegative = true;
                changeText = "-$" + numberWithCommas(diff);
            } else {
                changeNegative = false;
                changeText = "+$" + numberWithCommas(diff);
            }

            changeTime = API.getGlobalTime();
        }
    }
});

API.onUpdate.connect(function (sender, args) {
    if (!API.getHudVisible()) return;

    // money text
    if (moneyText == null) return;
    if (moneyNegative) {
        API.drawText(moneyText, resMR.Width - 19, 50, 0.6, 224, 50, 50, 255, 7, 2, false, true, 0);
    } else {
        API.drawText(moneyText, resMR.Width - 19, 50, 0.6, 114, 204, 114, 255, 7, 2, false, true, 0);
    }

    // change text
    if (changeText == null || changeTime == null) return;
    if (API.getGlobalTime() - changeTime <= 3500)
    {
        if (changeNegative) {
            API.drawText(changeText, resMR.Width - 19, 90, 0.6, 194, 80, 80, 255, 7, 2, false, true, 0);
        } else {
            API.drawText(changeText, resMR.Width - 19, 90, 0.6, 57, 102, 57, 255, 7, 2, false, true, 0);
        }
    }
});