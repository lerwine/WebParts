Debug Build and Deploy:
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTree.csproj" /p:TargetFX1_1=true /p:Configuration=Debug /t:Rebuild
C:\Windows\System32\MakeCAB.exe -f "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTree.ddf"
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\60\bin\STSADM.EXE" -o addwppack -filename "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTree.cab" -globalinstall -force
C:\Windows\System32\iisreset.exe
COPY /Y "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\bin\FX1_1\Debug\MenuTree.dll" C:\Inetpub\wwwroot\bin\
COPY /Y "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\bin\FX1_1\Debug\MenuTree.pdb" C:\Inetpub\wwwroot\bin\
"c:\Program Files\Internet Explorer\iexplore.exe" http://spsdev/sites/wptest/default.aspx

Debug Quick Build and Deploy:
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTree.csproj" /p:TargetFX1_1=true /p:Configuration=Debug
C:\Windows\System32\MakeCAB.exe -f "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTree.ddf"
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\60\bin\STSADM.EXE" -o addwppack -filename "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTree.cab" -globalinstall -force
C:\Windows\System32\iisreset.exe
COPY /Y "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\bin\FX1_1\Debug\MenuTree.dll" C:\Inetpub\wwwroot\bin\
COPY /Y "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\bin\FX1_1\Debug\MenuTree.pdb" C:\Inetpub\wwwroot\bin\
"c:\Program Files\Internet Explorer\iexplore.exe" http://spsdev/sites/wptest/default.aspx

Debug Build direct:
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTree.csproj" /p:TargetFX1_1=true /p:Configuration=Debug /t:Rebuild /p:OutputPath=C:\Inetpub\wwwroot\bin\ & C:\Windows\System32\iisreset.exe & "c:\Program Files\Internet Explorer\iexplore.exe" http://spsdev/sites/wptest/default.aspx

Debug Quick build direct:
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTree.csproj" /p:TargetFX1_1=true /p:Configuration=Debug /p:OutputPath=C:\Inetpub\wwwroot\bin\ & C:\Windows\System32\iisreset.exe & "c:\Program Files\Internet Explorer\iexplore.exe" http://spsdev/sites/wptest/default.aspx

Release Build and Deploy:
C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTree.csproj" /p:TargetFX1_1=true /p:Configuration=Release /t:Rebuild
C:\Windows\System32\MakeCAB.exe -f "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTreeR.ddf"
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\60\bin\STSADM.EXE" -o addwppack -filename "C:\Documents and Settings\leonard.erwine\My Documents\Visual Studio 2005\Projects\WebParts\MenuTree\MenuTree.cab" -globalinstall -force

Delete Web Part
"C:\Program Files\Common Files\Microsoft Shared\web server extensions\60\bin\STSADM.EXE" -o deletewppack -name MenuTree.cab
C:\Windows\System32\iisreset.exe

Calling Order, normal:
WebPart.Constructor
WebPart.Properties get/set
WebPart.CreateWebPartMenu
WebPart.CreateChildControls
WebPart.SaveViewState
WebPart.RenderWebPart

Calling Order, Child Control Event:
WebPart.Constructor
WebPart.Properties get/set
WebPart.CreateChildControls
WebPart.CreateWebPartMenu
WebPart.ChildControl_EventHandlers
WebPart.SaveViewState
WebPart.RenderWebPart

Calling Order, Edit initial:
WebPart.Constructor
WebPart.LoadViewState
WebPart.CreateWebPartMenu
WebPart.GetToolParts
ToolPart.Constructor
ToolPart.ToolPart_Init_Handler
WebPart.CreateChildControls
ToolPart.CreateChildControls
WebPart.Properties get/set
WebPart.SaveViewState
ToolPart.SaveViewState
WebPart.RenderWebPart
ToolPart.RenderToolPart

Calling Order, Edit, Web Part Child Control Event:
WebPart.Constructor
WebPart.LoadViewState
WebPart.CreateChildControls
WebPart.CreateWebPartMenu
WebPart.GetToolParts
ToolPart.Constructor
ToolPart.ToolPart_Init_Handler
ToolPart.LoadViewState
WebPart.Properties get/set
WebPart.ChildControl_Event_Handler
ToolPart.CreateChildControls
WebPart.SaveViewState
ToolPart.SaveViewState
WebPart.RenderWebPart
ToolPart.RenderToolPart

Calling Order, Edit, Tool Part Child Control Event:
WebPart.Constructor
WebPart.CreateWebPartMenu
WebPart.GetToolParts
ToolPart.Constructor
ToolPart.ToolPart_Init_Handler
ToolPart.LoadViewState
WebPart.Properties get/set
ToolPart.CreateChildControls
ToolPart.ChildControl_Event_Handler
WebPart.CreateChildControls
WebPart.SaveViewState
ToolPart.SaveViewState
WebPart.RenderWebPart
ToolPart.RenderToolPart

Calling Order, Edit, Cancel:
WebPart.Constructor
WebPart.CreateWebPartMenu
WebPart.GetToolParts
ToolPart.Constructor
ToolPart.ToolPart_Init_Handler
ToolPart.LoadViewState
WebPart.Properties get/set
WebPart.CreateChildControls
WebPart.SaveViewState
WebPart.RenderWebPart
