using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static Microsoft.Data.SqlClient.Internal.SqlClientEventSource;

namespace demo
{
    public partial class MainWindow : Window
    {
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();

        TasksRepo repo = new TasksRepo();
        NLPProcessor nlp = new NLPProcessor();

        string username = string.Empty;
        string currentUser = string.Empty;
        int counting = 0;

        public MainWindow()
        {
            InitializeComponent();
            new respond(reply, ignore) { };
            voice_greeting greet = new voice_greeting();
            greet.greet();
        }

        private void proceed(object sender, RoutedEventArgs e)
        {
            home_grid.Visibility = Visibility.Hidden;
            username_grid.Visibility = Visibility.Visible;
        }

        private void submit_name(object sender, RoutedEventArgs e)
        {
            string input = usernames_input.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Name cannot be empty.");
                usernames_input.Clear();
                usernames_input.Focus();
                return;
            }
            if (input.Length < 3)
            {
                MessageBox.Show("Name must be at least 3 characters long.");
                usernames_input.Clear();
                usernames_input.Focus();
                return;
            }
            if (!HasNoNumbers(input))
            {
                MessageBox.Show("Numbers are not allowed in names.");
                usernames_input.Clear();
                usernames_input.Focus();
                return;
            }

            bool isReturning = CheckNameExists(input);
            if (!isReturning) SaveName(input);

            username = input;
            currentUser = input;

            username_grid.Visibility = Visibility.Hidden;
            chat_grid.Visibility = Visibility.Visible;

            // NEW: TIME-BASED GREETING
            string timeGreeting = GetTimeGreeting();
            if (isReturning)
                error_method("ChatBot", $"{timeGreeting} {username} 👋 Welcome back!");
            else
                error_method("ChatBot", $"{timeGreeting} {username}! Welcome to AEGISNODE");
        }

