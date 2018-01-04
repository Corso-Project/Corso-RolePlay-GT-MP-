var login_page = null;

function user_register(username, password) {
	API.showCursor(false);
	API.setCanOpenChat(true);
	API.destroyCefBrowser(login_page);
	API.triggerServerEvent("User_Register", username, password);
}

API.onServerEventTrigger.connect(function (eventName, args) {
	switch (eventName) {
		case 'Player_Register':
			var res = API.getScreenResolution();
			login_page = API.createCefBrowser(res.Width, res.Height);
			API.waitUntilCefBrowserInit(login_page);
			API.setCefBrowserPosition(login_page, 0, 0);
			API.loadPageCefBrowser(login_page, "/web/register/index.html");
			API.showCursor(true);
			API.setCanOpenChat(false);
		break;
	}
});
