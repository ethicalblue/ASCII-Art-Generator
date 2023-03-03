using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

internal partial class Program
{
    [LibraryImport("user32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    public static partial int MessageBoxW(IntPtr hWnd, string text, string caption, uint type);

    [SupportedOSPlatform("windows")]
    private static void Main(string[] args)
    {
        Console.WriteLine(@"
         ASCII Art Generator in C#.NET
              _   _    _         _   _    _          
          ___| |_| |_ (_)__ __ _| | | |__| |_  _ ___ 
         / -_)  _| ' \| / _/ _` | |_| '_ \ | || / -_)
         \___|\__|_||_|_\__\__,_|_(_)_.__/_|\_,_\___|
                                             Magazine");

        string path;

        if (args.Length == 0)
        {
            MessageBoxW(0, "Please provide .PNG picture as first argument.", "Error", 0);
            return;
        }
        else
        {
            path = args[0];
        }

        if (File.Exists(path) == false)
        {
            MessageBoxW(0, "Selected file does not exist.", "Error", 0);
            return;
        }

        if (Path.GetExtension(path).ToLowerInvariant() != ".png")
        {
            MessageBoxW(0, "Selected file is not Portable Network Graphic (PNG).", "Error", 0);
            return;
        }

        string html =
        @"<!doctype html>
<html lang=""en"" style=""background-color: #000000; color: #ddeeff; font-weight: bold; font-family: 'Cascadia Code', Consolas, monospace; font-size: 17px;"">
  <head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <title>Unicode Art</title>
  </head>
  <body>";

        Bitmap bitmap = new(path);

        Random rand = new();
        string krzaki = string.Empty;

        for (int i = 0; i < 0xFF; i++)
        {
            if ((i >= 0x21 && i <= 0x7E) || (i >= 0xC0 && i <= 0xFF))
                krzaki += Convert.ToChar(i);
        }

        int length;

        if (bitmap.Width > bitmap.Height)
            length = bitmap.Width;
        else
            length = bitmap.Height;

        if (length > 127)
        {
            MessageBoxW(0, "Anti Denial of Service protection. Try smaller picture (127x127 pixels is maximum).", "Anti-DoS Protection", 0);
            return;
        }

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                string krzak = krzaki.ElementAt(rand.Next(0, krzaki.Length)).ToString();

                try
                {
                    Color pixel = bitmap.GetPixel(j, i);

                    var r = Convert.ToHexString(new byte[] { pixel.R });
                    var g = Convert.ToHexString(new byte[] { pixel.G });
                    var b = Convert.ToHexString(new byte[] { pixel.B });

                    html += $"<span style=\"color: #{r}{g}{b}\">{krzak}</span>";
                }
                catch (Exception)
                {
                    html += $"<span style=\"color: #000000\">{krzak}</span>";
                }
            }
            html += "<br />";
        }

        html += "</body></html>";

        File.WriteAllText("ascii-text-art.html", html);
    }
}