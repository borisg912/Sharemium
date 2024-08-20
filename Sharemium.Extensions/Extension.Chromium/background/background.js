chrome.runtime.onInstalled.addListener(function () {
  const UAresult = new UAParser().getResult();
  const browserName = UAresult.browser.name;
  chrome.storage.local.set({ thisBrowserName: browserName });
  chrome.contextMenus.removeAll(function () {
    chrome.contextMenus.create({
      id: 'RootPageContextEntry',
      title: 'Share this page',
      contexts: ['page']
    });
    chrome.contextMenus.create({
      id: 'HyperlinkContextEntry',
      title: 'Share this link',
      contexts: ['link']
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

          let AppLaunchURI = 'sharemium:' + currentURL + '#title=' + currentTitle + '&descr=' +currentDescr + '&app=' + browserName;
          chrome.tabs.update(tabs[0].id, { url: AppLaunchURI });
        });
      }
    });
  });
}

function ShareSelectedHyperlink(info) {
  chrome.storage.local.get("thisBrowserName", function (result) {
    var browserName = result.thisBrowserName;

    let AppLaunchURI = 'sharemium:' + info.linkUrl + '#app=' + browserName;
    chrome.tabs.update({ url: AppLaunchURI });
  });
}

chrome.contextMenus.onClicked.addListener(function (info, tab) {
  if (info.menuItemId === "RootPageContextEntry") {
    ShareCurrentTab();
  } else if (info.menuItemId === "HyperlinkContextEntry") {
    ShareSelectedHyperlink(info);
  }
});

chrome.runtime.onMessage.addListener(function (request, sender, sendResponse) {
  if (request.action === "RootPageBrowserActionEntry") {
    ShareCurrentTab();
  }
});
