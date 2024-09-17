chrome.runtime.onInstalled.addListener(function () {
  const UAresult = new UAParser().getResult();
  const browserName = UAresult.browser.name;
  chrome.storage.local.set({ thisBrowserName: browserName });
  chrome.contextMenus.removeAll(function () {
    chrome.contextMenus.create({
      id: 'RootPageEntry',
      title: 'Share this page',
      contexts: ['page']
    });
    chrome.contextMenus.create({
      id: 'LinkContextEntry',
      title: 'Share this link',
      contexts: ['link']
    });
    chrome.contextMenus.create({
      id: 'MediaContextEntry',
      title: 'Share this image',
      contexts: ['image']
    });
  });
});
function ShareCurrentTab() {
  chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
    chrome.tabs.sendMessage(tabs[0].id, { action: "getCurrentMetadata" }, function (response) {
      if (chrome.runtime.lastError) {
        console.error("Sharemium - background - Metadata recieve error: " + chrome.runtime.lastError.message);
        return;
      }
      if (response) {
        chrome.storage.local.get("thisBrowserName", function (result) {
          var browserName = result.thisBrowserName;
          var currentURL = response.tabURL;
          var currentTitle = response.tabTitle;
          var currentDescr = response.tabDescr;
          let AppLaunchURI = 'sharemium:'+currentURL+'#title='+currentTitle+'&descr='+currentDescr+'&app='+browserName+'&typeof=Link';
          chrome.tabs.update(tabs[0].id, { url: AppLaunchURI });
        });
      }
    });
  });
}
function ShareSelectedLink(info) {
  chrome.storage.local.get("thisBrowserName", function (result) {
    var browserName = result.thisBrowserName;
    let AppLaunchURI = 'sharemium:'+info.linkUrl+'#app='+browserName+'&typeof=Link';
    chrome.tabs.update({ url: AppLaunchURI });
  });
}
function ShareSelectedImage(info) {
  chrome.storage.local.get("thisBrowserName", function (result) {
    var browserName = result.thisBrowserName;
    var imageLink = info.srcUrl;
    var mediaTypeOf = info.mediaType;
    let AppLaunchURI = 'sharemium:'+imageLink+'#app='+browserName+'&typeof='+mediaTypeOf;
    chrome.tabs.update({ url: AppLaunchURI });
  });
}
chrome.contextMenus.onClicked.addListener(function (info) {
  switch (info.menuItemId) {
    case "RootPageEntry":
      ShareCurrentTab();
      break;
    case "LinkContextEntry":
      ShareSelectedLink(info);
      break;
    case "MediaContextEntry":
      ShareSelectedImage(info);
      break;
  }
});
