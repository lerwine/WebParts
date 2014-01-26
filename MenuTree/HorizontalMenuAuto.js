var _WPQ_NavRoot;
var _WPQ_ActiveTimer = null;
var _WPQ_ActiveNode = null;

function _WPQ_ContainsNode(parentNode, testNode) {
    var node;
    
    for (node = testNode; node != null; node = node.parentNode) {
        if (node.nodeType == 1 && node.id == "_WPQ_Nav")
            return false;
            
        if (parentNode == node)
            return true;
    }
    
    return false;
}

function _WPQ_GetUlNode(node) {
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

function _WPQ_GetActiveNode(node) {
    var childNode;
    
    if (node.nodeType != 1)
        return null;
    
    if (node.nodeName == "LI")
        return node;
        
    if (node.parentNode != null && node.parentNode.nodeType == 1 && node.parentNode.nodeName == "LI")
        return node.parentNode;

    return null;    
}

function _WPQ_HideNodeTree(ulNode) {
    var node;
    var liNode;
    
    for (liNode = ulNode.firstChild; liNode != null; liNode = liNode.nextSibling) {
        if (liNode.nodeType != null && liNode.nodeName == "LI" && (node = _WPQ_GetUlNode(liNode)) != null)
            _WPQ_HideNodeTree(node);
        
        if (ulNode.parentNode.nodeName == "DIV") {
            for (node = liNode.firstChild; node != null; node=node.nextSibling) {
                if (node.nodeType == 1 && node.className == "_WPQ_ExpandImage")
                    node.src = "/_layouts/images/TPMAX1.GIF";
            }
        }
    }
    
    if (ulNode.parentNode.nodeName != "DIV") {
        if (!ulNode.style || !ulNode.style.display || ulNode.style.display == "none")
            return;
            
        ulNode.style.display = "none";
    }
}

function _WPQ_MenuTimeout() {
    clearTimeout(_WPQ_ActiveTimer);
    _WPQ_ActiveTimer = null;
    
    for (var node = _WPQ_NavRoot.firstChild; node != null; node = node.nextSibling) {
        if (node.nodeType == 1 && node.nodeName == "UL")
            _WPQ_HideNodeTree(node);
    }
}

// Toggle display for top menu item
function _WPQ_MouseOverTop() {
    var node;
    
    if (_WPQ_ActiveTimer != null) {
        clearTimeout(_WPQ_ActiveTimer);
        _WPQ_ActiveTimer = null;
    }
    if ((ulNode = _WPQ_GetUlNode(this)) == null)
        return;

    _WPQ_ActiveNode = _WPQ_GetActiveNode(this);
    
    if (_WPQ_ActiveTimer != null) {
        clearTimeout(_WPQ_ActiveTimer);
        _WPQ_ActiveTimer = null;
    }
    for (var liNode = ulNode.parentNode.previousSibling; liNode != null; liNode=liNode.previousSibling) {
        if ((node = _WPQ_GetUlNode(liNode)) != null)
            _WPQ_HideNodeTree(node);
    }
    
    for (var liNode = ulNode.parentNode.nextSibling; liNode != null; liNode=liNode.nextSibling) {
        if ((node = _WPQ_GetUlNode(liNode)) != null)
            _WPQ_HideNodeTree(node);
    }
    
    if (!ulNode.style || !ulNode.style.display || ulNode.style.display != "block") {
        _WPQ_HideNodeTree(ulNode);
        ulNode.style.display = "block";
        ulNode.style.top = ulNode.parentNode.offsetHeight - 2;
        ulNode.style.left = 0;
        for (node = ulNode.parentNode.firstChild; node != null; node=node.nextSibling) {
            if (node.nodeType == 1 && node.className == "_WPQ_ExpandImage")
                node.src = "/_layouts/images/TPMIN1.GIF";
        }
    } else {
        _WPQ_HideNodeTree(ulNode);
        for (node = ulNode.parentNode.firstChild; node != null; node=node.nextSibling) {
            if (node.nodeType == 1 && node.className == "_WPQ_ExpandImage")
                node.src = "/_layouts/images/TPMAX1.GIF";
        }
    }
}

// Display contained menu
function _WPQ_MouseOver() {
    var ulNode;
    var node;
    var currentNode;
    
    if (_WPQ_ActiveTimer != null) {
        clearTimeout(_WPQ_ActiveTimer);
        _WPQ_ActiveTimer = null;
    }
    
    currentNode = _WPQ_GetUlNode(this);

    _WPQ_ActiveNode = _WPQ_GetActiveNode(this);
    
    if (currentNode == null)
        return;

    currentNode.style.display = "block";
    currentNode.style.top = 2;
    currentNode.style.left = currentNode.parentNode.offsetWidth + currentNode.parentNode.offsetLeft - 2;

    for (ulNode = currentNode; ulNode != null && ulNode.parentNode != null && ulNode.parentNode.nodeName != "DIV"; ulNode = ulNode.parentNode.parentNode) {
        for (var liNode = ulNode.parentNode.previousSibling; liNode != null; liNode=liNode.previousSibling) {
            if ((node = _WPQ_GetUlNode(liNode)) != null)
                _WPQ_HideNodeTree(node);
        }
        
        for (var liNode = ulNode.parentNode.nextSibling; liNode != null; liNode=liNode.nextSibling) {
            if ((node = _WPQ_GetUlNode(liNode)) != null)
                _WPQ_HideNodeTree(node);
        }
    }
}

// Start hiding timer
function _WPQ_MouseOut() {
    if (_WPQ_ActiveTimer != null) {
        clearTimeout(_WPQ_ActiveTimer);
        _WPQ_ActiveTimer = null;
    }
    
    if (_WPQ_ActiveNode == null || _WPQ_ActiveNode == _WPQ_GetActiveNode(this)) {
        _WPQ_ActiveTimer = setTimeout(_WPQ_MenuTimeout, 1000);
        _WPQ_ActiveNode = null;
    }
}

function _WPQ_SetupMenuNode(ulNode) {
    for (var childNode = ulNode.firstChild; childNode != null; childNode = childNode.nextSibling) {
        var foundUl = false;
        var foundAnchor = false;
        var imageNode = null;
        
        if (childNode.nodeType != 1 || childNode.nodeName != "LI")
            continue;
            
        for (var node = childNode.firstChild; node != null; node = node.nextSibling) {
            if (node.nodeType != 1 || node.nodeName != "UL")
                continue;
            
            foundUl = true;
            _WPQ_SetupMenuNode(node);
        }
        
        if (!foundUl)
            continue;

        childNode.onmouseover = _WPQ_MouseOver;
        childNode.onmouseout = _WPQ_MouseOut;
    }
}

function _WPQ_Init(navRootId) {
    if (!document.getElementById || (_WPQ_NavRoot = document.getElementById(navRootId)) == null)
        return;
    
    for (var tableNode = _WPQ_NavRoot.firstChild; tableNode != null; tableNode = tableNode.nextSibling) {
        if (tableNode.nodeType != 1 || tableNode.nodeName != "TABLE" || tableNode.className != "ms-WPMenu")
            continue;
        for (var r=0; r<tableNode.rows.length; r++) {
            for (var c=0; c<tableNode.rows[r].cells.length; c++) {
                tableNode.rows[r].onmouseover = _WPQ_MouseOverTop;
                tableNode.rows[r].onmouseout = _WPQ_MouseOutTop;
                _WPQ_SetupMenuNode(tableNode.rows[r].cells[c]);
            }
        }
    }
}
