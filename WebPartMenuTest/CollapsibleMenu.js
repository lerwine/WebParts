function WPQ3getUlNode(node) {
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

function WPQ3MenuCollapse() {
    var node;
    
    if (this.parentNode == null || this.parentNode.nodeName != "LI")
        return;
    
    for (node=this.parentNode.firstChild; node != null; node = node.nextSibling) {
        if (node.nodeType != 1 || node.nodeName != "UL")
            continue;
        if (!node.style || !node.style.display ||node.style.display != "block") {
            node.style.display = "block";
            this.src = "http://spsdev/sites/wptest/_layouts/images/TPMIN2.GIF";
        } else {
            node.style.display = "none";
            this.src = "http://spsdev/sites/wptest/_layouts/images/TPMAX2.GIF";
        }
        this.parentNode.scrollIntoView();
    }
}

function WPQ3CollapsableListNodeSetup(rootNode) {
    var liNode;
    var imgNode;
    
    if (rootNode == null)
        return;
    
    for (liNode = rootNode.firstChild; liNode != null; liNode=liNode.nextSibling) {
        if (liNode.nodeType != 1 || liNode.nodeName != "LI")
            continue;
        
        WPQ3CollapsableListNodeSetup(WPQ3getUlNode(liNode));
        
        for (imgNode = liNode.firstChild; imgNode != null; imgNode = imgNode.nextSibling) {
            if (imgNode.nodeType == 1 && imgNode.nodeName == "IMG" && imgNode.className == "WPQ3ExpandImage")
                imgNode.onclick = WPQ3MenuCollapse;
        }
    }
}

function WPQ3Setup() {
    var navRoot;
    
    if (!document.getElementById || (navRoot = document.getElementById("WPQ3Nav")) == null)
        return;
    
    for (var node = navRoot.firstChild; node != null; node = node.nextSibling) {
        if (node.nodeType != 1 || node.nodeName != "UL")
            continue;
        
        WPQ3CollapsableListNodeSetup(node);
    }
}

WPQ3Setup();