/**
 * WebPlus.js
 * Augments web apps with super powers.
 * @copyright 2023 Cliff Earl, Antix Development.
 * @license MIT
 * @namespace WebPlus
*/

'use_strict';

let debugOutput;

window.onload = () => {

    debugOutput = document.getElementById('debug_output');

    document.getElementById('wp_path_label').value = WP_PATH;

    window.addEventListener('windowresize', (e) => {
        console.log(`windowresize: ${e.detail}`);
    });

    generateImage();
}

function toggleHotReload() {
    wp.enableHotReload(!WP_HOTRELOAD_ENABLED);
    document.getElementById('toggle_hot_reload_button').innerHTML = (WP_HOTRELOAD_ENABLED) ? 'disable hot reload' : 'enable hot reload';
}

function toggleFullScreen() {
    wp.setFullScreen(!WP_FULLSCREEN);
    document.getElementById('toggle_fullscreen_button').innerHTML = (WP_FULLSCREEN) ? 'leave fullscreen' : 'enter fullscreen';
}

function toggleFrameless() {
    wp.setFrameless(!WP_FRAMELESS);
    document.getElementById('toggle_frameless_button').innerHTML = (WP_FRAMELESS) ? 'Go framed' : 'go frameless';
}

function toggleMinimizeToTray() {
    wp.minimizeToTray(!WP_MINIMIZE_TO_TRAY);
    document.getElementById('toggle_minimize_to_tray_button').innerHTML = (WP_MINIMIZE_TO_TRAY) ? 'minimize to bar' : 'minimize to tray';
}

function testOpenFileDialog() {
    const fd = wp.openFileDialog();
    if (fd) debugOutput.value += `\n{\n  name:${fd.name}\n  type:${fd.type}\n  extension:${fd.extension}\n  size:${fd.size.toLocaleString() }\n  path:${fd.path}\n  fullPath:${fd.fullPath}\n}`;
}

function testOpenFolderDialog() {
    const fd = wp.openFolderDialog();
    if (fd) debugOutput.value += `\n{\n  name:${fd.name}\n  type:${fd.type}\n  extension:${fd.extension}\n  size:${fd.size.toLocaleString() }\n  path:${fd.path}\n  fullPath:${fd.fullPath}\n}`;
}

function testOpenFileDialogMultiple() {
    const arr = wp.openFileDialog({ multiSelect: true });
    if (arr) arr.forEach((fd) => debugOutput.value += `\n{\n  name:${fd.name}\n  type:${fd.type}\n  extension:${fd.extension}\n  size:${fd.size.toLocaleString()}\n  path:${fd.path}\n  fullPath:${fd.fullPath}\n}`);
}

function testSetWindowTitle() {
    wp.setWindowTitle(document.getElementById('window_name').value);
}

function testBrowseForAndLoadTextFile() {
    const text = wp.brosweForAndLoadTextFile();
    debugOutput.value = `${text}\n`;
}

function testBrowseForAndSaveTextFile() {
    const text = wp.browseForAndSaveTextFile(debugOutput.value);
}

function generateImage() {
    const canvas = document.getElementById('image');
    const ctx = canvas.getContext('2d');
    ctx.fillStyle = `#${Math.floor(Math.random() * 0xffffff).toString(16).padStart(6, '0') }`; // Random color.
    ctx.fillRect(0, 0, 460, 200);
    ctx.fillStyle = '#0004'; // Darken.
    for (var y = 0; y < 10; y++) {
        for (var x = 0; x < 23; x++) if (Math.random() < .5) ctx.fillRect(x * 20, y * 20, 20, 20);
    }
}

function testSavePNG() {
    const fd = wp.saveFileDialog({ filter: WP_PNGFILE_FILTER });
    if (fd) wp.savePNG(document.getElementById('image'), fd.fullPath);
}
