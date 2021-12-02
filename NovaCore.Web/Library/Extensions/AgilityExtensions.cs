using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NovaCore.Files;

namespace NovaCore.Web.Extensions
{
    public static class AgilityExtensions
    {
        public static bool HasId(this HtmlNode node, params string[] ids)
        {
            return ids?.Any(id => node.Id == id) ?? false;
        }
        
        public static bool NodeMatch(this HtmlNode node, params string[] nodes)
        {
            return nodes?.Any(n => n == node.Name) ?? false;
        }
        
        public static bool ClassMatch(this HtmlNode node, params string[] classes)
        {
            return classes?.Any(node.HasClass) ?? false;
        }
        
        public static bool MultiClassMatch(this HtmlNode node, params string[] classes)
        {
            return classes?.All(node.HasClass) ?? false;
        }

        public static bool ClassPatternMatch(this HtmlNode node, params string[][] classPatternGroups)
        {
            return classPatternGroups?.Any(node.MultiClassMatch) ?? false;
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

        public static HtmlNode Filter(this HtmlNode node, params string[] nodesToRemove)
        {
            node.Descendants().Where(n => nodesToRemove.Any(id => n.HasId(id))).ToList().ForEach(n => n.Remove());
            return node;
        }
    }
}