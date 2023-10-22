using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Configuration;

public class Program
{
    static string filePath = ConfigurationManager.AppSettings["FilePath"];
    static string text;
    static int topCount = 5;

    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var threads = new Thread[6];



        Thread countWordsThread = new Thread(CountWords);
        threads[0] = countWordsThread;
        Thread shortestWordThread = new Thread(FindShortestWord);
        threads[1] = shortestWordThread;
        Thread longestWordThread = new Thread(FindLongestWord);
        threads[2] = longestWordThread;
        Thread averageWordLengthThread = new Thread(FindAverageWordLength);
        threads[3] = averageWordLengthThread;
        Thread findTopCommonWordsThread = new Thread(FindTopCommonWords);
        threads[4] = findTopCommonWordsThread;
        Thread findLeastCommonWordsThread = new Thread (FindLeastCommonWords);
        threads[5] = findLeastCommonWordsThread;




       
        try
        {
            text = File.ReadAllText(filePath);
            Console.WriteLine("Text read successfully.");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("The file was not found.");
        }
        catch (IOException)
        {
            Console.WriteLine("An error occurred while reading the file.");
        }



        foreach (var thread in threads) thread.Start();
        foreach (var thread in threads) thread.Join();


        stopwatch.Stop();

        Console.WriteLine();
        Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms");

    }


    static void CountWords()
    {
        //string text = o.ToString();
        string[] words = Regex.Split(text, @"\P{L}+");

        int wordCount = 0;

        foreach (string word in words)
        {
            if (!string.IsNullOrWhiteSpace(word) && word.Length >= 3)
            {
                wordCount++;
            }
        }

        Console.WriteLine($"The number of words is: {wordCount}");
    }

    static void FindShortestWord()
    {
        //string text = o.ToString();
        string[] words = Regex.Split(text, @"\P{L}+");

        string shortestWord = "";
        int wordLength = int.MaxValue;

        foreach (string word in words)
        {
            if (!string.IsNullOrWhiteSpace(word) && word.Length >= 3)
            {
                if (word.Length < wordLength)
                {
                    wordLength = word.Length;
                    shortestWord = word;
                }
            }
        }

        Console.WriteLine($"The shortest word is: {shortestWord}");
    }

    static void FindLongestWord()
    {
      //  string text = o.ToString();
        string[] words = Regex.Split(text, @"\P{L}+");

        string longestWord = "";
        int wordLength = 0;

        foreach (string word in words)
        {
            if (!string.IsNullOrWhiteSpace(word) && word.Length >= 3)
            {
                if (word.Length > wordLength)
                {
                    wordLength = word.Length;
                    longestWord = word;
                }
            }
        }

        Console.WriteLine($"The longest word is: {longestWord}");

    }

    static void FindAverageWordLength()
    {
      //  string text = o.ToString();
        
        string[] words = Regex.Split(text, @"\P{L}+");

        double totalWordLength = 0;
        int wordCount = 0;

        foreach (string word in words)
        {
            if (!string.IsNullOrWhiteSpace(word) && word.Length >= 3)
            {
                totalWordLength += word.Length;
                wordCount++;
            }
        }
        if (wordCount == 0)
        {
            Console.WriteLine(0);
        }

        Console.WriteLine($"The average word length is: {Math.Round(totalWordLength / wordCount)}");
    }


    static void FindTopCommonWords()
    {

        //string text = o.ToString();
        Dictionary<string, int> wordCounts = new Dictionary<string, int>();


        string[] words = Regex.Split(text, @"\P{L}+");

        foreach (string word in words)
        {
            if (!string.IsNullOrWhiteSpace(word) && word.Length >= 3)
            {
                if (wordCounts.ContainsKey(word))
                {
                    wordCounts[word]++;
                }
                else
                {
                    wordCounts[word] = 1;
                }
            }
        }


        List<KeyValuePair<string, int>> topWords = new List<KeyValuePair<string, int>>();
        topWords = wordCounts.ToList();

        QuickSort(topWords, 0, topWords.Count - 1);

        List<string> result = new List<string>();
        for (int i = 0; i < topCount && i < topWords.Count; i++)
        {
            result.Add(topWords[i].Key);
        }

        Console.Write("Most common words: ");
        foreach (string word in result)
        {
            Console.Write($"{word} ");
        }
        Console.WriteLine();
    }

    static void FindLeastCommonWords()
    {

        //string text = o.ToString();
        Dictionary<string, int> wordCounts = new Dictionary<string, int>();


        string[] words = Regex.Split(text, @"\P{L}+");

        foreach (string word in words)
        {
            if (!string.IsNullOrWhiteSpace(word) && word.Length >= 3)
            {
                if (wordCounts.ContainsKey(word))
                {
                    wordCounts[word]++;
                }
                else
                {
                    wordCounts[word] = 1;
                }
            }

        }


        List<KeyValuePair<string, int>> topWords = new List<KeyValuePair<string, int>>();
        topWords = wordCounts.ToList();

        QuickSort(topWords, 0, topWords.Count - 1);

        List<string> result = new List<string>();
        for (int i = topWords.Count - 1; i > topWords.Count - topCount - 1; i--)
        {
            result.Add(topWords[i].Key);
        }

        Console.WriteLine();
        Console.Write("Least common words: ");
        foreach (string word in result)
        {
            Console.Write($"{word} ");
        }
        Console.WriteLine();

    }




    static void QuickSort(List<KeyValuePair<string, int>> list, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(list, left, right);

            QuickSort(list, left, pivotIndex - 1);
            QuickSort(list, pivotIndex + 1, right);
        }
    }



    static int Partition(List<KeyValuePair<string, int>> list, int left, int right)
    {
        KeyValuePair<string, int> pivotValue = list[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (list[j].Value > pivotValue.Value)
            {
                i++;
                Swap(list, i, j);
            }
        }

        Swap(list, i + 1, right);
        return i + 1;
    }

    static void Swap(List<KeyValuePair<string, int>> list, int i, int j)
    {
        KeyValuePair<string, int> temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

}