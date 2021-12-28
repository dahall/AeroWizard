using System;
using System.Runtime.InteropServices;

namespace Vanara.Interop
{
	internal static partial class NativeMethods
	{
		/// <summary>The type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed). Used in <see cref="BITMAPINFOHEADER"/>.</summary>
		public enum BitmapCompressionMode : uint
		{
			/// <summary>An uncompressed format.</summary>
			BI_RGB = 0,

			/// <summary>
			/// A run-length encoded (RLE) format for bitmaps with 8 bpp. The compression format is a 2-byte format consisting of a count
			/// byte followed by a byte containing a color index.
			/// </summary>
			BI_RLE8 = 1,

			/// <summary>
			/// An RLE format for bitmaps with 4 bpp. The compression format is a 2-byte format consisting of a count byte followed by two
			/// word-length color indexes.
			/// </summary>
			BI_RLE4 = 2,

			/// <summary>
			/// Specifies that the bitmap is not compressed and that the color table consists of three DWORD color masks that specify the
			/// red, green, and blue components, respectively, of each pixel. This is valid when used with 16- and 32-bpp bitmaps.
			/// </summary>
			BI_BITFIELDS = 3,

			/// <summary>Indicates that the image is a JPEG image.</summary>
			BI_JPEG = 4,

			/// <summary>Indicates that the image is a PNG image.</summary>
			BI_PNG = 5
		}

		public enum DIBColorMode : int
		{
			/// <summary>The BITMAPINFO structure contains an array of literal RGB values.</summary>
			DIB_RGB_COLORS = 0,

			/// <summary>
			/// The bmiColors member of the BITMAPINFO structure is an array of 16-bit indexes into the logical palette of the device context
			/// specified by hdc.
			/// </summary>
			DIB_PAL_COLORS = 1
		}

