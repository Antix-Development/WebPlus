/**
 * WebPlus.js
 * Yet another web to native framework.
 * @copyright 2023 Cliff Earl, Antix Development.
 * @license MIT
 * @namespace WebPlus
*/

'use_strict';

// Global variables.

const WP_HOST = window.chrome.webview.hostObjects.sync.hostedObject; // Hosted object allowing access to native-code methods and properties.

let WP_PATH = WP_HOST.getPath();

let WP_HOTRELOAD_ENABLED = WP_HOST.restoreHotReloadState();

let WP_MESSAGE_HANDLER; // Callback function that will receive messages sent from the host.

let WP_FULLSCREEN = false;
let WP_BORDERLESS = false;

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
     * Send the given message to the host.
     * @param {String} message
     * @memberof WebPlus
     */
    messageHost: (message) => window.chrome.webview.postMessage(JSON.stringify(message)),

    /**
     * Set the callback that will receive responses from the host to the given callback function.
     * @param {Function} handler
     * @memberof WebPlus
     */
    setMessageHandler: (handler) => WP_MESSAGE_HANDLER = handler,

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
     * Enter or leave fullscreen mode according to the given state.
     * @param {Boolean} state
     * @memberof WebPlus
     */
    setFullScreen: (state) => {
        WP_HOST.setFullScreen(state);
        WP_FULLSCREEN = state;
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
     * Using the given options, display a dialog where a file can be selected, and return its `FileDetails` if it wasn't cancelled.
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
};

// Install listener to handle incomming messages from the host. Theese messages will be handed off to a user specified callback which can be set by calling `wp.setMessageHandler`.
window.chrome.webview.addEventListener('message', (e) => {
    try {

        if (WP_MESSAGE_HANDLER) {
            WP_MESSAGE_HANDLER(JSON.parse(e.data));

        } else {
            console.warn(`unhandled message: ${response}`);

        }

    } catch (e) {
        console.warn(`invalid message received: ${e.data}`);
    }
});
