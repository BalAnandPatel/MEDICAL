using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;
using Newtonsoft.Json.Linq; 
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Linq;  
using System.Configuration;
using System.Linq;
using System.Net;

namespace BaseApiApp.Framework
{
    /// <summary>
    /// Represents a common Utils helper
    /// </summary>
    public partial class CommonUtils
    {
        /// <summary>
        /// Ensures the subscriber email or throw.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string FormatWithSeperator(string seprator, params string[] list)
        {
            return string.Join(seprator, list.Where(s=>!string.IsNullOrEmpty(s)));
        }
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            string output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new  Exception("Email is not valid.");
            } 
            return output;
        }
        public static string GetHtml(string url)
        {
            string html = "";
            using (WebClient client = new WebClient())
            {
                try
                {
                    if (url.Contains("https://"))
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    html = client.DownloadString(url);
                }
                catch (Exception)
                {

                    html = "";
                }

            }
            return html;
        }

        public static string RemoveHtmlTags(string value)
        {

            
            // reference https://stackoverflow.com/questions/286813/how-do-you-convert-html-to-plain-text
            string output;

            output= WebUtility.HtmlDecode(value);
            //get rid of HTML tags
            output = Regex.Replace(output, "<[^>]*>", string.Empty);

            //get rid of multiple blank lines
            output = Regex.Replace(output, @"^\s*$\n", string.Empty, RegexOptions.Multiline);

            return output;
        } 
        public static string GetQueryString()
        {
            var qryString = HttpContext.Current.Request.QueryString.ToString();
            qryString = string.IsNullOrEmpty(qryString) ? "" : "?" + qryString;
            return qryString;
        }
        public static string GetHostName()
        {
            try
            {
                if (HttpContext.Current?.Request == null) return "";
                if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"] != null)
                    return HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
                return "";
            }
            catch
            {
                return "";
            }

        }
        public static string GetDomainId()
        {
            try
            {

                if (HttpContext.Current == null) return "";
                if (HttpContext.Current.Request == null) return "";
                var domid = HttpContext.Current.Request.QueryString["domid"] ?? "";
                Guid domGuId;
                Guid.TryParse(domid, out domGuId);
                return domGuId == Guid.Empty ? "" : domGuId.ToString();
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
                str = String.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// Returns an random interger number within a specified rage
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return string.Empty;

            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (Char.IsDigit(c))
                    result.Append(c);
            }
            return result.ToString();
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            if (str == null)
                return string.Empty;

            return str;
        }

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            bool result = false;
            Array.ForEach(stringsToValidate, str =>
            {
                if (string.IsNullOrEmpty(str)) result = true;
            });
            return result;
        }


        private static AspNetHostingPermissionLevel? _trustLevel = null;
        /// <summary>
        /// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>The current trust level.</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_trustLevel.HasValue)
            {
                //set minimum
                _trustLevel = AspNetHostingPermissionLevel.None;

                //determine maximum
                foreach (AspNetHostingPermissionLevel trustLevel in
                        new AspNetHostingPermissionLevel[] {
                                AspNetHostingPermissionLevel.Unrestricted,
                                AspNetHostingPermissionLevel.High,
                                AspNetHostingPermissionLevel.Medium,
                                AspNetHostingPermissionLevel.Low,
                                AspNetHostingPermissionLevel.Minimal
                            })
                {
                    try
                    {
                        new AspNetHostingPermission(trustLevel).Demand();
                        _trustLevel = trustLevel;
                        break; //we've set the highest permission we can
                    }
                    catch (System.Security.SecurityException)
                    {
                        continue;
                    }
                }
            }
            return _trustLevel.Value;
        }

        /// <summary>
        /// Sets a property on an object to a valuae.
        /// </summary>
        /// <param name="instance">The object whose property to set.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">The value to set the property to.</param>
          

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            string result = string.Empty;
            char[] letters = str.ToCharArray();
            foreach (char c in letters)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
            return result;
        }
        public static string GetNodeValue(XElement oNode, string childNodeName)
        {
            string nodeVal = "";
            if ((oNode == null)) return nodeVal;
            if ((oNode.Element(childNodeName) != null))
            {
                nodeVal = oNode.Element(childNodeName).ToString();
            }
            return nodeVal;
        }
        public static bool ToBool(XAttribute attribute)
        {
            bool val = false;
            if (attribute == null) return val;
            return attribute.Value == "1" ? true : false;
        }
        public static Guid ConvertToGuid(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                return new Guid(val);
            }
            return Guid.Empty;
        }
        public static string ConvertGuidToString(Guid val)
        {
            return !(val == Guid.Empty) ? val.ToString() : "";
        }

        public static string[] GetWordSerachList()
        {
            string[] words = {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N",
            "O","P","Q","R","S","T","U","V","W","X","Y","Z","0-9","All"
            };
            return words;
        }
        public static string[] GetWordPagination()
        {
            string[] words = {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N",
            "O","P","Q","R","S","T","U","V","W","X","Y","Z"
            };
            return words;
        }
        public static string CheckHasValue(object value)
        {
            return value != null ? value.ToString() : "";
        }

        public static string GetDisplayDateLong(DateTime? dateString)
        {
            return dateString != null && dateString != default(DateTime) ? dateString.ToString() : string.Empty;
        }
        public static string ToDisplayDate(DateTime date, string pattern, string timezoneId)
        {
            if (AllTimeZoneIds.Contains(timezoneId))
            {
                date = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById(timezoneId));
            }
            return date.Year == 1 ? "n/a" : date.ToString(pattern);
        }
        public static string ToDisplayDateLong(DateTime date, string pattern, string timezoneId)
        {
            if (AllTimeZoneIds.Contains(timezoneId))
            {
                date = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById(timezoneId));
            }
            return date.Year == 1 ? "n/a" : date.ToString(pattern);
        }
        public static DateTime ToLocalDateTime(DateTime date, string timezoneId)
        {
            if (AllTimeZoneIds.Contains(timezoneId))
            {
                date = TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById(timezoneId));
            }
            return date;
        }
        public static DateTime ToCurrentLocalDateTime(string timezoneId)
        {
            return ToLocalDateTime(DateTime.UtcNow, timezoneId);
        }
        public static bool ConvertStringToBoolean(string inpt)
        {
            if (string.IsNullOrEmpty(inpt))
            {
                return false;
            }
            return (inpt == "1");
        }

        public static string GetEnumNameByID(Type enumeration, string value)
        {
            return (Enum.GetName(enumeration, Convert.ToInt16(value))).ToString();

        }
        public static Int16 GetEnumValueByName(Type EnumObject, string enumname)
        {
            var enumStrings = Enum.GetValues(EnumObject);
            const short enumvalue = 1;
            foreach (var enumString in enumStrings)
            {
                var name = Enum.GetName(EnumObject, Convert.ToInt16(enumString));
                if (name != null && name.ToLower() == enumname.ToLower())
                {
                    return Convert.ToInt16(enumString);
                }
            }
            return enumvalue;
        }
        public static bool ValidateJson(string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string BooleanToYesNo(bool val)
        {
            if (val) { return "Yes"; } else { return "No"; }
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName) != null ? src.GetType().GetProperty(propName).GetValue(src, null) : null;
        }
        public static decimal ConvertToDecimal(string val)
        {
            return Math.Round(decimal.Parse(val), 2);
        }
        public static decimal ConvertToDecimal(decimal val)
        {
            return Math.Round(val, 2);
        }

        public static string ReplaceInvalidXmlEntity(string val)
        {
            if (string.IsNullOrEmpty(val) == false)
            {
                return val.Replace("\"", "&quot;").Replace("&", "&amp;").Replace("'", "&apos;").Replace("<", "&lt;").Replace(">", "&gt;");
            }
            else
            {
                return "";
            }
        }
        public static string ReplaceSpecialChar(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            else
            {
                return Regex.Replace(str, "[^A-Za-z0-9- ]", "");
                /*
                retval = str.Replace("-", " ");
                //-- this has been done for handling the URL redirects like SearchResult.aspx?qry=Be-Delicious-Shower-Gel-150ml so that the search can be done on Be Delicious Shower Gel 150ml
                retval = retval.Replace("  ", " ");
                retval = retval.Replace("*", " ");
                retval = retval.Replace("?", " ");
                retval = retval.Replace("!", " ");
                retval = retval.Replace("=", " ");
                retval = retval.Replace("%", " ");
                retval = retval.Trim();
                return retval;*/
            }



        }
        public static string FormatDescription(string stringVal)
        {
            var regex = "<(.|\n)+?>";
            if (string.IsNullOrEmpty(stringVal))
            {
                return "";
            }
            else
            {
                stringVal = Regex.Replace(stringVal, regex, String.Empty);
                stringVal = Regex.Replace(stringVal, ",", " ");
                stringVal = Regex.Replace(stringVal, ":", " ");
                stringVal = stringVal.Replace(System.Environment.NewLine, String.Empty).Replace("\r\n", " ").Replace("\r", String.Empty).Replace("\n", String.Empty);
                return stringVal;
            }
        }
        public static string GetThreeChracterCountryCode(string name)
        {
            var cultures = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures);
            string code = "";
            if (string.IsNullOrEmpty(name) == false)
            {
                foreach (var cultre in cultures)
                {

                    var region = new System.Globalization.RegionInfo(cultre.LCID);
                    if (region.TwoLetterISORegionName.ToLower() == name.ToLower())
                    {
                        code = region.ThreeLetterISORegionName;
                    }
                }
            }
            return code;
        }
        /// <summary>
        /// Gets url with http contains
        /// </summary>
        /// <param name="url">Use url</param>
        /// <returns>url with http contains</returns>
        public static string GetUrlWithHttp(string domainUrl)
        {
            if (!string.IsNullOrEmpty(domainUrl))
                return domainUrl.Contains("http") ? domainUrl : "http://" + domainUrl;
            else
                return domainUrl;
        }
        public static string Get24HourFormat(int time)
        {
            if (DateTime.Now.Minute > 30)
                time = time + 1;
            string calTime = null;
            calTime = (time.ToString() + ":00");

            return calTime;
        }
        public static string Get12HourFormat(int time)
        {
            string calTime = null;
            if (time > 12)
            {
                calTime = ((time % 12).ToString() + "PM");
            }
            else
            {
                calTime = (time.ToString() + "AM");
            }
            return calTime;
        }
        public static string GetSEName(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            string OKChars = "abcdefghijklmnopqrstuvwxyz1234567890 _-";
            name = name.Trim().ToLowerInvariant();
            dynamic sb = new StringBuilder();
            foreach (char c in name.ToCharArray())
            {
                if (OKChars.Contains(c.ToString()))
                {
                    sb.Append(c);
                }
            }
            string name2 = sb.ToString();
            name2 = name2.Replace(" ", "-");
            while (name2.Contains("--"))
            {
                name2 = name2.Replace("--", "-");
            }
            while (name2.Contains("__"))
            {
                name2 = name2.Replace("__", "_");
            }
            return name2;
        }
        private static readonly HashSet<string> AllTimeZoneIds =
                new HashSet<string>(TimeZoneInfo.GetSystemTimeZones().Select(tz => tz.Id));

        //public static string GetProductUrlByName(long productId, string productName, string manufacturerName, string subManufacturerName, string domainurl)
        //{
        //    string seName = GetSEName(productName);
        //    if (!String.IsNullOrEmpty(subManufacturerName))
        //    {
        //        seName = GetSEName(subManufacturerName + " " + seName);
        //    }
        //    if (!String.IsNullOrEmpty(manufacturerName))
        //    {
        //        seName = GetSEName(manufacturerName + " " + seName);
        //    }

        //    string url = string.Format("{0}/products/{1}-{2}.aspx", domainurl, seName, productId);
        //    return url.ToLowerInvariant();
        //}

        //public static string GetManufacturerUrl(long manufacturerId, string manufacturerName, string domainurl)
        //{

        //    string seName = GetSEName(manufacturerName);
        //    if (String.IsNullOrEmpty(seName))
        //    {
        //        seName = GetSEName(manufacturerName);
        //    }
        //    string url = string.Format("{0}/brands/{1}-{2}.aspx", domainurl, seName, manufacturerId);
        //    return url.ToLowerInvariant();
        //}
        public static string CreateStrongGenericPassword(int length = 8)
        {
            //create constant strings for each type of characters
            string alphaCaps = "QWERTYUIOPASDFGHJKLZXCVBNM";
            string alphaLow = "qwertyuiopasdfghjklzxcvbnm";
            string numerics = "1234567890";
            string special = "@#$";
            //create another string which is a concatenation of all above
            string allChars = alphaCaps + alphaLow + numerics + special;
            Random r = new Random();

            String generatedPassword = "";

            if (length < 4)
                throw new Exception("Number of characters should be greater than 4.");

            // Generate four repeating random numbers are postions of
            // lower, upper, numeric and special characters
            // By filling these positions with corresponding characters,
            // we can ensure the password has atleast one
            // character of those types
            int pLower, pUpper, pNumber, pSpecial;
            string posArray = "0123456789";
            if (length < posArray.Length)
                posArray = posArray.Substring(0, length);
            pLower = getRandomPosition(r, ref posArray);
            pUpper = getRandomPosition(r, ref posArray);
            pNumber = getRandomPosition(r, ref posArray);
            pSpecial = getRandomPosition(r, ref posArray);


            for (int i = 0; i < length; i++)
            {
                if (i == pLower)
                    generatedPassword += getRandomChar(r, alphaCaps);
                else if (i == pUpper)
                    generatedPassword += getRandomChar(r, alphaLow);
                else if (i == pNumber)
                    generatedPassword += getRandomChar(r, numerics);
                else if (i == pSpecial)
                    generatedPassword += getRandomChar(r, special);
                else
                    generatedPassword += getRandomChar(r, allChars);
            }
            return generatedPassword;
        }
        private static string getRandomChar(Random r, string fullString)
        {
            return fullString.ToCharArray()[(int)Math.Floor(r.NextDouble() * fullString.Length)].ToString();
        }

        private static int getRandomPosition(Random r, ref string posArray)
        {
            int pos;
            string randomChar = posArray.ToCharArray()[(int)Math.Floor(r.NextDouble()
                                           * posArray.Length)].ToString();
            pos = int.Parse(randomChar);
            posArray = posArray.Replace(randomChar, "");
            return pos;
        }
        public static string GetCountryPhoneCode(string countryCode)
        {
            countryCode = countryCode.ToUpper();
            switch (countryCode)
            {
                case "GB": return "+44";
                case "US": return "+1";
                case "IN": return "+91";
                case "DE": return "+49";
                case "FR": return "+33";
                case "AU": return "+61";
                case "BD": return "+880";
                case "BE": return "+32";
                case "BF": return "+226";
                case "BG": return "+359";
                case "BA": return "+387";
                case "BB": return "+1-246";
                case "WF": return "+681";
                case "BL": return "+590";
                case "BN": return "+673";
                case "BO": return "+591";
                case "BH": return "+973";
                case "BI": return "+257";
                case "BJ": return "+229";
                case "BT": return "+975";
                case "JM": return "+1-876";
                case "BW": return "+267";
                case "WS": return "+685";
                case "BQ": return "+599";
                case "BR": return "+55";
                case "BS": return "+1-242";
                case "JE": return "+44-1534";
                case "BY": return "+375";
                case "BZ": return "+501";
                case "RU": return "+7";
                case "RW": return "+250";
                case "RS": return "+381";
                case "TL": return "+670";
                case "RE": return "+262";
                case "TM": return "+993";
                case "TJ": return "+992";
                case "RO": return "+40";
                case "TK": return "+690";
                case "GW": return "+245";
                case "GU": return "+1-671";
                case "GT": return "+502";
                case "GR": return "+30";
                case "GQ": return "+240";
                case "GP": return "+590";
                case "JP": return "+81";
                case "GY": return "+592";
                case "GG": return "+44-1481";
                case "GD": return "+1-473";
                case "GA": return "+241";
                case "SV": return "+503";
                case "GN": return "+224";
                case "GL": return "+299";
                case "GI": return "+350";
                case "GH": return "+233";
                case "OM": return "+968";
                case "TN": return "+216";
                case "JO": return "+962";
                case "HR": return "+385";
                case "HT": return "+509";
                case "HU": return "+36";
                case "HK": return "+852";
                case "HN": return "+504";
                case "VE": return "+58";
                case "PR": return "+1-787";
                case "PS": return "+970";
                case "PW": return "+680";
                case "PT": return "+351";
                case "SJ": return "+47";
                case "PY": return "+595";
                case "IQ": return "+964";
                case "PA": return "+507";
                case "PF": return "+689";
                case "PG": return "+675";
                case "PE": return "+51";
                case "PK": return "+92";
                case "PH": return "+63";
                case "PN": return "+870";
                case "PL": return "+48";
                case "PM": return "+508";
                case "ZM": return "+260";
                case "EH": return "+212";
                case "EE": return "+372";
                case "EG": return "+20";
                case "ZA": return "+27";
                case "EC": return "+593";
                case "IT": return "+39";
                case "VN": return "+84";
                case "SB": return "+677";
                case "ET": return "+251";
                case "SO": return "+252";
                case "ZW": return "+263";
                case "SA": return "+966";
                case "ES": return "+34";
                case "ER": return "+291";
                case "ME": return "+382";
                case "MD": return "+373";
                case "MG": return "+261";
                case "MF": return "+590";
                case "MA": return "+212";
                case "MC": return "+377";
                case "UZ": return "+998";
                case "MM": return "+95";
                case "ML": return "+223";
                case "MO": return "+853";
                case "MN": return "+976";
                case "MH": return "+692";
                case "MK": return "+389";
                case "MU": return "+230";
                case "MT": return "+356";
                case "MW": return "+265";
                case "MV": return "+960";
                case "MQ": return "+596";
                case "MP": return "+1-670";
                case "MS": return "+1-664";
                case "MR": return "+222";
                case "IM": return "+44-1624";
                case "UG": return "+256";
                case "TZ": return "+255";
                case "MY": return "+60";
                case "MX": return "+52";
                case "IL": return "+972";               
                case "IO": return "+246";
                case "SH": return "+290";
                case "FI": return "+358";
                case "FJ": return "+679";
                case "FK": return "+500";
                case "FM": return "+691";
                case "FO": return "+298";
                case "NI": return "+505";
                case "NL": return "+31";
                case "NO": return "+47";
                case "NA": return "+264";
                case "VU": return "+678";
                case "NC": return "+687";
                case "NE": return "+227";
                case "NF": return "+672";
                case "NG": return "+234";
                case "NZ": return "+64";
                case "NP": return "+977";
                case "NR": return "+674";
                case "NU": return "+683";
                case "CK": return "+682"; 
                case "CI": return "+225";
                case "CH": return "+41";
                case "CO": return "+57";
                case "CN": return "+86";
                case "CM": return "+237";
                case "CL": return "+56";
                case "CC": return "+61";
                case "CA": return "+1";
                case "CG": return "+242";
                case "CF": return "+236";
                case "CD": return "+243";
                case "CZ": return "+420";
                case "CY": return "+357";
                case "CX": return "+61";
                case "CR": return "+506";
                case "CW": return "+599";
                case "CV": return "+238";
                case "CU": return "+53";
                case "SZ": return "+268";
                case "SY": return "+963";
                case "SX": return "+599";
                case "KG": return "+996";
                case "KE": return "+254";
                case "SS": return "+211";
                case "SR": return "+597";
                case "KI": return "+686";
                case "KH": return "+855";
                case "KN": return "+1-869";
                case "KM": return "+269";
                case "ST": return "+239";
                case "SK": return "+421";
                case "KR": return "+82";
                case "SI": return "+386";
                case "KP": return "+850";
                case "KW": return "+965";
                case "SN": return "+221";
                case "SM": return "+378";
                case "SL": return "+232";
                case "SC": return "+248";
                case "KZ": return "+7";
                case "KY": return "+1-345";
                case "SG": return "+65";
                case "SE": return "+46";
                case "SD": return "+249";
                case "DO": return "+1-809";
                case "DM": return "+1-767";
                case "DJ": return "+253";
                case "DK": return "+45";
                case "VG": return "+1-284";                
                case "YE": return "+967";
                case "DZ": return "+213";                
                case "UY": return "+598";
                case  "YT": return "+262";
                case "UM": return "+1";
                case "LB": return "+961";
                case "LC": return "+1-758";
                case "LA": return "+856";
                case "TV": return "+688";
                case "TW": return "+886";
                case "TT": return "+1-868";
                case "TR": return "+90";
                case "LK": return "+94";
                case "LI": return "+423";
                case "LV": return "+371";
                case "TO": return "+676";
                case "LT": return "+370";
                case "LU": return "+352";
                case "LR": return "+231";
                case "LS": return "+266";
                case "TH": return "+66";
                case "TF": return "+";
                case "TG": return "+228";
                case "TD": return "+235";
                case "TC": return "+1-649";
                case "LY": return "+218";
                case "VA": return "+379";
                case "VC": return "+1-784";
                case "AE": return "+971";
                case "AD": return "+376";
                case "AG": return "+1-268";
                case "AF": return "+93";
                case "AI": return "+1-264";
                case "VI": return "+1-340";
                case "IS": return "+354";
                case "IR": return "+98";
                case "AM": return "+374";
                case "AL": return "+355";
                case "AO": return "+244";
                case "AQ": return "+";
                case "AS": return "+1-684";
                case "AR": return "+54";               
                case "AT": return "+43";
                case "AW": return "+297";              
                case "AX": return "+358-18";
                case "AZ": return "+994";
                case "IE": return "+353";
                case "ID": return "+62";
                case "UA": return "+380"; 
                case "QA": return "+974";
                case "MZ":  return "+258";

            }


            /*
            * {   "PF": "689", "PG": "675", "PE": "51", "PK": "92", "PH": "63", "PN": "870", "PL": "48", "PM": "508", "ZM": "260", "EH": "212", "EE": "372", "EG": "20", "ZA": "27", "EC": "593", "IT": "39", "VN": "84", "SB": "677", "ET": "251", "SO": "252", "ZW": "263", "SA": "966", "ES": "34", "ER": "291", "ME": "382", "MD": "373", "MG": "261", "MF": "590", "MA": "212", "MC": "377", "UZ": "998", "MM": "95", "ML": "223", "MO": "853", "MN": "976", "MH": "692", "MK": "389", "MU": "230", "MT": "356", "MW": "265", "MV": "960", "MQ": "596", "MP": "+1-670", "MS": "+1-664", "MR": "222", "IM": "+44-1624", "UG": "256", "TZ": "255", "MY": "60", "MX": "52", "IL": "972", "FR": "33", "IO": "246", "SH": "290", "FI": "358", "FJ": "679", "FK": "500", "FM": "691", "FO": "298", "NI": "505", "NL": "31", "NO": "47", "NA": "264", "VU": "678", "NC": "687", "NE": "227", "NF": "672", "NG": "234", "NZ": "64", "NP": "977", "NR": "674", "NU": "683", "CK": "682", "XK": "", "CI": "225", "CH": "41", "CO": "57", "CN": "86", "CM": "237", "CL": "56", "CC": "61", "CA": "1", "CG": "242", "CF": "236", "CD": "243", "CZ": "420", "CY": "357", "CX": "61", "CR": "506", "CW": "599", "CV": "238", "CU": "53", "SZ": "268", "SY": "963", "SX": "599", "KG": "996", "KE": "254", "SS": "211", "SR": "597", "KI": "686", "KH": "855", "KN": "+1-869", "KM": "269", "ST": "239", "SK": "421", "KR": "82", "SI": "386", "KP": "850", "KW": "965", "SN": "221", "SM": "378", "SL": "232", "SC": "248", "KZ": "7", "KY": "+1-345", "SG": "65", "SE": "46", "SD": "249", "DO": "+1-809 and 1-829", "DM": "+1-767", "DJ": "253", "DK": "45", "VG": "+1-284", "DE": "49", "YE": "967", "DZ": "213", "US": "1", "UY": "598", "YT": "262", "UM": "1", "LB": "961", "LC": "+1-758", "LA": "856", "TV": "688", "TW": "886", "TT": "+1-868", "TR": "90", "LK": "94", "LI": "423", "LV": "371", "TO": "676", "LT": "370", "LU": "352", "LR": "231", "LS": "266", "TH": "66", "TF": "", "TG": "228", "TD": "235", "TC": "+1-649", "LY": "218", "VA": "379", "VC": "+1-784", "AE": "971", "AD": "376", "AG": "+1-268", "AF": "93", "AI": "+1-264", "VI": "+1-340", "IS": "354", "IR": "98", "AM": "374", "AL": "355", "AO": "244", "AQ": "", "AS": "+1-684", "AR": "54", "AU": "61", "AT": "43", "AW": "297", "IN": "91", "AX": "+358-18", "AZ": "994", "IE": "353", "ID": "62", "UA": "380", "QA": "974", "MZ": "258"}
            
            , {"BD": "880","BE": "32", "BF": "226", "BG": "359", "BA": "387", "BB": "+1-246", "WF": "681", "BL": "590", "BM": "+1-441", "BN": "673", "BO": "591", "BH": "973", "BI": "257", "BJ": "229", "BT": "975", "JM": "+1-876", "BV": "", "BW": "267", "WS": "685", "BQ": "599", "BR": "55", "BS": "+1-242", "JE": "+44-1534", "BY": "375", "BZ": "501", "RU": "7", "RW": "250", "RS": "381", "TL": "670", "RE": "262", "TM": "993", "TJ": "992", "RO": "40", "TK": "690", "GW": "245", "GU": "+1-671", "GT": "502", "GS": "", "GR": "30", "GQ": "240", "GP": "590", "JP": "81", "GY": "592", "GG": "+44-1481", "GF": "594", "GE": "995", "GD": "+1-473", "GB": "44", "GA": "241", "SV": "503", "GN": "224", "GM": "220", "GL": "299", "GI": "350", "GH": "233", "OM": "968", "TN": "216", "JO": "962", "HR": "385", "HT": "509", "HU": "36", "HK": "852", "HN": "504", "HM": " ", "VE": "58", "PR": "+1-787 and 1-939", "PS": "970", "PW": "680", "PT": "351", "SJ": "47", "PY": "595", "IQ": "964", "PA": "507", "PF": "689", "PG": "675", "PE": "51", "PK": "92", "PH": "63", "PN": "870", "PL": "48", "PM": "508", "ZM": "260", "EH": "212", "EE": "372", "EG": "20", "ZA": "27", "EC": "593", "IT": "39", "VN": "84", "SB": "677", "ET": "251", "SO": "252", "ZW": "263", "SA": "966", "ES": "34", "ER": "291", "ME": "382", "MD": "373", "MG": "261", "MF": "590", "MA": "212", "MC": "377", "UZ": "998", "MM": "95", "ML": "223", "MO": "853", "MN": "976", "MH": "692", "MK": "389", "MU": "230", "MT": "356", "MW": "265", "MV": "960", "MQ": "596", "MP": "+1-670", "MS": "+1-664", "MR": "222", "IM": "+44-1624", "UG": "256", "TZ": "255", "MY": "60", "MX": "52", "IL": "972", "FR": "33", "IO": "246", "SH": "290", "FI": "358", "FJ": "679", "FK": "500", "FM": "691", "FO": "298", "NI": "505", "NL": "31", "NO": "47", "NA": "264", "VU": "678", "NC": "687", "NE": "227", "NF": "672", "NG": "234", "NZ": "64", "NP": "977", "NR": "674", "NU": "683", "CK": "682", "XK": "", "CI": "225", "CH": "41", "CO": "57", "CN": "86", "CM": "237", "CL": "56", "CC": "61", "CA": "1", "CG": "242", "CF": "236", "CD": "243", "CZ": "420", "CY": "357", "CX": "61", "CR": "506", "CW": "599", "CV": "238", "CU": "53", "SZ": "268", "SY": "963", "SX": "599", "KG": "996", "KE": "254", "SS": "211", "SR": "597", "KI": "686", "KH": "855", "KN": "+1-869", "KM": "269", "ST": "239", "SK": "421", "KR": "82", "SI": "386", "KP": "850", "KW": "965", "SN": "221", "SM": "378", "SL": "232", "SC": "248", "KZ": "7", "KY": "+1-345", "SG": "65", "SE": "46", "SD": "249", "DO": "+1-809 and 1-829", "DM": "+1-767", "DJ": "253", "DK": "45", "VG": "+1-284", "DE": "49", "YE": "967", "DZ": "213", "US": "1", "UY": "598", "YT": "262", "UM": "1", "LB": "961", "LC": "+1-758", "LA": "856", "TV": "688", "TW": "886", "TT": "+1-868", "TR": "90", "LK": "94", "LI": "423", "LV": "371", "TO": "676", "LT": "370", "LU": "352", "LR": "231", "LS": "266", "TH": "66", "TF": "", "TG": "228", "TD": "235", "TC": "+1-649", "LY": "218", "VA": "379", "VC": "+1-784", "AE": "971", "AD": "376", "AG": "+1-268", "AF": "93", "AI": "+1-264", "VI": "+1-340", "IS": "354", "IR": "98", "AM": "374", "AL": "355", "AO": "244", "AQ": "", "AS": "+1-684", "AR": "54", "AU": "61", "AT": "43", "AW": "297", "IN": "91", "AX": "+358-18", "AZ": "994", "IE": "353", "ID": "62", "UA": "380", "QA": "974", "MZ": "258"}
             * */
            return "";
        }

    }
}
