// See https://aka.ms/new-console-template for more information
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;


Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

string filePath = ConfigurationManager.AppSettings["FilePath"];

try
{
    string text = File.ReadAllText(filePath);

        int wordsCount = CountWords(text);
        string shortestWord = FindShortestWord(text);
        string longestWord = FindLongestWord(text);
        double averageWordLength = FindAverageWordLength(text, wordsCount);

        Console.WriteLine($"Number of words in the text: {wordsCount}\nThe shortest word: {shortestWord}\nThe longest word: {longestWord}\nAverage word length:{averageWordLength}");
        Console.Write("Most common words: ");
        foreach (string word in FindTopCommonWords(text,5))
        {
            Console.Write($"{word} ");
        }
    Console.WriteLine();
        Console.Write("Least common words: ");
        foreach (string word in FindLeastCommonWords(text, 5))
        {
            Console.Write($"{word} ");
        }
    Console.WriteLine();
}
    catch (FileNotFoundException)
{
    Console.WriteLine("The file was not found.");
}
    catch (IOException)
{
    Console.WriteLine("An error occurred while reading the file.");
}
stopwatch.Stop();

Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms");

static int CountWords(string text)
{
    string[] words = Regex.Split(text, @"\P{L}+");

    int wordCount = 0;

    foreach (string word in words)
    {
        if (!string.IsNullOrWhiteSpace(word) && word.Length >= 3)
        {
            wordCount++;
        }
    }

    return wordCount;
}


static string FindShortestWord(string text)
{
    
    string[] words = Regex.Split(text, @"\P{L}+");

    string shortestWord = "";
    int wordLength = int.MaxValue;

    foreach (string word in words)
    {
        if (!string.IsNullOrWhiteSpace(word) && word.Length >= 3)
        {
            if(word.Length < wordLength) { 
                wordLength = word.Length;
                shortestWord = word; }          
        }
    }


    return shortestWord;
}

static string FindLongestWord(string text)
{
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


    return longestWord;

}

static double FindAverageWordLength(string text, int wordCount)
{
    string[] words = Regex.Split(text, @"\P{L}+");

    double totalWordLength = 0;
   // int wordCount = 0;

    foreach (string word in words)
    {
        if (!string.IsNullOrWhiteSpace(word) && word.Length >= 3)
        {
            totalWordLength += word.Length;
        }
    }
    if (wordCount == 0)
    {
        return 0;
        }

    return Math.Round(totalWordLength / wordCount);
}


static List<string> FindTopCommonWords(string text, int topCount)
{
    

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

    return result;

}

static List<string> FindLeastCommonWords(string text, int topCount)
{


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

    return result;

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
