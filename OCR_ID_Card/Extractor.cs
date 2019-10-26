using OCR_ID_Card.Models;
using Superpower;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Tesseract;

namespace OCR_ID_Card
{
    public class Extractor
    {
        public static List<ExtractedField> ExtractFields(string text, string[] fields, double tolerance)
        {
            var extractedFields = new List<ExtractedField>();
            var extractions = Extract(text);

            foreach (var field in fields)
            {
                var fieldVariants = field.Split(';');

                var extractedField = new ExtractedField()
                {
                    Title = fieldVariants[0]
                };
                foreach (var fieldVariant in fieldVariants)
                {
                    foreach (var word in extractions)
                    {
                        var index = extractions.IndexOf(word);
                        extractedField.Values.AddRange(ExtractFieldValue(extractions, index, fieldVariant, tolerance));
                        var inlineWords = word.Split(' ').ToList();
                        foreach (var inlineWord in inlineWords)
                        {
                            var inlineIndex = inlineWords.IndexOf(inlineWord);
                            extractedField.Values.AddRange(ExtractFieldValue(inlineWords, inlineIndex, fieldVariant, tolerance));
                        }
                    }
                    extractedField.Values.OrderByDescending(v => v.Similarity);
                }
                extractedFields.Add(extractedField);
            }

            return extractedFields;
        }

        public static List<ExtractedFieldValue> ExtractFieldValue(List<string> words, int index, string field, double tolerance)
        {
            var word = words[index];
            var extractedFieldValues = new List<ExtractedFieldValue>();
            var similarity = Extractor.GetSimilarity(field, word);
            if (similarity > tolerance)
            {
                var valueIndex = index + 1;
                if (words.Count > valueIndex)
                    extractedFieldValues.Add(new ExtractedFieldValue()
                    {
                        Value = words[valueIndex],
                        Similarity = similarity
                    });
            }
            return extractedFieldValues;
        }

        public static List<string> Extract(string input)
        {
            var parser = new TextParser(input);
            var extractions = new List<string>();

            while (!parser.EndOfText)
            {
                parser.MovePastWhitespace();
                int start = parser.Position;
                var whiteSpaceCount = 0;

                while (whiteSpaceCount < 2 && !parser.EndOfText && !parser.EndOfLine)
                {
                    if (char.IsWhiteSpace(parser.Peek()))
                        whiteSpaceCount++;
                    else
                        whiteSpaceCount = 0;
                    parser.MoveAhead();
                }
                if (!parser.EndOfText && !parser.EndOfLine)
                    parser.MoveAhead(-2);

                if (parser.Position > start)
                    extractions.Add(parser.Extract(start, parser.Position));
            }

            return extractions;
        }

        public static string RemoveDiacritics(string accentedStr)
        {
            if (string.IsNullOrEmpty(accentedStr))
            {
                return accentedStr;
            }

            byte[] tempBytes;
            tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            string asciiStr = Encoding.UTF8.GetString(tempBytes);

            return asciiStr;
        }

        public static float GetSimilarity(string string1, string string2)
        {
            string1 = RemoveDiacritics(string1.ToLower());
            string2 = RemoveDiacritics(string2.ToLower());
            float dis = ComputeDistance(string1, string2);
            float maxLen = string1.Length;
            if (maxLen < string2.Length)
                maxLen = string2.Length;
            if (maxLen == 0.0F)
                return 1.0F;
            else
                return 1.0F - dis / maxLen;
        }

        private static int Min3(int i1, int i2, int i3)
        {
            return Math.Min(i1, Math.Min(i2, i3));
        }

        private static int ComputeDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] distance = new int[n + 1, m + 1];
            int cost = 0;
            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; distance[i, 0] = i++) ;
            for (int j = 0; j <= m; distance[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    cost = (t.Substring(j - 1, 1) ==
                        s.Substring(i - 1, 1) ? 0 : 1);
                    distance[i, j] = Min3(distance[i - 1, j] + 1,
                    distance[i, j - 1] + 1,
                    distance[i - 1, j - 1] + cost);
                }
            }
            return distance[n, m];
        }

        public static void PrintDataFromImg(string dataPath)
        {
            try
            {
                var logger = new FormattedConsoleLogger();
                var resultPrinter = new ResultPrinter(logger);
                using (var engine = new TesseractEngine(@"./tessdata", "ces", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(dataPath))
                    {
                        using (logger.Begin("Process image"))
                        {
                            var i = 1;
                            using (var page = engine.Process(img))
                            {
                                var text = page.GetText();
                                logger.Log("Text: {0}", text);
                                logger.Log("Mean confidence: {0}", page.GetMeanConfidence());

                                using (var iter = page.GetIterator())
                                {
                                    iter.Begin();
                                    do
                                    {
                                        if (i % 2 == 0)
                                        {
                                            using (logger.Begin("Line {0}", i))
                                            {
                                                do
                                                {
                                                    using (logger.Begin("Word Iteration"))
                                                    {
                                                        if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                                        {
                                                            logger.Log("New block");
                                                        }
                                                        if (iter.IsAtBeginningOf(PageIteratorLevel.Para))
                                                        {
                                                            logger.Log("New paragraph");
                                                        }
                                                        if (iter.IsAtBeginningOf(PageIteratorLevel.TextLine))
                                                        {
                                                            logger.Log("New line");
                                                        }
                                                        logger.Log("word: " + iter.GetText(PageIteratorLevel.Word));
                                                    }
                                                } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                                            }
                                        }
                                        i++;
                                    } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);

        }

        private class ResultPrinter
        {
            #region Fields

            private FormattedConsoleLogger logger;

            #endregion Fields

            #region Constructors

            public ResultPrinter(FormattedConsoleLogger logger)
            {
                this.logger = logger;
            }

            #endregion Constructors

            #region Methods

            public void Print(ResultIterator iter)
            {
                logger.Log("Is beginning of block: {0}", iter.IsAtBeginningOf(PageIteratorLevel.Block));
                logger.Log("Is beginning of para: {0}", iter.IsAtBeginningOf(PageIteratorLevel.Para));
                logger.Log("Is beginning of text line: {0}", iter.IsAtBeginningOf(PageIteratorLevel.TextLine));
                logger.Log("Is beginning of word: {0}", iter.IsAtBeginningOf(PageIteratorLevel.Word));
                logger.Log("Is beginning of symbol: {0}", iter.IsAtBeginningOf(PageIteratorLevel.Symbol));

                logger.Log("Block text: \"{0}\"", iter.GetText(PageIteratorLevel.Block));
                logger.Log("Para text: \"{0}\"", iter.GetText(PageIteratorLevel.Para));
                logger.Log("TextLine text: \"{0}\"", iter.GetText(PageIteratorLevel.TextLine));
                logger.Log("Word text: \"{0}\"", iter.GetText(PageIteratorLevel.Word));
                logger.Log("Symbol text: \"{0}\"", iter.GetText(PageIteratorLevel.Symbol));
            }

            #endregion Methods
        }
    }
}

