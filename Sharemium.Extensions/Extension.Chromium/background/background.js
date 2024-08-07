console.log('Sharemium Console - Background proccess activated!');
chrome.runtime.onInstalled.addListener(function() {
  chrome.contextMenus.create({ // Create 'hyperlink' context menu Share entry
    id: "HyperlinkContextMenuEntry",
    title: "Send link via Sharemium",
    contexts: ["link"]
  });
  chrome.contextMenus.create({ // Create 'tab page' context menu Share entry
    id: "PageContextMenuEntry",
    title: "Share page via Sharemium",
    contexts: ["page"] 
  });
});

chrome.runtime.onMessage.addListener(function(message, sender, sendResponse) {
  if (message.action === "ShareCurrentPage") {
    // Call the function you want to execute
    ShareCurrentTabPage();
    sendResponse({ status: "Shared successfully!" });
  }
});

let shareTitle; // Global variable Page/Link Title tag 
let shareDescr; // Global variable Page/Link description meta tag
let shareURL; // Global variable Page/Link URL path

chrome.contextMenus.onClicked.addListener(function(info) {
  if (info.menuItemId === "HyperlinkContextMenuEntry") {
    shareURL = info.linkUrl; // Hyperlink URL
    LaunchSharemiumProccess();
  }
  if (info.menuItemId === "PageContextMenuEntry") {
    ShareCurrentTabPage();
  }
});

function ShareCurrentTabPage(window) {
  chrome.tabs.query({ active: true, currentWindow: true }, function(tabs) { // Query of active tabs on current window (first = the one opened)
    var activeTab = tabs[0];
    if (activeTab) {
      shareTitle = activeTab.title; // Tab page Title
      shareURL = activeTab.url; // Tab page URL
      LaunchSharemiumProccess();
    }});
}

function LaunchSharemiumProccess() {
  var SharemiumURI = "sharemium:" + shareURL; // Main end protocol schema
  var protocolTitle = "?title=" + shareTitle; // Title tag end protocol entry
  var protocolDescr = "&descr=" + shareDescr; // Description tag end protocol entry

  if (shareTitle !== null && shareTitle !== undefined) { SharemiumURI += protocolTitle; } // If Title is null, leave parameter off the end URI output
  if (shareDescr !== null && shareDescr !== undefined) { SharemiumURI += protocolDescr; } // If Descr is null, leave parameter off the end URI output
  chrome.tabs.update({ url: SharemiumURI });
};

