using System.Text;
using System.Text.RegularExpressions;

namespace Autosalon_Lab11_Strings;

public sealed class MainForm : Form
{
    private readonly ListBox listBoxLines = new();
    private readonly Label labelResult = new();
    private readonly Button buttonAnalyze = new();
    private readonly Button buttonWords = new();
    private readonly Button buttonDigits = new();
    private readonly Button buttonPunctuation = new();
    private readonly Button buttonSplit = new();
    private readonly Button buttonUkrainianLower = new();
    private readonly Button buttonPalindrome = new();
    private readonly Button buttonSwapWords = new();
    private readonly TextBox textBoxCustom = new();
    private readonly Button buttonAdd = new();

    public MainForm()
    {
        Text = "Лабораторна робота №11 — Рядки та ListBox";
        Width = 980;
        Height = 640;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.AliceBlue;
        Font = new Font("Segoe UI", 10);

        BuildInterface();
        FillListBox();
    }

    private void BuildInterface()
    {
        var title = new Label
        {
            Text = "Автосалон: обробка рядків у ListBox",
            AutoSize = true,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            Location = new Point(20, 15)
        };
        Controls.Add(title);

        var info = new Label
        {
            Text = "Оберіть рядок зі списку або додайте власний текст. Результат буде показано в Label.",
            AutoSize = true,
            Location = new Point(22, 55)
        };
        Controls.Add(info);

        listBoxLines.Location = new Point(25, 90);
        listBoxLines.Size = new Size(500, 245);
        listBoxLines.Font = new Font("Consolas", 10);
        Controls.Add(listBoxLines);

        textBoxCustom.Location = new Point(25, 350);
        textBoxCustom.Size = new Size(380, 30);
        textBoxCustom.PlaceholderText = "Введіть власний рядок для ListBox";
        Controls.Add(textBoxCustom);

        buttonAdd.Text = "Додати в ListBox";
        buttonAdd.Location = new Point(420, 348);
        buttonAdd.Size = new Size(140, 34);
        buttonAdd.Click += (_, _) => AddCustomLine();
        Controls.Add(buttonAdd);

        buttonAnalyze.Text = "Повний аналіз";
        buttonAnalyze.Location = new Point(560, 90);
        buttonAnalyze.Size = new Size(180, 35);
        buttonAnalyze.Click += (_, _) => AnalyzeSelectedLine();
        Controls.Add(buttonAnalyze);

        buttonWords.Text = "Кількість слів";
        buttonWords.Location = new Point(560, 135);
        buttonWords.Size = new Size(180, 35);
        buttonWords.Click += (_, _) => ShowWordCount();
        Controls.Add(buttonWords);

        buttonDigits.Text = "Показати цифри";
        buttonDigits.Location = new Point(560, 180);
        buttonDigits.Size = new Size(180, 35);
        buttonDigits.Click += (_, _) => ShowDigits();
        Controls.Add(buttonDigits);

        buttonPunctuation.Text = "Знаки пунктуації";
        buttonPunctuation.Location = new Point(560, 225);
        buttonPunctuation.Size = new Size(180, 35);
        buttonPunctuation.Click += (_, _) => ShowPunctuationCount();
        Controls.Add(buttonPunctuation);

        buttonSplit.Text = "Цифри / пунктуація / інше";
        buttonSplit.Location = new Point(560, 270);
        buttonSplit.Size = new Size(210, 35);
        buttonSplit.Click += (_, _) => SplitLine();
        Controls.Add(buttonSplit);

        buttonUkrainianLower.Text = "Малі українські букви";
        buttonUkrainianLower.Location = new Point(560, 315);
        buttonUkrainianLower.Size = new Size(210, 35);
        buttonUkrainianLower.Click += (_, _) => ShowUkrainianLowercase();
        Controls.Add(buttonUkrainianLower);

        buttonPalindrome.Text = "Перевірити паліндром";
        buttonPalindrome.Location = new Point(560, 360);
        buttonPalindrome.Size = new Size(210, 35);
        buttonPalindrome.Click += (_, _) => CheckPalindrome();
        Controls.Add(buttonPalindrome);

        buttonSwapWords.Text = "Поміняти 1 і останнє слово";
        buttonSwapWords.Location = new Point(560, 405);
        buttonSwapWords.Size = new Size(220, 35);
        buttonSwapWords.Click += (_, _) => SwapFirstAndLastWord();
        Controls.Add(buttonSwapWords);

        labelResult.Location = new Point(25, 405);
        labelResult.Size = new Size(910, 175);
        labelResult.BorderStyle = BorderStyle.FixedSingle;
        labelResult.BackColor = Color.White;
        labelResult.Font = new Font("Consolas", 10);
        labelResult.Text = "Результат буде тут.";
        Controls.Add(labelResult);
    }

