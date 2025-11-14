using Microsoft.VisualBasic.Devices;
using System.Collections;
using System.Security.Cryptography.Xml;
using System.Text;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // CSVHandler 클래스 선언
        private CSVHandler csvHandler = new CSVHandler();
        private CSVSearch csvSearch = new CSVSearch();
        private SearchOptionsWizard wizard;

        public Form1()
        {
            InitializeComponent();
        }

        // 파일의 첫 번째 행을 ListBox에 표시
        private void UpdateHeadlerInfo()
        {
            // 초기화
            listBox1.Items.Clear();

            // 1번째 행 받아옴
            string[] list = csvHandler.lines[0]; // 이거 수정

            // 배열 원소들 하나하나 item으로 추가
            foreach (var item in list)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // CSV 파일 로드
            csvHandler.Load();

            // 첫번째 행을 리스트 박스에표시
            UpdateHeadlerInfo();

            wizard = new SearchOptionsWizard(csvHandler.lines);

        }

        // 검색 결과 화면에 표시
        private void DisplaySearchResults(List<string[]> results, int selectedColumnIndex)
        {
            // 결과 표시용 ListBox (새로 추가해야 함)
            // 이 예제에서는 listBox2를 사용한다고 가정

            listBox2.Items.Clear();

            // 검색 결과를 ListBox에 추가
            foreach (var row in results)
            {

                string rowText = string.Join(" | ", row);
                listBox2.Items.Add(rowText);

            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int columnIndex = listBox1.SelectedIndex;
                string keyword = textBox1.Text;
                label5.Text = $"총 {csvHandler.count}건의 데이터";

                List<string[]> searchResults;

                
                if(comboBox1.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(keyword))
                {
                    MessageBox.Show("리저드 우선 검색 됩니다.");
                }

                // ComboBox에서 선택했는지 확인
                if (comboBox1.SelectedIndex != -1)
                {
                    string selectedValue = comboBox1.SelectedItem.ToString();


                    // Wizard에서 필터링
                    searchResults = wizard.FilterBySelectedValue(selectedValue);
                    columnIndex = wizard.GetSelectedColumnIndex();

                    
                }
                else
                {
                    // CSVSearch를 사용해 데이터 필터링
                    searchResults = csvSearch.FilterData(
                        csvHandler.lines,
                        columnIndex,
                        keyword
                    );
                }

                label5.Text = $"총 {searchResults.Count}건의 데이터";
                // 검색 결과 표시
                DisplaySearchResults(searchResults, columnIndex);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            listBox2.Items.Clear();
            comboBox1.SelectedIndex = -1;
            label5.Text = $"총 0건의 데이터";

            int columnIndex = listBox1.SelectedIndex;

            // Wizard에서 고유값 가져오기
            List<string> uniqueValues = wizard.GetUniqueValues(columnIndex);


            // ComboBox에 고유값 표시
            comboBox1.Items.Clear();
            foreach (var value in uniqueValues)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    comboBox1.Items.Add(value);
                }

            }
        }
    }
}
