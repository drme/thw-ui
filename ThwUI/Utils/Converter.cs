using System;
using System.Collections.Generic;
using ThW.UI.Controls;
using ThW.UI.Windows;

namespace ThW.UI.Utils
{
	public class Converter
	{
        public static ContentAlignment Convert(String value, ContentAlignment defaultValue)
        {
			if (value == "BottomCenter")
			{
                return ContentAlignment.BottomCenter;
			}
			else if (value == "BottomLeft")
			{
                return ContentAlignment.BottomLeft;
			}
			else if (value == "BottomRight")
			{
                return ContentAlignment.BottomRight;
			}
			else if (value == "MiddleCenter")
			{
                return ContentAlignment.MiddleCenter;
			}
			else if (value == "MiddleLeft")
			{
                return ContentAlignment.MiddleLeft;
			}
			else if (value == "MiddleRight")
			{
                return ContentAlignment.MiddleRight;
			}
			else if (value == "TopCenter")
			{
                return ContentAlignment.TopCenter;
			}
			else if (value == "TopLeft")
			{
                return ContentAlignment.TopLeft;
			}
			else if (value == "TopRight")
			{
                return ContentAlignment.TopRight;
			}
			else
			{
				return defaultValue;
			}
        }

        public static BorderStyle Convert(String value, BorderStyle defaultValue)
        {
            if (value == "None")
            {
                return BorderStyle.None;
            }
            else if (value == "Flat")
            {
                return BorderStyle.Flat;
            }
            else if (value == "Lowered")
            {
                return BorderStyle.Lowered;
            }
            else if (value == "Raised")
            {
                return BorderStyle.BorderRaised;
            }
            else if (value == "FlatDouble")
            {
                return BorderStyle.BorderFlatDouble;
            }
            else if (value == "LoweredDouble")
            {
                return BorderStyle.BorderLoweredDouble;
            }
            else if (value == "RaisedDouble")
            {
                return BorderStyle.BorderRaisedDouble;
            }
            else
            {
                return defaultValue;
            }
        }

        public static ImageLayout Convert(String value, ImageLayout defaultValue)
        {
            if (value == "ImageLayoutNone")
            {
                return ImageLayout.ImageLayoutNone;
            }
            else if (value == "ImageLayoutTile")
            {
                return ImageLayout.ImageLayoutTile;
            }
            else if (value == "ImageLayoutCenter")
            {
                return ImageLayout.ImageLayoutCenter;
            }
            else if (value == "ImageLayoutStretch")
            {
                return ImageLayout.ImageLayoutStretch;
            }
            else if (value == "ImageLayoutZoom")
            {
                return ImageLayout.ImageLayoutZoom;
            }
            else if (value == "ImageLayoutFillWidth")
            {
                return ImageLayout.ImageLayoutFillWidth;
            }
            else
            {
                return defaultValue;
            }
        }

        public static IconSize Convert(String value, IconSize defaultValue)
        {
            if (value == "IconSmall")
            {
                return IconSize.IconSmall;
            }
            else if (value == "IconLarge")
            {
                return IconSize.IconLarge;
            }
            else
            {
                return defaultValue;
            }
        }

        public static MousePointers Convert(String value, MousePointers defaultValue)
        {
            if (value == "PointerStandard")
            {
                return MousePointers.PointerStandard;
            }
            else if (value == "PointerWait")
            {
                return MousePointers.PointerWait;
            }
            else if (value == "PointerMove")
            {
                return MousePointers.PointerMove;
            }
            else if (value == "PointerHResize")
            {
                return MousePointers.PointerHResize;
            }
            else if (value == "PointerVResize")
            {
                return MousePointers.PointerVResize;
            }
            else if (value == "PointerResize1")
            {
                return MousePointers.PointerResize1;
            }
            else if (value == "PointerResize2")
            {
                return MousePointers.PointerResize2;
            }
            else if (value == "PointerText")
            {
                return MousePointers.PointerText;
            }
            else if (value == "PointerHand")
            {
                return MousePointers.PointerHand;
            }
            else
            {
                return defaultValue;
            }
        }

