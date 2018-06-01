"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
function toggleClass(elem, className, bool) {
    if (bool === true)
        return elem.classList.add(className);
    elem.classList.remove(className);
}
exports.toggleClass = toggleClass;
function createElement(tag, className, content) {
    var e = window.document.createElement(tag);
    className = className || "";
    content = content || "";
    e.className = className;
    if (content !== undefined)
        e.textContent = content;
    return e;
}
exports.createElement = createElement;
function clearNode(node) {
    while (node.firstChild)
        node.removeChild(node.firstChild);
}
exports.clearNode = clearNode;
function findParent(node, condition) {
    if (condition(node))
        return node;
    else if (node.parentNode)
        return findParent(node.parentNode, condition);
    return undefined;
}
exports.findParent = findParent;
function createNumberInput(inputClassName) {
    var wrapper = createElement("div", "numInputWrapper"), numInput = createElement("input", "numInput " + inputClassName), arrowUp = createElement("span", "arrowUp"), arrowDown = createElement("span", "arrowDown");
    numInput.type = "text";
    numInput.pattern = "\\d*";
    wrapper.appendChild(numInput);
    wrapper.appendChild(arrowUp);
    wrapper.appendChild(arrowDown);
    return wrapper;
}
exports.createNumberInput = createNumberInput;
