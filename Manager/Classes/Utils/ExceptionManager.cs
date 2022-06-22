using System;
using System.Text;
using System.Windows;

namespace Manager.Classes.Utils
{
    class ExceptionManager
    {
        // Instance
        private static ExceptionManager exceptionManager = null;



        /// <summary>
        /// Get instance 
        /// </summary>
        /// <returns></returns>
        public static ExceptionManager getInstance()
        {
            if (exceptionManager == null)
                exceptionManager = new ExceptionManager();

            return exceptionManager;
        }



        /// <summary>
        /// Exception message box 출력
        /// </summary>
        /// <param name="exception"></param>
        public void showMessageBox(Exception exception)
        {
            const string DEFAULT_TITLE = "RHYA.Network ExceptionManager";
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("서울 북부 지원교육청 알리미 관리자 클라이언트에서 예외가 발생했습니다. 자세한 정보는 다음 내용을 참조하십시오.");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine(exception.Message);


            MessageBox.Show(stringBuilder.ToString(), DEFAULT_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
        }



        /// <summary>
        /// 프로그램 종료
        /// </summary>
        /// <param name="exitCode">종료 코드</param>
        public void exitProgram(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }
}