        public static DialogResult Convert(String value, DialogResult defaultValue)
        {
            if (value == "none")
            {
                return DialogResult.DialogResultNone;
            }
            else if (value == "cancel")
            {
                return DialogResult.DialogResultCancel;
            }
            else if (value == "ok")
            {
                return DialogResult.DialogResultOK;
            }
            else
            {
                return defaultValue;
            }
        }

        public static ObjectType Convert(String value, ObjectType defaultValue)
        {
            if (value == "ObjectControl")
            {
                return ObjectType.ObjectControl;
            }
            else if (value == "ObjectReserved")
            {
                return ObjectType.ObjectReserved;
            }
            else if (value == "ObjectUndefined")
            {
                return ObjectType.ObjectUndefined;
            }
            else
            {
                return defaultValue;
            }
        }

        public static String Convert(ContentAlignment value)
        {
            switch (value)
			{
				case ContentAlignment.BottomCenter:
					return "BottomCenter";
				case ContentAlignment.BottomLeft:
					return "BottomLeft";
				case ContentAlignment.BottomRight:
					return "BottomRight";
				case ContentAlignment.MiddleCenter:
					return "MiddleCenter";
				case ContentAlignment.MiddleLeft:
					return "MiddleLeft";
				case ContentAlignment.MiddleRight:
					return "MiddleRight";
				case ContentAlignment.TopCenter:
					return "TopCenter";
				case ContentAlignment.TopLeft:
					return "TopLeft";
				case ContentAlignment.TopRight:
					return "TopRight";
				default:
					return "";
			}
        }

        public static String Convert(BorderStyle value)
        {
			switch (value)
			{
				case BorderStyle.None:
					return "None";
				case BorderStyle.Flat:
					return "Flat";
				case BorderStyle.Lowered:
					return "Lowered";
				case BorderStyle.BorderRaised:
					return "Raised";
				case BorderStyle.BorderFlatDouble:
					return "FlatDouble";
				case BorderStyle.BorderLoweredDouble:
					return "LoweredDouble";
				case BorderStyle.BorderRaisedDouble:
					return "RaisedDouble";
				default:
					return "";
			}
        }

        public static String Convert(ImageLayout value)
        {
			switch (value)
			{
				case ImageLayout.ImageLayoutNone:
					return "ImageLayoutNone";
				case ImageLayout.ImageLayoutTile:
					return "ImageLayoutTile";
				case ImageLayout.ImageLayoutCenter:
					return "ImageLayoutCenter";
				case ImageLayout.ImageLayoutStretch:
					return "ImageLayoutStretch";
				case ImageLayout.ImageLayoutZoom:
					return "ImageLayoutZoom";
                case ImageLayout.ImageLayoutFillWidth:
                    return "ImageLayoutFillWidth";
				default:
					return "";
			}
        }

        public static String Convert(IconSize value)
        {
			switch (value)
			{
				case IconSize.IconSmall:
					return "IconSmall";
				case IconSize.IconLarge:
					return "IconLarge";
				default:
					return "";
			}
        }

        public static String Convert(MousePointers value)
        {
            switch (value)
            {
                case MousePointers.PointerStandard:
                    return "PointerStandard";
                case MousePointers.PointerWait:
                    return "PointerWait";
                case MousePointers.PointerMove:
                    return "PointerMove";
                case MousePointers.PointerHResize:
                    return "PointerHResize";
                case MousePointers.PointerVResize:
                    return "PointerVResize";
                case MousePointers.PointerResize1:
                    return "PointerResize1";
                case MousePointers.PointerResize2:
                    return "PointerResize2";
                case MousePointers.PointerText:
                    return "PointerText";
                case MousePointers.PointerHand:
                    return "PointerHand";
                default:
                    return "";
            }
        }

        public static String Convert(DialogResult value)
        {
			switch (value)
			{
				case DialogResult.DialogResultNone:
					return "none";
				case DialogResult.DialogResultCancel:
					return "cancel";
				case DialogResult.DialogResultOK:
					return "ok";
				default:
					return "";
			}
        }

        public static String Convert(ObjectType value)
        {
            switch (value)
            {
                case ObjectType.ObjectControl:
                    return "ObjectControl";
                case ObjectType.ObjectReserved:
                    return "ObjectReserved";
                case ObjectType.ObjectUndefined:
                    return "ObjectUndefined";
                default:
                    return "";
            }
        }

