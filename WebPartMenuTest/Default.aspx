<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link rel="Stylesheet" type="text/css" href="CollapsibleMenu.css" /><style type="text/css">
					div#WPQ3Nav ul
{
	padding:0px;
	margin:0px;
	list-style:none;
	background-color:Silver;
}

div#WPQ3Nav li 
{
	float:left;
	position:relative;
	background-color:Silver;
	border:outset 2px Silver;
	color:Black;
	margin:0px;
	padding:2px;
	cursor:pointer;
}

div#WPQ3Nav li ul 
{
	display:none;
	position:absolute;
	border:solid 1px black;
	color:Black;
	margin:0px;
	padding:0px;
}

div#WPQ3Nav li ul li 
{
    float:none;
	border-bottom:solid 1px Gray;
	border-left:none;
	border-right:none;
}

				</style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="WPQ3Nav">
            <span class="WPQ3MenuTitle">Menu Tree</span>
            <ul id="WPQ3NavRoot">
                <li>Test 1<img class="WPQ3ExpandImage" height="12" src="/_layouts/images/TPMAX1.GIF"
                    width="9" />
                    <ul style="display: none; left: 0px; top: 19px">
                        <li><a href="asdfasdfsdfasdfas">Subtest</a>
                            <ul style="display: none; left: 45px; top: 2px" id="xxx">
                                <li><a href="hehehe">SubSub</a>
                                    <ul style="display: none; left: 46px; top: 2px">
                                        <li><a href="sdfasdfasdfa">asafsdfa</a> </li>
                                    </ul>
                                </li>
                                <li>NOther </li>
                            </ul>
                        </li>
                        <li>xx</li>
                        <li>asdfsdfasdfasdfasdfasdfasdfsdfsdf </li>
                    </ul>
                </li>
                <li><a href="asdflkajsdf;sdf">Test 2</a><img class="WPQ3ExpandImage" height="12"
                    src="/_layouts/images/TPMAX1.GIF" width="9" />
                    <ul>
                        <li><a href="asdfasdf">asdfasdf</a></li>
                        <li><a href="asdfasdf">asdfasdf</a></li>
                        <li><a href="asdfasdfasdfasdf">asdfasdf</a></li>
                        <li><a href="qagraqg">atqgaqg</a>
                            <ul>
                                <li><a href="asdfasdfasdf">asdfasdf</a> </li>
                            </ul>
                        </li>
                    </ul>
                </li>
                <li>t </li>
            </ul>
        </div>

        <script language="JavaScript" type="text/javascript">
					// JScript File

var WPQ3activeTimer = null;
var WPQ3activeNode = null;

function WPQ3containsNode(parentNode, testNode) {
    var node;
    
    for (node = testNode; node != null; node = node.parentNode) {
        if (node.nodeType == 1 && node.id == "WPQ3Nav")
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

function getActiveNode(node) {
    var childNode;
    
    if (node.nodeType != 1)
        return null;
    
    if (node.nodeName == "LI")
        return node;
        
    if (node.parentNode != null && node.parentNode.nodeType == 1 && node.parentNode.nodeName == "LI")
        return node.parentNode;

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

function WPQ3menuTimeout() {
    var navRoot;
    
    clearTimeout(WPQ3activeTimer);
    WPQ3activeTimer = null;
    
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

    WPQ3activeNode = getActiveNode(this);
    
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
                node.src = "/_layouts/images/TPMIN1.GIF";
        }
    } else {
        WPQ3hideNodeTree(ulNode);
        for (node = ulNode.parentNode.firstChild; node != null; node=node.nextSibling) {
            if (node.nodeType == 1 && node.className == "WPQ3ExpandImage")
                node.src = "/_layouts/images/TPMAX1.GIF";
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

    WPQ3activeNode = getActiveNode(this);
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

    WPQ3activeNode = getActiveNode(this);
    
    if (currentNode == null)
        return;

    currentNode.style.display = "block";
    currentNode.style.top = 2;
    currentNode.style.left = currentNode.parentNode.offsetWidth + currentNode.parentNode.offsetLeft - 2;

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
    if (WPQ3activeTimer != null) {
        clearTimeout(WPQ3activeTimer);
        WPQ3activeTimer = null;
    }
    
    if (WPQ3activeNode == null || WPQ3activeNode == getActiveNode(this)) {
        WPQ3activeTimer = setTimeout(WPQ3menuTimeout, 1000);
        WPQ3activeNode = null;
    }
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
        
        if (ulNode.parentNode.nodeName == "DIV") {
            if (foundAnchor) {
                if (imageNode == null)
                    childNode.onmouseover = WPQ3mouseOver;
                else {
                    childNode.onmouseover = WPQ3mouseOverTop;
                    imageNode.onclick = WPQ3mouseClick;
                }
            } else {
                childNode.onclick = WPQ3mouseClick;
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

        </script>

        &nbsp;
        </form>
</body>
</html>
