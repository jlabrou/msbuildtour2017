if (window.Windows && Windows.UI.Notifications) {

    var tileContent = new Windows.Data.Xml.Dom.XmlDocument();

    var tile = tileContent.createElement("tile");
    tileContent.appendChild(tile);

    var visual = tileContent.createElement("visual");
    tile.appendChild(visual);

    var bindingMedium = tileContent.createElement("binding");
    bindingMedium.setAttribute("template", "TileMedium");
    visual.appendChild(bindingMedium);

    var peekImage = tileContent.createElement("image");
    peekImage.setAttribute("placement", "peek");
    peekImage.setAttribute("src", "https://unsplash.it/150/150/?random");
    peekImage.setAttribute("alt", "Welcome to Knowsie!");
    bindingMedium.appendChild(peekImage);

    var text = tileContent.createElement("text");
    text.setAttribute("hint-wrap", "true");
    text.innerText = "Demo Message";
    bindingMedium.appendChild(text);

    //fire the notification
    var notifications = Windows.UI.Notifications;
    var tileNotification = new notifications.TileNotification(tileContent);
    notifications.TileUpdateManager.createTileUpdaterForApplication().update(tileNotification);
}