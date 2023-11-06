
![alt text](images/logo.svg)

WebPlus is a lightweight portable desktop application development framework that lets you turn your web applications into native desktop applications for the Microsoft Windows Operating System.

<div style="text-align: center">

# [xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx](#xxxxxxxxxxxxxxxxxxxxxx)

# [Getting Started](#getting-stared)

# [API Reference](#api-reference)


[dfghdfghf](# introduction)


</div>

# Introduction

## What is WebPlus

Transform web applications into Windows native desktop applications.


**^^wordy or not wordy??**

## Why WebPlus?

Well why not.




# Getting Started

## Requirements

All you really need is a text editor but if you want to build your own binary you will need [Visual Studio](https://visualstudio.microsoft.com/vs/).






<hr>

<br>

<br>

<br>

<br>

<br>

<br>

# API Reference


## Variables

WebPlus contains a number of global variables that you can access:

### wp

The WebPlus object that encapsulates all methods that can be called on the host.

### WP_HOST

The cached host which ???

### WP_PATH

The applications home directory.

### WP_MESSAGE_HANDLER

A callback function that will be called when messages arrive from the host.

### WP_FULLSCREEN

True if the application window is currently in full-screen mode.

### WP_FRAMELESS

True if the application window has no frame.

### WP_HOTRELOAD_ENABLED

True if the application is currently in hot-reload mode.

### WP_TEXTFILE_FILTER

A pregenerated filter for TEXT files.

### WP_JSONFILE_FILTER

A pregenerated filter for JSON files.

### WP_PNGFILE_FILTER

A pregenerated filter for PNG files.

## Structures

Some WebPlus methods return custom structures:

### FileDetails

## Methods

To call a WebPlus method use ```wp.``` + `methodname`. For example if you wanted to set the windows title to "WebPlus Rocks" you would use the following code:

```
wp.setWindowTitle("WebPlus Rocks");
```

WebPlus encapsulates the following methods:

### exit()

Exit the application.

<hr>

### messageHost(message)

Send the given message to the host.

@param (String) message

<hr>

### setMessageHandler(handler)

Set the callback that will receive responses from the host to the given callback function.

@param (Function) handler

<hr>

### enableHotReload(state)

Enable or disable hot reloading according to the given state.

@param (Boolean) state

<hr>

### getPath()

Get the applications folder.

@returns (String)

<hr>

### getLastError()

Get the last error.

@returns (String)

<hr>

### setWindowTitle(title)

Set the host window title to the given title.

@param (String) title

<hr>

### setWindowIcon(path)

Set the host window icon to the given path pointing to a .PNG or .ICO file.

@param (String) path

<hr>

### setFullScreen(state)

Enter or leave fullscreen mode according to the given state.

@param (Boolean) state

<hr>

### fileInfo(path)

Get the `FileDetails` for the file or directory at the given path.

@param (String) path

@returns (FileDetails)

<hr>

### deleteFile(path)

Delete the file with the given path.

@param (String) path

<hr>

### renameFile(path, name)

Rename the file with the given path to the given name.

@param (String) path

<hr>

### dirInfo(path)

Get an array of 'FileDetails' for all files and directories in the given path.

@param (String) path

@returns ([FileDetails])

<hr>

### createDirectory(path)

Create a directory with the given path.

@param ( String ) path

<hr>

### deleteDirectory(path)

Recursively delete the directory with the given path.

@param ( String ) path

<hr>

### openFileDialog(options = ())d) ? JSON.parse(fd) : null;

Using the given options, display a dialog where a file can be selected, and return its `FileDetails` if it wasn't cancelled.

@param (Object) options

@returns (FileDetails)

<hr>

### openFolderDialog()

Display a dialog where a folder can be selected, and return its `FileDetails` if it wasn't cancelled.

@returns (FileDetails)

<hr>

### loadTextFile(path)

Load the text file with the given path.

@param (String) path

@returns (String)

<hr>

### brosweForAndLoadTextFile(options = ())

Browse for a text file using a `OpenFileDialog`, and if not cancelled, load the selected text file.

@param (Object) options

@returns (String)

<hr>

### saveTextFile(text, path) => WP_HOST.saveTextFile(text, path),

Save the given text to the file at the given path.

@param (String) text

@param (String) path

<hr>

### browseForAndSaveTextFile(text, options = ())

Browse for a text file using a `SaveFileDialog`, and if not cancelled, save the given text to the selected file.

@param (String) text

@param (String) filter
