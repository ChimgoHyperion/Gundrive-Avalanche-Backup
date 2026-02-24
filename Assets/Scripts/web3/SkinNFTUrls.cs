using System;
using System.Collections.Generic;

public static class SkinNFTUrls
{
    // Case-insensitive, read-only dictionary of skin name -> url
    public static readonly IReadOnlyDictionary<string, string> Dictionary =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"KhufuSkin",   "https://res.cloudinary.com/norvirae/image/upload/v1758354697/gun_drive/KhufuSkin.png"},
            {"AlienSkin",   "https://res.cloudinary.com/norvirae/image/upload/v1758354697/gun_drive/AlienSkin.png"},
            {"Nnae-MechaSkin","https://res.cloudinary.com/norvirae/image/upload/v1758354697/gun_drive/Nnae-MechaSkin.png"},
            {"AlohaSkin",   "https://res.cloudinary.com/norvirae/image/upload/v1758354698/gun_drive/AlohaSkin.png"},
            {"AdaezeSkin",  "https://res.cloudinary.com/norvirae/image/upload/v1758354698/gun_drive/AdaezeSkin.png"},
            {"BryanSkin",   "https://res.cloudinary.com/norvirae/image/upload/v1758354698/gun_drive/BryanSkin.png"},
            {"AngelSkin",   "https://res.cloudinary.com/norvirae/image/upload/v1758354698/gun_drive/AngelSkin.png"},
            {"Dr.DispenserSkin","https://res.cloudinary.com/norvirae/image/upload/v1758354698/gun_drive/Dr.DispenserSkin.png"},
            {"Dr.VenethorSkin","https://res.cloudinary.com/norvirae/image/upload/v1758354700/gun_drive/Dr.VenethorSkin.png"},
            {"GhostSkin",   "https://res.cloudinary.com/norvirae/image/upload/v1758354700/gun_drive/GhostSkin.png"},
            {"NewbaccaSkin","https://res.cloudinary.com/norvirae/image/upload/v1758354701/gun_drive/NewbaccaSkin.png"},
            {"PineappleSkin","https://res.cloudinary.com/norvirae/image/upload/v1758354703/gun_drive/PineappleSkin.png"},
            {"SandySkin",   "https://res.cloudinary.com/norvirae/image/upload/v1758354703/gun_drive/SandySkin.png"}
        };

    // Convenience method that throws if key not found
    public static string GetUrl(string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        return Dictionary[key]; // will throw KeyNotFoundException if missing
    }

    // Safe try-get pattern
    public static bool TryGetUrl(string key, out string url)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        return Dictionary.TryGetValue(key, out url);
    }
}
