using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuizQuestion
{
    public string question;  // The text of the question
    public string[] options;  // The array of the options
    public int correctOptionIndex;  // The correct option index

    // Construct a new class of quiz questions
    public QuizQuestion(string question, string[] options, int correctOptionIndex)
    {
        this.question = question;
        this.options = options;
        this.correctOptionIndex = correctOptionIndex;
    }

    // the list of questions
    private static List<QuizQuestion> allQuestions = new List<QuizQuestion>
    {
        new QuizQuestion("Which Tang Dynasty poet has the most surviving works?", new string[] { "Li Bai", "Du Fu", "Bai Juyi", "Wang Wei"}, 2),
        new QuizQuestion("Where is the Wuhou Shrine located?", new string[] { "Inner Mongolia", "Chengdu", "Beijing", " Chongqing"}, 1),
        new QuizQuestion("In 230 AD, Sun Quan sent Wei Wen to lead an army to Yizhou. What is Yizhou today?", new string[] { "Fujian", "Sichuan", "Shanghai", "Taiwan"}, 3),
        new QuizQuestion("After Emperor Gaozu of Tang ascended to the throne, Li Shimin was known as:", new string[] { "Wei Wang", "Wen Wang", "Wu Wang", "Qin Wang"}, 3),
        new QuizQuestion("In the novel \"Journey to the West,\" where is the Flaming Mountain located?", new string[] { "Sichuan, China", "Pakistan", "Kazakhstan", "Turpan, China"}, 3),

        new QuizQuestion("Which of the following is NOT one of the Four Great Inventions of China?", new string[] { "Gunpowder", "Compass", "Papermaking", "Iron Armor"}, 3),
        new QuizQuestion("What is the capital of China?", new string[] {"Beijing", "Seoul", "Tokyo", "Bangkok"}, 0),
        new QuizQuestion("The princess of the Tang Dynasty who married Songtsen Gampo, the Tibetan king, was ( )." , new string[] { "Princess Jincheng", "Princess Taiping", "Princess Pingyang", "Princess Wencheng"}, 3),
        new QuizQuestion("What is the destination of Xuanzang's journey to the West during the Tang Dynasty? ", new string[] { "India", "Persia", "Arabia", "Japan"}, 0),
        new QuizQuestion("In the Tang Dynasty's imperial examination system, what did the Jinshi examination mainly test?", new string[] { "Classics", "Law", "Military", "Poetry and prose"}, 3),
        // 10 questions

        new QuizQuestion("Oracle Bone Script was inscribed on:", new string[] { "Fish bones", "Stone walls", "Turtle shells", "Stones"}, 2),
        new QuizQuestion("The reign title of Emperor Taizong of Tang was ( ).", new string[] { "Zhenguan", "Kaiyuan", "Tianbao", "Yonghui"}, 0),
        new QuizQuestion("Who found petroleum in China in the earliest record ?", new string[] { "Ying Zheng", "Confucius", "Xu Guangqi", "Shen Kuo"}, 3),
        new QuizQuestion("What is the capital of China in Tang Dynasty?", new string[] {"Beijing", "Chang'an", "Nanjing", "Luo Yang"}, 1),
        new QuizQuestion("Where is Chang'an now?", new string[] {"Xi'an", "Beijing", "Shan Xi", "Chong Qing"}, 0),

        new QuizQuestion("The largest palace in the world is:", new string[] { "The Forbidden City", "The White House", "Buckingham Palace", "The Kremlin"}, 0),
        new QuizQuestion("What are mooncake originally for?", new string[] { "Rituals", "Gifts", "Festival food", "Snacks"}, 0),
        new QuizQuestion("The oldest existing medical book in China is:", new string[] { "Shennong's Herbal Classic", "Huangdi Neijing", "Qi Min Yao Shu", "Compendium of Materia Medica"}, 1),
        new QuizQuestion("The author of \"The Art of War\" is:", new string[] { "Sun Wu", "Sun Bin", "Cao Cao", "Yue Fei"}, 0),
        new QuizQuestion("In \"Romance of the Three Kingdoms\", who is known as the \"Sage of War\"?", new string[] { "Liu Bei", "Zhang Fei", "Guan Yu", "Cao Cao"}, 2),
        // 20 questions

        new QuizQuestion("Which of the following was NOT one of the Four Great Poets of the Early Tang Dynasty?", new string[] { "Wang Bo", "Yang Jiong", "Lu Zhaolin", "Bai Juyi" }, 3),
        new QuizQuestion("Which of the following was a famous female poet in the Tang Dynasty?", new string[] { "Xue Tao", "Li Qingzhao", "Wu Zetian", "Cai Wenji" }, 0),
        new QuizQuestion("Who is known as the 'Poet Sage' in the Tang Dynasty?", new string[] { "Li Bai", "Du Fu", "Bai Juyi", "Wang Wei" }, 1),
        new QuizQuestion("Which of the following was a famous painter in the Tang Dynasty?", new string[] { "Wu Daozi", "Zhang Zeduan", "Gu Kaizhi", "Fan Kuan" }, 0),
        new QuizQuestion("What was the main religion supported by the Tang TaiZong?", new string[] { "Buddhism", "Taoism", "Confucianism", "Islam" }, 1),

        new QuizQuestion("Who was the first emperor of the Tang Dynasty?", new string[] { "Li Yuan", "Li Shimin", "Li Longji", "Li Zhi" }, 0),
        new QuizQuestion("Who was the first female emperor in Chinese history?", new string[] { "Wu Zetian", "Empress Dowager Cixi", "Empress L¨¹", "Empress Dowager Xiaozhuang" }, 0),
        new QuizQuestion("What is the traditional Chinese festival celebrated on the 15th day of the eighth lunar month?", new string[] { "Lantern Festival", "Mid-Autumn Festival", "Qingming Festival", "Dragon Boat Festival" }, 1),
        new QuizQuestion("Which ancient Chinese philosopher is known for the text 'Tao Te Ching'?", new string[] { "Confucius", "Mencius", "Laozi", "Sun Tzu" }, 2),
        new QuizQuestion("What is the Chinese name for 'The Great Wall of China'?", new string[] { "Ch¨¢ngch¨¦ng", "W¨¤n L¨« Ch¨¢ngch¨¦ng", "B¨§ij¨©ng Ch¨¢ngch¨¦ng", "Sh¨¡nh¨£igu¨¡n Ch¨¢ngch¨¦ng" }, 0),
        // 30 Questions

        new QuizQuestion("Which of the following is NOT one of the Four Great Classical Novels of Chinese literature?", new string[] { "Romance of the Three Kingdoms", "Journey to the West", "Water Margin", "The Scholars" }, 3),
        new QuizQuestion("What is the traditional Chinese festival known as the 'Chinese Valentine's Day'?", new string[] { "Qixi Festival", "Mid-Autumn Festival", "Lantern Festival", "Dragon Boat Festival" }, 0),
        new QuizQuestion("Which dynasty is known for the Terracotta Army?", new string[] { "Qin Dynasty", "Han Dynasty", "Tang Dynasty", "Ming Dynasty" }, 0),
        new QuizQuestion("What is the traditional Chinese medicine known for its comprehensive summary of herbal knowledge?", new string[] { "Huangdi Neijing", "Shennong's Herbal Classic", "Compendium of Materia Medica", "Qi Min Yao Shu" }, 2),
        new QuizQuestion("Which ancient Chinese text is famous for its military strategies and tactics?", new string[] { "The Art of War", "The Analects", "Tao Te Ching", "I Ching" }, 0),

        new QuizQuestion("Which dynasty is considered the first in Chinese history?", new string[] { "Xia Dynasty", "Shang Dynasty", "Zhou Dynasty", "Qin Dynasty" }, 0),
        new QuizQuestion("Who was the first emperor of China, known for unifying the six warring states?", new string[] { "Qin Shi Huang", "Emperor Wu of Han", "Emperor Taizong of Tang", "Emperor Kangxi of Qing" }, 0),
        new QuizQuestion("Which ancient Chinese philosopher is known for the concept of 'Ren' (benevolence)?", new string[] { "Confucius", "Mencius", "Laozi", "Sun Tzu" }, 0),
        new QuizQuestion("Which of the following is the oldest of the Four Great Classical Novels of Chinese literature?", new string[] { "Dream of the Red Chamber", "Romance of the Three Kingdoms", "Journey to the West", "Water Margin" }, 1),
        new QuizQuestion("Which Chinese dynasty is famous for its 'Golden Age' known as the 'Kaiyuan Prosperity'?", new string[] { "Tang Dynasty", "Han Dynasty", "Song Dynasty", "Ming Dynasty" }, 0),
        // 40 Questions

        new QuizQuestion("Which ancient Chinese invention is related to navigation and finding directions?", new string[] { "Gunpowder", "Papermaking", "Printing", "Compass" }, 3),
        new QuizQuestion("Who was the famous painter of the Tang Dynasty known for his paintings of horses?", new string[] { "Wu Daozi", "Han Gan", "Zhang Zeduan", "Fan Kuan" }, 1),
        new QuizQuestion("Which Chinese dynasty is known for the construction of the Terracotta Army?", new string[] { "Qin Dynasty", "Han Dynasty", "Tang Dynasty", "Ming Dynasty" }, 0),
        new QuizQuestion("Which of the following is NOT one of the 'Three Teachings' in Chinese culture?", new string[] { "Confucianism", "Taoism", "Buddhism", "Legalism" }, 3),
        new QuizQuestion("Which Chinese dynasty is the last dynasty of ancient China?", new string[] { "Han Dynasty", "Qin Dynasty", "Tang Dynasty", "Ming Dynasty" }, 1),

        new QuizQuestion("Which Tang Dynasty poet is known as the 'Poet Buddha'?", new string[] { "Li Bai", "Du Fu", "Bai Juyi", "Wang Wei" }, 3),
        new QuizQuestion("Which Tang Dynasty poet is known as the 'Poet Immortal'?", new string[] { "Li Bai", "Du Fu", "Bai Juyi", "Wang Wei" }, 0),
        new QuizQuestion("Which famous painting depicts the scene of Princess Wencheng's marriage to Songtsen Gampo?", new string[] { "The Step Ladder", "The Admonitions Scroll", "The Five Oxen", "The Spring Outing of the Tang Court" }, 0),
        new QuizQuestion("Which of the following is NOT a famous Tang Dynasty poet?", new string[] { "Li Bai", "Du Fu", "Wang Bo", "Cao Cao" }, 3),
        new QuizQuestion("Which Tang Dynasty emperor is known for the 'Kaiyuan Prosperity'?", new string[] { "Emperor Xuanzong", "Emperor Taizong", "Emperor Gaozu", "Emperor Gaozong" }, 0)
        // 50 Questions


    };

    // The function to randomly choose questions
    public static List<QuizQuestion> GetRandomQuestions(int numberOfQuestions)
    {
        List<QuizQuestion> shuffledQuestions = new List<QuizQuestion>(allQuestions);

        // Shuffle the order of the questions
        System.Random rng = new System.Random();
        int n = shuffledQuestions.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            QuizQuestion value = shuffledQuestions[k];
            shuffledQuestions[k] = shuffledQuestions[n];
            shuffledQuestions[n] = value;
        }

        // return first (numberOfQuestions) questions 
        return shuffledQuestions.GetRange(0, numberOfQuestions);
    }

    // Method to remove a specific question from the pool
    public static void RemoveQuestion(QuizQuestion question)
    {
        // Find the question to remove based on its properties
        QuizQuestion questionToRemove = allQuestions.Find(q =>
            q.question == question.question &&
            q.correctOptionIndex == question.correctOptionIndex &&
            AreArraysEqual(q.options, question.options)
        );

        if (questionToRemove != null)
        {
            allQuestions.Remove(questionToRemove); // Remove the found question
            Debug.Log($"Question removed: {question.question}");
        }
        else
        {
            Debug.LogWarning("Question not found in the list.");
        }
    }

    // Helper method to compare two string arrays
    private static bool AreArraysEqual(string[] array1, string[] array2)
    {
        if (array1.Length != array2.Length) return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i]) return false;
        }
        return true;
    }



}
