using System.Text.RegularExpressions;

namespace abremir.postcrossing.engine.Assets
{
    internal static class PostcrossingEventRegex
    {
        // <a href="/country/US"><i title="U.S.A." class="flag flag-US"></i></a> <a href="/user/rangermom">rangermom</a> received a <a href="/postcards/CA-724761">postcard</a> from <a href="/country/CA"><i title="Canada" class="flag flag-CA"></i></a> <a href="/user/green-eyes">green-eyes</a>
        public static readonly Regex Register = new Regex(@"^(\<a.+\/a\>) (\<a.+\/a\>) received a (\<a.+\>)postcard\<\/a\> from (\<a.+\/a\>) (\<a.+\/a\>)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // <a href="/country/TW"><i title="Taiwan" class="flag flag-TW"></i></a> <a href="/user/Cloris_Hsieh">Cloris_Hsieh</a> sent a postcard to <a href="/country/PT"><i title="Portugal" class="flag flag-PT"></i></a> <a href="/country/PT">Portugal</a>
        public static readonly Regex Send = new Regex(@"^(\<a.+\/a\>) (\<a.+\/a\>) sent a postcard to (\<a.+\/a\>) (\<a.+\/a\>)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // <a href="/user/21m0">21m0</a> from <i title="China" class="flag flag-CN"></i> <a href="/country/CN">China</a> just signed up
        public static readonly Regex SignUp = new Regex(@"^(\<a.+\/a\>).+(\<a.+\/a\>) just signed up$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // <a href="/country/AU"><i title="Australia" class="flag flag-AU"></i></a> <a href="/user/WattlePark">WattlePark</a> uploaded postcard <a href="/country/AU"><i title="Australia" class="flag flag-AU"></i></a> <a href="/postcards/AU-555156">AU-555156</a>
        public static readonly Regex Upload = new Regex(@"^(\<a.+\/a\>) (\<a.+\/a\>) uploaded postcard (\<a.+\/a\>) (\<a.+\/a\>)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
