using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    class SearchOptionsWizard
    {
        // 파일에서 로드한 모든 데이터 (2차원 배열리스트)
        private List<string[]> csvData;

        // 칼럼 인덱스
        private int selectedColumnIndex = -1;

        // 사용자가 선택한 값
        private string selectedValue = "";


        //생성자
        public SearchOptionsWizard(List<string[]> csvData)
        {
            this.csvData = csvData;
        }

        /// 특정 컬럼의 고유값(Unique Values)을 가져오는 메서드
        public List<string> GetUniqueValues(int columnIndex)
        {
            // 고유값 저장할 리스트
            var uniqueValues = new List<string>();

            // 이미추가된 값들을 빠르게 검색하기위한 HashSet
            var seen = new HashSet<string>();

            // 선택한 인덱스 칼럼 인덱스 저장
            selectedColumnIndex = columnIndex;

            int rowIndex = 0;
            //데이터 순회
            foreach (var row in csvData)
            {
                if (rowIndex == 0)
                {
                    rowIndex++;
                    continue;
                }

                string value = row[columnIndex];

                value = value.Trim(); // 공백제거

                if (!seen.Contains(value)) // 중복이 아니면
                {
                    seen.Add(value);
                    uniqueValues.Add(value);
                }
            }
            
            // 가나다 순으로정렬하고, 리스트로 반환
            return uniqueValues.OrderBy(x => x).ToList();
        }


        /// 선택된 값으로 데이터 필터링
        public List<string[]> FilterBySelectedValue(string selectedValue)
        {
            var results = new List<string[]>();


            this.selectedValue = selectedValue;

            // 필터링
            foreach (var row in csvData)
            {

                // 정확한 일치 검색 (완전 일치)
                if (row[selectedColumnIndex].Trim().Equals(selectedValue, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(row);
                }
            }

            return results;
        }

        //현재 선택된 컬럼 인덱스 반환
        public int GetSelectedColumnIndex()
        {
            return selectedColumnIndex;
        }



    }
}

