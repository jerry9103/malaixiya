using System.Drawing;
using UnityEditor;
using UnityEngine;

public class TextureSetting : Editor
{

    [MenuItem("Tools/Texture/Setting(选中图片执行设置ios、android图片质量)")]
    public static void SetTexture()
    {
        Texture2D[] texs = Selection.GetFiltered<Texture2D>(SelectionMode.TopLevel);
        string path;
        foreach (var tex in texs)
        {
            path = AssetDatabase.GetAssetPath(tex);

            AssetImporter importer = AssetImporter.GetAtPath(path);

            if (importer != null)
            {
                SetTexture(tex, importer);
                SaveImage(tex, AssetDatabase.GetAssetPath(tex));
                SetTexture(tex, importer, false);
            }
        }
    }

    /// <summary>
    /// 设置图片质量
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="importer"></param>
    static void SetTexture(Texture2D tex, AssetImporter importer, bool read = true)
    {
        TextureImporter textureImporter = (TextureImporter)importer;

        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.alphaIsTransparency = true;
        textureImporter.wrapMode = TextureWrapMode.Clamp;
        textureImporter.filterMode = FilterMode.Bilinear;
        textureImporter.mipmapEnabled = false;
        textureImporter.androidETC2FallbackOverride = AndroidETC2FallbackOverride.UseBuildSettings;
        textureImporter.isReadable = read;

        TextureImporterPlatformSettings platform = new TextureImporterPlatformSettings();
        platform.overridden = true;
        platform.format = TextureImporterFormat.ETC2_RGBA8;
        platform.name = "Android";
        platform.maxTextureSize = read ? 4096 : GetTextureMaxSize(tex);
        textureImporter.SetPlatformTextureSettings(platform);
        platform.format = TextureImporterFormat.ASTC_6x6;
        platform.name = "iOS";
        textureImporter.SetPlatformTextureSettings(platform);
        textureImporter.SaveAndReimport();
    }

    /// <summary>
    /// 获取图片最大尺寸
    /// </summary>
    /// <param name="tex"></param>
    /// <returns></returns>
    static int GetTextureMaxSize(Texture2D tex)
    {
        int width = tex.width;
        int height = tex.height;

        int targetWidth, targetHeight;
        int pot = 32;
        while (width > pot)
        {
            pot *= 2;
        }
        targetWidth = pot;
        pot = 32;
        while (height > pot)
        {
            pot *= 2;
        }
        targetHeight = pot;

        return targetWidth > targetHeight ? targetWidth : targetHeight;
    }

    /// <summary>
    /// resize满足压缩尺寸保存
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="path"></param>
    static void SaveImage(Texture2D tex, string path)
    {
        if (tex.width % 4 == 0 && tex.height % 4 == 0) return;

        byte[] bytes = tex.EncodeToPNG();
        Image image = getImage(bytes);

        System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;

        string fileName = Application.dataPath.Replace("Assets", "") + path;
        string[] s = path.Split('.');
        if (s[s.Length - 1].ToLower().Contains("jpg"))
        {
            format = System.Drawing.Imaging.ImageFormat.Jpeg;
        }

        Bitmap b = resizeImage(new Bitmap(image), tex.width % 4 == 0 ? tex.width : (tex.width / 4 + 1) * 4, tex.height % 4 == 0 ? tex.height : (tex.height / 4 + 1) * 4);
        b.Save(path, format);
    }

    /// <summary>
    /// 获取图片
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    static Image getImage(byte[] bytes)
    {
        if (bytes == null) return null;
        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
        {
            Image image = Image.FromStream(ms);
            ms.Flush();
            return image;
        }
    }

    /// <summary>
    /// 设置图片尺寸
    /// </summary>
    /// <param name="bmp"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    static Bitmap resizeImage(Bitmap bmp, int width, int height)
    {
        try
        {
            Bitmap b = new Bitmap(width, height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(bmp, new Rectangle(0, 0, width, height), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
            g.Dispose();
            return b;
        }
        catch
        {
            return null;
        }
    }
}
