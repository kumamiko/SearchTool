using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;
using SearchTool.Service;
using System.Collections.ObjectModel;
using SearchTool.Data;
using SearchTool.Utils;
using System;

namespace SearchTool.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "SearchTool";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _keyword;
        public string Keyword
        {
            get { return _keyword; }
            set { SetProperty(ref _keyword, value); }
        }

        private string _radiotype = "日";
        public string RadioType
        {
            get { return _radiotype; }
            set { SetProperty(ref _radiotype, value); }
        }

        private double _left;
        public double Left
        {
            get { return _left; }
            set { SetProperty(ref _left, value); }
        }

        private double _top;
        public double Top
        {
            get { return _top; }
            set { SetProperty(ref _top, value); }
        }

        private bool _topmost;
        public bool Topmost
        {
            get { return _topmost; }
            set { SetProperty(ref _topmost, value); }
        }

        private ObservableCollection<Meaning> meanings = new ObservableCollection<Meaning>();
        public ObservableCollection<Meaning> Meanings
        {
            get { return this.meanings; }
            set { SetProperty<ObservableCollection<Meaning>>(ref this.meanings, value); }
        }

        #region DelegateCommand
        public DelegateCommand ClearSearchCommand { get; set; }
        public DelegateCommand<string> TextBoxEnterKeyEventCommand { get; set; }
        public DelegateCommand LoadSettingCommand { get; set; }
        public DelegateCommand SaveSettingCommand { get; set; }
        #endregion

        #region Event
        public static event Action ScrollToTop;
        #endregion

        private static readonly string settingPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

        public MainWindowViewModel()
        {
            this.ClearSearchCommand = new DelegateCommand(() => Keyword = string.Empty);
            this.LoadSettingCommand = new DelegateCommand(() => LoadSetting());
            this.SaveSettingCommand = new DelegateCommand(() => SaveSetting());
            this.TextBoxEnterKeyEventCommand = new DelegateCommand<string>(async (keyword) =>
                await Task.Run(async () =>
                {
                    if (string.IsNullOrEmpty(keyword)) return;

                    Result result = await GetResult(keyword);

                    DispatchService.Invoke(() =>
                    {
                        Meanings.Clear();

                        ScrollToTop?.Invoke();

                        if (result != null && result?.Meanings?.Count > 0)
                            Meanings.AddRange(result.Meanings);
                        else
                            meanings.Add(new Meaning { Word = $"未找到 (´；ω；`)" });
                    });

                }));
        }

        private async Task<Result> GetResult(string keyword)
        {
            ISearch search;

            switch (RadioType)
            {
                case "日":
                    search = new Weblio();
                    break;
                case "英":
                    search = new Iciba();
                    break;
                default:
                    return null;
            }

            return await search.GetResult(keyword);
        }

        public void LoadSetting()
        {
            Top = double.Parse(Properties.Settings.Default.Top);
            Left = double.Parse(Properties.Settings.Default.Left);
            RadioType = Properties.Settings.Default.RadioType;
            Topmost = bool.Parse(Properties.Settings.Default.Topmost);
        }

        public void SaveSetting()
        {
            SettingUtils helper = new SettingUtils(settingPath, "SearchTool.Properties.Settings");

            helper.SetSettingValue("Top", Top.ToString());
            helper.SetSettingValue("Left", Left.ToString());
            helper.SetSettingValue("RadioType", RadioType);
            helper.SetSettingValue("Topmost", Topmost.ToString());

            helper.Save();
        }
    }
}
