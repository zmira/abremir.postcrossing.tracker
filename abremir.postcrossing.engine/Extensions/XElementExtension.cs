using abremir.postcrossing.engine.Models;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace abremir.postcrossing.engine.Extensions
{
    public static class XElementExtension
    {
        public static Country ToCountry(this XElement xelement, int position = 1)
        {
            var countryElement = xelement
                .XPathSelectElements($"//a[contains(concat('', @href, ''), '/country/')][position() = {position}]")
                .FirstOrDefault();

            if (countryElement == null)
            {
                return null;
            }

            var name = countryElement.Value;

            if (string.IsNullOrWhiteSpace(name))
            {
                name = ((XElement)countryElement.FirstNode).FirstAttribute.Value;
            }

            var code = countryElement.FirstAttribute.Value.Split('/')[2];

            return new Country
            {
                Name = name,
                Code = code
            };
        }

        public static Postcard ToPostcard(this XElement xelement, int countryIndex = 2)
        {
            var postcardElement = xelement
                .XPathSelectElement("//a[contains(concat('', @href, ''), '/postcards/')]");

            if (postcardElement == null)
            {
                return null;
            }

            return new Postcard
            {
                PostcardId = postcardElement.FirstAttribute.Value.Split('/')[2],
                Country = xelement.ToCountry(countryIndex)
            };
        }

        public static User ToUser(this XElement xelement, int userIndex = 1, int countryIndex = 1)
        {
            var userElement = xelement
                .XPathSelectElements($"//a[contains(concat('', @href, ''), '/user/')][position() = {userIndex}]")
                .FirstOrDefault();

            if (userElement == null)
            {
                return null;
            }

            return new User
            {
                Name = userElement.Value,
                Country = xelement.ToCountry(countryIndex)
            };
        }
    }
}
