using leehaeun.UIHelpers;
using System.Text.Json;


namespace leehaeun
{
    public partial class SearchAddressForm : Form
    {
        public SearchAddressForm()
        {
            InitializeComponent();
            SearchAddressFormUIHelper.ApplyStyles(this);
            ResultBox.DoubleClick += ResultBox_DoubleClick;
        }

        private readonly HttpClient client = new HttpClient();
        private string serviceKey = "devU01TX0FVVEgyMDI1MTEyMzIwMzYxMDExNjQ4NDU=";
        public string selectedAddress, selectedZipCode;

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            string keyword = AddressBox.Text.Trim();
            await SearchingAddress(keyword);
        }

        private async Task SearchingAddress(string keyword)
        {
            try
            {
                string url =
                    "https://www.juso.go.kr/addrlink/addrLinkApi.do" +
                    "?confmKey=" + serviceKey +
                    "&currentPage=1" +
                    "&countPerPage=20" +
                    "&keyword=" + Uri.EscapeDataString(keyword) +
                    "&resultType=json";

                string response = await client.GetStringAsync(url);

                ResultBox.Items.Clear();

                using (JsonDocument doc = JsonDocument.Parse(response))
                {
                    var root = doc.RootElement;

                    if (!root.TryGetProperty("results", out var results))
                    {
                        ResultBox.Items.Add("API 오류");
                        return;
                    }

                    if (!results.TryGetProperty("juso", out var jusoArray) ||
                        jusoArray.ValueKind != JsonValueKind.Array)
                    {
                        ResultBox.Items.Add("검색 결과 없음");
                        return;
                    }

                    foreach (var item in jusoArray.EnumerateArray())
                    {
                        string zip = item.GetProperty("zipNo").GetString();
                        string address = item.GetProperty("roadAddr").GetString();

                        ResultBox.Items.Add($"{zip} | {address}");
                    }
                }

                if (ResultBox.Items.Count == 0)
                    ResultBox.Items.Add("검색 결과 없음");

            }
            catch (Exception ex)
            {
                MessageBox.Show("오류: " + ex.Message);
            }
        }

        private void ResultBox_DoubleClick(object sender, EventArgs e)
        {
            if (ResultBox.SelectedItem == null) return;

            string line = ResultBox.SelectedItem.ToString();
            string[] split = line.Split('|');

            selectedZipCode = split[0].Trim();
            selectedAddress = split[1].Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}