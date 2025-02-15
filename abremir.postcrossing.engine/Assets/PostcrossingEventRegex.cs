using System.Text.RegularExpressions;

namespace abremir.postcrossing.engine.Assets
{
    internal static partial class PostcrossingEventRegex
    {
        // <a title=\"France flag\" href=\"/country/FR\"><i class=\"flag flag-FR\"></i></a> <a href=\"/user/Steve2006\">Steve2006</a> received a <a href=\"/postcards/RU-8231660\">postcard</a> from <a title=\"Russia flag\" href=\"/country/RU\"><i class=\"flag flag-RU\"></i></a> <a href=\"/user/Annakam41\">Annakam41</a>
        public static readonly Regex Register = RegisterRegex();

        // <a title=\"U.S.A. flag\" href=\"/country/US\"><i class=\"flag flag-US\"></i></a> <a href=\"/user/smgray01\">smgray01</a> sent a postcard to <i title=\"Russia flag\" class=\"flag flag-RU\"></i> <a href=\"/country/RU\">Russia</a>
        public static readonly Regex Send = SendRegex();

        // <a href =\"/user/MoonWencke\">MoonWencke</a> from <i title=\"Germany flag\" class=\"flag flag-DE\"></i> <a href=\"/country/DE\">Germany</a> just signed up
        public static readonly Regex SignUp = SignupRegex();

        // <a title=\"Japan flag\" href=\"/country/JP\"><i class=\"flag flag-JP\"></i></a> <a href=\"/user/morningchild\">morningchild</a> uploaded postcard <a title=\"Thailand flag\" href=\"/country/TH\"><i class=\"flag flag-TH\"></i></a> <a href=\"/postcards/TH-330886\">TH-330886</a>
        public static readonly Regex Upload = UploadRegex();

        [GeneratedRegex(@"^(\<a.+\/a\>) (\<a.+\/a\>) sent a postcard to (\<i.+\/i\>) (\<a.+\/a\>)$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
        private static partial Regex SendRegex();
        [GeneratedRegex(@"^(\<a.+\/a\>) (\<a.+\/a\>) received a (\<a.+\>)postcard\<\/a\> from (\<a.+\/a\>) (\<a.+\/a\>)$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
        private static partial Regex RegisterRegex();
        [GeneratedRegex(@"^(\<a.+\/a\>).+(\<a.+\/a\>) just signed up$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
        private static partial Regex SignupRegex();
        [GeneratedRegex(@"^(\<a.+\/a\>) (\<a.+\/a\>) uploaded postcard (\<a.+\/a\>) (\<a.+\/a\>)$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
        private static partial Regex UploadRegex();
    }
}
