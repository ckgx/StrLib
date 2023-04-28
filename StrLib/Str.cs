using System.Text;
using System.Text.RegularExpressions;

namespace StrLib
{
    /// <summary>
    /// 字符串常用操作工具类
    /// </summary>
    public static class Str
    {
        /// <summary>
        /// 特殊字符，用于正则过滤特殊字符
        /// </summary>
        public static string SpecialCharacters = "[ \\[ \\] \\^ \\-_*×――(^)$%~!@#$…&%￥—+=<>《》!！??？//:：•`·、。，；,.;\"‘’“”-]";

        /// <summary>
        /// <para>判断string是否有内容，无内容：true</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>是\"\"、null、"null"：true</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// <para>是有效内容</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>不是由\"\"、null、"null"、空格、开头结尾包含空格类别的"null"：true</returns>
        public static bool IsValid(this string? str)
        {
            return !IsInvalid(str);
        }

        /// <summary>
        /// <para>不是有效内容</para>
        /// <para>注意：此方法会检查空格类别字符串，空格类别组成的字符串为无效内容</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>是\"\"、null、"null"、空格、开头结尾包含空格类别的"null"：true</returns>
        public static bool IsInvalid(this string? str)
        {
            if (str == null)
                return true;
            var tempStr = str.Trim();
            return tempStr.Length == 0 || tempStr.Equals(("null"));
        }

        /// <summary>
        /// <para>是\"\"、null、"null"、空格、开头结尾包含空格类别的"null"抛出异常</para>
        /// </summary>
        /// <param name="str">The string.</param>
        /// <exception cref="Exception">nameof(str) + 不存在有效值，此方法不能是\"\"、null、\"null\"、空格、开头结尾包含空格的\"null\"，方法调用信息：" + CkStackTrace.GetStackTraceCallInfo()</exception>
        public static void IsInvalidToE(this string str)
        {
            if (str.IsInvalid())
            {
                throw new ArgumentException(nameof(str) + "不存在有效值，字符串不能是\"\"、null、\"null\"、空格、开头结尾包含空格类别的\"null\"", str);
            }
        }

        /// <summary>
        /// <para>是\"\"、null、"null"、空格、开头结尾包含空格类别的"null"抛出异常</para>
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="strings">更多参数</param>
        /// <exception cref="Exception">不存在有效值，此方法不能是\"\"、null、\"null\"、空格、开头结尾包含空格的\"null\"，方法调用信息：" + CkStackTrace.GetStackTraceCallInfo()</exception>
        public static void IsInvalidToE(this string str, params string[]? strings)
        {
            IsInvalidToE(str);

            if (strings != null)
            {
                foreach (var item in strings)
                {
                    IsInvalidToE(item);
                }
            }
        }

        /// <summary>
        /// 判断对象ToString()后是否是有效内容，有效：true
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>有效：true</returns>
        public static bool ExistForToStr(this object? obj)
        {
            return obj != null && obj.ToString().IsValid();
        }

        /// <summary>
        /// 从找到的内容截取字符串到结束
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startStr">要截取的字符串的开始</param>
        /// <param name="isIncludeStartStr">截取的时候是否包含截取的开始字符串，默认：false（不截取）</param>
        /// <returns>返回截取好的字符串，提取没有找到提取内容则为""</returns>
        public static string SubStringToRemain(this string? content, string startStr, bool isIncludeStartStr = false)
        {
            int startInt = 0;
            if (IsInvalid(content) || startStr.Length > content.Length)
            {
                return "";
            }
            if (isIncludeStartStr)
            {
                startInt = content.IndexOf(startStr, StringComparison.Ordinal);
            }
            else
            {
                startInt = content.IndexOf(startStr, StringComparison.Ordinal);
                if (startInt > -1)
                {
                    startInt = startInt + startStr.Length;
                }
            }

            if (startInt < 0)
            {
                return "";
            }
            return content.Substring(startInt);
        }

        /// <summary>
        /// 查找内容中是否包含任意一个需要查找的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="stringComparison">查找时的对比方案</param>
        /// <param name="findStrings">查找的内容</param>
        /// <returns>true：有包含内容</returns>
        public static bool IsFindAnyStr(this string content, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase, params string[]? findStrings)
        {
            bool returnBool = false;
            if (findStrings != null)
            {
                foreach (var item in findStrings)
                {
                    if (item == content || content.IsValid() && item.IsValid() && content.IndexOf(item, stringComparison) > -1)
                    {
                        returnBool = true;
                        break;
                    }
                }
            }
            return returnBool;
        }

