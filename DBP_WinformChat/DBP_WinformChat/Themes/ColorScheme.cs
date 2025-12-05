using System.Drawing;

namespace leehaeun.Themes
{
    public static class ColorScheme
    {
        // 메인 컬러 팔레트
        public static Color Background = ColorTranslator.FromHtml("#F1F3E0");      // 아이보리 배경
        public static Color CardBackground = ColorTranslator.FromHtml("#D2DCB6");  // 연한 올리브
        public static Color Primary = ColorTranslator.FromHtml("#A1BC98");         // 올리브 그린
        public static Color PrimaryDark = ColorTranslator.FromHtml("#778873");     // 다크 올리브

        // 텍스트 컬러
        public static Color TextPrimary = ColorTranslator.FromHtml("#3D4A3C");     // 진한 그린
        public static Color TextSecondary = ColorTranslator.FromHtml("#778873");   // 중간 그린
        public static Color TextMuted = ColorTranslator.FromHtml("#9BA89A");       // 연한 그린

        // 인풋 컬러
        public static Color InputBackground = Color.White;
        public static Color InputBorder = ColorTranslator.FromHtml("#A1BC98");
        public static Color InputFocus = ColorTranslator.FromHtml("#778873");

        // 상태 컬러
        public static Color Success = ColorTranslator.FromHtml("#6B9B6E");
        public static Color Error = ColorTranslator.FromHtml("#C85A54");
        public static Color Warning = ColorTranslator.FromHtml("#E8B86D");
    }
}
