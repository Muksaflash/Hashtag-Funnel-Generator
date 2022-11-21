using System.Reflection;
using static System.Windows.Forms.AxHost;

namespace WinFormsHashtagFunnel_without_.NetFrame_
{
    public partial class Model
    {
        public static Size sizeApp = new Size(1024, 768);
        public string InFilePath = string.Empty;
        public string OutFilePath = string.Empty;
        public int MinFreq;
        public int MaxFreq;
        public int freqInterval;
        public int numberInFunnel;
    }

    public partial class Form1 : Form
    {
        public Model model = new Model();
        public TextBox inPathTextbox = new TextBox();
        public TextBox outPathTextbox = new TextBox(); 
        public TextBox minTextbox = new TextBox();
        public TextBox maxTextbox = new TextBox(); 
        public TextBox freqTextbox = new TextBox();
        public TextBox numberInFunnelTextbox = new TextBox();
        public Form1()
        {
            InitializeComponent();
            Size = Model.sizeApp;
            MaximizeBox = false;
            DoubleBuffered = true;

            var mainLabel = new Label();
            mainLabel.Text = "Добро пожаловать в высокотехнологичный, компьютеризированный мир хештегов!";
            mainLabel.Location = new Point(10, 10);
            mainLabel.AutoSize = true;
            mainLabel.TextAlign = ContentAlignment.TopCenter;
            mainLabel.Anchor = AnchorStyles.Top;
            Controls.Add(mainLabel);

            var inLabel = new Label();
            SetLabel(inLabel, "Выберите текстовый документ(*.txt) с хештегами и частотностью", new Point(10, 70));

            var inPathButton = new Button();
            inPathButton.Size = new Size(120, 41);
            inPathButton.Text = "Select";
            inPathButton.Location = new Point(10, 120);
            inPathButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            inPathButton.Click += SelectFile;
            Controls.Add(inPathButton);

            var outLabel = new Label();
            SetLabel(outLabel, "Выберите папку для финального файла с воронками", new Point(10, 200));

            var outPathButton = new Button();
            outPathButton.Size = new Size(120, 41);
            outPathButton.Text = "Select";
            outPathButton.Location = new Point(10, 250);
            outPathButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            outPathButton.Click += SelectPath;
            Controls.Add(outPathButton);

            inPathTextbox.Size = new Size(400, 60);
            inPathTextbox.MaximumSize = new Size(1000, 200);
            inPathTextbox.MinimumSize = new Size(120, 30);
            inPathTextbox.Location = new Point(140, 120);
            inPathTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(inPathTextbox);

            outPathTextbox.Size = new Size(400, 60);
            outPathTextbox.MaximumSize = new Size(1000, 200);
            outPathTextbox.MinimumSize = new Size(120, 30);
            outPathTextbox.Location = new Point(140, 250);
            outPathTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(outPathTextbox);

            var minLabel = new Label();
            SetLabel(minLabel, "Впишите минимальную частотность воронок", new Point(10, 320));

            SetTextBox(minTextbox, new Point(10, 370));

            var maxLabel = new Label();
            SetLabel(maxLabel, "Впишите максимальную частотность воронок", new Point(10, 440));

            SetTextBox(maxTextbox, new Point(10, 490));

            var freqLabel = new Label();
            SetLabel(freqLabel, "Впишите шаг частотности воронок", new Point(10, 560));

            SetTextBox(freqTextbox, new Point(10, 610));

            var startButton = new Button();
            startButton.Size = new Size(200, 100);
            startButton.Text = "Start";
            startButton.Location = new Point(DisplayRectangle.Width-startButton.Width - 100,
                DisplayRectangle.Height - startButton.Height - 100);
            startButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            startButton.Click += Make;
            Controls.Add(startButton);

            var numberInFunnelLabel = new Label();
            numberInFunnelLabel.Text = "Впишите колличество хештегов в воронке";
            numberInFunnelLabel.Location = new Point(DisplayRectangle.Width - numberInFunnelLabel.Width - 250,
                DisplayRectangle.Height - startButton.Height - 240);
            numberInFunnelLabel.Size = new Size(300, 80);
            numberInFunnelLabel.TextAlign = ContentAlignment.TopCenter;
            numberInFunnelLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Controls.Add(numberInFunnelLabel);

            numberInFunnelTextbox.Size = new Size(200, 60);
            numberInFunnelTextbox.MaximumSize = new Size(300, 200);
            numberInFunnelTextbox.MinimumSize = new Size(120, 30);
            numberInFunnelTextbox.Location = new Point(DisplayRectangle.Width - numberInFunnelTextbox.Width - 100,
                DisplayRectangle.Height - startButton.Height - 150);
            numberInFunnelTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Controls.Add(numberInFunnelTextbox);
        }

        private void Make(object? sender, EventArgs e)
        {
            try
            {
                model.MinFreq = int.Parse(minTextbox.Text);
                model.MaxFreq = int.Parse(maxTextbox.Text);
                model.freqInterval = int.Parse(freqTextbox.Text);
                model.numberInFunnel = int.Parse(numberInFunnelTextbox.Text);
            }
            catch
            {
                MessageBox.Show("Пожалуйста, проверьте корректность заполнения форм",
                "Ошибка", MessageBoxButtons.OK);
                return;
            }
            FunnelLogic.Make(sender, e);
        }

        private void SetTextBox(TextBox textBox, Point location)
        {
            textBox.Size = new Size(400, 60);
            textBox.MaximumSize = new Size(1000, 200);
            textBox.MinimumSize = new Size(120, 30);
            textBox.Location = location;
            textBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(textBox);
        }

        private void SetLabel(Label label, string text, Point location)
        {
            label.Text = text;
            label.Location = location;
            label.AutoSize = true;
            label.TextAlign = ContentAlignment.TopCenter;
            label.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            Controls.Add(label);
        }

        private void SelectFile(object? sender, EventArgs e)
        {
            var fileContent = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    model.InFilePath = openFileDialog.FileName;
                }
            }
            inPathTextbox.Text = model.InFilePath;
        }

        private void SelectPath(object? sender, EventArgs e)
        {
            var fileContent = string.Empty;
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    model.OutFilePath = folderBrowserDialog.SelectedPath;
                }
            }
            outPathTextbox.Text = model.OutFilePath;
        }
    }
}