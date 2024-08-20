document.getElementById('SharePage').addEventListener('click', function() {
    chrome.runtime.sendMessage({ action: "RootPageBrowserActionEntry" },
        function(response) {
            console.log("Sharemium Foreground event:", response.status);
        }
    );
});