        /// <summary>
        /// 查找内容中是否包含任意一个需要查找的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="findStrings">查找的内容</param>
        /// <returns>true：有包含内容</returns>
        public static bool IsFindAnyStr(this string content, params string[]? findStrings)
        {
            return IsFindAnyStr(content, StringComparison.OrdinalIgnoreCase, findStrings);
        }

        /// <summary>
        /// 是否所以传进来的string都不存在有效值
        /// </summary>
        /// <param name="contents">内容</param>
        /// <returns>true：没有任何有效内容存在</returns>
        public static bool IsAllStrNoExist(params string?[] contents)
        {
            foreach (var content in contents)
            {
                if (IsValid(content))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startStr">要截取的字符串的开始</param>
        /// <param name="endString">要截取的字符串的结束</param>
        /// <param name="isIncludeStartStr">截取的时候是否包含截取的开始字符串，默认：false（不截取）</param>
        /// <param name="isIncludeEndStr">截取的时候是否包含截取的结束字符串，默认：false（不截取）</param>
        /// <returns>返回截取好的字符串，提取没有找到提取内容则为""</returns>
        public static string SubString(this string? content, string startStr, string endString, bool isIncludeStartStr = false, bool isIncludeEndStr = false)
        {
            var startInt = 0;
            if (content == null || content.Length < startStr.Length || content.Length < endString.Length)
            {
                return "";
            }
            if (isIncludeStartStr)
            {
                startInt = content.IndexOf(startStr, StringComparison.Ordinal);
            }
            else
            {
                startInt = content.IndexOf(startStr, StringComparison.Ordinal);
                if (startInt > -1)
                {
                    startInt = startInt + startStr.Length;
                }
            }

            if (startInt < 0)
            {
                return "";
            }

            if (startInt < content.Length)
            {
                var endInt = content.IndexOf(endString, startInt + 1, StringComparison.Ordinal);
                if (endInt > -1)
                {
                    var returnStr = content.Substring(startInt, endInt - startInt);
                    if (isIncludeEndStr)
                    {
                        returnStr += endString;
                    }
                    return returnStr;
                }
            }
            return content.Substring(startInt);
        }

        /// <summary>
        /// 移除HTML中的\r\n\t
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string RemoveRNT(this string content)
        {
            return content.Replace("", "\n", "\r", "\t", "  ");
        }

        /// <summary>
        /// 根据分割字符串分割内容，然后将分割的内容添加到list返回
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="splitRegexStr">基于正则的分割，注意换行符此处没有做额外的处理，如使用换行符分割使用[StringToListForNewLine]方法</param>
        /// <param name="isIncludeNullOrEmpty">分割中分割成空内容是否也添加到list</param>
        /// <returns>包含分割好的内容的list</returns>
        public static List<string?> StringToList(this string? content, string splitRegexStr, bool isIncludeNullOrEmpty = false)
        {
            return StringToListForRegex(content, splitRegexStr, isIncludeNullOrEmpty, true);
        }

        /// <summary>
        /// 根据换行符分割内容，然后将分割的内容添加到list返回。优先返回基于“NewLineSymbol()”的换行，其次基于“\n”
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="isIncludeNullOrEmpty">分割中分割成空内容是否也添加到list</param>
        /// <returns>包含分割好的内容的list</returns>
        public static List<string?> StringToListFotNewline(this string? content, bool isIncludeNullOrEmpty = false)
        {
            List<string?> list1 = StringToListForRegex(content, NewLineSymbol(), isIncludeNullOrEmpty, true);
            if (list1.Count > 1)
            {
                return list1;
            }
            else
            {
                List<string?> list2 = StringToListForRegex(content, "\n", isIncludeNullOrEmpty, true);
                return list2.Count > list1.Count ? list2 : list1;
            }
        }

        /// <summary>
        /// 根据分割字符串分割内容，然后将分割的内容添加到list返回
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="splitRegexStr">基于正则的分割，注意换行符此处没有做额外的处理，如使用换行符分割使用[StringToListForNewLine]方法</param>
        /// <param name="isIncludeNullOrEmpty">分割中分割成空内容是否也添加到list</param>
        /// <param name="isEscape">分割的内容是否保持原样，true：保持</param>
        /// <returns>包含分割好的内容的list</returns>
        public static List<string?> StringToListForRegex(this string? content, string splitRegexStr, bool isIncludeNullOrEmpty = false, bool isEscape = false)
        {
            List<string?> list = new List<string?>();

            if (IsValid(content))
            {
                if (isEscape)
                {
                    splitRegexStr = Regex.Escape(splitRegexStr);
                }

                string?[] contents = Regex.Split(content, splitRegexStr);
                foreach (var item in contents)
                {
                    if (isIncludeNullOrEmpty)
                    {
                        list.Add(item);
                    }
                    else
                    {
                        if (IsValid(item))
                        {
                            list.Add(item);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 根据分割字符串分割内容，然后将分割的内容添加到list返回
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="splitStr">基于正则的分割</param>
        /// <param name="isIncludeNullOrEmpty">分割中分割成空内容是否也添加到list</param>
        /// <returns>包含分割好的内容的list</returns>
        public static List<string?> SplitToList(this string? content, string splitStr, bool isIncludeNullOrEmpty = false)
        {
            return StringToListForRegex(content, splitStr, isIncludeNullOrEmpty, true);
        }

        /// <summary>
        /// 根据分割字符串分割内容，然后将分割的内容添加到list返回
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="splitRegexStr">基于正则的分割</param>
        /// <param name="isIncludeNullOrEmpty">分割中分割成空内容是否也添加到list</param>
        /// <param name="isEscape">分割的内容是否保持原样，true：保持</param>
        /// <returns>包含分割好的内容的list</returns>
        public static List<string?> SplitToListForRegex(this string? content, string splitRegexStr, bool isIncludeNullOrEmpty = false, bool isEscape = false)
        {
            return StringToListForRegex(content, splitRegexStr, isIncludeNullOrEmpty, isEscape);
        }

        /// <summary>
        /// 根据换行分割内容，然后将分割的内容添加到list返回
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="isIncludeNullOrEmpty">分割中分割成空内容是否也添加到list</param>
        /// <returns>包含分割好的内容的list</returns>
        public static List<string?> StringToListForNewLine(this string? content, bool isIncludeNullOrEmpty = false)
        {
            var split = "\n";
            if (content.IndexOf("\r\n", StringComparison.Ordinal) > -1)
            {
                split = "\r\n";
            }
            return StringToListForRegex(content, split, isIncludeNullOrEmpty);
        }

        /// <summary>
        /// 通过正则查找内容是否存在
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="regexStr">正则表达式</param>
        /// <returns>true：找到</returns>
        public static bool IsFindStrToRegex(this string content, string regexStr)
        {
            Regex r = new Regex(regexStr);
            var match = r.Match(content);
            return match.Success;
        }

        /// <summary>
        /// 查看开始的字符（串）是否是指定内容中的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="findStrs">指定内容</param>
        /// <returns>True：是指定内容</returns>
        public static bool IsFindFirstStr(this string? content, params string[] findStrs)
        {
            return IsFindStartStr(content, findStrs);
        }

        /// <summary>
        /// 查看开始的字符（串）是否是指定内容中的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="findStrs">指定内容</param>
        /// <returns>True：是指定内容</returns>
        public static bool IsFindStartStr(this string? content, params string[] findStrs)
        {
            return IsFindStartStr(content, StringComparison.Ordinal, findStrs);
        }

        /// <summary>
        /// 查看开始的字符（串）是否是指定内容中的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="stringComparison">对比时的方案</param>
        /// <param name="findStrs">指定内容</param>
        /// <returns>True：是指定内容</returns>
        public static bool IsFindStartStr(this string? content, StringComparison stringComparison = StringComparison.Ordinal, params string[] findStrs)
        {
            bool returnBool = false;
            if (IsValid(content))
            {
                foreach (var findStr in findStrs)
                {
                    if (content.Length - findStr.Length > -1)
                    {
                        returnBool = content.IndexOf(findStr, stringComparison) + findStr.Length == findStr.Length;
                        if (returnBool)
                        {
                            break;
                        }
                    }
                }
            }
            return returnBool;
        }

        /// <summary>
        /// 查看最后的字符（串）是否是指定内容中的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="findStrs">指定内容</param>
        /// <returns>True：是指定内容</returns>
        public static bool IsFindLastStr(this string? content, params string[] findStrs)
        {
            return IsFindLastStr(content, StringComparison.Ordinal, findStrs);
        }

        /// <summary>
        /// 查看最后的字符（串）是否是指定内容中的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="stringComparison">IndexOf时对比方案</param>
        /// <param name="findStrs">指定内容</param>
        /// <returns>True：是指定内容</returns>
        public static bool IsFindLastStr(this string? content, StringComparison stringComparison = StringComparison.Ordinal, params string[] findStrs)
        {
            bool returnBool = false;
            if (IsValid(content))
            {
                foreach (var findStr in findStrs)
                {
                    if (content.Length - findStr.Length > -1)
                    {
                        int lastFindIndex = content.LastIndexOf(findStr, stringComparison);
                        returnBool = lastFindIndex == (content.Length - findStr.Length);
                        if (returnBool)
                        {
                            break;
                        }
                    }
                }
            }
            return returnBool;
        }

        /// <summary>
        /// 移除字符串最后的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="lenght">移除的长度</param>
        /// <returns>移除后的内容</returns>
        public static string RemoveLastStr(this string content, int lenght)
        {
            var returnStr = content.Length > lenght ? content.Remove(content.Length - lenght) : "";
            return returnStr;
        }

        /// <summary>
        /// 如果字符串最后的内容是指定的内容，则移除字符串最后的指定内容（多项内容只匹配第一个找到的）
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="findStrs">指定内容</param>
        /// <returns>移除后的内容</returns>
        public static string? RemoveLastStr(this string? content, params string[] findStrs)
        {
            string? returnStr = content;
            if (IsValid(content))
            {
                foreach (var findStr in findStrs)
                {
                    if (IsFindLastStr(content, findStr))
                    {
                        returnStr = content?.Remove(content.Length - findStr.Length);
                        break;
                    }
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 如果字符串最后的内容是指定的内容，则移除字符串最后的指定内容（多项内容只匹配第一个找到的）
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="findStrs">指定内容</param>
        /// <returns>移除后的内容</returns>
        public static string? RemoveStartStr(this string? content, params string[] findStrs)
        {
            string? returnStr = content;
            if (IsValid(content))
            {
                foreach (var findStr in findStrs)
                {
                    if (IsFindStartStr(content, findStr))
                    {
                        returnStr = content.Substring(findStr.Length);
                        break;
                    }
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 通过String.Replace方式替换指定内容。注意"\r"、"\n"之类的不用加@
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="newStr">被替换的内容</param>
        /// <param name="oldStr">需要替换的内容</param>
        /// <returns>被替换过的新的字符串</returns>
        public static string Replace(this string content, string newStr, params string[] oldStr)
        {
            return oldStr.Aggregate(content, (current, item) => current.Replace(item, newStr));
        }

        /// <summary>
        /// 多个字符串拼接（StringBuilder）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="toStrObjects"></param>
        /// <returns></returns>
        public static string AddContent(this string content, params object[] toStrObjects)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(content);
            foreach (var strObject in toStrObjects)
            {
                stringBuilder.Append(strObject);
            }

            return stringBuilder.ToString();
        }


        /// <summary>
        /// 比较中文字符是否相等
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <param name="isRemoveSpacing">是否移除空格</param>
        /// <returns></returns>
        public static bool ChinesComparisonPare(this string name1, string name2, bool isRemoveSpacing = true)
        {
            if (isRemoveSpacing)
            {
                name1 = name1.Replace(" ", "");//去除掉名字直接的空格
                name2 = name2.Replace(" ", "");//去除掉名字直接的空格
            }
            byte[] utf81 = Encoding.UTF8.GetBytes(name1);
            byte[] utf82 = Encoding.UTF8.GetBytes(name2);
            bool equal = utf81.IsEqual(utf82);
            return equal;
        }

        /// <summary>
        /// 比较2个字节数组是否相等
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dis"></param>
        /// <returns></returns>
        public static bool IsEqual(this byte[] src, byte[] dis)
        {
            bool isEq = false;
            if (src.Length != dis.Length)
            {
                isEq = false;
            }
            else
            {
                isEq = true;
                for (int i = 0; i < src.Length; i++)
                {
                    if (src[i] != dis[i])
                    {
                        isEq = false;
                        break;
                    }
                }
            }
            return isEq;
        }

        /// <summary>
        /// 移除html标签
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="isRemoveAloneSpacing">true：移除单独的空格</param>
        /// <returns></returns>
        public static string RemoveHtmlTag(this string html, bool isRemoveAloneSpacing = false)
        {
            string src = string.Empty, title = string.Empty;
            foreach (Match item in Regex.Matches(html, "alt[\\s\\t\\r\\n]*=[\"']([\\S\\s]*?)[\"']"))
            {
                src += item.Groups[1].Value;
            }
            foreach (Match item in Regex.Matches(html, "title[\\s\\t\\r\\n]*=[\"']([\\S\\s]*?)[\"']"))
            {
                title += item.Groups[1].Value;
            }

            html = Regex.Replace(html, "<nscript[\\s\\S]*?>[\\s\\S]*?</nscript>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, "<style[\\s\\S]*?>[\\s\\S]*?</style>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, "<script[\\s\\S]*?>[\\s\\S]*?</script>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, "<script[\\s\\S]*?>[\\s\\S]*?</script>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html += src + title;
            html = Regex.Replace(html, "", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, "\"", "''", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, "<[^>]+>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (false == isRemoveAloneSpacing)
            {
                return html.Replace("\r\n", "\n").Replace("\n", "").Replace("\t", "").Replace("  ", "");
            }
            else
            {
                return html.Replace("\r\n", "\n").Replace("\n", "").Replace(" ", "").Replace("	", "").Replace("\t", "");
            }
        }

        /// <summary>
        /// 获取字符串中的所以数字
        /// </summary>
        /// <param name="includeNumStr">包含数字的字符串</param>
        /// <returns></returns>
        public static string GetAllNum(this string includeNumStr)
        {
            includeNumStr = Regex.Replace(includeNumStr, @"[^\d]", "");
            return includeNumStr;
        }

        /// <summary>
        /// 提取字符串中的字符串，将数字过滤
        /// </summary>
        /// <param name="content">要提取的字符串</param>
        /// <param name="isSplitDouble">分割时是否小数进行分割，true：基于小数</param>
        /// <returns>只包含字符串的列表</returns>
        public static List<string> StrNumSplitToStr(this string content, bool isSplitDouble = false)
        {
            var matches = Regex.Matches(content, isSplitDouble ? @"(?![\d.\d])([^0-9]+)" : @"(?![\d])([^0-9]+)");
            List<string> list = new List<string>();
            foreach (Match item in matches)
            {
                list.Add(item.Value);
            }
            return list;
        }

        /// <summary>
        /// 获取换行标识符
        /// </summary>
        /// <returns></returns>
        public static string NewLineSymbol()
        {
            return Environment.NewLine;
        }



        /// <summary>
        /// 字符串转byte数组
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string content)
        {
            return Encoding.ASCII.GetBytes(content);
        }

        /// <summary>
        /// 汉字转区位码（区位码：一定是4位，1980年中华人民共和国颁布，字符编码为gb2312）
        /// </summary>
        /// <param name="汉字"></param>
        /// <returns></returns>
        public static string ChineseCharactersCode(this string 汉字)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                var bytes = Encoding.GetEncoding("gb2312").GetBytes(汉字);
                for (int i = 0; i < bytes.Length; i = i + 2)
                {
                    int front = (short)(bytes[i] - '\0');
                    int back = (short)(bytes[i + 1] - '\0');
                    stringBuilder.Append((front - 160).ToString() + (back - 160));
                }
            }
            catch
            {
                throw new ArgumentException("参数内容不对，只能是汉字");
            }
            string 区位码 = stringBuilder.ToString();
            return 区位码;
        }

        /// <summary>
        /// 翻转字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Reverse(this string str)
        {
            var chars = str.ToCharArray();
            Array.Reverse(chars);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(chars).ToString();
            return stringBuilder.ToString();
        }
    }
}