using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Text.Json;

namespace KattisSearch
{
    public class Kattis
    {
        private const int INITIAL_LIST_CAPACITY = 2500;
        private static List<Problem> problems = new List<Problem>(INITIAL_LIST_CAPACITY);
        private static HtmlWeb web = new HtmlWeb();

        private static string dataJsonFileName = "data.json";

        private Kattis() { }

        private static string GetUrl(int page) => $"https://open.kattis.com/problems?page={page}&order=name";

        private static string GetHref(HtmlNode problem) => problem.FirstChild.GetAttributeValue("href", string.Empty);

        private static string GetProblemUrl(HtmlNode problem) => "https://open.kattis.com" + GetHref(problem);

        private static string GetName(HtmlNode problem) => WebUtility.HtmlDecode(problem.InnerText.Trim());

        private static string GetId(HtmlNode problem)
        {
            string href = GetHref(problem);
            string id = href.Substring(href.LastIndexOf("/", StringComparison.Ordinal) + 1);
            return id;
        }

        private static bool HasNextButton(HtmlDocument Doc)
        {
            HtmlNode nextButton = Doc.DocumentNode.SelectSingleNode("//*[@id=\"problem_list_next\"]");
            if (nextButton != null)
            {
                string classValue = nextButton.GetAttributeValue("class", string.Empty);
                if (string.IsNullOrEmpty(classValue))
                    return false;
                return classValue.Equals("enabled");
            }
            return false;
        }

        private static string GetText(HtmlNode problem)
        {
            string problemUrl = GetProblemUrl(problem);
            HtmlDocument doc = web.Load(problemUrl);
            var node = doc.DocumentNode.SelectSingleNode("//*[@id=\"wrapper\"]/div/div[1]/div/section/div/div[2]");
            string body = WebUtility.HtmlDecode(node.InnerText.Trim());
            string text = Regex.Replace(body, @"\s+", " ");
            return text;
        }

        private static string GetDifficulty(HtmlNode problem)
        {
            var parent = problem.ParentNode;
            return parent.ChildNodes[17].InnerText.Trim();
        }

        public static bool FileExists()
        {
            return File.Exists(dataJsonFileName);
        }

        public static void Generate()
        {
            int page = 0;
            bool done = false;
            while (!done)
            {
                done = true;
                string url = GetUrl(page);
                HtmlDocument doc = web.Load(url);
                var collection = doc.DocumentNode.SelectNodes("//*[@class=\"name_column\"]");
                if (collection != null)
                {
                    foreach (HtmlNode problem in collection)
                    {
                        string name = GetName(problem);
                        string id = GetId(problem);
                        string diff = GetDifficulty(problem);
                        string text = GetText(problem);
                        problems.Add(new Problem(name, id, diff, text));
                    }
                    if (HasNextButton(doc))
                    {
                        page++;
                        done = false;
                    }
                }
            }
        }

        public static void CreateJson()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(problems, options);
            File.WriteAllText(dataJsonFileName, jsonString);
        }

        public static void LoadJson()
        {
            string jsonString = File.ReadAllText(dataJsonFileName);
            problems = JsonSerializer.Deserialize<List<Problem>>(jsonString);
        }

        public static void Search()
        {
            string rawSearch = File.ReadAllText("search.txt").Trim();
            if (rawSearch.Length == 0)
                throw new Exception("You cannot have an empty search.");
            string fixedSearch = Regex.Replace(rawSearch, @"\s+", " ");
            using (StreamWriter file = new StreamWriter("results.txt", false, UnicodeEncoding.Default))
            {
                foreach (Problem p in problems)
                {
                    if (p.Text.Contains(fixedSearch, StringComparison.OrdinalIgnoreCase))
                        file.WriteLine(p);
                }
            }
        }
    }
}