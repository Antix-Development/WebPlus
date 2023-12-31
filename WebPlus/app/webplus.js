﻿/**
 * WebPlus.js
 * Augments web apps with super powers.
 * @copyright 2023 Cliff Earl, Antix Development.
 * @license MIT
 * @namespace WebPlus
*/

'use_strict';

// Global variables.

const WP_HOST = window.chrome.webview.hostObjects.sync.hostedObject; // Hosted object allowing access to native-code methods and properties.

let WP_PATH = WP_HOST.getPath(); // App home folder.

let WP_HOTRELOAD_ENABLED = WP_HOST.restoreHotReloadState(); // True when hot reloading is enabled.
let WP_FULLSCREEN = WP_HOST.getFullScreenState(); // True when app is in full-screen mode.
let WP_FRAMELESS = WP_HOST.getFrameLessState(); // True when the app window has no frame.
let WP_MINIMIZE_TO_TRAY = WP_HOST.getMinimizeToTrayState(); // True when app window will minimize to the system tray.

//let WP_MESSAGE_HANDLER; // Callback function that will receive messages sent from the host.

// Filetype filters.
const WP_TEXTFILE_FILTER = 'Text files (*.txt)|*.txt|All files (*.*)|*.*';
const WP_JSONFILE_FILTER = 'JSON files (*.json)|*.json';
const WP_PNGFILE_FILTER = 'Image files (*.png)|*.png';

