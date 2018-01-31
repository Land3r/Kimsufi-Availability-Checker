// Loads jQuery if it is undefined using jquery CDN.
if (typeof jQuery == 'undefined') {
    console.log('jQuery is undefined.');
    var headTag = document.getElementsByTagName("head")[0];
    var jqTag = document.createElement('script');
    jqTag.crossOrigin = 'anonymous';
    jqTag.src = 'https://code.jquery.com/jquery-1.12.4.min.js';
    jqTag.integrity = 'sha256-ZosEbRLbNQzLpnKIkEdrPv7lOy9C27hHQ+Xp8a4MxAQ=';
    headTag.appendChild(jqTag);
}
else {
    console.log('jQuery is defined');
}