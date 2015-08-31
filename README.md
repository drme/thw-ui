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

# Building
Visual Studio 2013 could be used to build library and demos. For building demos XNA framework is required. It can be downloaded from https://mxa.codeplex.com/releases.

# Usage
Basic usage examples are provided in the ThwUIDemo project. This demo uses XNA renderer. The Designer project uses GDI rendering for drawing user interface elements.
The basing library usage is as follows:
* Add reference to the ThwUI library to your project.
* Create instance of UIEngine object.
* Specify render implementation for ui engine (uiEngine.Render = new XNAREnder()).
* Specify working folder (WorkingFolder = this.Content.RootDirectory);
* create desktop (var desktop = uiEngine.NewDeskop());
* create window (desktop.NewWindow(“1.window.xml”);
* render desktop in your rendering loop (desktop.Render());
* pass mouse, keyboard events to desktop object: using desktop.MouseMove(), desktop.MousePress(), desktop.MouseRelease(), and desktop.KeyPress().

## Render
This is only one interface which has to be implemented by library user.
It’s necessary to implement ThW.UI.Utils.IRender interface and assign to UIEngine.Render property.
There are several sample imementations:
* RenderXNA – using XNA for rendering
* RenderDirect3D – using Direct3D for rendering.
* RenderOpenGL – using OpenGL for rendering.
* RenderGDI – using GDI for rendering.

## Creating Desktop.
All windows in user interface are grouped in Desktops (usually one is sufficient for an application, thus more can be used).
Desktop desktop = uiEngine.NewDesktop(); Desktop could be created empty or loaded from xml file by passing parameter to NewDesktop method.

## Adding window to the desktop
New window is created and added to desktop by calling deskop.NewWindow() method. Window can be created empty or loaded from xml file by passing file name to NewWindow method.

## Adding controls to window
Controls can be created by calling windows method CreateControl and passing  controls type as parameter
Button button = window.CreateControl<Button>() and after creation control has to be added to required parent:
*	adding to window window.AddControl(button);
*	adding to panel panel.AddControl(button);
Detailed example is provided in the ThwUIDemo projects file XNADemo.cs.

## Creating Windows from XML files.
For creating windows using XML, use ThwUIDesigned project for creating windows using editor. Created window can be later loaded using Window window = desktop.NewWindow("mywindow.window.xml");

## Rendering User interface
In order to render user interface developer has to add Desktop.Render(); method call into the application rendering loop.

## Responding to user events.
In order to handle mouse clicks, key presses it is required to call Desktop methods MousePresse, MouseReleases, KeyPressed, etc. Detailed examples are provided in ThwUIDemo project.

## User interface Internationalization.
User interface supports internalization. Thus text messages can be changed according required language. In order to use this feature, ILanguage interface has to be implemented and its instance has to be assigned to uiEngine.Language property.

## Screenshots

![Half-Life 2 sample image](/readme/hl2.png?raw=true "Half-Life 2 sample image")
![Mirrors Edge sample image](/readme/me.png?raw=true "Mirrors Edge sample image")
![Velvet Assassin sample image](/readme/va.png?raw=true "Velvet Assassin sample image")

# Licensing
Library is provided using BSD license. If other type license is required feel free to contact library maintainer sarunas@ieee.org

