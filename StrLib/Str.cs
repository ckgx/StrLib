using System.Collections;
using System.Reflection;
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
        public static void IsInvalidToE(this string str, params string[] strings)
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
        public static bool ExistForToStr(this object obj)
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
        /// 是否所有传进来的对象中的string属性都不存在有效值
        /// </summary>
        /// <param name="contentObject">对象</param>
        /// <returns>true：不存在有效值或对象为null</returns>
        public static bool IsAllStrNoExistForObject(this Object contentObject)
        {
            if (null == contentObject)
            {
                return true;
            }
            foreach (PropertyInfo propertyInfo in contentObject.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType.FullName != null && propertyInfo.PropertyType.FullName.Equals("System.String"))
                {
                    string? content = Convert.ToString(propertyInfo.GetValue(contentObject, null));
                    if (IsValid(content))
                    {
                        return false;
                    }
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
            int startInt = 0;
            if (IsInvalid(content) || content.Length < startStr.Length || content.Length < endString.Length)
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
                int endInt = content.IndexOf(endString, startInt + 1, StringComparison.Ordinal);
                if (endInt > -1)
                {
                    string returnStr = content.Substring(startInt, endInt - startInt);
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
        /// 提取关键字，查找到符合“finds”定义的第一组内容就返回找到的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="keys">关键字数组</param>
        /// <param name="finds">二维数组，每个元素的第一个和第二个表示“keys”的两个下标，分别代表查找内容的开始和结束（小于0为：空内容（""））</param>
        /// <param name="isIncludeStartStr">提取的时候是否包含截取的开始字符串，默认：false（不截取）</param>
        /// <param name="isIncludeEndStr">提取的时候是否包含截取的结束字符串，默认：false（不截取）</param>
        /// <returns>提取的内容，出错或提取没有找到提取内容则为""</returns>
        public static string SubStringToExtractKey(this string? content, string[] keys, int[][] finds, bool isIncludeStartStr = false, bool isIncludeEndStr = false)
        {
            string returnStr = "";
            foreach (var find in finds)
            {
                string start = "";
                if (find[0] > -1)
                {
                    start = keys[find[0]];
                }
                string end = "";
                if (find[1] > -1)
                {
                    end = keys[find[1]];
                }
                if (content.IsFindStr(new[] { start, end }, true))
                {
                    returnStr = SubString(content, start, end, isIncludeStartStr, isIncludeEndStr);
                }
                if (returnStr.IsValid())
                {
                    break;
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 根据正则表达式截取字符串（参数支持正则表达式）
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startPattern">要截取的字符串的开始（支持正则表达式）</param>
        /// <param name="contentPattern">内容正则表达式，注意：要和startStr、endStr组成一个正确的正则表达式</param>
        /// <param name="isIncludeStartStr">截取的时候是否包含截取的开始字符串，默认：false（只返回contentPattern对应的内容）</param>
        /// <returns>提取的内容，提取没有找到提取内容则为""</returns>
        public static string SubStringForAllRegex(this string content, string startPattern, string contentPattern, bool isIncludeStartStr = false)
        {
            string pattern = @startPattern + @contentPattern;
            Regex regex = new Regex(pattern); //定义一个Regex对象实例
            string findStr = regex.Match(content).Value;
            if (false == isIncludeStartStr)
            {
                regex = new Regex(contentPattern + "$");
                findStr = regex.Match(findStr).Value;
            }
            return findStr;
        }

        /// <summary>
        /// 根据正则表达式截取字符串（仅需要提取的内容部分支持正则表达式）
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startStr">要截取的字符串的开始（注意：不支持正则表达式）</param>
        /// <param name="contentPattern">内容正则表达式，注意：要和startStr组成一个正确的正则表达式，但是startStr不能是表达式</param>
        /// <returns>提取的内容，出错内容则为null，提取没有找到提取内容则为""</returns>
        public static string? SubStringForRegex(this string content, string startStr, string contentPattern)
        {
            return SubStringForRegex(content, startStr, contentPattern, "", false);
        }

        /// <summary>
        /// 根据正则表达式截取字符串（仅需要提取的内容部分支持正则表达式）
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startStr">要截取的字符串的开始（注意：不支持正则表达式）</param>
        /// <param name="contentPattern">内容正则表达式，注意：要和startStr组成一个正确的正则表达式，但是startStr不能是表达式</param>
        /// <param name="isIncludeStartStr">截取的时候是否包含截取的开始字符串，默认：false（只返回contentPattern对应的内容）</param>
        /// <returns>提取的内容，出错内容则为null，提取没有找到提取内容则为""</returns>
        public static string? SubStringForRegex(this string content, string startStr, string contentPattern, bool isIncludeStartStr = false)
        {
            return SubStringForRegex(content, startStr, contentPattern, "", isIncludeStartStr);
        }

        /// <summary>
        /// 根据正则表达式截取字符串（仅需要提取的内容部分支持正则表达式）
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startStr">要截取的字符串的开始（注意：不支持正则表达式）</param>
        /// <param name="contentPattern">内容正则表达式，注意：要和startStr、endStr组成一个正确的正则表达式，但是startStr、endStr不能是表达式</param>
        /// <param name="endStr">要截取的字符串的结束，注意末尾不能为:\</param>
        /// <param name="isIncludeStartStr">截取的时候是否包含截取的开始字符串，默认：false（只返回contentPattern对应的内容）</param>
        /// <returns>提取的内容，出错内容则为null，提取没有找到提取内容则为""</returns>
        public static string? SubStringForRegex(this string content, string startStr, string contentPattern, string endStr = "", bool isIncludeStartStr = false)
        {
            if (IsFindLastStr(endStr, @"\"))
            {
                return null;
            }
            string pattern = Regex.Escape(startStr) + @contentPattern + @endStr;
            Regex regex = new Regex(pattern, RegexOptions.CultureInvariant);
            string? findStr = regex.Match(content).Value;
            //string findStr = Regex.Match(contentPattern, Regex.Escape(content)).Value;
            if (IsValid(findStr))
            {
                if (isIncludeStartStr)
                {
                    return findStr;
                }
                string? returnStr = findStr.Substring(startStr.Length, findStr.Length - startStr.Length - endStr.Length);
                return returnStr;
            }
            return findStr;
            //return Exist(findStr) ? isIncludeStartStr ? findStr : findStr.Substring(startStr.Length, findStr.Length - startStr.Length - endStr.Length) : findStr;
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
        /// 根据正则表达式提取指定内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startStr">要截取的字符串的开始</param>
        /// <param name="contentPattern">内容正则表达式，注意：要和startStr、endStr组成一个正确的正则表达式，但是startStr、endStr不能是表达式</param>
        /// <param name="endStr">要截取的字符串的结束，注意末尾不能为:\</param>
        /// <param name="isIncludeStartStr">截取的时候是否包含截取的开始字符串，默认：false（不截取）</param>
        /// <returns>包含提取的内容的List，出错或提取没有找到提取内容则为null</returns>
        public static List<string> GetAllSubStringForRegex(this string content, string startStr, string contentPattern, string? endStr = "", bool isIncludeStartStr = false)
        {
            List<string> returnList = new List<string>();
            //List<int> insexList = new List<int>();
            if (IsFindLastStr(endStr, @"\"))
            {
                return null;
            }
            string pattern = @startStr + @contentPattern + @endStr;
            Regex regex = new Regex(pattern); //定义一个Regex对象实例
            MatchCollection mc = regex.Matches(content);
            for (int i = 0; i < mc.Count; i++) //在输入字符串中找到所有匹配
            {
                //根据isIncludeStartStr判断是否只提取内容字符串，默认只提取
                returnList.Add(isIncludeStartStr ? mc[i].Value : mc[i].Value.Substring(startStr.Length, mc[i].Value.Length - startStr.Length - endStr.Length));
                //insexList.Add(mc[i].Index); //记录匹配字符的位置
            }
            return returnList.Count == 0 ? null : returnList;
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
        /// 查找内容中是否包含指定内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="findStr">要查找的字符串</param>
        /// <param name="findAllIsTrue">是否找到全部内容才为True</param>
        /// <param name="comparisonType">StringComparison，默认：StringComparison.Ordinal</param>
        /// <returns>True：找到内容</returns>
        public static bool IsFindStr(this string? content, string[] findStr, bool findAllIsTrue = true, StringComparison comparisonType = StringComparison.Ordinal)
        {
            bool returnBool = false;
            if (findStr.Length == 0)
            {
                returnBool = false;
            }
            else
            {
                foreach (var item in findStr)
                {
                    if (false == IsFindStr(content, item))
                    {
                        if (findAllIsTrue)
                        {
                            returnBool = false;
                            break;
                        }
                    }
                    else
                    {
                        returnBool = true;
                    }
                }
            }
            return returnBool;
        }

        /// <summary>
        /// 查找内容中是否包含指定内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="findStr">要查找的字符串</param>
        /// <param name="comparisonType">StringComparison，默认：StringComparison.Ordinal</param>
        /// <returns>True：找到内容</returns>
        public static bool IsFindStr(this string? content, string findStr, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (IsInvalid(content))
            {
                return false;
            }
            return content.LastIndexOf(findStr, comparisonType) > -1;
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
        /// 根据正则表达式截取两个字符串中间的字符串
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startStr">开始查找部分</param>
        /// <param name="endRegexStr">结束查找部分</param>
        /// <param name="isEscapeStart">是否自动转义，true：保持“startStr”的原样（自动转义内容）</param>
        /// <param name="isEscapeContent">是否自动转义，true：保持“content”的原样（自动转义内容，注意不是手动输入的内容.net会自动转义）</param>
        /// <returns></returns>
        public static string GetCentreStrForRegexToEnd(this string content, string? startStr, string? endRegexStr, bool isEscapeStart = true, bool isEscapeContent = false)
        {
            return GetCentreStrForRegex(content, startStr, endRegexStr, isEscapeStart, false, isEscapeContent);
        }

        /// <summary>
        /// 根据正则表达式截取两个字符串中间的字符串
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startStr">开始查找部分</param>
        /// <param name="endStr">结束查找部分</param>
        /// <param name="isEscapeStart">是否自动转义，true：保持“startStr”的原样（自动转义内容，注意：转义后正则失效）</param>
        /// <param name="isEscapeEnd">是否自动转义，true：保持“endStr”的原样（自动转义内容，注意：转义后正则失效）</param>
        /// <param name="isEscapeContent">是否自动转义，true：保持“content”的原样（自动转义内容，注意：1【不是手动输入的内容.net会自动转义】；2【转义后正则失效】）</param>
        /// <returns></returns>
        public static string GetCentreStrForRegex(this string content, string? startStr, string? endStr, bool isEscapeStart = true, bool isEscapeEnd = true, bool isEscapeContent = false)
        {
            string value = content;
            if (isEscapeContent)
            {
                content = Regex.Escape(content);
            }
            if (isEscapeStart)
            {
                startStr = Regex.Escape(startStr);
            }
            if (isEscapeEnd)
            {
                endStr = Regex.Escape(endStr);
            }
            Regex regex = null;
            if (IsValid(startStr) && IsValid(endStr))
            {
                regex = new Regex("(?<=(" + startStr + "))[.\\s\\S]*?(?=(" + endStr + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            }
            else if (IsValid(startStr) && IsInvalid(endStr))
            {
                regex = new Regex("(?<=(" + startStr + "))[.\\startStr\\S]*", RegexOptions.Multiline | RegexOptions.Singleline);
            }
            else if (IsInvalid(startStr) && IsValid(endStr))
            {
                regex = new Regex("[.\\endStr\\S]*(?=(" + endStr + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            }

            if (null != regex)
            {
                var march = regex.Match(content);
                if (march.Success)
                {
                    value = march.Value;
                }
                else
                {
                    throw new Exception("正则表达式异常");
                }
            }
            return value;
        }


        /// <summary>
        /// 用正则表达式替换字符串中的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="regexStr">要替换内容的正则表达式</param>
        /// <param name="replaceStr">
        /// <para>被替换的内容</para>
        /// <para>注意：</para>
        /// <para>支持表达式，但是需要注意测试，如：</para>
        /// <para>$1,$2这样的替换就有问题，遇到这样需要替换内容的,请使用命名方式代替$1、$2，示例代码;<see cref="Mask"/></para>
        /// </param>
        /// <returns>被替换过的字符串</returns>
        public static string ReplaceForRegex(this string content, string regexStr, string replaceStr)
        {
            return Regex.Replace(content, regexStr, replaceStr);
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
        /// 将多条内容拼接成一个字符串
        /// </summary>
        /// <param name="splitStr">以什么进行分割</param>
        /// <param name="contents">可以ToString的内容</param>
        /// <returns>拼接后的字符串</returns>
        public static string ContentsToOneStr(string splitStr, params object[] contents)
        {
            return ObjectsToOneStr(splitStr, contents, false);
        }

        /// <summary>
        /// 将多条内容拼接成一个字符串
        /// </summary>
        /// <param name="splitStr">以什么进行分割</param>
        /// <param name="contents">可以ToString的内容</param>
        /// <param name="isIncludeLastSplitStr">字符串的最后是否包含用来分割的字符串</param>
        /// <returns>拼接后的字符串</returns>
        public static string ContentsToOneStr(string splitStr, object[] contents, bool isIncludeLastSplitStr)
        {
            return ObjectsToOneStr(splitStr, contents, isIncludeLastSplitStr);
        }

        /// <summary>
        /// 将多条内容拼接成一个字符串
        /// </summary>
        /// <param name="splitStr">以什么进行分割</param>
        /// <param name="contents">可以ToString的内容</param>
        /// <param name="isIncludeLastSplitStr">字符串的最后是否包含用来分割的字符串</param>
        /// <param name="isTrim">是否对内容.Trim()。True：执行</param>
        /// <returns>拼接后的字符串</returns>
        public static string ContentsToOneStr(string splitStr, object[] contents, bool isIncludeLastSplitStr, bool isTrim)
        {
            return ContentsToOneStr(splitStr, contents, "", "", isIncludeLastSplitStr, isTrim);
        }

        /// <summary>
        /// 将多条内容拼接成一个字符串
        /// </summary>
        /// <param name="splitStr">以什么进行分割</param>
        /// <param name="contents">可以ToString的内容</param>
        /// <param name="isIncludeLastSplitStr">字符串的最后是否包含用来分割的字符串</param>
        /// <param name="isTrim">是否对内容.Trim()。True：执行</param>
        /// <returns>拼接后的字符串</returns>
        public static string ObjectsToOneStr(string splitStr, object[] contents, bool isIncludeLastSplitStr = false, bool isTrim = true)
        {
            return ContentsToOneStr(splitStr, contents, "", "", isIncludeLastSplitStr, isTrim);
        }

        /// <summary>
        /// 将多条内容拼接成一个字符串
        /// </summary>
        /// <param name="splitStr">以什么进行分割</param>
        /// <param name="contents">可以ToString的内容</param>
        /// <param name="addStartContent">对分割后的内容添加开头的内容</param>
        /// <param name="addEndContent">对分割后的内容添加结尾的内容</param>
        /// <param name="isIncludeLastSplitStr">字符串的最后是否包含用来分割的字符串</param>
        /// <param name="isTrim">是否对内容.Trim()。True：执行</param>
        /// <returns>拼接后的字符串</returns>
        public static string ContentsToOneStr(string splitStr, object[] contents, string addStartContent, string addEndContent, bool isIncludeLastSplitStr = false, bool isTrim = true)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < contents.Length; i++)
            {
                stringBuilder.Append(addStartContent);
                stringBuilder.Append(isTrim ? contents[i].ToString().Trim() : contents[i].ToString());
                stringBuilder.Append(addEndContent);
                if (isIncludeLastSplitStr)
                {
                    stringBuilder.Append(splitStr);
                }
                else
                {
                    if (contents.Length - 1 > i)
                    {
                        stringBuilder.Append(splitStr);
                    }
                }
            }

            return stringBuilder.ToString();
        }



        /// <summary>
        /// 用此函数添加换行，能保证每个内容有一个换行。末尾不添加换行
        /// </summary>
        /// <param name="isNoContentToNoAdd">遇到没有内容的情况不添加换行。true：不添加换行</param>
        /// <param name="extraNewlineToNum">每几个内容额外添加个换行</param>
        /// <param name="contents">内容</param>
        /// <returns>新的内容</returns>
        public static string AddNewline(bool isNoContentToNoAdd = true, int extraNewlineToNum = 0, params object[] contents)
        {
            return AddNewline(isNoContentToNoAdd, true, false, extraNewlineToNum, contents);
        }

        /// <summary>
        /// 用此函数添加换行，能保证每个内容有一个换行。末尾不添加换行，空内容不处理
        /// </summary>
        /// <param name="extraNewlineToNum">每几个内容额外添加个换行</param>
        /// <param name="contents">内容</param>
        /// <returns>新的内容</returns>
        public static string AddNewline(int extraNewlineToNum = 0, params object[] contents)
        {
            return AddNewline(true, true, false, extraNewlineToNum, contents);
        }

        /// <summary>
        /// 用此函数添加换行，能保证每个内容有一个换行。末尾不添加换行，空内容不处理
        /// </summary>
        /// <param name="contents">内容</param>
        /// <returns>新的内容</returns>
        public static string AddNewline(params object[] contents)
        {
            return AddNewline(true, true, false, 0, contents);
        }

        /// <summary>
        /// 用此函数添加换行，能保证每个内容有一个换行。末尾不添加换行，空内容不处理
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contents">内容</param>
        /// <returns>新的内容</returns>
        public static string AddNewline(this string content, params object[] contents)
        {
            return AddNewline(true, true, false, 0, content, contents);
        }

        /// <summary>
        /// 用此函数添加换行，能保证每个内容有一个换行。末尾添加换行，空内容不处理
        /// </summary>
        /// <param name="contents">内容</param>
        /// <returns>新的内容</returns>
        public static string AddNewlines(params object[] contents)
        {
            return AddNewline(true, true, true, 0, contents);
        }

        /// <summary>
        /// 用此函数添加换行，能保证每个内容有一个换行。末尾添加换行，空内容不处理
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contents">内容</param>
        /// <returns>新的内容</returns>
        public static string AddNewlines(this string content, params object[] contents)
        {
            return AddNewline(true, true, true, 0, content, contents);
        }

        /*
        /// <summary>
        /// 每个内容有且只有一个换行
        /// </summary>
        /// <param name="isNoContentToNoAdd">遇到没有内容的情况不添加换行。true：不添加换行</param>
        /// <param name="extraNewlineToNum">每几个内容额外添加个换行</param>
        /// <param name="contents">内容</param>
        /// <returns>新的内容</returns>
        public static string AddNewlineToContent(bool isNoContentToNoAdd = true, int extraNewlineToNum = 0, params object[] contents)
        {
            return AddNewline(isNoContentToNoAdd, true, false, extraNewlineToNum, contents);
        }

        /// <summary>
        /// 为每个内容添加换行，空内容不添加换行
        /// </summary>
        /// <param name="contents">内容</param>
        /// <returns>新的内容</returns>
        public static string AddNewlineToContent(params object[] contents)
        {
            return AddNewlineToContent(true, 0, contents);
        }

        */

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
        /// 获取经过base64编码的内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToBase64(this string content, Encoding encoding = null)
        {
            if (null == encoding)
            {
                encoding = Encoding.UTF8;
            }
            byte[] bytes = encoding.GetBytes(content);
            //这里要还需要加上其他的一些格式
            return $"=?{encoding.HeaderName}?B?{Convert.ToBase64String(bytes)}?=";
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
        /// 提取字符串中的数字和字符内容
        /// </summary>
        /// <param name="content">要提取的字符串</param>
        /// <param name="isSplitDouble">分割时是否小数进行分割，true：基于小数</param>
        /// <param name="stringComparison">StringComparison规则</param>
        /// <returns>按照顺序提取出来的列表</returns>
        public static List<string> StrNumSplit(this string content, bool isSplitDouble = false, StringComparison stringComparison = StringComparison.Ordinal)
        {
            MatchCollection matches;
            if (isSplitDouble)
            {
                matches = Regex.Matches(content, @"(?![\d.\d])([^0-9]+)");
            }
            else
            {
                matches = Regex.Matches(content, @"(?![\d])([^0-9]+)");
            }

            return StrNumSplitHandle(content, matches, stringComparison);
        }

        /// <summary>
        /// 提取字符串中的数字和字符内容的处理函数
        /// </summary>
        /// <param name="content">要提取的字符串</param>
        /// <param name="matches">通过正则提取的内容</param>
        /// <param name="stringComparison">StringComparison规则</param>
        /// <returns></returns>
        public static List<string> StrNumSplitHandle(this string content, MatchCollection matches, StringComparison stringComparison = StringComparison.Ordinal)
        {
            int indexOf = 0;
            List<string> list = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                int tempOf = content.IndexOf(matches[i].Value, indexOf, stringComparison);
                list.Add(content.Substring(indexOf, tempOf - indexOf));
                list.Add(content.Substring(tempOf, matches[i].Value.Length));
                indexOf = tempOf + matches[i].Value.Length;

                if (i + 1 == matches.Count)
                {
                    list.Add(content.Substring(indexOf, content.Length - indexOf));
                }
            }

            return list;
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
        /// 提取字符串中的字符串，将数字过滤
        /// </summary>
        /// <param name="content">要提取的字符串</param>
        /// <param name="isEscapeContent">是否自动转义，true：保持“content”的原样（自动转义内容，注意：1【不是手动输入的内容.net会自动转义】；2【转义后正则失效】）</param>
        /// <returns>只包含字符串的列表或null</returns>
        public static List<string> SplitSpecialCharacter(this string? content, bool isEscapeContent = true)
        {
            if (isEscapeContent)
            {
                content = Regex.Escape(content);
            }
            var matches = Regex.Matches(content, SpecialCharacters);
            List<string> list = new List<string>();
            foreach (Match item in matches)
            {
                list.Add(item.Value);
            }

            if (list.Count == 0)
            {
                return null;
            }
            return list;
        }

        /// <summary>
        /// 移除字符串边缘的特殊字符
        /// </summary>
        /// <param name="content"></param>
        /// <param name="start">true：移除开头的特殊字符</param>
        /// <param name="end">true：移除结尾的特殊字符</param>
        /// <param name="stringComparison">IndexOf时对比方案</param>
        /// <returns></returns>
        public static string? RemoveSpecialCharacterToBorder(this string? content, bool start = true, bool end = true, StringComparison stringComparison = StringComparison.Ordinal)
        {
            var list = SplitSpecialCharacter(content);
            if (list != null)
            {
                if (start)
                {
                    if (content.IsFindStartStr(stringComparison, list[0]))
                    {
                        content = content.RemoveStartStr(list[0]);
                    }
                }
                if (end)
                {
                    if (content.IsFindLastStr(stringComparison, list[list.Count - 1]))
                    {
                        content = content.RemoveLastStr(list[list.Count - 1]);
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// 将对象转换为 String 然后不足位数填充指定内容
        /// </summary>
        /// <param name="obj">将要转换为字符串的对象</param>
        /// <param name="totalWidth">不达到什么长度会被填充</param>
        /// <param name="paddingChar">填充的字符</param>
        /// <returns></returns>
        public static string PadLeft(this object obj, int totalWidth, char paddingChar)
        {
            return obj.ToString().PadLeft(totalWidth, paddingChar);
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
        /// 16进制字符串转字符数组
        /// </summary>
        /// <param name="hexStr">16进制表示的 byte[] 的字符串，可用ToHexStr()方法获取</param>
        /// <returns></returns>
        public static byte[] HexStrToBytes(this string hexStr)
        {
            if (hexStr == null || hexStr.Equals(""))
            {
                return null;
            }
            var inputByteArray = new byte[hexStr.Length / 2];
            for (var x = 0; x < inputByteArray.Length; x++)
            {
                var i = Convert.ToInt32(hexStr.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            return inputByteArray;
        }


        /// <summary>
        /// 在<paramref name="content"/>中是否包含列表项的某个值
        /// </summary>
        /// <param name="content">主体内容</param>
        /// <param name="iEnumerable">The i enumerable.</param>
        /// <param name="stringComparison">比较规则</param>
        /// <returns>
        ///   true：找到内容
        /// </returns>
        public static bool StrFindItem(this string content, IEnumerable<string> iEnumerable, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return StrFindItem(content, iEnumerable, out string findStr, stringComparison);
        }

        /// <summary>
        /// 在<paramref name="content"/>中是否包含列表项的某个值
        /// </summary>
        /// <param name="content">主体内容</param>
        /// <param name="iEnumerable">The i enumerable.</param>
        /// <param name="findStr">找到的内容</param>
        /// <param name="stringComparison">比较规则</param>
        /// <returns>
        ///   true：找到内容
        /// </returns>
        public static bool StrFindItem(this string content, IEnumerable<string> iEnumerable, out string findStr, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            if (iEnumerable != null)
            {
                foreach (string item in iEnumerable)
                {
                    if (content.IndexOf(item, stringComparison) > -1)
                    {
                        findStr = item;
                        return true;
                    }
                }
            }

            findStr = "";
            return false;
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