        private void usernames_input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                submit_name(this, new RoutedEventArgs());
                e.Handled = true;
            }
        }

        // TIME GREETING HELPER
        private string GetTimeGreeting()
        {
            int hour = DateTime.Now.Hour;
            if (hour < 12) return "Good morning";
            else if (hour < 17) return "Good afternoon";
            else return "Good evening";
        }

        private void send(object sender, RoutedEventArgs e)
        {
            string Question = question.Text.ToString().Trim();
            if (string.IsNullOrWhiteSpace(Question))
            {
                error_method("ChatBot", "Please enter a question.");
                return;
            }

            string questions = RemoveSpecialCharacters(Question);
            error_method(username, Question);
            question.Clear();
            string lower = Question.ToLower();

            // === TASK COMMANDS ===
            if (lower.StartsWith("remind me to "))
            {
                string taskText = Question.Substring("remind me to ".Length).Trim();
                DateTime? date = nlp.ExtractDate(taskText);
                string title = nlp.ExtractTitle(taskText);
                repo.AddTask(currentUser, title, date);
                error_method("ChatBot", date.HasValue
                 ? $"Got it ✅ I'll remind you to '{title}' on {date.Value:dd MMM yyyy HH:mm}"
                  : $"Got it ✅ I'll remind you to '{title}'");
                return;
            }
            else if (lower == "view tasks")
            {
                var tasks = repo.GetTasks(currentUser);
                if (tasks.Count == 0)
                    error_method("ChatBot", "You have no active tasks 💤");
                else
                {
                    error_method("ChatBot", "Here are your tasks:");
                    foreach (var t in tasks)
                    {
                        string date = t.ReminderDate.HasValue? t.ReminderDate.Value.ToString("dd MMM yyyy HH:mm") : "No date";
                        error_method("ChatBot", $"[ID:{t.TaskId}] {t.Title} | Due: {date} | Status: {(t.IsCompleted ? "DONE" : "PENDING")}");
                    }
                }
                return;
            }
            else if (lower.StartsWith("delete task "))
            {
                string idPart = lower.Replace("delete task ", "").Trim();
                if (int.TryParse(idPart, out int taskId))
                {
                    repo.DeleteTask(taskId, currentUser);
                    error_method("ChatBot", $"Task {taskId} deleted 🗑️");
                }
                else
                    error_method("ChatBot", "Use: delete task 2");
                return;
            }

            auto_show_interest();
            ai_check(questions);
        }

        public bool HasNoNumbers(string input) => Regex.IsMatch(input, @"^\D+$");

        private bool CheckNameExists(string name)
        {
            string filename = "user_names.txt";
            if (!File.Exists(filename)) { File.WriteAllText(filename, ""); return false; }
            string[] names = File.ReadAllLines(filename);
            foreach (string name_found in names)
                if (name_found.Trim().Equals(name, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private void SaveName(string name)
        {
            string filename = "user_names.txt";
            File.AppendAllText(filename, name + Environment.NewLine);
        }

        //  Only saves if "interested in X" or "i like X"
        private void SaveInterest(string word)
        {
            if (string.IsNullOrWhiteSpace(currentUser)) return;
            string filename = "interested_topic.txt";
            string newLine = currentUser + ": interested in: " + word;

            if (!File.Exists(filename)) { File.WriteAllText(filename, newLine + Environment.NewLine); return; }

            var lines = File.ReadAllLines(filename).ToList();
            int index = lines.FindIndex(l => l.StartsWith(currentUser + ":"));
            if (index >= 0) lines[index] = newLine;
            else lines.Add(newLine);
            File.WriteAllLines(filename, lines);
        }

        private void ai_check(string questions)
        {
            if (string.IsNullOrWhiteSpace(questions))
            {
                error_method("ChatBot", "Please enter a valid question.");
                question.Clear();
                return;
            }

            questions = questions.ToLower();
            string[] words = questions.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string emotionReply = null;
            List<string> topicReplies = new List<string>();

            string[] emotions = { "frustrated", "angry", "confused", "sad", "happy", "excited", "worried" };
            foreach (string word in words)
            {
                if (emotions.Contains(word))
                {
                    foreach (string answerStr in reply)
                    {
                        if (answerStr.StartsWith(word + " "))
                        {
                            emotionReply = answerStr.Substring(word.Length + 1);
                            break;
                        }
                    }
                    if (emotionReply != null) break;
                }
            }

            string[] greetings = { "hello", "hi", "hey", "good morning", "good evening", "good day" };
            bool onlyGreeting = true;
            foreach (string word in words)
            {
                bool isGreetingWord = greetings.Contains(word);
                bool isIgnored = ignore.Contains(word);
                if (!isGreetingWord && !isIgnored) { onlyGreeting = false; break; }
            }

            if (onlyGreeting) topicReplies.Add("🤗Hey! Ask me about passwords, phishing, malware, encryption, 2FA, or ransomware.");

            foreach (string word in words)
            {
                foreach (string answerStr in reply)
                {
                    string[] parts = answerStr.Split(new char[] { ' ' }, 2);
                    string keyword = parts[0].ToLower();
                    if (word == keyword && !emotions.Contains(word) && !greetings.Contains(word))
                    {
                        string responseText = parts.Length > 1 ? parts[1] : "";
                        topicReplies.Add(responseText);

                        // ONLY save interest if user said the phrase, "i'm interested in"
                        if (questions.Contains("interested in " + keyword) || questions.Contains("i like " + keyword))
                        {
                            SaveInterest(keyword);
                        }
                    }
                }
            }

            Random random = new Random();
            List<string> responsesToSend = new List<string>();
            if (emotionReply != null) responsesToSend.Add(emotionReply);
            if (topicReplies.Count > 0) responsesToSend.Add(topicReplies[random.Next(0, topicReplies.Count)]);

            if (responsesToSend.Count > 0)
                error_method("ChatBot", string.Join("\n\n", responsesToSend));
            else
            {
                string[] fallbackMessages = {
                    "Hmm, I'm not sure I got that. Try asking me about phishing, passwords, or malware.",
                    "I didn't catch that. What part of cybersecurity do you want to talk about?",
                    "Mind rephrasing that? I'm still learning."
                };
                error_method("ChatBot", fallbackMessages[random.Next(fallbackMessages.Length)]);
            }
            question.Clear();
        }

        private string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            StringBuilder sanitized = new StringBuilder();
            int i = 0;
            while (i < input.Length)
            {
                string ch = input.Substring(i, 1);
                if (Regex.IsMatch(ch, "[a-zA-Z0-9\\s'\\-]")) sanitized.Append(ch);
                else sanitized.Append(" ");
                i = i + 1;
            }
            string result = sanitized.ToString();
            result = Regex.Replace(result, "\\s+", " ").Trim();
            return result;
        }

        private void auto_show_interest()
        {
            if (counting == 3)
            {
                string filename = "interested_topic.txt";
                if (File.Exists(filename))
                {
                    string[] lines = File.ReadAllLines(filename);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith(currentUser))
                        {
                            int colonIndex = line.IndexOf("interested in:");
                            if (colonIndex >= 0)
                            {
                                string interests = line.Substring(colonIndex + 14).Trim();
                                error_method("ChatBot", "I remember you're interested in " + interests + " 😊");
                                break;
                            }
                        }
                    }
                }
                counting = 0;
            }
            else counting += 1;
        }

        private void error_method(string name, string message)
        {
            Border messageBorder = new Border
            {
                Margin = new Thickness(0, 2, 0, 2),
                Padding = new Thickness(5, 3, 5, 3),
                CornerRadius = new CornerRadius(5)
            };

            if (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat"))
            {
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(240, 248, 255));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(173, 216, 230));
            }
            else
            {
                messageBorder.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                messageBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(211, 211, 211));
            }
            messageBorder.BorderThickness = new Thickness(1);

            TextBlock messageText = new TextBlock { TextWrapping = TextWrapping.Wrap, Margin = new Thickness(2) };
            Brush nameColor = (name.ToLower().Contains("chatbot") || name.ToLower().Contains("chat")) ? Brushes.DarkBlue : Brushes.DarkGreen;
            Brush messageColor = Brushes.Black;

            messageText.Inlines.Add(new Run { Text = name + ": ", Foreground = nameColor, FontWeight = FontWeights.Bold });
            messageText.Inlines.Add(new Run { Text = message, Foreground = messageColor });

            messageBorder.Child = messageText;
            chats.Items.Add(messageBorder);
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
        }
    }
}