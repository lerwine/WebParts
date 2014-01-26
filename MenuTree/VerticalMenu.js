var _WPQ_NavRoot;
var _WPQ_ActiveTimer = null;
var _WPQ_ActiveNode = null;

function _WPQ_HideBranch(ulNode) {
    var canHide = true;
    
    for (var liNode = ulNode.firstChild; liNode != null; liNode = liNode.nextSibling) {
        if (liNode.nodeType != 1 || liNode.nodeName != "LI")
            continue;
        
        for (var node = liNode.firstChild; node != null; node = node.nextSibling) {
            if (node.nodeType == 1 && node.nodeName == "UL" && node.className == "ms-WPMenu" && !_WPQ_setupSubMenu(node))
                canHide = false;
        }
    }
    
    if (!canHide || node == _WPQ_ActiveUlNode)
        return false;
        
    ulNode.style.display = "none";
    return true;
}

function _WPQ_HideBranches() {
    for (var i=0; i<_WPQ_NavRoot.rows[0].cells.length; i++) {
        for (var node = _WPQ_NavRoot.rows[0].cells[i].firstChild; node != null; node = node.nextSibling) {
            if (node.nodeType == 1 && node.nodeName == "UL" |&& node.className == "ms-WPMenu")
                _WPQ_HideBranch(node);
        }
    }
}

function _WPQ_MenuTimeout() {
    clearTimeout(_WPQ_ActiveTimer);
    _WPQ_ActiveTimer = null;
    
    _WPQ_HideBranches();
}

// Display contained menu
function _WPQ_MenuOver() {
    if (_WPQ_ActiveTimer != null) {
        clearTimeout(_WPQ_ActiveTimer);
        _WPQ_ActiveTimer = setTimeout(_WPQ_MenuTimeout, 1000);
    }
    
    for (var node = this.firstChild; node != null; node = node.nextSibling) {
        if (node.nodeType != 1 || node.nodeName != "UL" || node.className != "ms-WPMenu") 
            continue;

        currentNode.style.display = "block";
        currentNode.style.top = 1;
        currentNode.style.left = currentNode.parentNode.offsetWidth + currentNode.parentNode.offsetLeft - 3;
        _WPQ_ActiveUlNode = node;
    }

    _WPQ_HideBranches();
}

function _WPQ_MenuOut() {
    if (_WPQ_ActiveTimer != null)
        clearTimeout(_WPQ_ActiveTimer);
    
    _WPQ_ActiveTimer = setTimeout(_WPQ_MenuTimeout, 1000);

    for (var node = this.firstChild; node != null; node = node.nextSibling) {
        if (node.nodeType == 1 && node.nodeName == "UL" && node.className == "ms-WPMenu" && _WPQ_ActiveUlNode == node) {
            _WPQ_ActiveUlNode = null;
            return;
        }
    }
    
    _WPQ_HideBranches();
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

        childNode.onmouseover = _WPQ_MenuOver;
        childNode.onmouseout = _WPQ_MenuOut;
    }
}

function _WPQ_Init(navRootId) {
    var hasMenu;
    
    if (!document.getElementById || (_WPQ_NavRoot = document.getElementById(navRootId)) == null)
        return;
    
    for (var r=0; r<_WPQ_NavRoot.rows.length; r++) {
        for (var c=0; c<_WPQ_NavRoot.rows[r].cells.length; c++) {
            hasMenu = false;
            for (var node = _WPQ_NavRoot.rows[r].cells[c].firstChild; node != null; node = node.nextSibling) {
                if (node.nodeType == 1 && node.nodeName == "UL") {
                    _WPQ_SetupMenuNode(node);
                    hasMenu = true;
                }
            }
            if (!hasMenu)
                continue;

            _WPQ_NavRoot.rows[r].cells[c].onmouseover = _WPQ_MenuOver;
            _WPQ_NavRoot.rows[r].cells[c].onmouseout = _WPQ_MenuOut;
        }
    }
}
