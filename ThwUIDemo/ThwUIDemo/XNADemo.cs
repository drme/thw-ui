using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using ThW.UI.Controls;
using ThW.UI.Design;
using ThW.UI.Sample;
using ThW.UI.Sample.Renderers.XNA;
using ThW.UI.Utils;

namespace ThW.UI.Demo
{
    public class XNADemo : Microsoft.Xna.Framework.Game, IScriptsHandler
    {
        public XNADemo()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += this.WindowSizeChanged;
            this.graphics.PreparingDeviceSettings += this.WindowSizeChanged;
			this.IsMouseVisible = true;
			this.IsFixedTimeStep = false;
		}

        private void WindowSizeChanged(object sender, EventArgs e)
        {
			if (null != this.GraphicsDevice)
			{
				if (null != this.activeDesktop)
				{
					this.activeDesktop.SetSize(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
				}

				if (null != this.demoSelectionDesktop)
				{
					this.demoSelectionDesktop.SetSize(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
				}
			}
        }

        protected override void LoadContent()
        {
            this.engine = new UIEngine();
            this.engine.WorkingFolder = this.Content.RootDirectory;
            this.engine.VirtualFileSystem = new AssemblyFilesSystem();
            this.engine.ScriptsHandler = this;
            this.engine.Render = new XNARenderer(this.GraphicsDevice, this.Content);
            this.engine.Audio = new XNAAudio(this.Content);
			this.engine.RegisterControlsCreator(new ControlsCreator<MenuRibbon>((x, y) => { return new MenuRibbon(x, y); }), MenuRibbon.TypeName, typeof(MenuRibbon));
			this.engine.RegisterControlsCreator(new ControlsCreator<AcButton>((x, y) => { return new AcButton(x, y); }), AcButton.TypeName, typeof(AcButton));

            this.demoSelectionDesktop = this.engine.NewDesktop(null, "ui/themes/generic/");
			this.demoSelectionDesktop.DrawCursor = false;
            
            var selectionWindow = this.demoSelectionDesktop.NewWindow("ui_demo_main.window.xml");

            selectionWindow.FindControl<Button>("hl2.demo").Clicked += this.HL2DemoClicked;
            selectionWindow.FindControl<Button>("va.demo").Clicked += this.VADemoClicked;
			selectionWindow.FindControl<Button>("me.demo").Clicked += this.MEDemoClicked;
			selectionWindow.FindControl<Button>("ac.demo").Clicked += this.ACDemoClicked;

            base.LoadContent();

			WindowSizeChanged(null, null);
        }

		private void ACDemoClicked(Button sender, EventArgs args)
		{
			this.activeDesktop = this.engine.NewDesktop(null, "ac/theme/");
			this.activeDesktop.SetSize(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

			var mainWindow = this.activeDesktop.NewWindow("ac/main_menu.window.xml");

			mainWindow.Animations.Add(new LinearPropertyAnimation(0, 100, 0.5, (x) => { mainWindow.Opacity = (float)x / 100.0f; }));
			mainWindow.Closing += this.GameDemoMainWindowClosing;
		}

		private void MEDemoClicked(Button sender, EventArgs args)
		{
			this.activeDesktop = this.engine.NewDesktop(null, "me/theme/");
			this.activeDesktop.SetSize(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

			var mainWindow = this.activeDesktop.NewWindow("me/main.window.xml");

			mainWindow.Animations.Add(new LinearPropertyAnimation(0, 100, 0.5, (x) => { mainWindow.Opacity = (float)x / 100.0f; }));
			mainWindow.Animations.Add(new LinearPropertyAnimation(800, 0, 0.5, (x) => { mainWindow.X = x; }));
			mainWindow.Closing += this.GameDemoMainWindowClosing;

			var story = mainWindow.FindControl<MenuRibbon>("story");
			var options = mainWindow.FindControl<MenuRibbon>("options");
			var extra = mainWindow.FindControl<MenuRibbon>("extra");

			meMenus.Clear();
			meMenus.Add(story);
			meMenus.Add(options);
			meMenus.Add(extra);

			story.ActiveChanged += this.MERibbonActiveChanged;
			options.ActiveChanged += this.MERibbonActiveChanged;
			extra.ActiveChanged += this.MERibbonActiveChanged;
		}

		private void MERibbonActiveChanged(Object sender, EventArgs e)
		{
			var ribbon = (MenuRibbon)sender;

			if (ribbon.Active)
			{
				foreach (var r in this.meMenus)
				{
					if (ribbon != r)
					{
						r.Active = false;
					}
				}
			}
		}

        private void HL2DemoClicked(Button sender, EventArgs args)
        {
            this.activeDesktop = this.engine.NewDesktop(null, "hl2/theme/");
            this.activeDesktop.SetSize(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

            var mainWindow = this.activeDesktop.NewWindow("hl2/main_menu.window.xml");

            mainWindow.Animations.Add(new LinearPropertyAnimation(0, 100, 0.5, (x) => { mainWindow.Opacity = (float)x / 100.0f; }));
          //  mainWindow.Animations.Add(new LinearPropertyAnimation(800, 0, 0.5, (x) => { mainWindow.X = x; }));
            mainWindow.Closing += this.GameDemoMainWindowClosing;
        }

        private void VADemoClicked(Button sender, EventArgs args)
        {
            this.activeDesktop = this.engine.NewDesktop(null, "hl2/theme/");
            this.activeDesktop.SetSize(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

            var mainWindow = this.activeDesktop.NewWindow("va/main.window.xml");

            mainWindow.Animations.Add(new LinearPropertyAnimation(0, 100, 0.5, (x) => { mainWindow.Opacity = (float)x / 100.0f; }));
            mainWindow.Animations.Add(new LinearPropertyAnimation(800, 0, 0.5, (x) => { mainWindow.X = x; }));
            mainWindow.Closing += this.GameDemoMainWindowClosing;
        }

        private void GameDemoMainWindowClosing(Windows.Window sender, EventArgs args)
        {
            this.activeDesktop = null;

            var window = this.demoSelectionDesktop.FindWindow("main_menu");

            window.Animations.Add(new LinearPropertyAnimation(800, 0, 0.5, (x) => { window.X = x; }));
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            this.ActiveDesktop.MouseMove(Mouse.GetState().X, Mouse.GetState().Y);

            if (Mouse.GetState().LeftButton != this.lastLeftMouseButtonState)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    this.ActiveDesktop.MousePress(Mouse.GetState().X, Mouse.GetState().Y);
                }
                else
                {
                    this.ActiveDesktop.MouseRelease(Mouse.GetState().X, Mouse.GetState().Y);
                }
            }

            if (true == IsPressed(Keys.Down))
            {
                this.ActiveDesktop.KeyPress((char)0, Key.Down);
            }

            if (true == IsPressed(Keys.Up))
            {
                this.ActiveDesktop.KeyPress((char)0, Key.Up);
            }

            if (true == IsPressed(Keys.Tab))
            {
                    this.ActiveDesktop.KeyPress((char)0, Key.Tab);
            }

            if (true == IsPressed(Keys.Enter))
            {
                this.ActiveDesktop.KeyPress((char)0, Key.Enter);
            }

            this.lastLeftMouseButtonState = Mouse.GetState().LeftButton;
            this.lastKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }

        private bool IsPressed(Keys key)
        {
            return ((true == lastKeyboardState.IsKeyDown(key)) && (true == Keyboard.GetState().IsKeyUp(key)));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            float dt = (this.lastTime == TimeSpan.Zero) ? 0.0f : (float)(gameTime.TotalGameTime - this.lastTime).Milliseconds / 1000.0f;

            ((XNARenderer)this.engine.Render).Init2d();

            this.ActiveDesktop.Render(dt);

            this.lastTime = gameTime.TotalGameTime;

            base.Draw(gameTime);
        }

        /// <summary>
        /// Handles uiser interface scripted events.
        /// </summary>
        /// <param name="script">script to execute.</param>
        /// <param name="control">clicked control.</param>
        public void OnUIEvent(string script, Controls.Control control)
        {
            if ("quit" == script)
            {
                Exit();
            }
            else if ("demo.close" == script)
            {
                control.Window.Close();
            }
        }

        private Desktop ActiveDesktop
        {
            get
            {
                if (null != this.activeDesktop)
                {
                    return this.activeDesktop;
                }
                else
                {
                    return this.demoSelectionDesktop;
                }
            }
        }

        private UIEngine engine = null;
        private Desktop demoSelectionDesktop = null;
        private Desktop activeDesktop = null;
        private TimeSpan lastTime = TimeSpan.Zero;
        private ButtonState lastLeftMouseButtonState = ButtonState.Released;
        private KeyboardState lastKeyboardState;
        private GraphicsDeviceManager graphics = null;
		private List<MenuRibbon> meMenus = new List<MenuRibbon>();
    }
}
