 ![image](https://raw.githubusercontent.com/borisg912/Sharemium/main/Assets/RawLogo200x200.png)
# Sharemium
 A Windows UWP sharing assistant
 
 The perfect assistant for sharing content easily from your browser to your favourite UWP apps, working with earlier versions of Windows, including 10240 RTM
 ## How it works?
 The app works by getting a website link and protocol parameters from the 'sharemium:' URI.
 The website link is automatically formated to include the http/https extension, and currently supports the parameters <code>title</code> and <code>descr</code>, supposed to be taken advantage by the extension to give the shaing dialog infobox the webpage's meta tags.
 In the future, I plan to use the <code>favicon</code> to include link to the webpage's favicon for the Windows 11 modern Share dialog.
 
 The URI structure follows this simple scheme:
 
 <code>sharemium:example.com?title=Example domain&descr=An example domain @.com</code>
 
 sharemium:*//a/website/path*?title=*Page title*&descr=*Page description*

 A disadvantage of this is the fact that some pages that rely on in-URL parameters, like YouTube (<code>youtube.com/watch?v=dQw4w9WgXcQ</code>) where the 'v' key handles the video tag, will be discarded. I plan to find a solution in the future.

## NEW! Published the first (buggy) beta version of the Chromium extension v9.7 Beta 2.
Tested on Brave and Opera. Before installing on Chrome, Brave or Vivaldi import the registries from /Sharemium.Extensions/FixChromiumRegistry.reg (adds the extension as trusted), because it's not in the Chrome Web Store. Opera doesn't require this, and I plan to launch it on the Opera add-on store.

### Here's 3 cents:
- My first actual UWP C# app :)
- Waiting until v10.0 to upload to Store
- Working out bugs on the Chromium extension + Gecko add-on (Firefox, Floorp, Mullvad) later on

### My 2 free cents:
- Anyone can contribute or give notes/suggestions/feedback
- The app is as-is, no warranty, no guarantee, no nuthin'

#### Credits and thanks
- Empyreal96 for [Appx Re-Sign] for signing the app
- License: GNU GPLv3

[Appx Re-Sign]: <https://github.com/Empyreal96/Appx_Re-Sign>
