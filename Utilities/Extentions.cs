using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace CraftMagicItems.Utilities
{
    public static class Extentions
    {
        public static TextMeshProUGUI AssignFontApperanceProperties(this TextMeshProUGUI tmp, TextMeshProUGUI source, bool copyColor = true)
        {
            if (tmp == null) return null;

            if (copyColor)
            {
                tmp.color = source.color;
                tmp.colorGradient = source.colorGradient;
                tmp.colorGradientPreset = source.colorGradientPreset;
            }
            tmp.font = source.font;
            tmp.fontMaterial = source.fontMaterial;
            tmp.fontStyle = source.fontStyle;
            tmp.fontWeight = source.fontWeight;
            tmp.outlineColor = source.outlineColor;
            tmp.outlineWidth = source.outlineWidth;
            tmp.faceColor = source.faceColor;

            return tmp;
        }
    }
}
