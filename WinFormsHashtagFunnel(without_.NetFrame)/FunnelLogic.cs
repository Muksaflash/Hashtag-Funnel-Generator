using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsHashtagFunnel_without_.NetFrame_
{
    internal class FunnelLogic
    {
        public static void Make(object? sender, EventArgs e)
        {
            var hashtagTXTPath = Program.form.model.InFilePath;
            StreamReader sr = new StreamReader(hashtagTXTPath);
            List<string?> listHashtags = new();
            while (!sr.EndOfStream)
            {
                listHashtags.Add(sr.ReadLine());
            }
            sr.Close();
            ILookup<int, string> hashtagFreq = listHashtags
                .Where(line => line != null && line != string.Empty)
                .ToLookup
                (line =>
                {
                    int x;
                    try
                    {
                        if(line != null)
                            x = int.Parse(line.Split('\t')[1]);
                        else
                            x = 0;
                    }
                    catch
                    {
                        return -1;
                    }
                    return x;
                },
                line =>
                {
                    string x;
                    if (line != null)
                        x = line.Split('\t')[0];
                    else
                        x = "#tytyvvcjkksskfhcls";
                    return x;
                });
            var floorFreq = Program.form.model.MinFreq;
            var topFreq = Program.form.model.MaxFreq;
            var freqStep = Program.form.model.freqInterval;
            var hashtagFunnelNumber = Program.form.model.numberInFunnel;

            var hashtagFreq1 = hashtagFreq.Where(x => x.Key >= floorFreq && x.Key <= topFreq);
            var count = hashtagFreq1.ToList().Count;
            var hashtagFreq2 = hashtagFreq1.ToDictionary(group => group.Key, group => group.ToList())
                .OrderBy(group => group.Key);
            int nextBound;

            int hashtagFunnelCount;
            int fullFunnelCount = 0;
            int funnelCount = 0;
            bool isDictEmpty = false;
            while (!isDictEmpty)
            {
                hashtagFunnelCount = hashtagFunnelNumber;
                nextBound = floorFreq;
                File.AppendAllText(@"C:\\Users\\Александр\\Desktop\\Воронка.txt", "\n");
                foreach (var item in hashtagFreq2)
                {
                    if (item.Key >= nextBound)
                    {
                        if (item.Value.Count != 0)
                        {
                            File.AppendAllText(@"C:\\Users\\Александр\\Desktop\\Воронка.txt", item.Value[0] + '\t' + item.Key + '\n');
                            item.Value.RemoveAt(0);
                            nextBound = item.Key + freqStep;
                            if (--hashtagFunnelCount == 0)
                            {
                                fullFunnelCount++;
                                break;
                            }
                        }
                    }
                }
                foreach (var item in hashtagFreq2)
                {
                    if (item.Value.Count != 0)
                    {
                        isDictEmpty = false;
                        break;
                    }
                    isDictEmpty = true;
                }
                funnelCount++;
            }
            MessageBox.Show("Создано " + fullFunnelCount + " воронок по " + hashtagFunnelNumber
                + " хэштегов и ещё " +
                +funnelCount + " поменьше" + '\n' + '\n' +
                "Хештеги записаны в файл \"Воронка.txt\"",
                "Работа успешно завершена!", MessageBoxButtons.OK);
        }
    }
}
