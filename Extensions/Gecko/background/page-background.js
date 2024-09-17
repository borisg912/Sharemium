chrome.runtime.onMessage.addListener(function (request, sender, sendResponse) {
    if (request.action == "getCurrentMetadata") {
        console.log("Page-background.js: Received getPageMetadata request");
        try {
            var currentURL = window.location.href;
            var currentTitle = document.title;
            var currentDescr = document.querySelector('meta[name="description"]')?.content || 
            document.querySelector('meta[property="og:description"]')?.content ||
            document.querySelector('meta[name="twitter:description"]')?.content ||
            null;
            console.log("Page-background.js: Sending response:", { tabURL: currentURL, tabTitle: currentTitle, tabDescr: currentDescr });
            sendResponse({ tabURL: currentURL, tabTitle: currentTitle, tabDescr: currentDescr });
        } catch (error) {
            console.error("Sharemium - page-background.js - Metadata send error:", error);
            sendResponse({ tabURL: currentURL, tabTitle: currentTitle, tabDescr: null });
        }
    }
    return true; // Keep channel open for async sendResponse
});