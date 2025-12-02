window.downloadFileFromBytes = (filename, bytesBase64) => {
    const link = document.createElement('a');
    link.download = filename;
    link.href = "data:text/plain;base64," + bytesBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};