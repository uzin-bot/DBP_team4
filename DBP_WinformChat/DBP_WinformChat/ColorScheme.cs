using System.Drawing;

namespace leehaeun
{
    public static class ColorSchemes
    {
        // 현재 테마 모드
        public static bool IsDarkMode { get; set; } = false;

        // 메인 컬러 팔레트
        public static Color Ivory => IsDarkMode ? ColorTranslator.FromHtml("#1E1E1E") : ColorTranslator.FromHtml("#F1F3E0");
        public static Color LightOlive => IsDarkMode ? ColorTranslator.FromHtml("#2D2D2D") : ColorTranslator.FromHtml("#D2DCB6");
        public static Color SageGreen => IsDarkMode ? ColorTranslator.FromHtml("#E0E0E0") : ColorTranslator.FromHtml("#A1BC98");
        public static Color DarkOlive => IsDarkMode ? ColorTranslator.FromHtml("#B0B0B0") : ColorTranslator.FromHtml("#778873");

        // 흰색 (메인)
        public static Color White => IsDarkMode ? ColorTranslator.FromHtml("#2D2D2D") : Color.White;

        // 역할별 컬러
        public static class Background
        {
            public static Color Primary => IsDarkMode ? ColorTranslator.FromHtml("#1E1E1E") : ColorTranslator.FromHtml("#F1F3E0");
            public static Color Secondary => IsDarkMode ? ColorTranslator.FromHtml("#2D2D2D") : ColorTranslator.FromHtml("#D2DCB6");
            public static Color Panel => IsDarkMode ? ColorTranslator.FromHtml("#2D2D2D") : Color.White;
        }

        public static class Accent
        {
            public static Color Primary => IsDarkMode ? ColorTranslator.FromHtml("#E0E0E0") : ColorTranslator.FromHtml("#A1BC98");
            public static Color Dark => IsDarkMode ? ColorTranslator.FromHtml("#B0B0B0") : ColorTranslator.FromHtml("#778873");
        }

        public static class Text
        {
            public static Color Primary => IsDarkMode ? ColorTranslator.FromHtml("#E0E0E0") : ColorTranslator.FromHtml("#778873");
            public static Color Light => IsDarkMode ? ColorTranslator.FromHtml("#B0B0B0") : ColorTranslator.FromHtml("#A1BC98");
            public static Color OnAccent => IsDarkMode ? ColorTranslator.FromHtml("#1E1E1E") : ColorTranslator.FromHtml("#F1F3E0");
            // White 제거 (메인에 있음)
        }

        public static class Border
        {
            public static Color Light => IsDarkMode ? ColorTranslator.FromHtml("#404040") : ColorTranslator.FromHtml("#D2DCB6");
            public static Color Medium => IsDarkMode ? ColorTranslator.FromHtml("#E0E0E0") : ColorTranslator.FromHtml("#A1BC98");
            public static Color Dark => IsDarkMode ? ColorTranslator.FromHtml("#B0B0B0") : ColorTranslator.FromHtml("#778873");
        }

        // 상태별 컬러
        public static class Button
        {
            public static Color Normal => IsDarkMode ? ColorTranslator.FromHtml("#E0E0E0") : ColorTranslator.FromHtml("#A1BC98");
            public static Color Hover => IsDarkMode ? ColorTranslator.FromHtml("#B0B0B0") : ColorTranslator.FromHtml("#778873");
            public static Color Pressed => IsDarkMode ? ColorTranslator.FromHtml("#B0B0B0") : ColorTranslator.FromHtml("#778873");
            public static Color Disabled => IsDarkMode ? ColorTranslator.FromHtml("#404040") : ColorTranslator.FromHtml("#D2DCB6");
        }

        public static class Input
        {
            public static Color Background => IsDarkMode ? ColorTranslator.FromHtml("#2D2D2D") : ColorTranslator.FromHtml("#D2DCB6");
            public static Color Border => IsDarkMode ? ColorTranslator.FromHtml("#E0E0E0") : ColorTranslator.FromHtml("#A1BC98");
            public static Color Focus => IsDarkMode ? ColorTranslator.FromHtml("#B0B0B0") : ColorTranslator.FromHtml("#778873");
        }
    }
}
