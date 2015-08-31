# About
ThwUI is platform independent user interface library for games.
It allows to create and display various windows and controls on it in games interfaces, and also interact with them.
Library is written in C# and allows rendering user interface using pluggable render engine. The sample XNA, Direct3D, OpenGL renderers are provided.
 Library provides:
 * User interface designer (WYSIWYG style).
 * Saving and loading user interface to/from XML files.
 * Themes support.
 * Skins support.
 * True type fonts support.
 * Rendering using XNA, OpenGL, Direct3D or any other pluggable method.

2. Building
Visual Studio 2013 could be used to build library and demos. For building demos XNA framework is required. It can be downloaded from https://mxa.codeplex.com/releases.

3. Usage
Basic usage examples are provided in the ThwUIDemo project. This demo uses XNA renderer. The Designer project uses GDI rendering for drawing user interface elements.
The basing library usage is as follows:
* Add reference to the ThwUI library to your project.
* Create instance of UIEngine object.
* Specify render implementation for ui engine (uiEngine.Render = new XNAREnder()).
* Specify working folder (WorkingFolder = this.Content.RootDirectory);
* create desktop (var desktop = uiEngine.NewDeskop());
* create window (desktop.NewWindow(“1.window.xml”);
* render desktop in your rendering loop (desktop.Render());
* pass mouse, keyboard events to desktop object:
desktop.MouseMove()
desktop.MousePress()
desktop.MouseRelease()
desktop.KeyPress()

3.1. Render
This is only one interface which has to be implemented by library user.
It’s necessary to implement ThW.UI.Utils.IRender interface and assign to UIEngine.Render property.
There are several sample imementations:
* RenderXNA – using XNA for rendering
* RenderDirect3D – using Direct3D for rendering.
* RenderOpenGL – using OpenGL for rendering.
* RenderGDI – using GDI for rendering.

3.2. Creating Desktop.
All windows in user interface are grouped in Desktops (usually one is sufficient for an application, thus more can be used).
Desktop desktop = uiEngine.NewDesktop();
 Desktop could be created empty or loaded from xml file by passing parameter to NewDesktop method.

3.3. Adding window to the desktop
New window is created and added to desktop by calling deskop.NewWindow() method.
 Window can be created empty or loaded from xml file by passing file name to NewWindow method.

3.4. Adding controls to window
Controls can be created by calling windows method CreateControl and passing  controls type as parameter
Button button = window.CreateControl<Button>() and after creation control has to be added to required parent:
•	adding to window window.AddControl(button);
•	adding to panel panel.AddControl(button);
Detailed example is provided in the ThwUIDemo projects file XNADemo.cs.

3.5. Creating Windows from XML files.
For creating windows using XML, use ThwUIDesigned project for creating windows using editor.
 Created window can be later loaded using
Window window = desktop.NewWindow("mywindow.window.xml");

3.6. Rendering User interface
In order to render user interface developer has to add.
Desktop.Render(); method call into the application rendering loop.
3.7. Responding to user events.
In order to handle mouse clicks, key presses it is required to call Desktop methods MousePresse, MouseReleases, KeyPressed, etc..
 Detailed examples are provided in ThwUIDemo project.

3.8. User interface Internationalization.
User interface supports internalization. Thus text messages can be changed according required language. In order to use this feature, ILanguage interface has to be implemented and its instance has to be assigned to uiEngine.Language property.

4. Licensing
Library is provided using BSD license. If other type license is required feel free to contact library maintainer sarunas@ieee.org

