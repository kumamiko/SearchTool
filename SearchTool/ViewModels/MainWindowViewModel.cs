using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;
using SearchTool.Service;
using System.Collections.ObjectModel;
using SearchTool.Service.Models;
using SearchTool.Utils;

namespace SearchTool.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
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

        public MainWindowViewModel()
        {
            this.ClearSearchCommand = new DelegateCommand(() => Keyword = string.Empty);
            this.TextBoxEnterKeyEventCommand = new DelegateCommand<string>(async (keyword) =>
                await Task.Run(async () =>
                {
                    if (string.IsNullOrEmpty(keyword)) return;

                    ISearch search = new Kotobank();
                    var result = await search.GetResult(keyword);

                    DispatchService.Invoke(() =>
                    {
                        Meanings.Clear();

                        if (result != null && result?.Meanings?.Count > 0)
                            Meanings.AddRange(result.Meanings);
                        else
                            meanings.Add(new Meaning { Head = $"未找到 (´；ω；`)" });
                    });

                }));
        }
    }
}
