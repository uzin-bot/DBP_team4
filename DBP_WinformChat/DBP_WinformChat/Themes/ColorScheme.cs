using System.Drawing;

namespace leehaeun.Themes
{
    public static class ColorSchemes
    {
        // 메인 컬러 팔레트
        public static readonly Color Ivory = ColorTranslator.FromHtml("#F1F3E0");
        public static readonly Color LightOlive = ColorTranslator.FromHtml("#D2DCB6");
        public static readonly Color SageGreen = ColorTranslator.FromHtml("#A1BC98");
        public static readonly Color DarkOlive = ColorTranslator.FromHtml("#778873");

        // 역할별 컬러
        public static class Background
        {
            public static readonly Color Primary = Ivory;        // #F1F3E0
            public static readonly Color Secondary = LightOlive; // #D2DCB6
        }

        public static class Accent
        {
            public static readonly Color Primary = SageGreen;    // #A1BC98
            public static readonly Color Dark = DarkOlive;       // #778873
        }

        public static class Text
        {
            public static readonly Color Primary = DarkOlive;    // #778873
            public static readonly Color Light = SageGreen;      // #A1BC98
            public static readonly Color OnAccent = Ivory;       // #F1F3E0
        }

        public static class Border
        {
            public static readonly Color Light = LightOlive;     // #D2DCB6
            public static readonly Color Medium = SageGreen;     // #A1BC98
            public static readonly Color Dark = DarkOlive;       // #778873
        }

        // 상태별 컬러
        public static class Button
        {
            public static readonly Color Normal = SageGreen;     // #A1BC98
            public static readonly Color Hover = DarkOlive;      // #778873
            public static readonly Color Pressed = DarkOlive;    // #778873
            public static readonly Color Disabled = LightOlive;  // #D2DCB6
        }

        public static class Input
        {
            public static readonly Color Background = LightOlive; // #D2DCB6
            public static readonly Color Border = SageGreen;      // #A1BC98
            public static readonly Color Focus = DarkOlive;       // #778873
        }
    }
}
