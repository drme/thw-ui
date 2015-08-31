using System.Collections.Generic;

namespace ThW.UI.Utils.FilesSystem
{
    internal class FilesSort : IComparer<IVirtualFile>
    {
        public int Compare(IVirtualFile leftFile, IVirtualFile rightFile)
        {
            if ((leftFile == null) && (rightFile == null))
            {
                return 0;
            }

            if ((leftFile == null) && (rightFile != null))
            {
                return -1;
            }

            if ((leftFile != null) && (rightFile == null))
            {
                return 1;
            }

            int leftType = (int)leftFile.Type;
            int rightType = (int)rightFile.Type;

            if (leftType != rightType)
            {
                return leftType - rightType;
            }

            if ((null == leftFile.Name) && (null == rightFile.Name))
            {
                return 0;
            }

            if ((leftFile.Name == null) && (rightFile.Name != null))
            {
                return -1;
            }

            if ((leftFile.Name != null) && (rightFile.Name == null))
            {
                return 1;
            }


            return leftFile.Name.CompareTo(rightFile.Name);
        }
    }
}
