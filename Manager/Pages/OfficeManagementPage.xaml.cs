using Firebase.Database;
using Newtonsoft.Json.Linq;
using Manager.Classes.Utils;
using Manager.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// OfficeManagementPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OfficeManagementPage : Page
    {
        // ListView item binding
        private ObservableCollection<DefaultDataVO> officeListItem = new ObservableCollection<DefaultDataVO>();

        // MainWindow
        private MainWindow mainWindow;

        // 마우스 더블클릭 변수
        private int doubleClickIndex = 0;
        private int clickNum = 0; // 클릭 횟수
        private long Firsttime = 0; // 첫번째 클릭시간

        // 선택된 학원,교습소 정보
        private DefaultDataVO selectedOfficeInfo = null;

        // 초기화 감지 변수
        private bool isInit = false;

        // 선택된 검색 형식
        private string searchComboBoxContent = "학원/교습소 이름";




        public OfficeManagementPage(MainWindow mainWindow)
        {
            InitializeComponent();

            // 인자 설정
            this.mainWindow = mainWindow;
        }



        /// <summary>
        /// Grid load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // User info panel init
            officeInfoPanel.Visibility = Visibility.Collapsed;
            warningPanel.Visibility = Visibility.Hidden;

            // 초기화 변수 설정
            isInit = true;

            // Item 초기화
            officeListItem.Clear();

            // Item source 설정
            officeListView.ItemsSource = officeListItem;

            // Item 개수 설정
            Mediator.NotifyColleagues("_MAIN_WINDOW_SET_ITEM_COUNT_", AdminLoginManager.getInstance().defaultDataVODict.Count + "개");

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
            mainWindow.loadingProgressbarPanel.getProgressbar().Maximum = AdminLoginManager.getInstance().defaultDataVODict.Count;
            mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0;
            mainWindow.loadingProgressbarPanel.setTitle("학원/교습소 정보 초기화 중...");

            // 학원,교습소 정보 ListView 설정
            await Task.Run(() => reloadOfficeItem());

            // Progressbar dialog 숨기기
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;

            // ComboBox init
            searchTypeComboBox.SelectedIndex = 0;

            // 변수 초기화
            selectedOfficeInfo = null;

            // 초기화 변수 설정
            isInit = false;
        }



        /// <summary>
        /// 학원,교습소 데이터 로딩
        /// </summary>
        private void reloadOfficeItem()
        {
            foreach (string key in AdminLoginManager.getInstance().defaultDataVODict.Keys)
            {
                try
                {
                    DefaultDataVO defaultDataVO = AdminLoginManager.getInstance().defaultDataVODict[key];

                    Application.Current.Dispatcher.Invoke(() => mainWindow.loadingProgressbarPanel.getProgressbar().Value = mainWindow.loadingProgressbarPanel.getProgressbar().Value + 1);
                    Application.Current.Dispatcher.Invoke(() => officeListItem.Add(defaultDataVO));
                }
                catch (Exception ex)
                {
                    ExceptionManager.getInstance().showMessageBox(ex);
                }
            }
        }



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



        // ListView_1 Click CurrentIndex
        // ============================================================================================== //
        private int GetCurrentIndex_ListView(GetPositionDelegate_ListView getPosition, ListView listView)
        {
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



        /// <summary>
        /// Office info ListView click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int index = GetCurrentIndex_ListView(e.GetPosition, officeListView);

            if (!(index == -1) && clickNth() == 2 && index == doubleClickIndex && doubleClickIndex != -1)
            {
                doubleClickIndex = -1;

                DefaultDataVO defaultDataVO = officeListItem[index];

                officeInfoForNumber.Text = defaultDataVO.num;
                officeInfoForOfficeName.Text = defaultDataVO.name;
                officeInfoForOfficeFounder.Text = defaultDataVO.founder;

                if (defaultDataVO.type.Equals("Topics2"))
                    officeInfoForType.SelectedIndex = 0;
                else
                    officeInfoForType.SelectedIndex = 1;

                selectedOfficeInfo = defaultDataVO;

                officeInfoPanel.Visibility = Visibility.Visible;
            }

            doubleClickIndex = index;
        }



        /// <summary>
        /// 학원,교습소 정보 출력 panel 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeInfoPanelClose_OnClick(object sender, RoutedEventArgs e)
        {
            officeInfoPanel.Visibility = Visibility.Collapsed;
        }



        /// <summary>
        /// 학원,교습소 정보 생성 버튼 event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OfficeInfoSave_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // 길이 확인
                if (!(officeInfoForNumber.Text.Replace(" ", "").Length > 0))
                {
                    warningPanel.Visibility = Visibility.Visible;
                    officeInfoForNumber.Focus();
                    userInfoForErrorLog.Text = "등록번호가 입력되지 않았습니다. 확인 후 다시 시도해 주세요.";

                    return;
                }
                if (!(officeInfoForOfficeName.Text.Replace(" ", "").Length > 0))
                {
                    warningPanel.Visibility = Visibility.Visible;
                    officeInfoForOfficeName.Focus();
                    userInfoForErrorLog.Text = "학원/교습소 이름이 입력되지 않았습니다. 확인 후 다시 시도해 주세요.";

                    return;
                }
                if (!(officeInfoForOfficeFounder.Text.Replace(" ", "").Length > 0))
                {
                    warningPanel.Visibility = Visibility.Visible;
                    officeInfoForOfficeFounder.Focus();
                    userInfoForErrorLog.Text = "학원/교습소 설립자 명이 입력되지 않았습니다. 확인 후 다시 시도해 주세요.";

                    return;
                }

                // Null 확인
                if (selectedOfficeInfo == null)
                {
                    return;
                }

                // Progress dialog 설정
                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                mainWindow.loadingProgressbarPanel.setTitle("학원/교습소 정보 저장 중...");

                // Realtime database 데이터 수정
                // -------------------------------------------------------------------------------------------------------------------
                FirebaseSaveDataUserVO firebaseSaveDataUserVO = new FirebaseSaveDataUserVO();
                FirebaseClient firebaseClient = AdminLoginManager.getInstance().getRealtimeDatabaseClient();

                JObject jsonObject = new JObject();
                jsonObject.Add("founder", officeInfoForOfficeFounder.Text);
                jsonObject.Add("name", officeInfoForOfficeName.Text);

                string writeType = null;
                if (officeInfoForType.SelectedIndex == 0)
                    writeType = "Topics2";
                else
                    writeType = "Topics1";

                jsonObject.Add("type", writeType);

                await firebaseClient.Child("/reg-key/%UID%".Replace("%UID%", selectedOfficeInfo.num)).PutAsync(jsonObject.ToString());
                // -------------------------------------------------------------------------------------------------------------------

                // User info panel 숨기기
                officeInfoPanel.Visibility = Visibility.Collapsed;
                warningPanel.Visibility = Visibility.Hidden;

                // ListView 데이터 수정
                foreach (DefaultDataVO defaultDataVO in officeListItem)
                {
                    if (defaultDataVO.num.Equals(selectedOfficeInfo.num))
                    {
                        defaultDataVO.num = officeInfoForNumber.Text;
                        defaultDataVO.name = officeInfoForOfficeName.Text;
                        defaultDataVO.founder = officeInfoForOfficeFounder.Text;

                        officeListView.Items.Refresh();

                        break;
                    }
                }

                // 원본 데이터 수정
                if (AdminLoginManager.getInstance().defaultDataVODict.ContainsKey(officeInfoForNumber.Text))
                {
                    AdminLoginManager.getInstance().defaultDataVODict[officeInfoForNumber.Text].founder = officeInfoForOfficeFounder.Text;
                    AdminLoginManager.getInstance().defaultDataVODict[officeInfoForNumber.Text].name = officeInfoForOfficeName.Text;
                    AdminLoginManager.getInstance().defaultDataVODict[officeInfoForNumber.Text].num = officeInfoForNumber.Text;
                    AdminLoginManager.getInstance().defaultDataVODict[officeInfoForNumber.Text].type = writeType;
                }

                // Progress dialog 숨기기
                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
                mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                ExceptionManager.getInstance().showMessageBox(ex);
            }
        }



        /// <summary>
        /// 학원,교습소 정보 제거 버튼 event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeInfoDelete_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainWindow.dialogYesOrNoPanel.setMessage("정말로 해당 학원/교습소 정보를 삭제하시겠습니까? 삭제하시려면 `예` 취소하시려면 `아니요` 버튼을 눌러 주세요. (이 사항은 되돌릴 수 없습니다)");
                mainWindow.dialogYesOrNoPanel.getButton1().Content = "예";
                mainWindow.dialogYesOrNoPanel.getButton2().Content = "아니요";
                mainWindow.dialogYesOrNoPanel.setAction1(async () =>
                {
                    try
                    {
                        // Progress dialog 설정
                        mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                        mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                        mainWindow.loadingProgressbarPanel.setTitle("학원/교습소 삭제 중...");

                        // Realtime database 데이터 수정
                        // -------------------------------------------------------------------------------------------------------------------
                        FirebaseSaveDataUserVO firebaseSaveDataUserVO = new FirebaseSaveDataUserVO();
                        FirebaseClient firebaseClient = AdminLoginManager.getInstance().getRealtimeDatabaseClient();

                        await firebaseClient.Child("/reg-key/%UID%".Replace("%UID%", selectedOfficeInfo.num)).DeleteAsync();
                        // -------------------------------------------------------------------------------------------------------------------

                        // ListView 데이터 수정
                        for (int index = 0; index < officeListItem.Count; index++)
                        {
                            if (officeListItem[index].num.Equals(selectedOfficeInfo.num))
                            {
                                officeListItem.RemoveAt(index);
                                officeListView.Items.Refresh();

                                break;
                            }
                        }

                        // 원본 데이터 수정
                        await Task.Run(() =>
                        {
                            if (AdminLoginManager.getInstance().defaultDataVODict.ContainsKey(selectedOfficeInfo.num))
                                AdminLoginManager.getInstance().defaultDataVODict.Remove(selectedOfficeInfo.num);
                            
                        });
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.getInstance().showMessageBox(ex);
                    }

                    // Progress dialog 숨기기
                    mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;
                    mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;

                    // YesOrNo dialog 숨기기
                    mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Hidden;

                    officeInfoPanel.Visibility = Visibility.Collapsed;
                });
                mainWindow.dialogYesOrNoPanel.setAction2(() =>
                {
                    // YesOrNo dialog 숨기기
                    mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Hidden;
                });

                mainWindow.dialogYesOrNoGridPanel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ExceptionManager.getInstance().showMessageBox(ex);
            }
        }



        /// <summary>
        /// Search text type combBox item change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 초기화 감지
            if (isInit) return;

            // Get comboBox item value
            ComboBox a = sender as ComboBox;
            ComboBoxItem c = a.SelectedItem as ComboBoxItem;
            // ComboBox content
            searchComboBoxContent = (string)c.Content;

            search();
        }



        /// <summary>
        /// 통합 검색 TextCahange event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchOffice_TextChanged(object sender, TextChangedEventArgs e)
        {
            search();
        }



        /// <summary>
        /// 검색
        /// </summary>
        private async void search()
        {
            string searchText = searchOffice.Text;

            // 기본 상태
            if (searchText.Length == 0)
            {
                // 초기화
                officeListItem.Clear();

                // 사용자 정보 ListView 설정
                await Task.Run(() => reloadOfficeItem());

                return;
            }

            searchTaskProgressbae.Visibility = Visibility.Visible;
            searchTaskProgressbae.IsIndeterminate = true;

            List<DefaultDataVO> officeSearchList = new List<DefaultDataVO>();

            await Task.Run(() =>
            {
                // ListView 데이터 검색
                foreach (string defaultDataVOKey in AdminLoginManager.getInstance().defaultDataVODict.Keys)
                {
                    DefaultDataVO defaultDataVO = AdminLoginManager.getInstance().defaultDataVODict[defaultDataVOKey];

                    // 검색 타입
                    switch (searchComboBoxContent)
                    {
                        case "학원/교습소 이름":
                            {
                                if (defaultDataVO.name.Contains(searchText))
                                    officeSearchList.Add(defaultDataVO);
                                break;
                            }

                        case "설립자 이름":
                            {
                                if (defaultDataVO.founder.Contains(searchText))
                                    officeSearchList.Add(defaultDataVO);
                                break;
                            }

                        case "등록번호":
                            {
                                if (defaultDataVO.num.Contains(searchText))
                                    officeSearchList.Add(defaultDataVO);
                                break;
                            }
                    }
                }

                // 초기화
                Application.Current.Dispatcher.Invoke(() => officeListItem.Clear());

                // 데이터 추가
                foreach (DefaultDataVO defaultDataVO in officeSearchList)
                {
                    try
                    {
                        Application.Current.Dispatcher.Invoke(() => officeListItem.Add(defaultDataVO));
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.getInstance().showMessageBox(ex);
                    }
                }
            });

            searchTaskProgressbae.Visibility = Visibility.Collapsed;
            searchTaskProgressbae.IsIndeterminate = false;
        }



        /// <summary>
        /// 학원,교습소 정보 생성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeCreateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // 초기화
            // ------------------------------------------------------------------------------
            // 버튼 활성화
            mainWindow.dialogCreateOfficePanel.getButton1().IsEnabled = true;
            mainWindow.dialogCreateOfficePanel.getButton2().IsEnabled = true;
            // TextBox 활성화
            mainWindow.dialogCreateOfficePanel.getTextBoxFounderName().IsEnabled = true;
            mainWindow.dialogCreateOfficePanel.getTextBoxOfficeName().IsEnabled = true;
            mainWindow.dialogCreateOfficePanel.getTextBoxRegNum().IsEnabled = true;
            // ComboBox 활성화
            mainWindow.dialogCreateOfficePanel.getComboBox().IsEnabled = true;
            // ------------------------------------------------------------------------------

            // 보여주기
            mainWindow.dialogCreateOfficeGridPanel.Visibility = Visibility.Visible;
            // 경고 화면 숨기기
            mainWindow.dialogCreateOfficePanel.getWarningPanel().Visibility = Visibility.Hidden;
            // 생성 버튼 이벤트
            mainWindow.dialogCreateOfficePanel.setAction1(async () =>
            {
                // 버튼 비활성화
                mainWindow.dialogCreateOfficePanel.getButton1().IsEnabled = false;
                mainWindow.dialogCreateOfficePanel.getButton2().IsEnabled = false;
                // TextBox 비활성화
                mainWindow.dialogCreateOfficePanel.getTextBoxFounderName().IsEnabled = false;
                mainWindow.dialogCreateOfficePanel.getTextBoxOfficeName().IsEnabled = false;
                mainWindow.dialogCreateOfficePanel.getTextBoxRegNum().IsEnabled = false;
                // ComboBox 비활성화
                mainWindow.dialogCreateOfficePanel.getComboBox().IsEnabled = false;


                // 입력 확인
                if (!(mainWindow.dialogCreateOfficePanel.getTextBoxOfficeName().Text.Replace(" ", "").Length > 0))
                {
                    // 버튼 활성화
                    mainWindow.dialogCreateOfficePanel.getButton1().IsEnabled = true;
                    mainWindow.dialogCreateOfficePanel.getButton2().IsEnabled = true;
                    // TextBox 활성화
                    mainWindow.dialogCreateOfficePanel.getTextBoxFounderName().IsEnabled = true;
                    mainWindow.dialogCreateOfficePanel.getTextBoxOfficeName().IsEnabled = true;
                    mainWindow.dialogCreateOfficePanel.getTextBoxRegNum().IsEnabled = true;
                    // ComboBox 활성화
                    mainWindow.dialogCreateOfficePanel.getComboBox().IsEnabled = true;
                    // 경고 화면 보여주기
                    mainWindow.dialogCreateOfficePanel.getWarningPanel().Visibility = Visibility.Visible;
                    // 오류 메시지 설정
                    mainWindow.dialogCreateOfficePanel.getWarningMessage().Text = "학원/교습소 이름이 입력되지 않았습니다. 확인 후 다시 시도해 주세요.";
                    mainWindow.dialogCreateOfficePanel.getTextBoxOfficeName().Focus();
                    return;
                }
                if (!(mainWindow.dialogCreateOfficePanel.getTextBoxFounderName().Text.Replace(" ", "").Length > 0))
                {
                    // 버튼 활성화
                    mainWindow.dialogCreateOfficePanel.getButton1().IsEnabled = true;
                    mainWindow.dialogCreateOfficePanel.getButton2().IsEnabled = true;
                    // TextBox 활성화
                    mainWindow.dialogCreateOfficePanel.getTextBoxFounderName().IsEnabled = true;
                    mainWindow.dialogCreateOfficePanel.getTextBoxOfficeName().IsEnabled = true;
                    mainWindow.dialogCreateOfficePanel.getTextBoxRegNum().IsEnabled = true;
                    // ComboBox 활성화
                    mainWindow.dialogCreateOfficePanel.getComboBox().IsEnabled = true;
                    // 경고 화면 보여주기
                    mainWindow.dialogCreateOfficePanel.getWarningPanel().Visibility = Visibility.Visible;
                    // 오류 메시지 설정
                    mainWindow.dialogCreateOfficePanel.getWarningMessage().Text = "학원/교습소 설립자 명이 입력되지 않았습니다. 확인 후 다시 시도해 주세요.";
                    mainWindow.dialogCreateOfficePanel.getTextBoxFounderName().Focus();
                    return;
                }
                if (!(mainWindow.dialogCreateOfficePanel.getTextBoxRegNum().Text.Replace(" ", "").Length > 0))
                {
                    // 버튼 활성화
                    mainWindow.dialogCreateOfficePanel.getButton1().IsEnabled = true;
                    mainWindow.dialogCreateOfficePanel.getButton2().IsEnabled = true;
                    // TextBox 활성화
                    mainWindow.dialogCreateOfficePanel.getTextBoxFounderName().IsEnabled = true;
                    mainWindow.dialogCreateOfficePanel.getTextBoxOfficeName().IsEnabled = true;
                    mainWindow.dialogCreateOfficePanel.getTextBoxRegNum().IsEnabled = true;
                    // ComboBox 활성화
                    mainWindow.dialogCreateOfficePanel.getComboBox().IsEnabled = true;
                    // 경고 화면 보여주기
                    mainWindow.dialogCreateOfficePanel.getWarningPanel().Visibility = Visibility.Visible;
                    // 오류 메시지 설정
                    mainWindow.dialogCreateOfficePanel.getWarningMessage().Text = "등록번호가 입력되지 않았습니다. 확인 후 다시 시도해 주세요.";
                    mainWindow.dialogCreateOfficePanel.getTextBoxRegNum().Focus();
                    return;
                }

                // 경고 화면 숨기기
                mainWindow.dialogCreateOfficePanel.getWarningPanel().Visibility = Visibility.Hidden;
                // Progressbar 설정
                mainWindow.dialogCreateOfficePanel.getProgressbar().Visibility = Visibility.Visible;
                mainWindow.dialogCreateOfficePanel.getProgressbar().IsIndeterminate = true;

                // 예외 처리
                try
                {
                    // 입력 데이터 정리
                    string OFFICE_NAME = mainWindow.dialogCreateOfficePanel.getTextBoxOfficeName().Text;
                    string FOUNDER_NAME = mainWindow.dialogCreateOfficePanel.getTextBoxFounderName().Text;
                    string OFFICE_REG_NUM = mainWindow.dialogCreateOfficePanel.getTextBoxRegNum().Text;
                    string OFFICE_TYPE = mainWindow.dialogCreateOfficePanel.getComboBox().SelectedIndex == 0 ? "Topics2" : "Topics1";

                    // Realtime database 데이터 수정
                    // -------------------------------------------------------------------------------------------------------------------
                    FirebaseSaveDataUserVO firebaseSaveDataUserVO = new FirebaseSaveDataUserVO();
                    FirebaseClient firebaseClient = AdminLoginManager.getInstance().getRealtimeDatabaseClient();

                    JObject jsonObject = new JObject();
                    jsonObject.Add("founder", FOUNDER_NAME);
                    jsonObject.Add("name", OFFICE_NAME);
                    jsonObject.Add("type", OFFICE_TYPE);

                    await firebaseClient.Child("/reg-key/%UID%".Replace("%UID%", OFFICE_REG_NUM)).PutAsync(jsonObject.ToString());
                    // -------------------------------------------------------------------------------------------------------------------

                    // 데이터 생성
                    DefaultDataVO defaultDataVOInput = new DefaultDataVO();
                    defaultDataVOInput.founder = FOUNDER_NAME;
                    defaultDataVOInput.name = OFFICE_NAME;
                    defaultDataVOInput.num = OFFICE_REG_NUM;
                    defaultDataVOInput.type = OFFICE_TYPE;

                    // ListView 데이터 추가
                    officeListItem.Add(defaultDataVOInput);

                    // Root data 변경
                    AdminLoginManager.getInstance().defaultDataVODict.Add(OFFICE_REG_NUM, defaultDataVOInput);
                }
                catch (Exception ex)
                {
                    ExceptionManager.getInstance().showMessageBox(ex);
                }

                // 버튼 활성화
                mainWindow.dialogCreateOfficePanel.getButton1().IsEnabled = true;
                mainWindow.dialogCreateOfficePanel.getButton2().IsEnabled = true;
                // TextBox 활성화
                mainWindow.dialogCreateOfficePanel.getTextBoxFounderName().IsEnabled = true;
                mainWindow.dialogCreateOfficePanel.getTextBoxOfficeName().IsEnabled = true;
                mainWindow.dialogCreateOfficePanel.getTextBoxRegNum().IsEnabled = true;
                // ComboBox 활성화
                mainWindow.dialogCreateOfficePanel.getComboBox().IsEnabled = true;

                // Progressbar 설정
                mainWindow.dialogCreateOfficePanel.getProgressbar().Visibility = Visibility.Hidden;
                mainWindow.dialogCreateOfficePanel.getProgressbar().IsIndeterminate = false;

                // 초기화
                mainWindow.dialogCreateOfficePanel.resetTextBox();
                // 숨기기
                mainWindow.dialogCreateOfficeGridPanel.Visibility = Visibility.Hidden;
            });
            // 닫기 버튼 이벤트
            mainWindow.dialogCreateOfficePanel.setAction2(() =>
            {
                // 초기화
                mainWindow.dialogCreateOfficePanel.resetTextBox();
                // 숨기기
                mainWindow.dialogCreateOfficeGridPanel.Visibility = Visibility.Hidden;
            });
        }
    }
}
