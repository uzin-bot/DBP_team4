using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    class CSVSearch
    {

        // 검색 조건에 맞는 데이터를 필터링하는 메서드
        public List<string[]> FilterData(List<string[]> csvData, int columnIndex, string keyword)
        {
            var results = new List<string[]>();

            // 검색 조건에 맞는 데이터 찾기
            foreach (var row in csvData)
            {
                // 부분 일치 검색
                if (row[columnIndex].Contains(keyword))
                {
                    results.Add(row);
                }

            }

            return results;
        }
    }
}
