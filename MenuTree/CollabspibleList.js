var _WPQ_NavRoot;

function _WPQ_GetUlNode(node) {
    var childNode;
    
    if (node == null || node.nodeType != 1)
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

function _WPQ_MenuCollapse() {
    var node;
    
    if (this.parentNode == null || this.parentNode.nodeName != "LI")
        return;
    
    for (node=this.parentNode.firstChild; node != null; node = node.nextSibling) {
        if (node.nodeType != 1 || node.nodeName != "UL")
            continue;
        if (!node.style || !node.style.display ||node.style.display != "block") {
            node.style.display = "block";
            this.src = "/_layouts/images/TPCOL.GIF";
        } else {
            node.style.display = "none";
            this.src = "/_layouts/images/TPEXP.GIF";
        }
        this.parentNode.scrollIntoView();
    }
}

function _WPQ_CollapsableListNodeSetup(rootNode) {
    var liNode;
    var imgNode;
    
    if (rootNode == null)
        return;
    
    for (liNode = rootNode.firstChild; liNode != null; liNode=liNode.nextSibling) {
        if (liNode.nodeType != 1 || liNode.nodeName != "LI")
            continue;
        
        _WPQ_CollapsableListNodeSetup(_WPQ_GetUlNode(liNode));
        
        for (imgNode = liNode.firstChild; imgNode != null; imgNode = imgNode.nextSibling) {
            if (imgNode.nodeType == 1 && imgNode.nodeName == "IMG" && imgNode.className == "_WPQ_ExpandImage")
                imgNode.onclick = _WPQ_MenuCollapse;
        }
    }
}

function _WPQ_Init(navRootId) {
    if (!document.getElementById || (_WPQ_NavRoot = document.getElementById(navRootId)) == null)
        return;
    
    for (var node = _WPQ_NavRoot.firstChild; node != null; node = node.nextSibling) {
        if (node.nodeType != 1 || node.nodeName != "UL")
            continue;
        
        _WPQ_CollapsableListNodeSetup(node);
    }
}
