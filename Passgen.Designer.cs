namespace passgen
{
    partial class Passgen
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Passgen));
            boxOptions = new GroupBox();
            boxUniqueChars = new GroupBox();
            numUniqueChars = new NumericUpDown();
            boxPassLength = new GroupBox();
            numPassLength = new NumericUpDown();
            checkboxSymbols = new CheckBox();
            checkboxNumbers = new CheckBox();
            checkboxUppercase = new CheckBox();
            checkboxLowercase = new CheckBox();
            labelPassword = new Label();
            buttonGenerate = new Button();
            buttonCopy = new Button();
            labelStrengthTitle = new Label();
            panelStrengthBar = new Panel();
            panelStrength = new Panel();
            labelPasswordStrength = new Label();
            textboxPasswordOutput = new RichTextBox();
            boxOptions.SuspendLayout();
            boxUniqueChars.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numUniqueChars).BeginInit();
            boxPassLength.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPassLength).BeginInit();
            panelStrengthBar.SuspendLayout();
            SuspendLayout();
            // 
            // boxOptions
            // 
            boxOptions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            boxOptions.Controls.Add(boxUniqueChars);
            boxOptions.Controls.Add(boxPassLength);
            boxOptions.Controls.Add(checkboxSymbols);
            boxOptions.Controls.Add(checkboxNumbers);
            boxOptions.Controls.Add(checkboxUppercase);
            boxOptions.Controls.Add(checkboxLowercase);
            boxOptions.Location = new Point(12, 12);
            boxOptions.Name = "boxOptions";
            boxOptions.Size = new Size(358, 150);
            boxOptions.TabIndex = 0;
            boxOptions.TabStop = false;
            boxOptions.Text = "Options";
            // 
            // boxUniqueChars
            // 
            boxUniqueChars.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            boxUniqueChars.Controls.Add(numUniqueChars);
            boxUniqueChars.Location = new Point(193, 86);
            boxUniqueChars.Name = "boxUniqueChars";
            boxUniqueChars.Size = new Size(144, 58);
            boxUniqueChars.TabIndex = 6;
            boxUniqueChars.TabStop = false;
            boxUniqueChars.Text = "Unique Characters";
            // 
            // numUniqueChars
            // 
            numUniqueChars.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numUniqueChars.Location = new Point(6, 25);
            numUniqueChars.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numUniqueChars.Name = "numUniqueChars";
            numUniqueChars.Size = new Size(132, 27);
            numUniqueChars.TabIndex = 4;
            numUniqueChars.Value = new decimal(new int[] { 12, 0, 0, 0 });
            // 
            // boxPassLength
            // 
            boxPassLength.Controls.Add(numPassLength);
            boxPassLength.Location = new Point(6, 86);
            boxPassLength.Name = "boxPassLength";
            boxPassLength.Size = new Size(144, 58);
            boxPassLength.TabIndex = 5;
            boxPassLength.TabStop = false;
            boxPassLength.Text = "Password Length";
            // 
            // numPassLength
            // 
            numPassLength.Location = new Point(6, 25);
            numPassLength.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numPassLength.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numPassLength.Name = "numPassLength";
            numPassLength.Size = new Size(132, 27);
            numPassLength.TabIndex = 4;
            numPassLength.Value = new decimal(new int[] { 12, 0, 0, 0 });
            // 
            // checkboxSymbols
            // 
            checkboxSymbols.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkboxSymbols.AutoSize = true;
            checkboxSymbols.Checked = true;
            checkboxSymbols.CheckState = CheckState.Checked;
            checkboxSymbols.Location = new Point(193, 56);
            checkboxSymbols.Name = "checkboxSymbols";
            checkboxSymbols.Size = new Size(159, 24);
            checkboxSymbols.TabIndex = 3;
            checkboxSymbols.Text = "Symbols (@#$ etc.)";
            checkboxSymbols.UseVisualStyleBackColor = true;
            // 
            // checkboxNumbers
            // 
            checkboxNumbers.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkboxNumbers.AutoSize = true;
            checkboxNumbers.Checked = true;
            checkboxNumbers.CheckState = CheckState.Checked;
            checkboxNumbers.Location = new Point(193, 26);
            checkboxNumbers.Name = "checkboxNumbers";
            checkboxNumbers.Size = new Size(127, 24);
            checkboxNumbers.TabIndex = 2;
            checkboxNumbers.Text = "Numbers (0-9)";
            checkboxNumbers.UseVisualStyleBackColor = true;
            // 
            // checkboxUppercase
            // 
            checkboxUppercase.AutoSize = true;
            checkboxUppercase.Checked = true;
            checkboxUppercase.CheckState = CheckState.Checked;
            checkboxUppercase.Location = new Point(6, 56);
            checkboxUppercase.Name = "checkboxUppercase";
            checkboxUppercase.Size = new Size(140, 24);
            checkboxUppercase.TabIndex = 1;
            checkboxUppercase.Text = "Uppercase (A-Z)";
            checkboxUppercase.UseVisualStyleBackColor = true;
            // 
            // checkboxLowercase
            // 
            checkboxLowercase.AutoSize = true;
            checkboxLowercase.Checked = true;
            checkboxLowercase.CheckState = CheckState.Checked;
            checkboxLowercase.Location = new Point(6, 26);
            checkboxLowercase.Name = "checkboxLowercase";
            checkboxLowercase.Size = new Size(135, 24);
            checkboxLowercase.TabIndex = 0;
            checkboxLowercase.Text = "Lowercase (a-z)";
            checkboxLowercase.UseVisualStyleBackColor = true;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(12, 165);
            labelPassword.Name = "labelPassword";
            labelPassword.Padding = new Padding(1);
            labelPassword.Size = new Size(75, 22);
            labelPassword.TabIndex = 2;
            labelPassword.Text = "Password:";
            // 
            // buttonGenerate
            // 
            buttonGenerate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonGenerate.Location = new Point(176, 233);
            buttonGenerate.Name = "buttonGenerate";
            buttonGenerate.Size = new Size(94, 29);
            buttonGenerate.TabIndex = 4;
            buttonGenerate.Text = "Generate";
            buttonGenerate.Click += buttonGenerate_Click_1;
            // 
            // buttonCopy
            // 
            buttonCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCopy.Location = new Point(276, 233);
            buttonCopy.Name = "buttonCopy";
            buttonCopy.Size = new Size(94, 29);
            buttonCopy.TabIndex = 5;
            buttonCopy.Text = "Copy";
            buttonCopy.Click += buttonCopy_Click_1;
            // 
            // labelStrengthTitle
            // 
            labelStrengthTitle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelStrengthTitle.AutoSize = true;
            labelStrengthTitle.Location = new Point(12, 237);
            labelStrengthTitle.Name = "labelStrengthTitle";
            labelStrengthTitle.Padding = new Padding(1);
            labelStrengthTitle.Size = new Size(70, 22);
            labelStrengthTitle.TabIndex = 3;
            labelStrengthTitle.Text = "Strength:";
            // 
            // panelStrengthBar
            // 
            panelStrengthBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelStrengthBar.Controls.Add(panelStrength);
            panelStrengthBar.Location = new Point(12, 217);
            panelStrengthBar.Name = "panelStrengthBar";
            panelStrengthBar.Size = new Size(358, 10);
            panelStrengthBar.TabIndex = 6;
            // 
            // panelStrength
            // 
            panelStrength.BackColor = SystemColors.GrayText;
            panelStrength.BorderStyle = BorderStyle.FixedSingle;
            panelStrength.Location = new Point(-1, -1);
            panelStrength.Name = "panelStrength";
            panelStrength.Size = new Size(0, 10);
            panelStrength.TabIndex = 7;
            // 
            // labelPasswordStrength
            // 
            labelPasswordStrength.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelPasswordStrength.AutoSize = true;
            labelPasswordStrength.Font = new Font("Segoe UI Variable Text", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelPasswordStrength.ForeColor = Color.Red;
            labelPasswordStrength.Location = new Point(79, 237);
            labelPasswordStrength.Name = "labelPasswordStrength";
            labelPasswordStrength.Padding = new Padding(1);
            labelPasswordStrength.Size = new Size(2, 22);
            labelPasswordStrength.TabIndex = 7;
            // 
            // textboxPasswordOutput
            // 
            textboxPasswordOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textboxPasswordOutput.Location = new Point(12, 190);
            textboxPasswordOutput.Multiline = false;
            textboxPasswordOutput.Name = "textboxPasswordOutput";
            textboxPasswordOutput.Size = new Size(358, 27);
            textboxPasswordOutput.TabIndex = 9;
            textboxPasswordOutput.Text = "";
            textboxPasswordOutput.TextChanged += textboxPasswordOutput_TextChanged;
            // 
            // Passgen
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 273);
            Controls.Add(textboxPasswordOutput);
            Controls.Add(labelPasswordStrength);
            Controls.Add(panelStrengthBar);
            Controls.Add(buttonCopy);
            Controls.Add(buttonGenerate);
            Controls.Add(labelStrengthTitle);
            Controls.Add(labelPassword);
            Controls.Add(boxOptions);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MaximumSize = new Size(400, 320);
            MinimumSize = new Size(400, 320);
            Name = "Passgen";
            Text = "Password Generator";
            boxOptions.ResumeLayout(false);
            boxOptions.PerformLayout();
            boxUniqueChars.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numUniqueChars).EndInit();
            boxPassLength.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numPassLength).EndInit();
            panelStrengthBar.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox boxOptions;
        private CheckBox checkboxLowercase;
        private CheckBox checkboxNumbers;
        private CheckBox checkboxUppercase;
        private CheckBox checkboxSymbols;
        private GroupBox boxPassLength;
        private NumericUpDown numPassLength;
        private GroupBox boxUniqueChars;
        private NumericUpDown numUniqueChars;
        private Label labelPassword;
        private Button buttonGenerate;
        private Button buttonCopy;
        private Label labelStrengthTitle;
        private Panel panelStrengthBar;
        private Panel panelStrength;
        private Label labelPasswordStrength;
        private RichTextBox textboxPasswordOutput;
    }
}