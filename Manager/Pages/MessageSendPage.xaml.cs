using Manager.Classes.Utils;
using Manager.Windows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Manager.Pages
{
    /// <summary>
    /// MessageSendPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MessageSendPage : Page
    {
        // Text color
        // ================================================= //
        private readonly string COLOR_BLACK = "#000000";
        private readonly string COLOR_BLUE = "#FF006BFF";
        // ================================================= //

        // 마우스 더블클릭 변수
        private int doubleClickIndex = 0;
        private int clickNum = 0; // 클릭 횟수
        private long Firsttime = 0; // 첫번째 클릭시간

        // 이벤트 막기 변수
        private bool noEventForCheckbox = false;

        // TreeView 데이터
        private ObservableCollection<NotificationSendTargetVO> officeList1 = new ObservableCollection<NotificationSendTargetVO>();
        private ObservableCollection<NotificationSendTargetVO> officeList2 = new ObservableCollection<NotificationSendTargetVO>();

        // MainWindow
        private MainWindow mainWindow;




        public MessageSendPage(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
        }




        /// <summary>
        /// 페이지 로딩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // Item 개수 설정
            Mediator.NotifyColleagues("_MAIN_WINDOW_SET_ITEM_COUNT_", "");

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
            mainWindow.loadingProgressbarPanel.setTitle("초기화 중...");

            // 데이터 초기화
            officeList1.Clear();
            officeList2.Clear();
            await Task.Run(() => { reloadTreeViewData(); });

            // ComboBox 초기화
            officeTypeComboBox.SelectedIndex = 0;

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
        }



        /// <summary>
        /// TreeView 데이터 초기화
        /// </summary>
        private void reloadTreeViewData()
        {
            foreach (string regNum in AdminLoginManager.getInstance().defaultDataVODict.Keys)
            {
                DefaultDataVO defaultDataVO = AdminLoginManager.getInstance().defaultDataVODict[regNum];
                if (defaultDataVO.type.Equals("Topics2"))
                    System.Windows.Application.Current.Dispatcher.Invoke(() => officeList1.Add(new NotificationSendTargetVO() { officeName = defaultDataVO.name, officeRegNum = regNum, IsChecked = false, txtColor = COLOR_BLACK }));
                else
                    System.Windows.Application.Current.Dispatcher.Invoke(() => officeList2.Add(new NotificationSendTargetVO() { officeName = defaultDataVO.name, officeRegNum = regNum, IsChecked = false, txtColor = COLOR_BLACK }));
            }
        }



        /// <summary>
        /// 선택해제 버튼 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allunSelectButton_Click(object sender, RoutedEventArgs e)
        {
            allUnCheckForOfficeItem();
            allSelectCheckbox.IsChecked = false;
        }



        /// <summary>
        /// ListView 표시 데이터 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void officeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get comboBox item value
            System.Windows.Controls.ComboBox a = sender as System.Windows.Controls.ComboBox;
            ComboBoxItem c = a.SelectedItem as ComboBoxItem;
            // ComboBox content
            switch ((string)c.Content)
            {
                case "학원":
                    {
                        officeSelectListView.ItemsSource = officeList1;
                        await Task.Run(() => selectItemCheck());
                        break;
                    }

                case "교습소":
                    {
                        officeSelectListView.ItemsSource = officeList2;
                        await Task.Run(() => selectItemCheck());
                        break;
                    }
            }
        }



        // ListView_1 Click CurrentIndex
        // ============================================================================================== //
        private int GetCurrentIndex_ListView(GetPositionDelegate_ListView getPosition, ListView listView)
        {
            if (getPosition == null) return -1;

            int index = -1;
            for (int i = 0; i < listView.Items.Count; ++i)
            {
                ListViewItem item = GetListViewItem(i, listView);
                if (this.IsMouseOverTarget_ListView(item, getPosition))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        private bool IsMouseOverTarget_ListView(Visual target, GetPositionDelegate_ListView getPosition)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            Point mousePos = getPosition((IInputElement)target);
            return bounds.Contains(mousePos);
        }
        delegate Point GetPositionDelegate_ListView(IInputElement element);
        ListViewItem GetListViewItem(int index, ListView listView)
        {
            if (listView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) { return null; }
            return listView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }
        // ============================================================================================== //



        // 마우스 더블 클릭 감지
        // ------------------------------------------------------------------------------------------------------ //
        private int clickNth()
        {
            clickNum++; // 클릭 횟수 증가

            // 현재시각 CurrentTime에 저장 
            long CurrentTime = DateTime.Now.Ticks;

            // 원클릭 시 실행
            if (CurrentTime - Firsttime > 4000000) // 0.4초
            {
                clickNum = 1;
                Firsttime = CurrentTime;
            }

            return clickNum;
        }
        // ------------------------------------------------------------------------------------------------------ //



        /// <summary>
        /// Office select listview click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void officeSelectListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int index = GetCurrentIndex_ListView(e.GetPosition, officeSelectListView);
            if (!(index == -1) && clickNth() == 2 && index == doubleClickIndex && doubleClickIndex != -1)
            {
                doubleClickIndex = -1;
                if (officeTypeComboBox.SelectedIndex == 0)
                {
                    if (officeList1[index].IsChecked)
                    {
                        officeList1[index].txtColor = COLOR_BLACK;
                        officeList1[index].IsChecked = false;
                    }
                    else
                    {
                        officeList1[index].txtColor = COLOR_BLUE;
                        officeList1[index].IsChecked = true;
                    }
                }
                else
                {
                    if (officeList2[index].IsChecked)
                    {
                        officeList2[index].txtColor = COLOR_BLACK;
                        officeList2[index].IsChecked = false;
                    }
                    else
                    {
                        officeList2[index].txtColor = COLOR_BLUE;
                        officeList2[index].IsChecked = true;
                    }
                }
            }

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
            mainWindow.loadingProgressbarPanel.setTitle("작업 처리 중...");

            await Task.Run(() => selectItemCheck());

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;

            officeSelectListView.Items.Refresh();
            doubleClickIndex = index;
        }



        /// <summary>
        /// 전체선택 CheckBox 선택 해제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (noEventForCheckbox) return;
            allUnCheckForOfficeItem();
        }
        /// <summary>
        /// 전체선택 CheckBox 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (noEventForCheckbox) return;

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
            mainWindow.loadingProgressbarPanel.setTitle("작업 처리 중...");

            int comboBoxIndex = officeTypeComboBox.SelectedIndex;
            await Task.Run(() =>
            {
                if (comboBoxIndex == 0)
                    foreach (NotificationSendTargetVO notificationSendTargetVO in officeList1)
                        Application.Current.Dispatcher.Invoke(() => { notificationSendTargetVO.IsChecked = true; notificationSendTargetVO.txtColor = COLOR_BLUE; });

                else
                    foreach (NotificationSendTargetVO notificationSendTargetVO in officeList2)
                        Application.Current.Dispatcher.Invoke(() => { notificationSendTargetVO.IsChecked = true; notificationSendTargetVO.txtColor = COLOR_BLUE; });
            });

            officeSelectListView.Items.Refresh();

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
        }



        /// <summary>
        /// 선택 해제
        /// </summary>
        private async void allUnCheckForOfficeItem()
        {
            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
            mainWindow.loadingProgressbarPanel.setTitle("작업 처리 중...");

            int comboBoxIndex = officeTypeComboBox.SelectedIndex;
            await Task.Run(() => 
            {
                if (comboBoxIndex == 0)
                    foreach (NotificationSendTargetVO notificationSendTargetVO in officeList1)
                        Application.Current.Dispatcher.Invoke(() => { notificationSendTargetVO.IsChecked = false; notificationSendTargetVO.txtColor = COLOR_BLACK; });
                else
                    foreach (NotificationSendTargetVO notificationSendTargetVO in officeList2)
                        Application.Current.Dispatcher.Invoke(() => { notificationSendTargetVO.IsChecked = false; notificationSendTargetVO.txtColor = COLOR_BLACK; });
                
            });

            officeSelectListView.Items.Refresh();

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
        }



        /// <summary>
        /// ListView item 선택 확인
        /// </summary>
        private void selectItemCheck()
        {
            bool isChecked = true;
            int comboBoxIndex = 0;
            Application.Current.Dispatcher.Invoke(() => { comboBoxIndex = officeTypeComboBox.SelectedIndex; });

            if (comboBoxIndex == 0)
            {
                foreach (NotificationSendTargetVO notificationSendTargetVO in officeList1)
                    if (!notificationSendTargetVO.IsChecked) { isChecked = false; break; }
            }
            else
            {
                foreach (NotificationSendTargetVO notificationSendTargetVO in officeList2)
                    if (!notificationSendTargetVO.IsChecked) { isChecked = false; break; }
            }

            noEventForCheckbox = true;
            Application.Current.Dispatcher.Invoke(() => { allSelectCheckbox.IsChecked = isChecked; });
            noEventForCheckbox = false;
        }


        /// <summary>
        /// 공지사항 전송 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void messageSendButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.dialogYesOrNoPanel.setMessage("선택된 사용자들에게 해당 공지사항을 전송 하지겠습니까?");
            mainWindow.dialogYesOrNoPanel.getButton1().Content = "예";
            mainWindow.dialogYesOrNoPanel.getButton2().Content = "아니요";
            mainWindow.dialogYesOrNoPanel.setAction1(async () =>
            {
                // Progress dialog 설정
                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                mainWindow.loadingProgressbarPanel.setTitle("공지사항 전송 중...");

                List<string> tokenList = new List<string>();
                bool isItem1All = true;
                bool isItem2All = true;
                await Task.Run(() =>
                {
                    foreach (NotificationSendTargetVO notificationSendTargetVO in officeList1)
                    {
                        if (notificationSendTargetVO.IsChecked)
                        {
                            // 사용자 정보 추출
                            foreach (UserVO userVO in AdminLoginManager.getInstance().userVOList)
                            {
                                if (userVO.key.Equals(notificationSendTargetVO.officeRegNum))
                                {
                                    tokenList.Add(userVO.token);
                                }
                            }
                        }
                        else
                        {
                            isItem1All = false;
                        }
                    }
                });
                await Task.Run(() =>
                {
                    foreach (NotificationSendTargetVO notificationSendTargetVO in officeList2)
                    {
                        if (notificationSendTargetVO.IsChecked)
                        {
                            // 사용자 정보 추출
                            foreach (UserVO userVO in AdminLoginManager.getInstance().userVOList)
                            {
                                if (userVO.key.Equals(notificationSendTargetVO.officeRegNum))
                                {
                                    tokenList.Add(userVO.token);
                                }
                            }
                        }
                        else
                        {
                            isItem2All = false;
                        }
                    }
                });


                string title = this.title.Text;
                string messages = new TextRange(this.messages.Document.ContentStart, this.messages.Document.ContentEnd).Text;
                Console.WriteLine(messages);
                await Task.Run(() =>
                {
                    try
                    {
                        bool isTask = true;

                        if (isItem1All)
                        {
                            JObject jsonObject = new JObject();
                            jsonObject.Add("to", "/topics/Topics2");
                            jsonObject.Add("time_to_live", 60 * 60 * 24 * 7);
                            JObject jsonObjectForMessage = new JObject();
                            jsonObjectForMessage.Add("notification-title", title);
                            jsonObjectForMessage.Add("notification-message", messages);
                            jsonObjectForMessage.Add("notification-priority", 3);
                            jsonObject.Add("data", jsonObjectForMessage);
                            sendFCM(jsonObject);

                            isTask = false;
                        }

                        if (isItem2All)
                        {
                            JObject jsonObject = new JObject();
                            jsonObject.Add("to", "/topics/Topics1");
                            jsonObject.Add("time_to_live", 60 * 60 * 24 * 7);
                            JObject jsonObjectForMessage = new JObject();
                            jsonObjectForMessage.Add("notification-title", title);
                            jsonObjectForMessage.Add("notification-message", messages);
                            jsonObjectForMessage.Add("notification-priority", 3);
                            jsonObject.Add("data", jsonObjectForMessage);
                            sendFCM(jsonObject);

                            isTask = false;
                        }

                        if (isTask)
                        {
                            if (tokenList.Count <= 500)
                            {
                                JArray array = new JArray();
                                foreach (string token in tokenList)
                                    array.Add(token);

                                JObject jsonObject = new JObject();
                                jsonObject.Add("registration_ids", array);
                                jsonObject.Add("time_to_live", 60 * 60 * 24 * 7);
                                JObject jsonObjectForMessage = new JObject();
                                jsonObjectForMessage.Add("notification-title", title);
                                jsonObjectForMessage.Add("notification-message", messages);
                                jsonObjectForMessage.Add("notification-priority", 3);
                                jsonObject.Add("data", jsonObjectForMessage);
                                sendFCM(jsonObject);
                            }
                            else
                            {
                                Application.Current.Dispatcher.Invoke(() => mainWindow.dialogOKPanel.setMessage("500명 이상은 개별로 전송할 수 없습니다. 전체 선택 메뉴를 선택하거나 500명씩 따로 전송해 주십시오."));
                                Application.Current.Dispatcher.Invoke(() => mainWindow.dialogOKPanel.setAction(() => mainWindow.dialogOKGridPanel.Visibility = Visibility.Hidden ));
                                Application.Current.Dispatcher.Invoke(() => mainWindow.dialogOKGridPanel.Visibility = Visibility.Visible);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.getInstance().showMessageBox(ex);
                    }
                });
                
                // Progress dialog 숨기기
                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
                mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0;

                // YesOrNo dialog 숨기기
                mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Hidden;
            });
            mainWindow.dialogYesOrNoPanel.setAction2(() =>
            {
                // YesOrNo dialog 숨기기
                mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Hidden;
            });

            mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Visible;
        }



        /// <summary>
        /// FCM 메시지 전송
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        private void sendFCM(JObject jsonObject)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8;";
            request.Headers.Add(string.Format("Authorization: key={0}", AdminLoginManager.getInstance().fcmAPIKey));

            Byte[] byteArray = Encoding.UTF8.GetBytes(jsonObject.ToString());
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            reader.ReadToEnd();
            reader.Close();
            responseStream.Close();
            response.Close();
        }
    }
}
