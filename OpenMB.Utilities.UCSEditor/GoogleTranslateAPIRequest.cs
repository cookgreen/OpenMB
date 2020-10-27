using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Utilities.UCSEditor
{
    public class GoogleTranslateAPIRequest
    {
        private bool isAuto = false;
        private string baseUrl = "http://translate.google.cn/translate_a/single?client=gtx&dt=t&dj=1&ie=UTF-8&sl={0}&tl={1}&q={2}";
        private string baseUrlAuto = "http://translate.google.cn/translate_a/single?client=gtx&dt=t&dj=1&ie=UTF-8&sl=auto&tl={0}&q={1}";
        private string srcLangID;
        private string destLangID;
        public string DestLangID
        {
            get { return destLangID; }
            set { destLangID = value; }
        }
        private BackgroundWorker worker;
        public event Action<string> TranslateFinished;
        public GoogleTranslateAPIRequest(string srcLangID, string destLangID)
        {
            this.srcLangID = srcLangID;
            this.destLangID = destLangID;
        }

        public GoogleTranslateAPIRequest(string destLangID)
        {
            isAuto = true;
            this.destLangID = destLangID;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] arr = e.Result as object[];
            if (arr[0].ToString() == "done")
            {
                string json = arr[1].ToString();
                GoogleTranslateAPIResponse response = JsonConvert.DeserializeObject<GoogleTranslateAPIResponse>(json);
                if (response != null && response.sentences.Count > 0)
                {
                    TranslateFinished?.Invoke(response.sentences[0].trans);
                }
                else
                {
                    TranslateFinished?.Invoke("No Suggestion");
                }
            }
            else
            {
                TranslateFinished?.Invoke("No Suggestion");
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string url = !isAuto ? string.Format(baseUrl, srcLangID, destLangID) : string.Format(baseUrlAuto, destLangID, e.Argument.ToString());
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "UserAgent";
                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                string responseJson = null;
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseJson = reader.ReadToEnd();
                }
                e.Result = new object[] {
                    "done",
                    responseJson
                };
            }
            catch (Exception ex)
            {
                e.Result = new object[] {
                    "error",
                    ex.Message
                };
            }
        }

        public void TranslateAsync(string text)
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync(text);
        }
    }
}