		public static List<String> GetValues(BorderStyle value)
        {
			List<String> lst = new List<String>();

			lst.Add("None");
			lst.Add("Flat");
			lst.Add("Lowered");
			lst.Add("Raised");
			lst.Add("FlatDouble");
			lst.Add("LoweredDouble");
			lst.Add("RaisedDouble");

			return lst;
        }

		public static List<String> GetValues(ContentAlignment value)
        {
			List<String> lst = new List<String>();

			lst.Add("BottomCenter");
			lst.Add("BottomLeft");
			lst.Add("BottomRight");
			lst.Add("MiddleCenter");
			lst.Add("MiddleLeft");
			lst.Add("MiddleRight");
			lst.Add("TopCenter");
			lst.Add("TopLeft");
			lst.Add("TopRight");

			return lst;
        }

		public static List<String> GetValues(IconSize value)
        {
			List<String> lst = new List<String>();

			lst.Add("IconSmall");
			lst.Add("IconLarge");

			return lst;
        }

		public static List<String> GetValues(MousePointers value)
        {
            List<String> lst = new List<String>();

			lst.Add("PointerStandard");
			lst.Add("PointerWait");
			lst.Add("PointerMove");
			lst.Add("PointerHResize");
			lst.Add("PointerVResize");
			lst.Add("PointerResize1");
			lst.Add("PointerResize2");
			lst.Add("PointerText");
			lst.Add("PointerHand");

			return lst;
        }

		public static List<String> GetValues(ImageLayout value)
        {
			List<String> lst = new List<String>();

            lst.Add("ImageLayoutNone");
			lst.Add("ImageLayoutTile");
			lst.Add("ImageLayoutCenter");
			lst.Add("ImageLayoutStretch");
			lst.Add("ImageLayoutZoom");

			return lst;
        }

        public static List<String> GetValues(DialogResult value)
        {
			List<String> lst = new List<String>();

			lst.Add("none");
			lst.Add("cancel");
			lst.Add("ok");

			return lst;
        }

        public static List<String> GetValues(Object obj)
        {
            if (obj is DialogResult)
            {
                return GetValues(DialogResult.DialogResultCancel);
            }
            else if (obj is BorderStyle)
            {
                return GetValues(BorderStyle.Flat);
            }
            else if (obj is ContentAlignment)
            {
                return GetValues(ContentAlignment.BottomCenter);
            }
            else if (obj is ImageLayout)
            {
                return GetValues(ImageLayout.ImageLayoutCenter);
            }
            else if (obj is IconSize)
            {
                return GetValues(IconSize.IconLarge);
            }
            else if (obj is MousePointers)
            {
                return GetValues(MousePointers.PointerHand);
            }
            else
            {
                return null;
            }
        }

        public static String Convert(Object obj)
        {
            if (obj is DialogResult)
            {
                return Convert((DialogResult)obj);
            }
            else if (obj is BorderStyle)
            {
                return Convert((BorderStyle)obj);
            }
            else if (obj is ContentAlignment)
            {
                return Convert((ContentAlignment)obj);
            }
            else if (obj is ImageLayout)
            {
                return Convert((ImageLayout)obj);
            }
            else if (obj is IconSize)
            {
                return Convert((IconSize)obj);
            }
            else if (obj is MousePointers)
            {
                return Convert((MousePointers)obj);
            }
            else if (obj is ObjectType)
            {
                return Convert((ObjectType)obj);
            }
            else
            {
                return null;
            }
        }

        public static Object Convert(String value, Object obj)
        {
            if (obj is DialogResult)
            {
                return Convert(value, (DialogResult)obj);
            }
            else if (obj is BorderStyle)
            {
                return Convert(value, (BorderStyle)obj);
            }
            else if (obj is ContentAlignment)
            {
                return Convert(value, (ContentAlignment)obj);
            }
            else if (obj is ImageLayout)
            {
                return Convert(value, (ImageLayout)obj);
            }
            else if (obj is IconSize)
            {
                return Convert(value, (IconSize)obj);
            }
            else if (obj is MousePointers)
            {
                return Convert(value, (MousePointers)obj);
            }
            else if (obj is ObjectType)
            {
                return Convert(value, (ObjectType)obj);
            }
            else
            {
                return null;
            }
        }
    }
}
