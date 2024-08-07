
document.getElementById('SharePage').addEventListener('click', function() {
    chrome.runtime.sendMessage({ action: "ShareCurrentPage" }, function(response) {
    console.log("Sharemium - Background event:", response.status);
    });
});