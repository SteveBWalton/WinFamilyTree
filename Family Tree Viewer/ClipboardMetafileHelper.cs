using System;
using System.Drawing;							// Graphics
using System.Drawing.Imaging;					// Metafiles
using System.Runtime.InteropServices;			// [DllImport()]

namespace FamilyTree.Viewer
{
    /// <summary>
    /// Class to convert a .NET metafile into Win32 metafile that Word, Excel etc can use
    /// 
    /// This was taken from
    /// 
    /// http://www.dotnet247.com/247reference/msgs/23/118514.aspx
    /// 
    /// I'm currently writing a KB article to cover this. The problem is that the framework
    /// uses a new clipboard format for its metafiles - one that other apps, and
    /// even the OS,
    /// don't know about and therefore cannot translate into EMF.
    /// The workaround is to interoperate with Win32 clipboard APIs, per the following
    /// </summary>
    public class ClipboardMetafileHelper
    {
        [DllImport("user32.dll")]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);
        [DllImport("user32.dll")]
        static extern bool EmptyClipboard();
        [DllImport("user32.dll")]
        static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
        [DllImport("user32.dll")]
        static extern bool CloseClipboard();
        [DllImport("gdi32.dll")]
        static extern IntPtr CopyEnhMetaFile(IntPtr hemfSrc, IntPtr hNULL);
        [DllImport("gdi32.dll")]
        static extern bool DeleteEnhMetaFile(IntPtr hemf);

        /// <summary>Class constructor.</summary>
        public ClipboardMetafileHelper()
        {
        }

        /// <summary>Metafile mf is set to an invalid state inside this function.</summary>
        /// <param name="hWnd"></param>
        /// <param name="metafile"></param>
        /// <returns></returns>
        static public bool putEnhMetafileOnClipboard(IntPtr hWnd, Metafile metafile)
        {
            bool isResult = false;
            IntPtr hEMF, hEMF2;
            hEMF = metafile.GetHenhmetafile(); // invalidates mf
            if (!hEMF.Equals(new IntPtr(0)))
            {
                hEMF2 = CopyEnhMetaFile(hEMF, new IntPtr(0));
                if (!hEMF2.Equals(new IntPtr(0)))
                {
                    if (OpenClipboard(hWnd))
                    {
                        if (EmptyClipboard())
                        {
                            IntPtr hRes = SetClipboardData(14, hEMF2);  //  14 is CF_ENHMETAFILE
                            isResult = hRes.Equals(hEMF2);
                            CloseClipboard();
                        }
                    }
                }
                DeleteEnhMetaFile(hEMF);
            }
            return isResult;
        }
    }

    /*
	//You might call the above function with code like:
	Metafile mf = new Metafile( "filename.emf" );
	ClipboardMetafileHelper.PutEnhMetafileOnClipboard(this.Handle, mf );
	*/

}
