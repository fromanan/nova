using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NovaCore.Library.Files;

namespace NovaCore.Web.Library.Extensions
{
    public static class AgilityExtensions
    {
        public static bool HasId(this HtmlNode node, params string[] ids)
        {
            return ids.Any(id => node.Id == id);
        }
        
        public static bool NodeMatch(this HtmlNode node, params string[] nodes)
        {
            return nodes.Any(n => n == node.Name);
        }
        
        public static bool ClassMatch(this HtmlNode node, string[] classes)
        {
            return classes.Any(node.HasClass);
        }
        
        public static bool MultiClassMatch(this HtmlNode node, params string[] classes)
        {
            return classes.All(node.HasClass);
        }

        public static bool ClassPatternMatch(this HtmlNode node, params string[][] classPatternGroups)
        {
            return classPatternGroups.Any(node.MultiClassMatch);
        }

        public static void PrintBody(this HtmlDocument htmlDoc)
        {
            Console.WriteLine(htmlDoc.GetBody().OuterHtml);
        }

        public static HtmlNode GetBody(this HtmlDocument htmlDocument)
        {
            return htmlDocument.DocumentNode.SelectSingleNode("//body");
        }
        
        public static HtmlDocument Strip(this HtmlDocument htmlDoc, string[] nodesToRemove = null, 
            string[] idsToRemove = null, string[] classesToRemove = null, params string[][] classPatternGroups)
        {
            htmlDoc.DocumentNode.Descendants()
                .Where(n => n.HasId("header") || n.NodeMatch(nodesToRemove) ||
                            n.ClassMatch(classesToRemove) || n.ClassPatternMatch(classPatternGroups)
                )
                .ToList()
                .ForEach(n => n.Remove());

            return htmlDoc;
        }

        public static IEnumerable<HtmlNode> GetNodes(this HtmlDocument htmlDoc, params string[] nodes)
        {
            return htmlDoc.DocumentNode.Descendants().Where(n => n.NodeMatch(nodes));
        }

        public static void SaveToFile(this HtmlDocument htmlDoc, string filename, params string[] folderHierarchy)
        {
            FileSystem.SaveToFile(htmlDoc.DocumentNode.OuterHtml, filename, folderHierarchy);
        }

        public static string ToString(this HtmlDocument htmlDoc)
        {
            return htmlDoc.DocumentNode.OuterHtml;
        }
    }
}