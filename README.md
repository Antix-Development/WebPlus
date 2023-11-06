
![alt text](images/logo.svg)

WebPlus lets you transform your boring old web applications into exciting Windows desktop applications.



# Introduction

## Why WebPlus?

Web Apps are fantastic but they have no access to the local file system. This issue can be overcome by using one of many available frameworks that transform web applications into desktop applications.

I have used one such framework (Electron) before but it includes an entire browser with your distribution, making every application you create with it at least 90Mb or larger. NWJS and CefSharp are some other notable *monolithic* frameworks.

Other frameworks exist that don't create gigantic distributions but when I tried them I faced issues that I could not resolve. Tauri and Neutralino are some notable *lithe* frameworks.

If I had not already made good progress with my own framework when I tried Neutralino I'd have used it because it was easy to get a working packaged app create in less than 10 minutes, without bizzarre issues.

## What is WebPlus?

WebPlus is yet another framework for transforming web applications into desktop applications.

WebPlus was created for my own personal use and is publicly availabe in case anyone finds it interesting or of use. It does *not* strive to compete with any established more advanced frameworks that provide similar functionality. If you really want serious grown-up stuff, then I recommend trying one of the frameworks I previously mentioned, they are all more mature and secure than WebPlus.

I also don't own any Apple or Linux stuff so WebPlus only works on Windows.

So I created WebPlus with the following requirements:

- Local file access.

- Small distribution size.

- Easy to setup.

- Easy to use.

- No real requirement to compile anything.

Currently WebPlus meets all of my requirements and I'm very pleased with the final distribution size (minus user generated HTML, CSS, and JavaScript) of just under 800Kb.

<br>



# Getting Started

## Requirements

All you really need is a text editor but if you want to build your own binary you will need [Visual Studio](https://visualstudio.microsoft.com/vs/).

## Your first WebPlus app

Lets make an app called *MyCoolApp*.

1. Copy the "dist" folder somewhere handy and rename it as "MyCoolApp".

2. Rename the WebPlus.exe file to "MyCoolApp.exe".

3. Run the "MyCoolApp.exe" file and an empty grey window will magically appear.

4. Edit app.html, style.css, and app.js inside the app folder using your chosen text editor, and when you save them the changes will be visible in the app window.

5. Rinse and repeat step 4 until your app is done.

Once you are happy with your app, remove the line in app.js that reads `wp.enableHotReload(true);` to disable hot reloading.

Replace the "icon.ico" with your own icon, and then you're ready to commence distribution.

<br>



# Under the Hood






<br>



# API Reference

## Variables

WebPlus provides a number of global variables that you might find useful...

### `wp`

The WebPlus object that encapsulates all methods that can be called on the host.

### `WP_HOST`

The cached host (`window.chrome.webview.hostObjects.sync.hostedObject`).

### `WP_PATH`

The applications home directory.

### `WP_MESSAGE_HANDLER`

A callback function that will be called when messages arrive from the host.

### `WP_FULLSCREEN`

True if the application window is currently in full-screen mode.

### `WP_FRAMELESS`

True if the application window has no frame.

### `WP_HOTRELOAD_ENABLED`

True if the application is currently in hot-reload mode.

### `WP_TEXTFILE_FILTER`

A pre generated filetype filter for TEXT files.

### `WP_JSONFILE_FILTER`

A pre generated filetype filter for JSON files.

### `WP_PNGFILE_FILTER`

A pre generated filetype filter for PNG files.




## Objects

Some WebPlus methods return objects, and others may require you to supply an object. These objects are...

### FileDetails

```
{
  name: {String},       // Name of file, including extension.
  extension: {String},  // Forced to lowercase.
  type: {String},       // "FILE" or "DIRECTORY".
  size: {Number},       // Size in bytes.
  path: {String},       // Directory where file is stored.
  fullPath: {String},   // Fully qualified file path.
}
```

### DialogOptions

```
{
  filter: {String},
  multiSelect: {Boolean},
  title: {String}
}
```




## Methods

All callable host methods are encapsulated inside the wp object, which is a bit like a class, but without a constructor and all that *"this"* stuff.

To call a method, just reference it as you would a class, so if you wanted to set the windows title to "WebPlus Rocks" you would use the following code:

```
wp.setWindowTitle("WebPlus Rocks");
```

## `exit()`

Exit the application.



## `messageHost(message)`

Send the given message to the host. The message will be sent as a JSON string.

&emsp; @param (String) message



## `setMessageHandler(handler)`

Set the callback that will receive responses from the host to the given callback function.

&emsp; @param (Function) handler



## `enableHotReload(state)`

Enable or disable hot reloading according to the given state.

&emsp; @param (Boolean) state



## `getPath()`

Get the applications folder.

&emsp; @returns (String)



## `getLastError()`

Get the last error encountered by the host. Useful for determining why some method or another failed.

&emsp; @returns (String)



## `setWindowTitle(title)`

Set the host window title to the given title.

&emsp; @param (String) title



## `setWindowIcon(path)`

Set the host window icon to the given path pointing to a .PNG or .ICO file.

&emsp; @param (String) path



## `setFullScreen(state)`

Enter or leave fullscreen mode according to the given state.

&emsp; @param (Boolean) state



## `setFrameless(state)`

Remove or add window frame according to the given state.

&emsp; @param (Boolean) state



## `fileInfo(path)`

Get the [`FileDetails`](#filedetails) for the file or directory at the given path.

&emsp; @param (String) path

&emsp; @returns (FileDetails)



## `deleteFile(path)`

Delete the file with the given path.

&emsp; @param (String) path



## `renameFile(path, name)`

Rename the file with the given path to the given name.

&emsp; @param (String) path



## `dirInfo(path)`

Get an array of [`FileDetails`](#filedetails) for all files and directories in the given path.

&emsp; @param (String) path

&emsp; @returns ([FileDetails])



## `createDirectory(path)`

Create a directory with the given path.

&emsp; @param ( String ) path



## `deleteDirectory(path)`

Recursively delete the directory with the given path.

&emsp; @param ( String ) path




## `renameDirectory(path, name)`

Rename the directory with the given path to the given name.

&emsp; @param (String) path



## `openFileDialog(options = {})`

Using the given options, display a dialog where a file can be selected, and return its `FileDetails` if it wasn't cancelled.

&emsp; @param (DialogOptions) [`options`](#dialogoptions)

&emsp; @returns (FileDetails)



## `openFolderDialog()`

Display a dialog where a folder can be selected, and return its `FileDetails` if it wasn't cancelled.

&emsp; @returns (FileDetails)



## `loadTextFile(path)`

Load the text file with the given path.

&emsp; @param (String) path

&emsp; @returns (String)



## `brosweForAndLoadTextFile(options = {})`

Browse for a text file using a `OpenFileDialog`, and if not cancelled, load the selected text file.

&emsp; @param (DialogOptions) [`options`](#dialogoptions)

&emsp; @returns (String)



## `saveTextFile(text, path) => WP_HOST.saveTextFile(text, path),`

Save the given text to the file at the given path.

&emsp; @param (String) text

&emsp; @param (String) path



## `browseForAndSaveTextFile(text, options = {})`

Browse for a text file using a `SaveFileDialog`, and if not cancelled, save the given text to the selected file.

&emsp; @param (String) text

&emsp; @param (DialogOptions) [`options`](#dialogoptions)
