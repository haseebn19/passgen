using System.Text;
using System.Diagnostics;

namespace passgen
{
    public partial class Passgen : Form
    {
        // Define character sets
        string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string numberChars = "0123456789";
        string symbolChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

        public Passgen()
        {
            InitializeComponent();
        }

        private static Color GetStrengthColor(int score)
        {
            // Define the start (weak) and end (strong) colors
            Color startColor = Color.Red;
            Color endColor = Color.Green;

            // Calculate the ratio based on the score (0 to 4)
            float ratio = score / 4f;

            // Interpolate between the start and end colors
            int r = (int)(startColor.R * (1 - ratio) + endColor.R * ratio);
            int g = (int)(startColor.G * (1 - ratio) + endColor.G * ratio);
            int b = (int)(startColor.B * (1 - ratio) + endColor.B * ratio);

            return Color.FromArgb(r, g, b);
        }

        private async void buttonGenerate_Click_1(object sender, EventArgs e)
        {
            // Disable the button to prevent multiple clicks
            buttonGenerate.Enabled = false;

            // Start the stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string generatedPassword = await Task.Run(() => GeneratePassword());

            // Stop the stopwatch
            stopwatch.Stop();


            // Set the generated password to the textbox if generated password was not empty
            if (!string.IsNullOrEmpty(generatedPassword))
            {
                textboxPasswordOutput.Text = generatedPassword;
                // Write the elapsed time to the debug output
                Debug.WriteLine($"Time taken to generate password: {stopwatch.ElapsedMilliseconds} ms");
            }

            // Re-enable the button
            buttonGenerate.Enabled = true;
        }


        private string GeneratePassword()
        {
            StringBuilder passwordBuilder = new StringBuilder();
            Random rand = new Random();

            // Create a list to hold all selected character sets
            List<string> selectedCharSets = new List<string>();

            if (checkboxLowercase.Checked)
                selectedCharSets.Add(lowercaseChars);
            if (checkboxUppercase.Checked)
                selectedCharSets.Add(uppercaseChars);
            if (checkboxNumbers.Checked)
                selectedCharSets.Add(numberChars);
            if (checkboxSymbols.Checked)
                selectedCharSets.Add(symbolChars);

            // Ensure at least one character set is selected
            if (selectedCharSets.Count == 0)
            {
                MessageBox.Show("Please select at least one character set for password generation.");
                return string.Empty;
            }

            // Get the desired password length and unique characters count
            int passwordLength = (int)numPassLength.Value;
            int uniqueCharsCount = (int)numUniqueChars.Value;

            // Check if the unique characters is greater than password length
            if (uniqueCharsCount > passwordLength)
            {
                MessageBox.Show("The number of unique characters cannot be greater than the password length.");
                return string.Empty;
            }

            // Check if number of unique characters is bigger than the total number of characters
            int totalUniqueCharsAvailable = selectedCharSets.Sum(set => set.Length);
            if (uniqueCharsCount > totalUniqueCharsAvailable)
            {
                MessageBox.Show($"Cannot generate a password with {uniqueCharsCount} unique characters from the selected character sets. Please choose a number {totalUniqueCharsAvailable} or lower.");
                return string.Empty;
            }

            // Generate the unique characters first
            List<char> availableChars = selectedCharSets.SelectMany(set => set.ToCharArray()).ToList();
            for (int i = 0; i < uniqueCharsCount; i++)
            {
                int randomIndex = rand.Next(availableChars.Count);
                char randomChar = availableChars[randomIndex];
                passwordBuilder.Append(randomChar);
                availableChars.RemoveAt(randomIndex); // Remove the chosen character to ensure uniqueness
            }

            // Generate the remaining characters (which can include duplicates)
            for (int i = uniqueCharsCount; i < passwordLength; i++)
            {
                string charSet = selectedCharSets[rand.Next(selectedCharSets.Count)];
                char randomChar = charSet[rand.Next(charSet.Length)];
                passwordBuilder.Append(randomChar);
            }

            return passwordBuilder.ToString();
        }

        private void buttonCopy_Click_1(object sender, EventArgs e)
        {
            // Check if the text is empty before copying to clipboard
            if (!string.IsNullOrEmpty(textboxPasswordOutput.Text))
            {
                Clipboard.SetText(textboxPasswordOutput.Text);
            }
            else
            {
                MessageBox.Show("No password to copy!");
            }
        }

        private CancellationTokenSource cts = new CancellationTokenSource();
        private async void textboxPasswordOutput_TextChanged(object sender, EventArgs e)
        {
            // Get the password from the textbox
            string password = textboxPasswordOutput.Text;

            // Check if the password is empty
            if (string.IsNullOrEmpty(password))
            {
                panelStrength.Width = 0; // Reset the strength bar
                labelPasswordStrength.Text = ""; // Reset the strength label
                return; // Exit the method
            }

            cts.Cancel(); // Cancel any previous task
            cts = new CancellationTokenSource(); // Create a new cancellation token source

            panelStrength.Width = 0;
            labelPasswordStrength.ForeColor = Color.Black;
            labelPasswordStrength.Text = "Evaluating...";

            try
            {
                var result = await Task.Run(() => Zxcvbn.Core.EvaluatePassword(password), cts.Token);

                // Calculate target width for the inner panel (panelStrength) based on score
                int maxWidth = panelStrengthBar.Width;
                int targetWidth = (int)(maxWidth * (result.Score / 4f));

                // Directly set the width and color based on score
                var newColor = GetStrengthColor(result.Score);

                panelStrength.BackColor = newColor;
                panelStrength.Width = targetWidth;

                // Set the labelPasswordStrength color and text based on score
                labelPasswordStrength.ForeColor = newColor;
                labelPasswordStrength.Text = result.Score switch
                {
                    0 => "Very Weak",
                    1 => "Weak",
                    2 => "Fair",
                    3 => "Good",
                    4 => "Strong",
                    _ => "",
                };
                Debug.WriteLine($"Password strength value: {result.Score}");
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"Strength calculation canceled");
            }
        }
    }
}