// The WebPlus object.
const wp = {
    /**
     * Exit the application.
     * @memberof WebPlus
     */
    exit: () => WP_HOST.exit(),

    /**
     * Launch the process with the given name.
     * @param {String} name
     * @memberof WebPlus
     */
    launchProcess: (name) => WP_HOST.launchProcess(name),

    /**
     * Enable or disable hot reloading according to the given state.
     * @param {Boolean} state
     * @memberof WebPlus
     */
    enableHotReload: (state) => {
        WP_HOST.enableHotReload(state);
        WP_HOTRELOAD_ENABLED = state;
    },

    /**
     * Get the applications folder.
     * @returns {String}
     * @memberof WebPlus
     */
    getPath: () => {
        WP_PATH = WP_HOST.getPath();
        return WP_PATH;
    },

    /**
     * Get the last error encountered by the host.
     * @returns {String}
     * @memberof WebPlus
     */
    getLastError: () => WP_HOST.getLastError(),

    ///**
    // * Send the given message to the host.
    // * @param {String} message
    // * @memberof WebPlus
    // */
    //messageHost: (message) => window.chrome.webview.postMessage(JSON.stringify(message)),

    ///**
    // * Set the callback that will receive responses from the host to the given callback function.
    // * @param {Function} handler
    // * @memberof WebPlus
    // */
    //setMessageHandler: (handler) => WP_MESSAGE_HANDLER = handler,

    /**
     * Set the host window location to the given coordinates.
     * @param {Number} x
     * @param {Number} y
     * @memberof WebPlus
     */
    setWindowLocation: (x, y) => WP_HOST.setWindowLocation(x, y),

    /**
     * Set the host window size to the given dimensions.
     * @param {Number} w
     * @param {Number} h
     * @memberof WebPlus
     */
    setWindowSize: (w, h) => WP_HOST.setWindowSize(w, h),
    
    /**
     * Get the bounds of the host window.
     * @returns {WindowBounds}
     * @memberof WebPlus
     */
    getWindowBounds: () => JSON.parse(WP_HOST.getWindowBounds()),

    /**
     * Set the host window title to the given title.
     * @param {String} title
     * @memberof WebPlus
     */
    setWindowTitle: (title) => WP_HOST.setWindowTitle(title),
    
    /**
     * Set the host window icon to the given path pointing to a .PNG or .ICO file.
     * @param {String} path
     * @memberof WebPlus
     */
    setWindowIcon: (path) => WP_HOST.setWindowIcon(path),
        
    /**
     * Set the host window to minify to the system tray instead of the task bar according to the given state.
     * @param {Boolean} state
     * @memberof WebPlus
     */
    minimizeToTray: (state) => {
        WP_HOST.minimizeToTray(state);
        WP_MINIMIZE_TO_TRAY = state;
    },

    /**
     * Set the app to open in fullscreen mode according to the given state.
     * @param {Boolean} state
     * @memberof WebPlus
     */
    startInFullScreen: (state) => {
        WP_HOST.startInFullScreen(state);
    },

    /**
     * Enter or leave fullscreen mode according to the given state.
     * @param {Boolean} state
     * @memberof WebPlus
     */
    setFullScreen: (state) => {
        WP_HOST.setFullScreen(state);
        WP_FULLSCREEN = state;
    },
    
    /**
     * Set the app to open framed or frameless according to the given state.
     * @param {Boolean} state
     * @memberof WebPlus
     */
    startFrameless: (state) => {
        WP_HOST.startFrameless(state);
    },

    /**
     * Remove or add window frame according to the given state.
     * @param {Boolean} state
     * @memberof WebPlus
     */
    setFrameless: (state) => {
        WP_HOST.setFrameless(state);
        WP_FRAMELESS = state;
    },

    /**
     * Get the `FileDetails` for the file or directory at the given path.
     * @param {String} path
     * @returns {FileDetails}
     * @memberof WebPlus
     */
    fileInfo: (path) => {
        const fd = WP_HOST.fileInfo(path);
        return (fd) ? JSON.parse(fd) : null;
    },
    
    /**
     * Delete the file with the given path.
     * @param {String} path
     * @memberof WebPlus
     */
    deleteFile: (path) => WP_HOST.deleteFile(path),
        
    /**
     * Rename the file with the given path to the given name.
     * @param {String} path
     * @memberof WebPlus
     */
    renameFile: (path, name) => WP_HOST.renameFile(path, name),

    /**
     * Get an array of 'FileDetails' for all files and directories in the given path.
     * @param {String} path
     * @returns {[FileDetails]}
     * @memberof WebPlus
     */
    dirInfo: (path) => {
        const di = WP_HOST.dirInfo(path);
        return (di) ? JSON.parse(di) : null;
    },

    /** 
     * Create a directory with the given path.
     * @param { String } path
     * @memberof WebPlus
    */
    createDirectory: (path) => WP_HOST.createDirectory(path),
        
    /** 
     * Recursively delete the directory with the given path.
     * @param { String } path
     * @memberof WebPlus
    */
    deleteDirectory: (path) => WP_HOST.deleteDirectory(path),

    /**
     * Rename the directory with the given path to the given name.
     * @param {String} path
     * @memberof WebPlus
     */
    renameDirectory: (path, name) => WP_HOST.renameDirectory(path, name),

    /**
     * Using the given options, display an open dialog where a file can be selected, and return its `FileDetails` if it wasn't cancelled.
     * @param {Object} options
     * @returns {FileDetails}
     * @memberof WebPlus
     */
    openFileDialog: (options = {}) => {

        const o = Object.assign({
            filter: WP_TEXTFILE_FILTER,
            multiSelect: false,
            title: (options.multiSelect) ? 'Select files' : 'Select file',
        }, options);

        const fd = WP_HOST.openFileDialog(JSON.stringify(o));

        return (fd) ? JSON.parse(fd) : null;
    },
    
    /**
     * Using the given options, display a save file dialog where a file can be selected, and return its `FileDetails` if it wasn't cancelled.
     * @param {Object} options
     * @returns {FileDetails}
     * @memberof WebPlus
     */
    saveFileDialog: (options = {}) => {
        const o = Object.assign({
            filter: WP_TEXTFILE_FILTER,
            multiSelect: false,
            title: "select file"
        }, options);

        const fd = WP_HOST.saveFileDialog(JSON.stringify(o));
        return (fd) ? JSON.parse(fd) : null;
    },

    /**
     * Display a dialog where a folder can be selected, and return its `FileDetails` if it wasn't cancelled.
     * @returns {FileDetails}
     * @memberof WebPlus
     */
    openFolderDialog: () => {
        const fd = WP_HOST.openFolderDialog();
        return (fd) ? JSON.parse(fd) : null;
    },

    /**
     * Load the text file with the given path.
     * @param {String} path
     * @returns {String}
     * @memberof WebPlus
     */
    loadTextFile: (path) => WP_HOST.loadTextFile(path),

    /**
     * Browse for a text file using a `OpenFileDialog`, and if not cancelled, load the selected text file.
     * @param {Object} options
     * @returns {String}
     * @memberof WebPlus
     */
    brosweForAndLoadTextFile: (options = {}) => {

        const o = Object.assign({
            filter: WP_TEXTFILE_FILTER,
            multiSelect: false,
            title: 'Select text file',
        }, options);

        return WP_HOST.brosweForAndLoadTextFile(JSON.stringify(o));
    },

    /**
     * Save the given text to the file at the given path.
     * @param {String} text
     * @param {String} path
     * @memberof WebPlus
     */
    saveTextFile: (text, path) => WP_HOST.saveTextFile(text, path),

    /**
     * Browse for a text file using a `SaveFileDialog`, and if not cancelled, save the given text to the selected file.
     * @param {String} text
     * @param {Object} options
     * @returns
     * @memberof WebPlus
     */
    browseForAndSaveTextFile: (text, options = {}) => {
        const o = Object.assign({
            filter: WP_TEXTFILE_FILTER,
            multiSelect: false,
            title: 'Select text file',
        }, options);

        WP_HOST.browseForAndSaveTextFile(text, JSON.stringify(o));
    },

    /**
     * Save the given canvas as a PNG image at the given path.
     * @param {HTMLCanvasElement} canvas
     * @param {String} path
     * @memberof WebPlus
     */
    savePNG: (canvas, path) => {
        WP_HOST.savePNG(canvas.toDataURL(), path);
    },

};

//// Install listener to handle incomming messages from the host. Theese messages will be handed off to a user specified callback which can be set by calling `wp.setMessageHandler`.
//window.chrome.webview.addEventListener('message', (e) => {
//    try {

//        if (WP_MESSAGE_HANDLER) {
//            WP_MESSAGE_HANDLER(JSON.parse(e.data));

//        } else {
//            console.warn(`unhandled message: ${response}`);

//        }

//    } catch (e) {
//        console.warn(`invalid message received: ${e.data}`);
//    }
//});
