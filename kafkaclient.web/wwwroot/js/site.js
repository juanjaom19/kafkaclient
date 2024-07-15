// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function createRipple(event, element, className) {
    const ripple = document.createElement('span');
    const rect = element.getBoundingClientRect();
    const size = Math.max(rect.width, rect.height);
    const radius = size / 2;
    
    ripple.style.width = ripple.style.height = `${size}px`;
    ripple.style.left = `${event.clientX - rect.left - radius}px`;
    ripple.style.top = `${event.clientY - rect.top - radius}px`;
    ripple.classList.add(className);

    element.appendChild(ripple);

    setTimeout(() => {
        ripple.remove();
    }, 500);
}

function getSubstringBeforeFirstHyphenByRegex(str) {
    const match = str.match(/^[^-]+/);
    return match ? match[0] : str;
}

function getSubstringBeforeFirstHyphenByDestructing(str) {
    const [firstPart] = str.split('-');
    return firstPart;
}

function getSubstringBeforeFirstHyphenBySubstring(str) {
    const index = str.indexOf('-');
    return index === -1 ? str : str.substring(0, index);
}

function getSubstringBeforeFirstHyphenBySplit(str) {
    return str.split('-').shift();
}