		/// <summary>
		/// The CreateDIBSection function creates a DIB that applications can write to directly. The function gives you a pointer to the
		/// location of the bitmap bit values. You can supply a handle to a file-mapping object that the function will use to create the
		/// bitmap, or you can let the system allocate the memory for the bitmap.
		/// </summary>
		/// <param name="hdc">
		/// A handle to a device context. If the value of iUsage is DIB_PAL_COLORS, the function uses this device context's logical palette
		/// to initialize the DIB colors.
		/// </param>
		/// <param name="pbmi">
		/// A pointer to a BITMAPINFO structure that specifies various attributes of the DIB, including the bitmap dimensions and colors.
		/// </param>
		/// <param name="iUsage">
		/// The type of data contained in the bmiColors array member of the BITMAPINFO structure pointed to by pbmi (either logical palette
		/// indexes or literal RGB values).
		/// </param>
		/// <param name="ppvBits">A pointer to a variable that receives a pointer to the location of the DIB bit values.</param>
		/// <param name="hSection">
		/// A handle to a file-mapping object that the function will use to create the DIB. This parameter can be NULL.
		/// <para>
		/// If hSection is not NULL, it must be a handle to a file-mapping object created by calling the CreateFileMapping function with the
		/// PAGE_READWRITE or PAGE_WRITECOPY flag. Read-only DIB sections are not supported. Handles created by other means will cause
		/// CreateDIBSection to fail.
		/// </para>
		/// <para>
		/// If hSection is not NULL, the CreateDIBSection function locates the bitmap bit values at offset dwOffset in the file-mapping
		/// object referred to by hSection. An application can later retrieve the hSection handle by calling the GetObject function with the
		/// HBITMAP returned by CreateDIBSection.
		/// </para>
		/// <para>
		/// If hSection is NULL, the system allocates memory for the DIB. In this case, the CreateDIBSection function ignores the dwOffset
		/// parameter. An application cannot later obtain a handle to this memory. The dshSection member of the DIBSECTION structure filled
		/// in by calling the GetObject function will be NULL.
		/// </para>
		/// </param>
		/// <param name="dwOffset">
		/// The offset from the beginning of the file-mapping object referenced by hSection where storage for the bitmap bit values is to
		/// begin. This value is ignored if hSection is NULL. The bitmap bit values are aligned on doubleword boundaries, so dwOffset must be
		/// a multiple of the size of a DWORD.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is a handle to the newly created DIB, and *ppvBits points to the bitmap bit values.
		/// <para>If the function fails, the return value is NULL, and *ppvBits is NULL.</para>
		/// </returns>
		/// <remarks>
		/// As noted above, if hSection is NULL, the system allocates memory for the DIB. The system closes the handle to that memory when
		/// you later delete the DIB by calling the DeleteObject function. If hSection is not NULL, you must close the hSection memory handle
		/// yourself after calling DeleteObject to delete the bitmap.
		/// <para>You cannot paste a DIB section from one application into another application.</para>
		/// <para>
		/// CreateDIBSection does not use the BITMAPINFOHEADER parameters biXPelsPerMeter or biYPelsPerMeter and will not provide resolution
		/// information in the BITMAPINFO structure.
		/// </para>
		/// <para>
		/// You need to guarantee that the GDI subsystem has completed any drawing to a bitmap created by CreateDIBSection before you draw to
		/// the bitmap yourself. Access to the bitmap must be synchronized. Do this by calling the GdiFlush function. This applies to any use
		/// of the pointer to the bitmap bit values, including passing the pointer in calls to functions such as SetDIBits.
		/// </para>
		/// <para>ICM: No color management is done.</para>
		/// </remarks>
		[DllImport(GDI32, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateDIBSection(SafeDCHandle hdc, in BITMAPINFO pbmi, DIBColorMode iUsage, out IntPtr ppvBits, [In, Optional] IntPtr hSection, [In, Optional] int dwOffset);

		/// <summary>
		/// The GetDIBits function retrieves the bits of the specified compatible bitmap and copies them into a buffer as a DIB using the
		/// specified format.
		/// </summary>
		/// <param name="hdc">A handle to the device context.</param>
		/// <param name="hbmp">A handle to the bitmap. This must be a compatible bitmap (DDB).</param>
		/// <param name="uStartScan">The first scan line to retrieve.</param>
		/// <param name="cScanLines">The number of scan lines to retrieve.</param>
		/// <param name="lpvBits">
		/// A pointer to a buffer to receive the bitmap data. If this parameter is NULL, the function passes the dimensions and format of the
		/// bitmap to the BITMAPINFO structure pointed to by the lpbi parameter.
		/// </param>
		/// <param name="lpbi">A pointer to a BITMAPINFO structure that specifies the desired format for the DIB data.</param>
		/// <param name="uUsage">The format of the bmiColors member of the BITMAPINFO structure.</param>
		/// <returns>
		/// If the lpvBits parameter is non-NULL and the function succeeds, the return value is the number of scan lines copied from the bitmap.
		/// <para>If the lpvBits parameter is NULL and GetDIBits successfully fills the BITMAPINFO structure, the return value is nonzero.</para>
		/// <para>If the function fails, the return value is zero.</para>
		/// </returns>
		/// <remarks>
		/// If the requested format for the DIB matches its internal format, the RGB values for the bitmap are copied. If the requested
		/// format doesn't match the internal format, a color table is synthesized. The following table describes the color table synthesized
		/// for each format.
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Meaning</description>
		/// </listheader>
		/// <item>
		/// <term>1_BPP</term>
		/// <description>The color table consists of a black and a white entry.</description>
		/// </item>
		/// <item>
		/// <term>4_BPP</term>
		/// <description>The color table consists of a mix of colors identical to the standard VGA palette.</description>
		/// </item>
		/// <item>
		/// <term>8_BPP</term>
		/// <description>
		/// The color table consists of a general mix of 256 colors defined by GDI. (Included in these 256 colors are the 20 colors found in
		/// the default logical palette.)
		/// </description>
		/// </item>
		/// <item>
		/// <term>24_BPP</term>
		/// <description>No color table is returned.</description>
		/// </item>
		/// </list>
		/// <para>
		/// If the lpvBits parameter is a valid pointer, the first six members of the BITMAPINFOHEADER structure must be initialized to
		/// specify the size and format of the DIB. The scan lines must be aligned on a DWORD except for RLE compressed bitmaps.
		/// </para>
		/// <para>
		/// A bottom-up DIB is specified by setting the height to a positive number, while a top-down DIB is specified by setting the height
		/// to a negative number. The bitmap color table will be appended to the BITMAPINFO structure.
		/// </para>
		/// <para>
		/// If lpvBits is NULL, GetDIBits examines the first member of the first structure pointed to by lpbi. This member must specify the
		/// size, in bytes, of a BITMAPCOREHEADER or a BITMAPINFOHEADER structure. The function uses the specified size to determine how the
		/// remaining members should be initialized.
		/// </para>
		/// <para>
		/// If lpvBits is NULL and the bit count member of BITMAPINFO is initialized to zero, GetDIBits fills in a BITMAPINFOHEADER structure
		/// or BITMAPCOREHEADER without the color table. This technique can be used to query bitmap attributes.
		/// </para>
		/// <para>
		/// The bitmap identified by the hbmp parameter must not be selected into a device context when the application calls this function.
		/// </para>
		/// <para>
		/// The origin for a bottom-up DIB is the lower-left corner of the bitmap; the origin for a top-down DIB is the upper-left corner.
		/// </para>
		/// </remarks>
		[DllImport(GDI32, ExactSpelling = true, SetLastError = true)]
		public static extern int GetDIBits(SafeDCHandle hdc, IntPtr hbmp, int uStartScan, int cScanLines, [Out, Optional] byte[] lpvBits, ref BITMAPINFO lpbi, DIBColorMode uUsage);

		/// <summary>
		/// The GetDIBits function retrieves the bits of the specified compatible bitmap and copies them into a buffer as a DIB using the
		/// specified format.
		/// </summary>
		/// <param name="hdc">A handle to the device context.</param>
		/// <param name="hbmp">A handle to the bitmap. This must be a compatible bitmap (DDB).</param>
		/// <param name="uStartScan">The first scan line to retrieve.</param>
		/// <param name="cScanLines">The number of scan lines to retrieve.</param>
		/// <param name="lpvBits">
		/// A pointer to a buffer to receive the bitmap data. If this parameter is NULL, the function passes the dimensions and format of the
		/// bitmap to the BITMAPINFO structure pointed to by the lpbi parameter.
		/// </param>
		/// <param name="lpbi">A pointer to a BITMAPINFO structure that specifies the desired format for the DIB data.</param>
		/// <param name="uUsage">The format of the bmiColors member of the BITMAPINFO structure.</param>
		/// <returns>
		/// If the lpvBits parameter is non-NULL and the function succeeds, the return value is the number of scan lines copied from the bitmap.
		/// <para>If the lpvBits parameter is NULL and GetDIBits successfully fills the BITMAPINFO structure, the return value is nonzero.</para>
		/// <para>If the function fails, the return value is zero.</para>
		/// </returns>
		/// <remarks>
		/// If the requested format for the DIB matches its internal format, the RGB values for the bitmap are copied. If the requested
		/// format doesn't match the internal format, a color table is synthesized. The following table describes the color table synthesized
		/// for each format.
		/// <list type="table">
		/// <listheader>
		/// <term>Value</term>
		/// <description>Meaning</description>
		/// </listheader>
		/// <item>
		/// <term>1_BPP</term>
		/// <description>The color table consists of a black and a white entry.</description>
		/// </item>
		/// <item>
		/// <term>4_BPP</term>
		/// <description>The color table consists of a mix of colors identical to the standard VGA palette.</description>
		/// </item>
		/// <item>
		/// <term>8_BPP</term>
		/// <description>
		/// The color table consists of a general mix of 256 colors defined by GDI. (Included in these 256 colors are the 20 colors found in
		/// the default logical palette.)
		/// </description>
		/// </item>
		/// <item>
		/// <term>24_BPP</term>
		/// <description>No color table is returned.</description>
		/// </item>
		/// </list>
		/// <para>
		/// If the lpvBits parameter is a valid pointer, the first six members of the BITMAPINFOHEADER structure must be initialized to
		/// specify the size and format of the DIB. The scan lines must be aligned on a DWORD except for RLE compressed bitmaps.
		/// </para>
		/// <para>
		/// A bottom-up DIB is specified by setting the height to a positive number, while a top-down DIB is specified by setting the height
		/// to a negative number. The bitmap color table will be appended to the BITMAPINFO structure.
		/// </para>
		/// <para>
		/// If lpvBits is NULL, GetDIBits examines the first member of the first structure pointed to by lpbi. This member must specify the
		/// size, in bytes, of a BITMAPCOREHEADER or a BITMAPINFOHEADER structure. The function uses the specified size to determine how the
		/// remaining members should be initialized.
		/// </para>
		/// <para>
		/// If lpvBits is NULL and the bit count member of BITMAPINFO is initialized to zero, GetDIBits fills in a BITMAPINFOHEADER structure
		/// or BITMAPCOREHEADER without the color table. This technique can be used to query bitmap attributes.
		/// </para>
		/// <para>
		/// The bitmap identified by the hbmp parameter must not be selected into a device context when the application calls this function.
		/// </para>
		/// <para>
		/// The origin for a bottom-up DIB is the lower-left corner of the bitmap; the origin for a top-down DIB is the upper-left corner.
		/// </para>
		/// </remarks>
		[DllImport(GDI32, ExactSpelling = true, SetLastError = true)]
		public static extern int GetDIBits(SafeDCHandle hdc, IntPtr hbmp, int uStartScan, int cScanLines, [Out, Optional] IntPtr lpvBits, ref BITMAPINFO lpbi, DIBColorMode uUsage);

		/// <summary>The BITMAPINFO structure defines the dimensions and color information for a DIB.</summary>
		/// <remarks>
		/// A DIB consists of two distinct parts: a BITMAPINFO structure describing the dimensions and colors of the bitmap, and an array of
		/// bytes defining the pixels of the bitmap. The bits in the array are packed together, but each scan line must be padded with zeros
		/// to end on a LONG data-type boundary. If the height of the bitmap is positive, the bitmap is a bottom-up DIB and its origin is
		/// the lower-left corner. If the height is negative, the bitmap is a top-down DIB and its origin is the upper left corner.
		/// <para>
		/// A bitmap is packed when the bitmap array immediately follows the BITMAPINFO header. Packed bitmaps are referenced by a single
		/// pointer. For packed bitmaps, the biClrUsed member must be set to an even number when using the DIB_PAL_COLORS mode so that the
		/// DIB bitmap array starts on a DWORD boundary.
		/// </para>
		/// <para><c>Note</c></para>
		/// <para>
		/// The bmiColors member should not contain palette indexes if the bitmap is to be stored in a file or transferred to another application.
		/// </para>
		/// <para>
		/// Unless the application has exclusive use and control of the bitmap, the bitmap color table should contain explicit RGB values.
		/// </para>
		/// </remarks>
		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAPINFO
		{
			/// <summary>A BITMAPINFOHEADER structure that contains information about the dimensions of color format.</summary>
			public BITMAPINFOHEADER bmiHeader;

			/// <summary>
			/// The bmiColors member contains one of the following:
			/// <list type="bullet">
			/// <item>
			/// <description>An array of RGBQUAD. The elements of the array that make up the color table.</description>
			/// </item>
			/// <item>
			/// <description>
			/// An array of 16-bit unsigned integers that specifies indexes into the currently realized logical palette. This use of
			/// bmiColors is allowed for functions that use DIBs. When bmiColors elements contain indexes to a realized logical palette,
			/// they must also call the following bitmap
			/// functions: CreateDIBitmap, CreateDIBPatternBrush, CreateDIBSection (The iUsage parameter of CreateDIBSection must be set to DIB_PAL_COLORS.)
			/// </description>
			/// </item>
			/// </list>
			/// <para>
			/// The number of entries in the array depends on the values of the biBitCount and biClrUsed members of the BITMAPINFOHEADER structure.
			/// </para>
			/// <para>The colors in the bmiColors table appear in order of importance. For more information, see the Remarks section.</para>
			/// </summary>
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
			public RGBQUAD[] bmiColors;

			/// <summary>Initializes a new instance of the <see cref="BITMAPINFO"/> structure.</summary>
			/// <param name="width">The width.</param>
			/// <param name="height">The height.</param>
			/// <param name="bitCount">The bit count.</param>
			public BITMAPINFO(int width, int height, ushort bitCount = 32)
				: this()
			{
				bmiHeader = new BITMAPINFOHEADER(width, height, bitCount);
			}
		}

		/// <summary>
		/// <para>
		/// The <c>BITMAPINFOHEADER</c> structure contains information about the dimensions and color format of a device-independent bitmap (DIB).
		/// </para>
		/// <para>
		/// <c>Note</c> This structure is also described in the GDI documentation. However, the semantics for video data are slightly
		/// different than the semantics used for GDI. If you are using this structure to describe video data, use the information given here.
		/// </para>
		/// </summary>
		/// <remarks>
		/// <para>Color Tables</para>
		/// <para>
		/// The <c>BITMAPINFOHEADER</c> structure may be followed by an array of palette entries or color masks. The rules depend on the
		/// value of <c>biCompression</c>.
		/// </para>
		/// <list type="bullet">
		/// <item>
		/// <term>
		/// If <c>biCompression</c> equals <c>BI_RGB</c> and the bitmap uses 8 bpp or less, the bitmap has a color table immediatelly
		/// following the <c>BITMAPINFOHEADER</c> structure. The color table consists of an array of <c>RGBQUAD</c> values. The size of the
		/// array is given by the <c>biClrUsed</c> member. If <c>biClrUsed</c> is zero, the array contains the maximum number of colors for
		/// the given bitdepth; that is, 2^ <c>biBitCount</c> colors.
		/// </term>
		/// </item>
		/// <item>
		/// <term>
		/// If <c>biCompression</c> equals <c>BI_BITFIELDS</c>, the bitmap uses three <c>DWORD</c> color masks (red, green, and blue,
		/// respectively), which specify the byte layout of the pixels. The 1 bits in each mask indicate the bits for that color within the pixel.
		/// </term>
		/// </item>
		/// <item>
		/// <term>
		/// If <c>biCompression</c> is a video FOURCC, the presence of a color table is implied by the video format. You should not assume
		/// that a color table exists when the bit depth is 8 bpp or less. However, some legacy components might assume that a color table
		/// is present. Therefore, if you are allocating a <c>BITMAPINFOHEADER</c> structure, it is recommended to allocate space for a
		/// color table when the bit depth is 8 bpp or less, even if the color table is not used.
		/// </term>
		/// </item>
		/// </list>
		/// <para>
		/// When the <c>BITMAPINFOHEADER</c> is followed by a color table or a set of color masks, you can use the BITMAPINFO structure to
		/// reference the color table of the color masks. The <c>BITMAPINFO</c> structure is defined as follows:
		/// </para>
		/// <para>
		/// <code>typedef struct tagBITMAPINFO { BITMAPINFOHEADER bmiHeader; RGBQUAD bmiColors[1]; } BITMAPINFO;</code>
		/// </para>
		/// <para>
		/// If you cast the <c>BITMAPINFOHEADER</c> to a BITMAPINFO, the <c>bmiHeader</c> member refers to the <c>BITMAPINFOHEADER</c> and
		/// the <c>bmiColors</c> member refers to the first entry in the color table, or the first color mask.
		/// </para>
		/// <para>
		/// Be aware that if the bitmap uses a color table or color masks, then the size of the entire format structure (the
		/// <c>BITMAPINFOHEADER</c> plus the color information) is not equal to
		/// <code>sizeof(BITMAPINFOHEADER)</code>
		/// or
		/// <code>sizeof(BITMAPINFO)</code>
		/// . You must calculate the actual size for each instance.
		/// </para>
		/// <para>Calculating Surface Stride</para>
		/// <para>
		/// In an uncompressed bitmap, the stride is the number of bytes needed to go from the start of one row of pixels to the start of
		/// the next row. The image format defines a minimum stride for an image. In addition, the graphics hardware might require a larger
		/// stride for the surface that contains the image.
		/// </para>
		/// <para>
		/// For uncompressed RGB formats, the minimum stride is always the image width in bytes, rounded up to the nearest <c>DWORD</c>. You
		/// can use the following formula to calculate the stride:
		/// </para>
		/// <para>
		/// <code>stride = ((((biWidth * biBitCount) + 31) &amp; ~31) &gt;&gt; 3)</code>
		/// </para>
		/// <para>
		/// For YUV formats, there is no general rule for calculating the minimum stride. You must understand the rules for the particular
		/// YUV format. For a description of the most common YUV formats, see Recommended 8-Bit YUV Formats for Video Rendering.
		/// </para>
		/// <para>
		/// Decoders and video sources should propose formats where biWidth is the width of the image in pixels. If the video renderer
		/// requires a surface stride that is larger than the default image stride, it modifies the proposed media type by setting the
		/// following values:
		/// </para>
		/// <list type="bullet">
		/// <item>
		/// <term>It sets <c>biWidth</c> equal to the surface stride in pixels.</term>
		/// </item>
		/// <item>
		/// <term>It sets the <c>rcTarget</c> member of the VIDEOINFOHEADER or VIDEOINFOHEADER2 structure equal to the image width, in pixels.</term>
		/// </item>
		/// </list>
		/// <para>
		/// Then the video renderer proposes the modified format by calling IPin::QueryAccept on the upstream pin. For more information
		/// about this mechanism, see Dynamic Format Changes.
		/// </para>
		/// <para>
		/// If there is padding in the image buffer, never dereference a pointer into the memory that has been reserved for the padding. If
		/// the image buffer has been allocated in video memory, the padding might not be readable memory.
		/// </para>
		/// </remarks>
		// https://docs.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-bitmapinfoheader typedef struct tagBITMAPINFOHEADER { DWORD
		// biSize; LONG biWidth; LONG biHeight; WORD biPlanes; WORD biBitCount; DWORD biCompression; DWORD biSizeImage; LONG
		// biXPelsPerMeter; LONG biYPelsPerMeter; DWORD biClrUsed; DWORD biClrImportant; } BITMAPINFOHEADER, *LPBITMAPINFOHEADER, *PBITMAPINFOHEADER;
		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		public struct BITMAPINFOHEADER
		{
			/// <summary>
			/// Specifies the number of bytes required by the structure. This value does not include the size of the color table or the size
			/// of the color masks, if they are appended to the end of structure. See Remarks.
			/// </summary>
			public uint biSize;

			/// <summary>
			/// Specifies the width of the bitmap, in pixels. For information about calculating the stride of the bitmap, see Remarks.
			/// </summary>
			public int biWidth;

			/// <summary>
			/// <para>Specifies the height of the bitmap, in pixels.</para>
			/// <list type="bullet">
			/// <item>
			/// <term>
			/// For uncompressed RGB bitmaps, if <c>biHeight</c> is positive, the bitmap is a bottom-up DIB with the origin at the lower
			/// left corner. If <c>biHeight</c> is negative, the bitmap is a top-down DIB with the origin at the upper left corner.
			/// </term>
			/// </item>
			/// <item>
			/// <term>
			/// For YUV bitmaps, the bitmap is always top-down, regardless of the sign of <c>biHeight</c>. Decoders should offer YUV formats
			/// with postive <c>biHeight</c>, but for backward compatibility they should accept YUV formats with either positive or negative <c>biHeight</c>.
			/// </term>
			/// </item>
			/// <item>
			/// <term>For compressed formats, <c>biHeight</c> must be positive, regardless of image orientation.</term>
			/// </item>
			/// </list>
			/// </summary>
			public int biHeight;

			/// <summary>Specifies the number of planes for the target device. This value must be set to 1.</summary>
			public ushort biPlanes;

			/// <summary>
			/// Specifies the number of bits per pixel (bpp). For uncompressed formats, this value is the average number of bits per pixel.
			/// For compressed formats, this value is the implied bit depth of the uncompressed image, after the image has been decoded.
			/// </summary>
			public ushort biBitCount;

			/// <summary>
			/// <para>
			/// For compressed video and YUV formats, this member is a FOURCC code, specified as a <c>DWORD</c> in little-endian order. For
			/// example, YUYV video has the FOURCC 'VYUY' or 0x56595559. For more information, see FOURCC Codes.
			/// </para>
			/// <para>For uncompressed RGB formats, the following values are possible:</para>
			/// <list type="table">
			/// <listheader>
			/// <term>Value</term>
			/// <term>Meaning</term>
			/// </listheader>
			/// <item>
			/// <term>BI_RGB</term>
			/// <term>Uncompressed RGB.</term>
			/// </item>
			/// <item>
			/// <term>BI_BITFIELDS</term>
			/// <term>Uncompressed RGB with color masks. Valid for 16-bpp and 32-bpp bitmaps.</term>
			/// </item>
			/// </list>
			/// <para>See Remarks for more information. Note that <c>BI_JPG</c> and <c>BI_PNG</c> are not valid video formats.</para>
			/// <para>
			/// For 16-bpp bitmaps, if <c>biCompression</c> equals <c>BI_RGB</c>, the format is always RGB 555. If <c>biCompression</c>
			/// equals <c>BI_BITFIELDS</c>, the format is either RGB 555 or RGB 565. Use the subtype GUID in the AM_MEDIA_TYPE structure to
			/// determine the specific RGB type.
			/// </para>
			/// </summary>
			public BitmapCompressionMode biCompression;

			/// <summary>Specifies the size, in bytes, of the image. This can be set to 0 for uncompressed RGB bitmaps.</summary>
			public uint biSizeImage;

			/// <summary>Specifies the horizontal resolution, in pixels per meter, of the target device for the bitmap.</summary>
			public int biXPelsPerMeter;

			/// <summary>Specifies the vertical resolution, in pixels per meter, of the target device for the bitmap.</summary>
			public int biYPelsPerMeter;

			/// <summary>
			/// Specifies the number of color indices in the color table that are actually used by the bitmap. See Remarks for more information.
			/// </summary>
			public uint biClrUsed;

			/// <summary>
			/// Specifies the number of color indices that are considered important for displaying the bitmap. If this value is zero, all
			/// colors are important.
			/// </summary>
			public uint biClrImportant;

			/// <summary>Initializes a new instance of the <see cref="BITMAPINFOHEADER"/> structure.</summary>
			/// <param name="width">The width.</param>
			/// <param name="height">The height.</param>
			/// <param name="bitCount">The bit count.</param>
			public BITMAPINFOHEADER(int width, int height, ushort bitCount = 32)
				: this()
			{
				biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFO));
				biWidth = width;
				biHeight = height;
				biPlanes = 1;
				biBitCount = bitCount;
				biCompression = BitmapCompressionMode.BI_RGB;
				biSizeImage = 0; // (uint)width * (uint)height * bitCount / 8;
			}

			/// <summary>Gets the default value for this structure with size fields set appropriately.</summary>
			public static readonly BITMAPINFOHEADER Default = new() { biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER)) };
		}

		/// <summary>The RGBQUAD structure describes a color consisting of relative intensities of red, green, and blue.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct RGBQUAD
		{
			/// <summary>The intensity of blue in the color.</summary>
			public byte rgbBlue;

			/// <summary>The intensity of green in the color.</summary>
			public byte rgbGreen;

			/// <summary>The intensity of red in the color.</summary>
			public byte rgbRed;

			/// <summary>This member is reserved and must be zero.</summary>
			public byte rgbReserved;

			/// <summary>Gets or sets the color associated with the <see cref="RGBQUAD"/> structure.</summary>
			/// <value>The color.</value>
			public System.Drawing.Color Color
			{
				get => System.Drawing.Color.FromArgb(rgbReserved, rgbRed, rgbGreen, rgbBlue);
				set { rgbReserved = value.A; rgbBlue = value.B; rgbGreen = value.G; rgbRed = value.R; }
			}
		}
	}
}