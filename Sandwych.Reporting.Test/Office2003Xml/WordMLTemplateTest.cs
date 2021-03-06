using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Schema;

using NUnit.Framework;

using Sandwych.Reporting.Test;

namespace Sandwych.Reporting.OfficeXml
{
    /// <summary>
    /// "Word 2003 XML 格式模板的测试
    /// </summary>
    [TestFixture]
    [Category("MSOffice2003/Word")]
    public sealed class WordMLTemplateTest
    {

        [Test]
        public void TestReferenceReplacement()
        {
            var ctx = new Dictionary<string, object>() {
                {"var1", "_HELLO_" },
                {"var2", "_WORLD_" },
            };

            var result = TemplateTestHelper.RenderTemplate<WordMLTemplate>(
                TemplateTestHelper.GetTestResourceAbsolutleFilePath(@"resources/word2003xml_docs/template_reference_replacement.doc"), ctx);

            var xmldoc = TemplateTestHelper.GetlXmlDocument((WordMLTemplate)result);
            xmldoc.ShouldBeWellFormedWordML();

            var body = xmldoc.GetElementsByTagName("w:body")[0];
            var bodyText = body.InnerText.Trim();

            Assert.AreEqual("TEST_HELLO_REFERENCE_WORLD_REPLACEMENT", bodyText);
        }


        [Test]
        public void Url_placeholder_in_Word2003_should_be_ok()
        {
            var ctx = new Dictionary<string, object>()
            {
            };

            var result = TemplateTestHelper.RenderTemplate<WordMLTemplate>(
                TemplateTestHelper.GetTestResourceAbsolutleFilePath(@"resources/word2003xml_docs/template_escape_url.doc"), ctx);

            var xmldoc = TemplateTestHelper.GetlXmlDocument((WordMLTemplate)result);
            xmldoc.ShouldBeWellFormedWordML();

            var body = xmldoc.GetElementsByTagName("w:body")[0];
            var bodyText = body.InnerText.Trim();

            Assert.AreEqual("革命", bodyText);
        }

        [Test]
        public void TestSimpleRowLoop()
        {
            var ctx = new Dictionary<string, object>()
            {
                {"chars", new char[] {'A', 'B', 'C', 'D', 'E', 'F'} },
            };

            var result = TemplateTestHelper.RenderTemplate<WordMLTemplate>(
                TemplateTestHelper.GetTestResourceAbsolutleFilePath(@"resources/word2003xml_docs/template_row_loop.doc"), ctx);

            var xmldoc = TemplateTestHelper.GetlXmlDocument((WordMLTemplate)result);
            xmldoc.ShouldBeWellFormedWordML();

            var rows = xmldoc.GetElementsByTagName("w:tr");

            Assert.AreEqual(6, rows.Count);
            var row0 = rows[0].InnerText;
            var row5 = rows[5].InnerText;

            Assert.AreEqual("AAAAA", row0);
            Assert.AreEqual("FFFFF", row5);
        }

    }
}
