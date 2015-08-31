using System;

namespace ThW.UI.Utils
{
	/// <summary>
	/// 2D vertex. Holds X, Y coordinates.
	/// </summary>
	public class Point2D
	{
		/// <summary>
		/// Constructs vertex object.
		/// </summary>
		/// <param name="X">X coordiante.</param>
		/// <param name="Y">Y coordinate</param>
		public Point2D(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Constructs vertex objext with (0, 0) coordiates.
		/// </summary>
		public Point2D()
		{
		}

		public void SetCoords(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public override bool Equals(object obj)
		{
			if (obj is Point2D)
			{
				Point2D r = (Point2D)obj;

				return ((this.X == r.X) && (this.Y == r.Y));
			}

			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return this.Y ^ this.Y;
		}

		public static bool operator ==(Point2D l, Point2D r)
		{
			return ((l.X == r.X) && (l.Y == r.Y));
		}

		public static bool operator !=(Point2D l, Point2D r)
		{
			return ((l.X != r.X) || (l.Y != r.Y));
		}

		public override string ToString()
		{
			return "(" + this.X + ", " + this.Y + ")";
		}

		/// <summary>
		/// X position.
		/// </summary>
		public int X
		{
			set;
			get;
		}

		/// <summary>
		/// Y position.
		/// </summary>
		public int Y
		{
			set;
			get;
		}
	}
}
