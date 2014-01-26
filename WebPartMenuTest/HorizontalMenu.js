// JScript File

var WPQ3activeTimer = null;

function WPQ3containsNode(parentNode, testNode) {
    var node;
    
    for (node = testNode; node != null; node = node.parentNode) {
        if (node.nodeType == 1 && node.Id == "WPQ3Nav")
            return false;
            
        if (parentNode == node)
            return true;
    }
    
    return false;
}

function WPQ3getUlNode(node) {
    var childNode;
    
    if (node.nodeType != 1)
        return null;
    
    if (node.nodeName != "LI") {
        if (node.parentNode == null || node.parentNode.nodeType != 1 || node.parentNode.nodeName != "LI")
            return null;
            
        node = node.parentNode;
    }
    
    for (childNode = node.firstChild; childNode != null; childNode = childNode.nextSibling) {
        if (childNode.nodeType == 1 && childNode.nodeName == "UL")
            return childNode;
    }
    
    return null;    
}

function WPQ3hideNodeTree(ulNode) {
    var node;
    var liNode;
    
    for (liNode = ulNode.firstChild; liNode != null; liNode = liNode.nextSibling) {
        if (liNode.nodeType != null && liNode.nodeName == "LI" && (node = WPQ3getUlNode(liNode)) != null)
            WPQ3hideNodeTree(node);
        
        if (ulNode.parentNode.nodeName == "DIV") {
            for (node = liNode.firstChild; node != null; node=node.nextSibling) {
                if (node.nodeType == 1 && node.className == "WPQ3ExpandImage")
                    node.src = "http://spsdev/sites/wptest/_layouts/images/TPMAX1.GIF";
            }
        }
    }
    
    if (ulNode.parentNode.nodeName != "DIV") {
        if (!ulNode.style || !ulNode.style.display || ulNode.style.display == "none")
            return;
            
        ulNode.style.display = "none";
    }
}

function WPQ3menuTimeout() {
    var navRoot;
    
    if (!document.getElementById || (navRoot = document.getElementById("WPQ3Nav")) == null)
        return;

    for (var node = navRoot.firstChild; node != null; node = node.nextSibling) {
        if (node.nodeType == 1 && node.nodeName == "UL")
            WPQ3hideNodeTree(node);
    }
}

// Toggle display for top menu item
function WPQ3mouseClick() {
    var node;
    
    if ((ulNode = WPQ3getUlNode(this)) == null)
        return;

    if (WPQ3activeTimer != null) {
        clearTimeout(WPQ3activeTimer);
        WPQ3activeTimer = null;
    }
    
    for (var liNode = ulNode.parentNode.previousSibling; liNode != null; liNode=liNode.previousSibling) {
        if ((node = WPQ3getUlNode(liNode)) != null)
            WPQ3hideNodeTree(node);
    }
    
    for (var liNode = ulNode.parentNode.nextSibling; liNode != null; liNode=liNode.nextSibling) {
        if ((node = WPQ3getUlNode(liNode)) != null)
            WPQ3hideNodeTree(node);
    }
    
    if (!ulNode.style || !ulNode.style.display || ulNode.style.display != "block") {
        WPQ3hideNodeTree(ulNode);
        ulNode.style.display = "block";
        ulNode.style.top = ulNode.parentNode.offsetHeight - 2;
        ulNode.style.left = 0;
        for (node = ulNode.parentNode.firstChild; node != null; node=node.nextSibling) {
            if (node.nodeType == 1 && node.className == "WPQ3ExpandImage")
                node.src = "http://spsdev/sites/wptest/_layouts/images/TPMIN1.GIF";
        }
    } else {
        WPQ3hideNodeTree(ulNode);
        for (node = ulNode.parentNode.firstChild; node != null; node=node.nextSibling) {
            if (node.nodeType == 1 && node.className == "WPQ3ExpandImage")
                node.src = "http://spsdev/sites/wptest/_layouts/images/TPMAX1.GIF";
        }
    }
}

// Track where mouse is
function WPQ3mouseOverTop() {
    if ((ulNode = WPQ3getUlNode(this)) == null)
        return;
        
    if (WPQ3activeTimer != null) {
        clearTimeout(WPQ3activeTimer);
        WPQ3activeTimer = null;
    }
}

// Display contained menu
function WPQ3mouseOver() {
    var ulNode;
    var node;
    var currentNode;
    
    if (WPQ3activeTimer != null) {
        clearTimeout(WPQ3activeTimer);
        WPQ3activeTimer = null;
    }
    
    currentNode = WPQ3getUlNode(this);

    currentNode.style.display = "block";
    currentNode.style.top = currentNode.parentNode.offsetTop + 2;
    currentNode.style.left = currentNode.parentNode.offsetWidth + currentNode.parentNode.offsetLeft - 4;

    for (ulNode = currentNode; ulNode != null && ulNode.parentNode != null && ulNode.parentNode.nodeName != "DIV"; ulNode = ulNode.parentNode.parentNode) {
        for (var liNode = ulNode.parentNode.previousSibling; liNode != null; liNode=liNode.previousSibling) {
            if ((node = WPQ3getUlNode(liNode)) != null)
                WPQ3hideNodeTree(node);
        }
        
        for (var liNode = ulNode.parentNode.nextSibling; liNode != null; liNode=liNode.nextSibling) {
            if ((node = WPQ3getUlNode(liNode)) != null)
                WPQ3hideNodeTree(node);
        }
    }
}

// Start hiding timer
function WPQ3mouseOut() {
    if ((ulNode = WPQ3getUlNode(this)) == null)
        return;
        
    if (WPQ3activeTimer != null) {
        clearTimeout(WPQ3activeTimer);
        WPQ3activeTimer = null;
    }
    
    WPQ3activeTimer = setTimeout(WPQ3menuTimeout, 1000);
}

function WPQ3setupMenuNode(ulNode) {
    for (var childNode = ulNode.firstChild; childNode != null; childNode = childNode.nextSibling) {
        var foundUl = false;
        var foundAnchor = false;
        var imageNode = null;
        
        if (childNode.nodeType != 1 || childNode.nodeName != "LI")
            continue;
            
        for (var node = childNode.firstChild; node != null; node = node.nextSibling) {
            if (node.nodeType != 1)
                continue;
            
            if (node.nodeName == "A")
                foundAnchor = true;
                
            if (node.nodeName == "IMG")
                imageNode = node;
                
            if (node.nodeName != "UL")
                continue;
            
            foundUl = true;
            WPQ3setupMenuNode(node);
        }
        
        if (!foundUl)
            continue;
        
        if (ulNode.parentNode.nodeName == "DIV") {
            if (foundAnchor) {
                if (imageNode == null)
                    childNode.onmouseover = WPQ3mouseOver;
                else {
                    childNode.onmouseover = WPQ3mouseOverTop;
                    imageNode.onclick = WPQ3mouseClick;
                    imageNode.onmousehover = WPQ3mouseClick;
                }
            } else {
                childNode.onclick = WPQ3mouseClick;
                childNode.onmousehover = WPQ3mouseClick;
            }
        } else {
            childNode.onmouseover = WPQ3mouseOver;
        }
        childNode.onmouseout = WPQ3mouseOut;
    }
}

function WPQ3setup() {
    var navRoot;
    
    if (!document.getElementById || (navRoot = document.getElementById("WPQ3Nav")) == null)
        return;
    
    for (var node = navRoot.firstChild; node != null; node = node.nextSibling) {
        if (node.nodeType != 1 || node.nodeName != "UL")
            continue;
        
        WPQ3setupMenuNode(node);
    }
}

WPQ3setup();
