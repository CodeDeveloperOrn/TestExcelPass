using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Pdf.Exporting.XPS.Schema;
using Spire.Xls;

namespace TestExcelPass
{
    public partial class Form1 : Form
    {
        public CancellationTokenSource cts = new CancellationTokenSource();
        public CancellationToken token;

        public delegate void SignalToMainFormHandler();
        public delegate void SignalToMainFormWithParamHandler(int param);
        public event SignalToMainFormHandler callTypeCurrentPassword;
        public event SignalToMainFormHandler callOutFindedPass;
        public event SignalToMainFormWithParamHandler messageThreadWasClose;

        private string currentPassword { get; set; }
        private int numbDigit { get; set; }
        private string findedPass { get; set; }
        private Stopwatch timerWatch { get; set; }

        public Form1()
        {
            InitializeComponent();

            token = cts.Token;
            callTypeCurrentPassword += Form1_callTypeCurrentPassword;
            callOutFindedPass += Form1_callOutFindedPass;
            messageThreadWasClose += Form1_messageThreadWasClose;
            timerWatch = new Stopwatch();
        }

        private void Form1_messageThreadWasClose(int param)
        {
            timerWatch.Stop();
            var elaps = timerWatch.Elapsed;
            label6.Text = string.Format("Поиск завершён за {0}:{1}:{2}", (int)elaps.TotalHours, elaps.Minutes, elaps.Seconds);
        }

        private void Form1_callOutFindedPass()
        {
            textBox4.Text = findedPass;
        }

        private void Form1_callTypeCurrentPassword()
        {
            label3.Text = currentPassword;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timerWatch.Start();
            Task.Run(() => procFindPass());
        }

        private async void procFindPass()
        {
            await Task.CompletedTask;
            if (string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
            {
                return;
            }
            // Количество разрядов
            numbDigit = Convert.ToInt32(!string.IsNullOrWhiteSpace(textBox3.Text) ? textBox3.Text : "0");
            List<Task> lstTask = new List<Task>();
            for (int i = 0; i < Instrument.symbols.Count(); i++)
            {
                lstTask.Add(Task.Run(() => FindPassword(numbDigit, (byte)i)));
            }

            Task.WaitAll(lstTask.ToArray());
            await Task.CompletedTask;
            messageThreadWasClose(0);
        }

        private async void FindPassword(int countBit, byte valueHighBit)
        {
            Workbook workbook = new Workbook();

            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream file = File.OpenRead(textBox1.Text))
                {
                    file.CopyTo(memory);
                }

                workbook.LoadFromStream(memory);
                Worksheet sheet = workbook.Worksheets[textBox2.Text];

                SearcherPass searcherPass = new SearcherPass();
                searcherPass.SetBitDefault(countBit, valueHighBit);
                int counter = 0;

                await Task.CompletedTask;
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        sheet.Dispose();
                        workbook.Dispose();
                        MessageBox.Show("Выполнена остановка поиска!");
                        break;
                    }

                    byte[] arrFromText = searcherPass.GetByteArray();
                    string tempPas = searcherPass.GetString();

                    try
                    {
                        sheet.Unprotect(tempPas);
                        findedPass = tempPas;
                        callOutFindedPass();
                    }
                    catch
                    {
                        if (!searcherPass.IncrementByteArray()) {
                            break;
                        };
                        currentPassword = tempPas;
                        counter++;
                        await Task.CompletedTask;
                        continue;
                    }

                    await Task.CompletedTask;
                    Console.WriteLine("Асинхронный процесс закрыт!");
                    try
                    {
                        // Вызов отмены прочих потоков
                        cts.Cancel();
                    }
                    catch { }
                    break;
                }

                //messageThreadWasClose(valueHighBit);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                cts.Cancel();
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.callTypeCurrentPassword();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
        }
    }
}
