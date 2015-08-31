using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Library utilities.
    /// </summary>
	internal class UIUtils
	{
        /// <summary>
        /// Converts string to integer, if not successfull returs default value.
        /// </summary>
        /// <param name="text">string vlaue</param>
        /// <param name="defaultValue">default value, to return on conversion failure.</param>
        /// <returns>converter integer.</returns>
        public static int FromString(String text, int defaultValue)
        {
            if ((null == text) || (text.Length == 0))
            {
                return defaultValue;
            }

            try
            {
                return int.Parse(text);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Converts string to boolean, if not successfull returs default value.
        /// </summary>
        /// <param name="text">string vlaue</param>
        /// <param name="defaultValue">default value, to return on conversion failure.</param>
        /// <returns>converter integer.</returns>
		public static bool FromString(String text, bool defaultValue)
        {
			if (text.Length > 0)
            {
                if ( ('t' == text[0]) || ('T' == text[0]) || ('1' == text[0]) )
                {
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return defaultValue;
			}
        }

        /// <summary>
        /// Compates two strngs ignoring case.
        /// </summary>
        /// <param name="leftValue">left string.</param>
        /// <param name="rightValue">right string.</param>
        /// <returns>are those strngs equal.</returns>
		public static bool EqualsIgnoringCase(String leftValue, String rightValue)
        {
            return (0 == String.Compare(leftValue, rightValue, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Converts fileName string to windows file name.
        /// (replaces / with \)
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>replaced file name</returns>
        public static String ConvertToWindows(String fileName)
        {
            if (true == fileName.StartsWith("\\\\"))
            {
                return "\\\\" + ConvertToWindows(fileName.Substring(2));
            }
            else
            {
                String result = fileName.Replace('/', '\\').Replace(@"\\", @"\");

                if (true == result.Contains("\\\\"))
                {
                    return ConvertToWindows(result);
                }
                else
                {
                    return result;
                }
            }
        }
	}
}
