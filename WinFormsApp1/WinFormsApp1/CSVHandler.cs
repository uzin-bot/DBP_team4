using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace WinFormsApp1
{
    internal class CSVHandler
    {
        // CSV 파일의 각 행을 저장할 List<string[]> (각 행은 string[]로 저장)
        public List<string[]> lines = new List<string[]>();

        // CSV파일을 읽을 StreamReader객체
        public StreamReader sr;

        public int count = 0;

        // Load 메서드는 파일을 로드하고 데이터를 로드하는 함수 호출
        public void Load()
        {
            LoadFile(); // 파일로드
            LoadCSV(); // CSV파일 읽고 데이터를 lines에 저장하는 함수 호출

        }

        // 소멸자 자동으로 해제
        ~CSVHandler()
        {
            sr.Close ();
        }

        // CSV파일을 읽고 데이터를 lines에 저장하는 메서드
        public void LoadCSV()
        {
            // 파일을 끝까지 읽을 때 까지 반복
            while (sr.EndOfStream == false)
            {
                // 한줄을 읽어옴
                var line = sr.ReadLine();

                count++;
                
                // 한 줄을 , 로 나누어 배열로 전환
                var values = SplitCSV(line);

                // 변환된 배열을 lines에 추가
                lines.Add(values);
            }
        }

        // 따옴표 사이의 쉼표는 스플릿하지 않기
        private string[] SplitCSV(string line)
        {
            List<string> fields = new List<string>();
            string field = "";
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;  // 따옴표 상태 토글
                    field += c;
                }
                else if (c == ',' && !inQuotes)  // 따옴표 밖의 쉼표만 스플릿!
                {
                    //따옴표 제거 및 공백 제거
                    field = field.Trim().Replace("\"", "");
                    fields.Add(field);
                    field = "";
                }
                else
                {
                    field += c;
                }
            }

            // 마지막 필드 추가
            field = field.Trim().Replace("\"", "");
            fields.Add(field);

            return fields.ToArray();
        }

        // 파일을 선택하고 StreamReader를 반환하는 메서드
        private StreamReader LoadFile()
        {
            // 파일 열기 대화상자를 띄우는 OpenFileDialog 객체생성
            var FD = new System.Windows.Forms.OpenFileDialog();

            // 파일을 선택하고 "열기"버튼을 클릭하면
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 선택한 파일 경로를 가지고옴
                string fileToOpen = FD.FileName;

                // 파일 경로를 사용해서 StreamReader 객체생성
                // FileStream을 통해 파일을 열고 인코딩은 기본값을 사용
                sr = new StreamReader(new FileStream(FD.FileName, FileMode.Open), Encoding.Default);

                return sr;
            }

            return null;
        }

    }
}