    private void FillListBox()
    {
        listBoxLines.Items.Add("Автосалон AutoLux: BMW X5, Audi A6, Toyota Camry — 2024!");
        listBoxLines.Items.Add("вихідні дні: 1, 2 січня, 8 березня, 1 травня, 9 травня!");
        listBoxLines.Items.Add("A roza upala na lapu Azora");
        listBoxLines.Items.Add("111000101010 — перевірка нулів та одиниць");
        listBoxLines.Items.Add("sale price 25000 usd; discount 10%; delivery 3 days.");
        listBoxLines.Items.Add("київ львів одеса харків дніпро");
        listBoxLines.SelectedIndex = 0;
    }

    private string GetSelectedLine()
    {
        if (listBoxLines.SelectedIndex < 0)
        {
            MessageBox.Show("Оберіть рядок у ListBox.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return string.Empty;
        }

        return listBoxLines.Items[listBoxLines.SelectedIndex]?.ToString() ?? string.Empty;
    }

    private void AddCustomLine()
    {
        string text = textBoxCustom.Text.Trim();
        if (string.IsNullOrWhiteSpace(text))
        {
            MessageBox.Show("Введіть текст перед додаванням.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        listBoxLines.Items.Add(text);
        listBoxLines.SelectedIndex = listBoxLines.Items.Count - 1;
        textBoxCustom.Clear();
        labelResult.Text = "Новий рядок додано до ListBox.";
    }

    private void AnalyzeSelectedLine()
    {
        string line = GetSelectedLine();
        if (line.Length == 0) return;

        int words = CountWords(line);
        int digits = line.Count(char.IsDigit);
        int punctuation = line.Count(char.IsPunctuation);
        int spaces = line.Count(char.IsWhiteSpace);
        int ukrLower = line.Count(IsUkrainianLowercase);

        var sb = new StringBuilder();
        sb.AppendLine($"Рядок: {line}");
        sb.AppendLine($"Довжина рядка: {line.Length}");
        sb.AppendLine($"Кількість слів: {words}");
        sb.AppendLine($"Кількість цифр: {digits}");
        sb.AppendLine($"Кількість знаків пунктуації: {punctuation}");
        sb.AppendLine($"Кількість пробілів/пропусків: {spaces}");
        sb.AppendLine($"Малі українські букви: {ukrLower}");
        labelResult.Text = sb.ToString();
    }

    private void ShowWordCount()
    {
        string line = GetSelectedLine();
        if (line.Length == 0) return;
        labelResult.Text = $"Кількість слів у вибраному рядку: {CountWords(line)}";
    }

    private void ShowDigits()
    {
        string line = GetSelectedLine();
        if (line.Length == 0) return;
        string digits = new(line.Where(char.IsDigit).ToArray());
        labelResult.Text = string.IsNullOrEmpty(digits) ? "Цифр у рядку немає." : $"Цифри в рядку: {digits}";
    }

    private void ShowPunctuationCount()
    {
        string line = GetSelectedLine();
        if (line.Length == 0) return;
        labelResult.Text = $"Кількість знаків пунктуації: {line.Count(char.IsPunctuation)}";
    }

    private void SplitLine()
    {
        string line = GetSelectedLine();
        if (line.Length == 0) return;

        var digits = new StringBuilder();
        var punct = new StringBuilder();
        var other = new StringBuilder();

        foreach (char ch in line)
        {
            if (char.IsDigit(ch)) digits.Append(ch);
            else if (char.IsPunctuation(ch)) punct.Append(ch);
            else other.Append(ch);
        }

        labelResult.Text = $"Цифри: {digits}\nПунктуація: {punct}\nІнші символи: {other}";
    }

    private void ShowUkrainianLowercase()
    {
        string line = GetSelectedLine();
        if (line.Length == 0) return;
        string letters = new(line.Where(IsUkrainianLowercase).ToArray());
        labelResult.Text = string.IsNullOrWhiteSpace(letters)
            ? "Малих українських букв не знайдено."
            : $"Малі українські букви: {letters}\nКількість: {letters.Length}";
    }

    private void CheckPalindrome()
    {
        string line = GetSelectedLine();
        if (line.Length == 0) return;

        string normalized = Regex.Replace(line.ToLowerInvariant(), "[^a-zа-яіїєґ0-9]", "");
        string reversed = new(normalized.Reverse().ToArray());
        labelResult.Text = normalized == reversed
            ? "Рядок є паліндромом."
            : "Рядок не є паліндромом.";
    }

    private void SwapFirstAndLastWord()
    {
        string line = GetSelectedLine();
        if (line.Length == 0) return;

        string[] words = Regex.Split(line.Trim(), "\\s+");
        if (words.Length < 2)
        {
            labelResult.Text = "У рядку менше двох слів.";
            return;
        }

        (words[0], words[^1]) = (words[^1], words[0]);
        labelResult.Text = "Новий рядок:\n" + string.Join(" ", words);
    }

    private static int CountWords(string line)
    {
        return Regex.Matches(line.Trim(), @"\S+").Count;
    }

    private static bool IsUkrainianLowercase(char ch)
    {
        return "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя".Contains(ch);
    }
}
