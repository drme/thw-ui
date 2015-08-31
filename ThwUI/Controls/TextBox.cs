using System;
using System.Collections.Generic;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    public interface ITextEntry
    {
        void StartTextEntry(TextBox sender);
    }

    /// <summary>
    /// Text box control for text entry using keyboard.
    /// </summary>
	public class TextBox : Control
	{
        /// <summary>
        /// Constructs text box object.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
		public TextBox(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
			this.Border = BorderStyle.BorderLoweredDouble;
			this.Cursor = MousePointers.PointerText;
			this.NeedTranslation = false;
			this.TextOffset.X = 3;
			this.TextOffset.Y = 1;
            this.HintTextColor = window.Desktop.Theme.Colors.TextLight;
        }

		protected override void KeyPress(char c, Key key)
        {
			if (false == this.HasFocus)
			{
                base.KeyPress(c, key);

				return;
			}

            if (true == this.multiLine)
            {
                if (this.textLines.Count == 0)
                {
                    this.textLines.Add("");
                }
            }

            if ((Key.None == key) || (Key.ShiftLeft == key) || (Key.ShiftRight == key) || (Key.Space == key))
			{
				CharKeyPressed(c);

                RaiseTextChangedEvent();
			}
			else
			{
				switch (key)
				{
                    case Key.Enter:
                        EnterPressed();
                        break;
                    case Key.Backspace:
                        BackspacePressed();
                        break;
					case Key.Left:
						LeftPressed();
						break;
					case Key.Right:
						RightPressed();
						break;
					case Key.Up:
						UpPressed();
						break;
					case Key.Down:
						DownPressed();
						break;
					case Key.PgUp:
						PageUpPressed();
						break;
					case Key.PgDn:
						PageDownPressed();
						break;
					case Key.Home:
						HomePressed();
						break;
					case Key.End:
						EndPressed();
						break;
					case Key.Delete:
						DeleteKeyPressed();
                        if (false == this.multiLine)
                        {
                            RaiseTextChangedEvent();
                        }
						break;
					default:
						break;
				}
			}

            FixCursor();

            base.KeyPress(c, key);
        }

        /// <summary>
        /// Entered text in the text box.
        /// </summary>
		public override String Text
        {
            set
            {
                if (null == value)
                {
                    value = "";
                }

                if (true == this.multiLine)
                {
                    this.textLines.Clear();

                    int s = 0;

                    for (int i = 0; i < value.Length; i++)
                    {
                        if (value[i] == '\n')
                        {
                            this.textLines.Add(value.Substring(s, (i - s - 1)));
                            s = (int)(i) + 1;
                        }
                    }

                    this.textLines.Add(value.Substring(s));
                }
                else
                {
                    base.Text = value;
                    this.editingPosition.X = value.Length - 1;
                }

                RaiseTextChangedEvent();
            }
            get
            {
                return base.Text;
            }
        }

		protected override void OnMousePressed(int x, int y)
        {
			this.selectStart.X = FindPos();
            this.selectStart.Y = FindRow();
            this.selectEnd.X = this.selectStart.X;
            this.selectEnd.Y = this.selectStart.Y;
            this.selecting = true;
        }

		protected override void OnMouseReleased(int x, int y)
        {
            this.selectEnd.X = FindPos();
            this.selectEnd.Y = FindRow();
			this.selecting = false;
        }

		protected override void OnMouseMove(int x, int y)
        {
			if (true == this.selecting)
			{
                this.selectEnd.X = FindPos();
                this.selectEnd.Y = FindRow();
                this.editingPosition.X = this.selectEnd.X;
                this.editingPosition.Y = this.selectEnd.Y;
            }

			base.OnMouseMove(x, y);
        }

        protected override void Render(Graphics graphics, int x, int y)
        {
			graphics.SetRegion(x + this.bounds.X, y + this.bounds.Y, this.bounds.Width, this.bounds.Height);
			base.Render(graphics, x, y);
			graphics.ClearRegion();
        }

		public void AddLine(String strText)
        {
			this.textLines.Add(strText);
			UpdateSize();
            RaiseTextChangedEvent();
        }

		public bool AutoSize
        {
            set
            {
                this.autoSize = value;
            }
            get
            {
                return this.autoSize;
            }
        }

		public bool MultiLine
        {
            set
            {
                this.multiLine = value;
            }
            get
            {
                return this.multiLine;
            }
        }

        protected override void RenderText(Graphics graphics, int x, int y)
        {
            if (null == this.FontInfo.Font) 
			{
				return;
			}

			graphics.SetColor(this.textColor, 1.0f);

			if (true == this.multiLine)
			{
				RenderLines(graphics, x, y);
			}
			else
			{
				RenderTextSingleLine(graphics, x, y);
			}
        }

        protected virtual void RenderTextSingleLine(Graphics graphics, int x, int y)
        {
			String text = this.controlText;
            bool useHint = false;

            if ((text == null) || (text.Length <= 0))
            {
                text = this.hintText.Text;

                if ((text == null) || (text.Length <= 0))
                {
                    return;
                }

                useHint = true;
            }

			int sst = this.selectStart.X + 1;
			int sse = this.selectEnd.X + 1;

			if (sse < sst)
			{
				sst = this.selectEnd.X + 1;
				sse = this.selectStart.X + 1;
			}

			int ssl = sse - sst;
            int offy = (this.bounds.Height - 0 - this.FontInfo.Font.TextHeight(text)) / 2;

			RenderSelection(graphics, text, x, y + this.TextOffset.Y + this.Bounds.Y + offy - 2, sst, ssl);

            if (true == useHint)
            {
                graphics.SetColor(this.hintTextColor);
            }
            else
            {
                graphics.SetColor(this.textColor);
            }

            this.FontInfo.Font.DrawText(graphics, this.borderSize + this.TextOffset.X + x + this.Bounds.X, offy + y + this.Bounds.Y, text);

			if (false == this.HasFocus)
			{
				return;
			}

            long t = DateTime.Now.Millisecond;

			if (Math.Sin(3.14 * t / 1 * 4) <= 0.0)
			{
				return;
			}

			int l = this.editingPosition.X + 1;

			if (l > text.Length)
			{
                l = text.Length;
			}

            int len = this.FontInfo.Font.TextLength(text, 0, l);

			//int len = this.m_pFont.TextLength(&(this.controlText.c_str())[p], 0, this.m_nEditPos - p);

            //int tw = 1;
            int th = this.bounds.Height - 8;

			//int tx = this.bounds.X + X + len + 5;
			int ty = this.bounds.Y + y + 4;

			/*if (this.m_eBorder == BorderStyle.BorderNone)
			{
				th += 6;
				ty -= 3;
			}
			else if (this.m_eBorder == BorderStyle.BorderFlat)
			{
				th += 6;
				ty -= 3;
			}*/

			graphics.DrawRectangle(this.borderSize + this.TextOffset.X + x + this.Bounds.X + len, ty, 2, th);
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
		protected override void AddProperties()
        {
			base.AddProperties();

            String groupName = "TextBox";

            AddProperty(new PropertyBoolean(this.AutoSize, "autoSize", groupName, "autoSize", (x) => { this.AutoSize = x; }, ()=> {return this.AutoSize;}));
            AddProperty(new PropertyBoolean(this.MultiLine, "multiLine", groupName, "multiLine", (x) => { this.MultiLine = x; }, () => { return this.MultiLine; }));
            AddProperty(new PropertyBoolean(this.AutoShrink, "autoShrink", groupName, "autoShrink", (x) => { this.AutoShrink = x; }, () => { return this.AutoShrink; }));
            AddProperty(new PropertyString(this.HintText, "hintText", groupName, "hintText", (x) => { this.HintText = x; }, () => { return this.HintText; }));
            AddProperty(new PropertyColor(this.HintTextColor, "hintTextColor", groupName, "hintTextColor", (x) => { this.HintTextColor = x;}, ()=>{return this.HintTextColor;}));
        }

        /// <summary>
        /// Handles mouse click event on textbox.
        /// </summary>
        /// <param name="X">mouse click X position.</param>
        /// <param name="Y">mouse click Y position.</param>
		protected override void OnClick(int x, int y)
        {
			this.editingPosition.X = FindPos();
			this.editingPosition.Y = FindRow();

		    base.OnClick(x, y);
        }

        protected override void OnLanguageChanged()
        {
            base.OnLanguageChanged();

            if (false == this.NeedTranslation)
            {
                this.hintText.Text = this.hintText.ReferenceText;

                return;
            }

            if (null != this.Engine.Language)
            {
                String groupName = (null == this.Window) ? this.Name : this.Window.Name;

                this.hintText.Text = this.Engine.Language.Translate("window." + groupName, this.hintText.ReferenceText);
            }
            else
            {
                this.hintText.Text = this.hintText.ReferenceText;
            }
        }

		protected override void UpdateSizeSelf()
        {
			if ( (true == this.multiLine) && (true == this.autoSize) )
			{
				int yy = 0;
				int xx = 0;

				for (int i = 0; i < this.textLines.Count; i++)
				{
                    int len = this.FontInfo.Font.TextLength(this.textLines[i], 0, -1);
                    yy += this.FontInfo.Font.TextHeight(this.textLines[i], 0, -1) + 4;

					if (len > xx)
					{
						xx = len;
					}
				}

				xx += 4;

				if (yy > this.bounds.Height)
				{
					this.bounds.Height = yy + this.borderSize * 2;
				}

				if (xx > this.bounds.Width)
				{
					this.bounds.Width = xx + this.borderSize * 2;
				}
			}

			base.UpdateSizeSelf();
        }

        protected void RenderSelection(Graphics render, String text, int x, int y, int start, int len)
        {
			if (start < 0)
			{
				start = 0;
				len--;
			}

            render.SetColor(this.Window.Desktop.Theme.Colors.Highlight);

			if (start > (text.Length))
			{
				return;
			}

			if (start + len > (text.Length))
			{
				len = (text.Length) - start;
			}

			if (len <= 0)
			{
				return;
			}

            int textH = this.FontInfo.Font.TextHeight(text.Substring(start), 0, len) + 4;
            int startX = this.FontInfo.Font.TextLength(text, 0, start);
            int selectionW = this.FontInfo.Font.TextLength(text.Substring(start), 0, len);
			int offx = this.TextOffset.X + this.borderSize;

			render.DrawRectangle(offx + x + this.Bounds.X + startX, y, selectionW, textH);
        }

        protected void RenderLines(Graphics render, int x, int y)
        {
			int yy = 0;
			int xx = this.TextOffset.X + this.borderSize * 2;

			Point2D start = this.selectStart;
			Point2D end = this.selectEnd;

            if (this.selectEnd.Y < this.selectStart.Y)
			{
                start = this.selectEnd;
                end = this.selectStart;
			}

            int str = this.selectStart.Y;
            int ser = this.selectEnd.Y;

			if (str > ser)
			{
				int t = ser;
				ser = str;
				str = t;
			}

			int sst = this.selectStart.X + 1;
            int sse = this.selectEnd.X + 1;

			if (sse < sst)
			{
                sst = this.selectEnd.X + 1;
                sse = this.selectStart.X + 1;
			}

            int h = this.FontInfo.Font.TextHeight("") + 4;

			for (int i = 0; i < (this.textLines.Count); i++)
			{
				if (this.TextOffset.Y + this.borderSize + h * (i + 1) > this.bounds.Height)
				{
					break;
				}

                if ((i == this.selectStart.Y) && (this.selectStart.Y == this.selectEnd.Y))
				{
					RenderSelection(render, this.textLines[i], x, this.TextOffset.Y + this.borderSize + y + h * i, sst, sse-sst);
				}
				else
				{
					if (i == str)
					{
						RenderSelection(render, this.textLines[i], x, this.TextOffset.Y + this.borderSize + y + h * i, start.X + 1, (this.textLines[i].Length) - start.X);
					}
					else if (i == ser)
					{
						RenderSelection(render, this.textLines[i], x, this.TextOffset.Y + this.borderSize + y + h * i, 0, end.X + 1);
					}
					else if ( (i > str) && (i < ser) )
					{
						RenderSelection(render, this.textLines[i], x, this.TextOffset.Y + this.borderSize + y + h * i, 0, (this.textLines[i].Length));
					}
				}

				render.SetColor(this.textColor);

                this.FontInfo.Font.DrawText(render, this.TextOffset.X + this.borderSize + this.Bounds.X + x, this.TextOffset.Y + this.borderSize + this.bounds.Y + y + this.topOffset + yy, this.textLines[i]);

                int len = this.FontInfo.Font.TextLength(this.textLines[i]);

				yy += h;

				if (len > xx)
				{
					xx = len;
				}
			}

			xx += 4;

			if (true == this.autoSize)
			{
				yy = this.TextOffset.Y + this.borderSize * 2 + h * this.textLines.Count + 20;

				if (true == this.autoShrink)
				{
					this.bounds.Height = yy + this.borderSize * 2 + this.TextOffset.Y;
					this.bounds.Width = xx + this.borderSize * 2 + this.TextOffset.X + 50;
				}
				else
				{
					if (yy > this.bounds.Height)
					{
						this.bounds.Height = yy + this.borderSize * 2 + this.TextOffset.Y;
					}

					if (xx > this.bounds.Width)
					{
						this.bounds.Width = xx + this.borderSize * 2 + this.TextOffset.X + 50;
					}
				}
			}

			RenderBeam(render, x - 1, y - 2);
        }

		protected int FindPos()
        {
			// after which char to draw pointer.. -1 before 0 char, 0 after 0 char, 1 - after 1 char. etc..

			int x = TranslateX(this.Window.Desktop.MouseX);

			String strText = this.controlText;

			if (true == this.multiLine)
			{
				int row = FindRow();

				if (this.textLines.Count == 0)
				{
					this.textLines.Add("");
				}

				if (row < 0)
				{
					int y = TranslateX(this.Window.Desktop.MouseX);

                    if (y >= (int)(this.TextOffset.Y + this.borderSize + this.textLines.Count * (this.FontInfo.Font.TextHeight("") + 4)))
					{
						if (this.textLines.Count > 0)
						{
							return (int)(this.textLines[this.textLines.Count - 1].Length) - 1;
						}
						else
						{
							return -1;
						}
					}
					else
					{
						return -1;
					}
				}

				strText = this.textLines[row];
			}

			int offx = this.borderSize + this.TextOffset.X;

			if (strText.Length <= 0)
			{
				return -1;
			}

            int w = this.FontInfo.Font.TextLength(strText, 0, 1);
	
			if (x <= w/2 + offx) // before first char
			{
				return -1;
			}

			int l = w;

			for (int j = 2; j <= (int)(strText.Length); j++)
			{
                int tl = this.FontInfo.Font.TextLength(strText, 0, j);

				int cl = (tl - l) / 2;

				if (x <= offx + l + cl)
				{
					return j - 2;
				}

				l = tl;
			}

			return (int)(strText.Length) - 1;
        }

		protected int FindRow()
        {
			int y = TranslateY(this.Window.Desktop.MouseY);

			int yy = this.borderSize + this.topOffset + this.TextOffset.Y;

			if ( (y < yy) || (0 == this.textLines.Count) )
			{
				return 0;//-1;
			}

			for (int i = 0; i < this.textLines.Count; i++)
			{
                int h = this.FontInfo.Font.TextHeight(this.textLines[i]) + 4;

				if ( (y >= yy) && (y <= yy + h) )
				{
					return (int)(i);
				}

				yy += h;
			}

			return this.textLines.Count - 1;
        }

        protected void RenderBeam(Graphics render, int x, int y)
        {
			if (false == this.HasFocus)
			{
				return;
			}

            long t = DateTime.Now.Millisecond;
            long dt = this.lastDraw - t;
            this.lastDraw = t;
            this.drawTime += dt;

			if (Math.Sin((3.14 * t / 1 * 4)) <= 0.0)
			{
//				return;
			}

			if (this.textLines.Count == 0)
			{
				this.textLines.Add("");
			}

			if ( (this.editingPosition.Y < 0) || (this.editingPosition.Y >= this.textLines.Count) )
			{
				return;
			}

			String strText = this.textLines[this.editingPosition.Y];

			int l = this.editingPosition.X + 1;

			if (l > strText.Length)
			{
				l = strText.Length;
			}

            int len = this.FontInfo.Font.TextLength(strText, 0, l);

            int nTextHeight = this.FontInfo.Font.TextHeight(strText) + 4;

			int ty = this.borderSize + this.Bounds.Y + y + this.TextOffset.Y + nTextHeight * this.editingPosition.Y;
			int tx = this.borderSize + this.Bounds.X + x + this.TextOffset.X + len;

			render.DrawRectangle(tx, ty, 1, nTextHeight);
        }

		protected void DeleteSelection()
        {
			if (false == HasSelection())
			{
				return;
			}

			if (true == this.multiLine)
			{
				Point2D start = this.selectStart;
				Point2D end = this.selectEnd;

                if (this.selectEnd.Y < this.selectStart.Y)
			    {
                    start = this.selectEnd;
                    end = this.selectStart;
			    }

                this.editingPosition.X = start.X;
                this.editingPosition.Y = start.Y;

                if (this.selectStart.Y == this.selectEnd.Y)
			    {
                    if (selectStart.Y != -1)
				    {
                        int sst = this.selectStart.X + 1;
                        int sse = this.selectEnd.X + 1;

					    if (sse < sst)
					    {
                            sst = this.selectEnd.X + 1;
                            sse = this.selectStart.X + 1;
					    }

                        this.textLines[this.selectStart.Y] = this.textLines[this.selectStart.Y].Remove(sst, sse - sst);
				    }
			    }
			    else
			    {
				    //erase last line.
                    this.textLines[end.Y] = this.textLines[end.Y].Remove(0, end.X + 1);

				    //remove middle lines
				    for (int i = end.Y - 1; i > start.Y; i--)
				    {
					    this.textLines[i] = "";
				    }

				    //clean first line
				    if (start.Y >= 0)
				    {
                        if (this.textLines[start.Y].Length > start.X + 1)
                        {
                            this.textLines[start.Y] = this.textLines[start.Y].Remove(start.X + 1, 1);
                        }
				    }

				    if (start.Y < 0)
				    {
					    start.Y = 0;
				    }

				    for (int i = end.Y; i >= start.Y; i--)
				    {
					    if (this.textLines[i].Length == 0)
					    {
						    this.textLines.RemoveAt(i);
					    }
				    }
			    }
			}
			else
			{
                int sst = this.selectStart.X + 1;
                int sse = this.selectEnd.X + 1;

				if (sse < sst)
				{
                    sst = this.selectEnd.X + 1;
                    sse = this.selectStart.X + 1;
				}

				int ssl = sse - sst;

				if (ssl <= 0)
				{
                    this.controlText = this.controlText.Remove(this.editingPosition.X + 1, 1);
				}
				else
				{
                    this.controlText = this.controlText.Remove(sst, ssl);
					this.editingPosition.X = sst - 1;
                    this.selectEnd.X = this.selectStart.X = 0;
				}
			}
				
			ClearSelection();
        }

		protected void ClearSelection()
        {
			this.selectStart.SetCoords(-1, -1);
            this.selectEnd.SetCoords(-1, -1);
            this.selecting = false;
        }

		protected bool HasSelection()
        {
            return ((this.selectStart.X != this.selectEnd.X) || (this.selectStart.Y != this.selectEnd.Y));
        }

		protected void DeleteKeyPressed()
        {
			if (true == HasSelection())
			{
				DeleteSelection();
				return;
			}

			if (true == this.multiLine)
			{
				FixCursor();

				if (this.editingPosition.X == (int)(textLines[this.editingPosition.Y].Length) - 1)
				{
					if (this.editingPosition.Y < (int)(this.textLines.Count) - 1)
					{
						List<String>.Enumerator it = this.textLines.GetEnumerator();
						for (int i = 0; i < this.editingPosition.Y + 1; i++, it.MoveNext());
                        String s = it.Current;
                        this.textLines[this.editingPosition.Y] = this.textLines[this.editingPosition.Y] + s;
                        this.textLines.Remove(s);
					}
				}
				else
				{
                    this.textLines[this.editingPosition.Y] = this.textLines[this.editingPosition.Y].Remove(this.editingPosition.X + 1, 1);
				}
			}
			else
			{
				FixCursor();
                this.controlText = this.controlText.Remove(this.editingPosition.X + 1, 1);
			}
        }

        protected void CharKeyPressed(char c)
        {
            if (c > 0)
            {
                if (true == this.multiLine)
                {
                    DeleteSelection();
                    FixCursor();

                    this.editingPosition.X++;
                    String entryText = "" + c;
                    this.textLines[this.editingPosition.Y] = this.textLines[this.editingPosition.Y].Insert(this.editingPosition.X, entryText);
                }
                else
                {
                    DeleteSelection();
                    FixCursor();
                    this.controlText = this.controlText.Insert(this.editingPosition.X + 1, "" + c);
                    this.editingPosition.X++;
                }
            }
        }

        protected void FixCursor()
        {
			if (true == this.multiLine)
			{
				if (this.editingPosition.Y < 0)
				{
					this.editingPosition.Y = 0;
				}

				if (0 == this.textLines.Count)
				{
					this.textLines.Add("");
				}

				if (this.editingPosition.Y >= this.textLines.Count)
				{
					this.editingPosition.Y = (this.textLines.Count) - 1;
				}

				if (this.editingPosition.X < -1)
				{
					this.editingPosition.X = -1;
				}

				if (this.editingPosition.X > (this.textLines[this.editingPosition.Y].Length) - 1)
				{
					this.editingPosition.X = (this.textLines[this.editingPosition.Y].Length) - 1;
				}
			}
			else
			{
				if (this.editingPosition.X < -1)
				{
					this.editingPosition.X = -1;
				}

				if (this.editingPosition.X > (this.controlText.Length) - 1)
				{
					this.editingPosition.X = (this.controlText.Length) - 1;
				}
			}
        }

        protected void EnterPressed()
        {
			if (true == this.multiLine)
			{
				DeleteSelection();
				FixCursor();

				String strTxt = this.textLines[this.editingPosition.Y].Substring(this.editingPosition.X+1);

                if (this.editingPosition.X + 1 < this.textLines[this.editingPosition.Y].Length)
                {
                    this.textLines[this.editingPosition.Y] = this.textLines[this.editingPosition.Y].Remove(this.editingPosition.X + 1, 1);
                }

                this.editingPosition.Y++;
                this.textLines.Insert(this.editingPosition.Y, strTxt);
				this.editingPosition.X = -1;
			}

            RaiseTextChangedEvent();
        }

        protected void BackspacePressed()
        {
			if (true == HasSelection())
			{
				DeleteSelection();
				FixCursor();
				return;
			}

			if (true == this.multiLine)
			{
                FixCursor();

				if (this.editingPosition.X == -1)
				{
					if (this.editingPosition.Y != 0)
					{
						//std::vector<std::wstring>::iterator it = this.textLines.begin();
						//for (int i = 0; i < this.editingPosition.Y; i++, it++);
                        String s = this.textLines[this.editingPosition.Y];
						this.textLines.RemoveAt(this.editingPosition.Y);
						this.editingPosition.Y--;

						if (this.textLines[this.editingPosition.Y].Length > 0)
						{
							this.editingPosition.X = this.textLines[this.editingPosition.Y].Length - 1;
						}
						else
						{
							this.editingPosition.X = -1;
						}

						this.textLines[this.editingPosition.Y] += s;
					}
				}
				else
				{
                    this.textLines[this.editingPosition.Y] = this.textLines[this.editingPosition.Y].Remove(this.editingPosition.X, 1);
					this.editingPosition.X--;
				}
			}
			else
			{
				FixCursor();

				if (this.controlText.Length > 0)
				{
					if (this.editingPosition.X >= 0)
					{
						this.controlText = this.controlText.Remove(this.editingPosition.X, 1);
						this.editingPosition.X--;
					}
					else
					{
						this.editingPosition.X = -1;
					}
				}
			}
        }

        protected void LeftPressed()
        {
            if (true == this.multiLine)
            {
                ClearSelection();
                this.editingPosition.X--;
                FixCursor();
            }
            else
            {
                ClearSelection();
                this.editingPosition.X--;
            }
        }

        protected void RightPressed()
        {
            if (true == this.multiLine)
            {
                ClearSelection();
                FixCursor();

                this.editingPosition.X++;

                if (this.editingPosition.X > (this.textLines[this.editingPosition.Y].Length) - 1)
                {
                    if (this.editingPosition.Y + 1 < (this.textLines.Count))
                    {
                        this.editingPosition.Y++;
                        this.editingPosition.X = -1;
                    }
                    else
                    {
                        this.editingPosition.X--;// = static_cast<int>(this.textLines[this.editingPosition.Y].length()) - 1;
                    }
                }
            }
            else
            {
                ClearSelection();

                this.editingPosition.X++;
            }
        }

        protected void UpPressed()
        {
            if (true == this.multiLine)
            {
                ClearSelection();
                FixCursor();

                if (this.editingPosition.Y > 0)
                {
                    this.editingPosition.Y--;

                    if ((this.textLines[this.editingPosition.Y].Length) - 1 < this.editingPosition.X)
                    {
                        this.editingPosition.X = (this.textLines[this.editingPosition.Y].Length) - 1;
                    }
                }
                //else
                //{
                //	this.editingPosition.SetCoords(-1, 0);
                //}
            }
            else
            {
            }
        }

        protected void DownPressed()
        {
            if (true == this.multiLine)
            {
                ClearSelection();
                FixCursor();

                if (this.editingPosition.Y < (this.textLines.Count) - 1)
                {
                    this.editingPosition.Y++;

                    if ((this.textLines[this.editingPosition.Y].Length) - 1 < this.editingPosition.X)
                    {
                        this.editingPosition.X = (this.textLines[this.editingPosition.Y].Length) - 1;
                    }
                }
                //else
                //{
                //	this.editingPosition.Y = static_cast<int>(this.textLines.size()) - 1;
                //	this.editingPosition.X = static_cast<int>(this.textLines[this.editingPosition.Y].length()) - 1;
                //}
            }
            else
            {
            }
        }

        protected void HomePressed()
        {
            if (true == this.multiLine)
            {
                ClearSelection();
                FixCursor();
                this.editingPosition.X = -1;
            }
            else
            {
                ClearSelection();
                this.editingPosition.X = -1;
            }
        }

        protected void EndPressed()
        {
            if (true == this.multiLine)
            {
                ClearSelection();
                FixCursor();
                this.editingPosition.X = (this.textLines[this.editingPosition.Y].Length) - 1;
            }
            else
            {
                ClearSelection();
                this.editingPosition.X = (this.controlText.Length) - 1;
            }
        }

        protected void PageUpPressed()
        {
            if (true == this.multiLine)
            {
                ClearSelection();
            }
            else
            {
                HomePressed();
            }
        }

        protected void PageDownPressed()
        {
            if (true == this.multiLine)
            {
                ClearSelection();
            }
            else
            {
                EndPressed();
            }
        }

        public bool AutoShrink
        {
            get
            {
                return this.autoShrink;
            }
            set
            {
                this.autoShrink = value;
            }
        }

        public override void OnFocus()
        {
            if (null != this.textEntry)
            {
                this.textEntry.StartTextEntry(this);
            }

            base.OnFocus();
        }

        public ITextEntry TextEntry
        {
            set
            {
                this.textEntry = value;
            }
        }

        /// <summary>
        /// The message, that is displayed than no text is entered
        /// </summary>
        public String HintText
        {
            get
            {
                return this.hintText.ReferenceText;
            }
            set
            {
                this.hintText.ReferenceText = value;
            }
        }

        /// <summary>
        /// Hint texts color
        /// </summary>
        public Color HintTextColor
        {
            set
            {
                this.hintTextColor = value;
            }
            get
            {
                return this.hintTextColor;
            }
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "textBox";
            }
        }

        protected void RaiseTextChangedEvent()
        {
            if (null != this.TextChanged)
            {
                this.TextChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Event notiefies that entered text has been changed.
        /// </summary>
        public event UIEventHandler<TextBox> TextChanged = null;

        private ITextEntry textEntry = null;
        private MultiLanguageString hintText = new MultiLanguageString();
        private Color hintTextColor = Colors.White;
		private List<String> textLines = new List<String>();
        private bool autoSize = false;
        private bool multiLine = false;
        private bool selecting = false;
        private bool autoShrink = false;
		private Point2D selectStart = new Point2D();
		private Point2D selectEnd = new Point2D();
		private Point2D editingPosition = new Point2D(-1, 0);
        private long drawTime = 0;
        private long lastDraw = 0;
    }
}
