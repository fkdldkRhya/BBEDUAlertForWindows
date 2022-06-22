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
    /// UserManagementPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserManagementPage : Page
    {
        // ListView item binding
        private ObservableCollection<UserManagementListViewVO> userListItem = new ObservableCollection<UserManagementListViewVO>();

        // MainWindow
        private MainWindow mainWindow;


        // 마우스 더블클릭 변수
        private int doubleClickIndex = 0;
        private int clickNum = 0; // 클릭 횟수
        private long Firsttime = 0; // 첫번째 클릭시간

        // 초기화 감지 변수
        private bool isInit = true;

        // 선택된 사용자 정보
        private UserManagementListViewVO selectedUserInfo = null;

        // 선택된 검색 형식
        private string searchComboBoxContent = "사용자 이메일";




        public UserManagementPage(MainWindow mainWindow)
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
            isInit = true;

            // ComboBox default value
            sortComboBox.SelectedIndex = 0;
            searchTypeComboBox.SelectedItem = 0;

            // 변수 초기화
            selectedUserInfo = null;

            // User info panel init
            userInfoPanel.Visibility = Visibility.Collapsed;
            warningPanel.Visibility = Visibility.Hidden;

            // Item 바인딩
            userListView.ItemsSource = userListItem;


            // Item 개수 설정
            Mediator.NotifyColleagues("_MAIN_WINDOW_SET_ITEM_COUNT_", AdminLoginManager.getInstance().userVOList.Count + "개");

            // Progress dialog 설정
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
            mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = false;
            mainWindow.loadingProgressbarPanel.getProgressbar().Maximum = AdminLoginManager.getInstance().userVOList.Count;
            mainWindow.loadingProgressbarPanel.getProgressbar().Value = 0;
            mainWindow.loadingProgressbarPanel.setTitle("사용자 정보 초기화 중...");

            // Item 초기화
            userListItem.Clear();

            // 사용자 정보 ListView 설정
            await Task.Run(()=> reloadUserItem());

            // Progressbar dialog 숨기기
            mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Hidden;

            isInit = false;
        }



        /// <summary>
        /// 사용자 정보 새로고침
        /// </summary>
        private void reloadUserItem()
        { 
            foreach (UserVO userVO in AdminLoginManager.getInstance().userVOList)
            {
                try
                {
                    UserManagementListViewVO userManagementListViewVO = new UserManagementListViewVO();

                    userManagementListViewVO.uid = userVO.uid;
                    userManagementListViewVO.email = userVO.email;
                    userManagementListViewVO.loginDate = userVO.date;
                    userManagementListViewVO.token = userVO.token;

                    if (AdminLoginManager.getInstance().defaultDataVODict.ContainsKey(userVO.key))
                    {
                        userManagementListViewVO.regFounder = AdminLoginManager.getInstance().defaultDataVODict[userVO.key].founder;
                        userManagementListViewVO.regName = AdminLoginManager.getInstance().defaultDataVODict[userVO.key].name;
                        userManagementListViewVO.regNum = AdminLoginManager.getInstance().defaultDataVODict[userVO.key].num;
                    }
                    else
                    {
                        userManagementListViewVO.regFounder = "NoValue";
                        userManagementListViewVO.regName = "NoValue";
                        userManagementListViewVO.regNum = "NoValue";
                    }

                    Application.Current.Dispatcher.Invoke(() => mainWindow.loadingProgressbarPanel.getProgressbar().Value = mainWindow.loadingProgressbarPanel.getProgressbar().Value + 1);
                    Application.Current.Dispatcher.Invoke(() => userListItem.Add(userManagementListViewVO));
                }
                catch (Exception ex)
                {
                    ExceptionManager.getInstance().showMessageBox(ex);
                }
            }
        }



        /// <summary>
        /// 오름차순/내림차순 정렬 ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 초기화 확인
            if (isInit) return;

            // Get comboBox item value
            ComboBox a = sender as ComboBox;
            ComboBoxItem c = a.SelectedItem as ComboBoxItem;
            // ComboBox content
            string sortComboBoxContent = (string)c.Content;
            // ComboBox 변경 확인
            switch (sortComboBoxContent)
            {
                case "오름차순 정렬":
                    {
                        userListItem = new ObservableCollection<UserManagementListViewVO>(userListItem.OrderBy(x => x.email));
                        // Item 바인딩
                        userListView.ItemsSource = userListItem;
                        break;
                    }

                case "내림차순 정렬":
                    {
                        userListItem = new ObservableCollection<UserManagementListViewVO>(userListItem.OrderByDescending(x => x.email));
                        // Item 바인딩
                        userListView.ItemsSource = userListItem;
                        break;
                    }
            }
        }



        /// <summary>
        /// 검색 형식 ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 초기화 확인
            if (isInit) return;

            // Get comboBox item value
            ComboBox a = sender as ComboBox;
            ComboBoxItem c = a.SelectedItem as ComboBoxItem;
            // ComboBox content
            searchComboBoxContent = (string)c.Content;

            search();
        }



        /// <summary>
        /// User info ListView click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserListView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int index = GetCurrentIndex_ListView(e.GetPosition, userListView);

            if (!(index == -1) && clickNth() == 2 && index == doubleClickIndex && doubleClickIndex != -1)
            {
                doubleClickIndex = -1;

                UserManagementListViewVO userInfoVO = userListItem[index];

                userInfoForEmail.Text = userInfoVO.email;
                userInfoForOfficeName.Text = userInfoVO.regName;
                userInfoForOfficeFounder.Text = userInfoVO.regFounder;
                userInfoForOfficeNumber.Text = userInfoVO.regNum;
                userInfoForDate.Text = userInfoVO.loginDate;
                
                selectedUserInfo = userInfoVO;

                userInfoPanel.Visibility = Visibility.Visible;
            }

            doubleClickIndex = index;
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
        /// 사용자 정보 출력 panel 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInfoPanelClose_OnClick(object sender, RoutedEventArgs e)
        {
            userInfoPanel.Visibility = Visibility.Collapsed;
        }



        /// <summary>
        /// 사용자 정보 저장 버튼 클릭 event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UserInfoSave_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // 길이 확인
                if (!(userInfoForEmail.Text.Replace(" ", "").Length > 0))
                {
                    warningPanel.Visibility = Visibility.Visible;
                    userInfoForEmail.Focus();
                    userInfoForErrorLog.Text = "사용자 이메일이 입력되지 않았습니다. 확인 후 다시 시도해 주세요.";

                    return;
                }
                if (!(userInfoForOfficeNumber.Text.Replace(" ", "").Length > 0))
                {
                    warningPanel.Visibility = Visibility.Visible;
                    userInfoForEmail.Focus();
                    userInfoForErrorLog.Text = "등록번호가 입력되지 않았습니다. 확인 후 다시 시도해 주세요.";

                    return;
                }

                // Null 확인
                if (selectedUserInfo == null)
                {
                    return;
                }

                // Progress dialog 설정
                mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                mainWindow.loadingProgressbarPanel.setTitle("사용자 정보 저장 중...");

                // Realtime database 데이터 수정
                // -------------------------------------------------------------------------------------------------------------------
                FirebaseSaveDataUserVO firebaseSaveDataUserVO = new FirebaseSaveDataUserVO();
                FirebaseClient firebaseClient = AdminLoginManager.getInstance().getRealtimeDatabaseClient();

                JObject jsonObject = new JObject();
                jsonObject.Add("email", userInfoForEmail.Text);
                jsonObject.Add("date", selectedUserInfo.loginDate);

                StringBuilder sb = new StringBuilder();
                sb.Append(userInfoForOfficeNumber.Text);
                sb.Append("-");
                sb.Append(AdminLoginManager.getInstance().privateKey);
                jsonObject.Add("key", sb.ToString());

                jsonObject.Add("token", selectedUserInfo.token);

                await firebaseClient.Child("/users/%UID%".Replace("%UID%", selectedUserInfo.uid)).PutAsync(jsonObject.ToString());
                // -------------------------------------------------------------------------------------------------------------------

                // User info panel 숨기기
                userInfoPanel.Visibility = Visibility.Collapsed;
                warningPanel.Visibility = Visibility.Hidden;

                // ListView 데이터 수정
                foreach (UserManagementListViewVO userManagementListViewVO in userListItem)
                {
                    // Uid 확인
                    if (userManagementListViewVO.uid.Equals(selectedUserInfo.uid))
                    {
                        userManagementListViewVO.uid = selectedUserInfo.uid;
                        userManagementListViewVO.email = userInfoForEmail.Text;
                        userManagementListViewVO.loginDate = selectedUserInfo.loginDate;
                        userManagementListViewVO.token = selectedUserInfo.token;

                        if (AdminLoginManager.getInstance().defaultDataVODict.ContainsKey(userInfoForOfficeNumber.Text))
                        {
                            userManagementListViewVO.regFounder = AdminLoginManager.getInstance().defaultDataVODict[userInfoForOfficeNumber.Text].founder;
                            userManagementListViewVO.regName = AdminLoginManager.getInstance().defaultDataVODict[userInfoForOfficeNumber.Text].name;
                            userManagementListViewVO.regNum = AdminLoginManager.getInstance().defaultDataVODict[userInfoForOfficeNumber.Text].num;
                        }
                        else
                        {
                            userManagementListViewVO.regFounder = "NoValue";
                            userManagementListViewVO.regName = "NoValue";
                            userManagementListViewVO.regNum = "NoValue";
                        }

                        userListView.Items.Refresh();

                        break;
                    }
                }

                // 원본 데이터 수정
                string EDIT_USER_EMAIL = userInfoForEmail.Text;
                string EDIT_USER_KEY = userInfoForOfficeNumber.Text;
                await Task.Run(() =>
                {
                    foreach (UserVO userVO in AdminLoginManager.getInstance().userVOList)
                    {
                        // Uid 확인
                        if (userVO.uid.Equals(selectedUserInfo.uid))
                        {
                            userVO.email = EDIT_USER_EMAIL;
                            userVO.key = EDIT_USER_KEY;

                            break;
                        }
                    }
                });

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
        /// 사용자 정보 삭제 버튼 클릭 event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInfoDelete_ButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                mainWindow.dialogYesOrNoPanel.setMessage("정말로 해당 사용자를 삭제하시겠습니까? 삭제하시려면 `예` 취소하시려면 `아니요` 버튼을 눌러 주세요. (이 사항은 되돌릴 수 없습니다)");
                mainWindow.dialogYesOrNoPanel.getButton1().Content = "예";
                mainWindow.dialogYesOrNoPanel.getButton2().Content = "아니요";
                mainWindow.dialogYesOrNoPanel.setAction1(async () =>
                {
                    try
                    {
                        // Progress dialog 설정
                        mainWindow.loadingProgressbarGridPanel.Visibility = Visibility.Visible;
                        mainWindow.loadingProgressbarPanel.getProgressbar().IsIndeterminate = true;
                        mainWindow.loadingProgressbarPanel.setTitle("사용자 삭제 중...");

                        // Realtime database 데이터 수정
                        // -------------------------------------------------------------------------------------------------------------------
                        FirebaseSaveDataUserVO firebaseSaveDataUserVO = new FirebaseSaveDataUserVO();
                        FirebaseClient firebaseClient = AdminLoginManager.getInstance().getRealtimeDatabaseClient();

                        await firebaseClient.Child("/users/%UID%".Replace("%UID%", selectedUserInfo.uid)).DeleteAsync();
                        // -------------------------------------------------------------------------------------------------------------------

                        // ListView 데이터 수정
                        for (int index = 0; index < userListItem.Count; index++)
                        {
                            // Uid 확인
                            if (userListItem[index].uid.Equals(selectedUserInfo.uid))
                            {
                                userListItem.RemoveAt(index);
                                userListView.Items.Refresh();

                                break;
                            }
                        }

                        // 원본 데이터 수정
                        await Task.Run(() =>
                        {
                            for (int index = 0; index < AdminLoginManager.getInstance().userVOList.Count; index++)
                            {
                                // Uid 확인
                                if (AdminLoginManager.getInstance().userVOList[index].uid.Equals(selectedUserInfo.uid))
                                {
                                    AdminLoginManager.getInstance().userVOList.RemoveAt(index);
                                    break;
                                }
                            }
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

                    userInfoPanel.Visibility = Visibility.Collapsed;
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
        /// 통합 검색 TextCahange event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 초기화 확인
            if (isInit) return;

            // 검색
            search();
        }



        /// <summary>
        /// 검색
        /// </summary>
        private async void search()
        {
            // 초기화 확인
            if (isInit) return;

            string searchText = searchUser.Text;

            // 기본 상태
            if (searchText.Length == 0)
            {
                // 초기화
                userListItem.Clear();

                // 사용자 정보 ListView 설정
                await Task.Run(() => reloadUserItem());

                return;
            }

            searchTaskProgressbae.Visibility = Visibility.Visible;
            searchTaskProgressbae.IsIndeterminate = true;

            List<UserVO> userSearchList = new List<UserVO>();

            await Task.Run(() =>
            {
                // ListView 데이터 검색
                foreach (UserVO userVO in AdminLoginManager.getInstance().userVOList)
                {
                    // 검색 타입
                    switch (searchComboBoxContent)
                    {
                        case "사용자 이메일":
                            {
                                if (userVO.email.Contains(searchText))
                                    userSearchList.Add(userVO);
                                break;
                            }

                        case "학원/교습소 이름":
                            {
                                if (AdminLoginManager.getInstance().defaultDataVODict.ContainsKey(userVO.key) && AdminLoginManager.getInstance().defaultDataVODict[userVO.key].name.Contains(searchText))
                                    userSearchList.Add(userVO);
                                break;
                            }

                        case "설립자 이름":
                            {
                                if (AdminLoginManager.getInstance().defaultDataVODict.ContainsKey(userVO.key) && AdminLoginManager.getInstance().defaultDataVODict[userVO.key].founder.Contains(searchText))
                                    userSearchList.Add(userVO);
                                break;
                            }

                        case "등록번호":
                            {
                                if (userVO.key.Contains(searchText))
                                    userSearchList.Add(userVO);
                                break;
                            }

                        case "최근 로그인 날짜":
                            {
                                if (userVO.date.Contains(searchText))
                                    userSearchList.Add(userVO);
                                break;
                            }
                    }
                }

                // 초기화
                Application.Current.Dispatcher.Invoke(() => userListItem.Clear());

                // 데이터 추가
                foreach (UserVO userVO in userSearchList)
                {
                    try
                    {
                        UserManagementListViewVO userManagementListViewVO = new UserManagementListViewVO();

                        userManagementListViewVO.uid = userVO.uid;
                        userManagementListViewVO.email = userVO.email;
                        userManagementListViewVO.loginDate = userVO.date;
                        userManagementListViewVO.token = userVO.token;

                        if (AdminLoginManager.getInstance().defaultDataVODict.ContainsKey(userVO.key))
                        {
                            userManagementListViewVO.regFounder = AdminLoginManager.getInstance().defaultDataVODict[userVO.key].founder;
                            userManagementListViewVO.regName = AdminLoginManager.getInstance().defaultDataVODict[userVO.key].name;
                            userManagementListViewVO.regNum = AdminLoginManager.getInstance().defaultDataVODict[userVO.key].num;
                        }
                        else
                        {
                            userManagementListViewVO.regFounder = "NoValue";
                            userManagementListViewVO.regName = "NoValue";
                            userManagementListViewVO.regNum = "NoValue";
                        }

                        Application.Current.Dispatcher.Invoke(() => mainWindow.loadingProgressbarPanel.getProgressbar().Value = mainWindow.loadingProgressbarPanel.getProgressbar().Value + 1);
                        Application.Current.Dispatcher.Invoke(() => userListItem.Add(userManagementListViewVO));
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
    }
}
