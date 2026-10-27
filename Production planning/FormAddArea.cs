using data;
using database;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Production_planning
{
    public partial class FormAddArea : MaterialForm
    {
        private readonly bool _edit;
        private WorkAreaInfo _workArea;

        // Таблица сопоставления строк интерфейса и свойств класса-модели настроек
        private readonly List<ColumnMappingItem> _mappingTable = new List<ColumnMappingItem>();

        public FormAddArea(WorkAreaInfo workArea = null)
        {
            InitializeComponent();

            _edit = workArea != null;
            _workArea = workArea ?? new WorkAreaInfo();
            materialButton1.Text = _edit ? "Сохранить" : "Добавить";

            materialButton1.DialogResult = DialogResult.OK;
            materialButton2.DialogResult = DialogResult.Cancel;
        }

        private async Task LoadWorkArea()
        {
            // Получаем настройки (из JSON существующего или дефолтные для нового участка)
            WorkAreaVisibleColumns currentSettings = _edit && !string.IsNullOrEmpty(_workArea.VisibleColumnsJson)
                ? _workArea.Settings
                : WorkAreaVisibleColumns.GetDefaultSettings();

            // Инициализируем таблицу сопоставления опциональных колонок
            BuildMappingTable(currentSettings);

            // Наполняем CheckedListBox на форме
            PopulateCheckedListBox();

            if (_edit)
            {
                textBoxWorkAreaName.Text = _workArea.Name;
            }
        }

        private void BuildMappingTable(WorkAreaVisibleColumns settings)
        {
            _mappingTable.Clear();
            _mappingTable.Add(new ColumnMappingItem("Наименование заказа", "OrderName", settings.OrderName));
            _mappingTable.Add(new ColumnMappingItem("Элемент тиража", "Element", settings.Element));
            _mappingTable.Add(new ColumnMappingItem("Красочность (Лицо/Оборот)", "Colors", settings.Colors));
            _mappingTable.Add(new ColumnMappingItem("Материал (Бумага)", "Material", settings.Material));
            _mappingTable.Add(new ColumnMappingItem("Заметки технолога", "TechNotes", settings.TechNotes));
            _mappingTable.Add(new ColumnMappingItem("Плановая выработка", "PlannedOutput", settings.PlannedOutput));
            _mappingTable.Add(new ColumnMappingItem("Дата и время начала", "DateTimeStart", settings.DateTimeStart));
            _mappingTable.Add(new ColumnMappingItem("Дата и время окончания", "DateTimeEnd", settings.DateTimeEnd));
            _mappingTable.Add(new ColumnMappingItem("Длительность", "Duration", settings.Duration));
            _mappingTable.Add(new ColumnMappingItem("Штамп (оснастка)", "Stamp", settings.Stamp));
            _mappingTable.Add(new ColumnMappingItem("Подрезка листа", "SheetTrimming", settings.SheetTrimming));
        }

        private void PopulateCheckedListBox()
        {
            checkedListColumns.Items.Clear();

            // 1. Наполняем список строго по порядку из таблицы маппинга
            for (int i = 0; i < _mappingTable.Count; i++)
            {
                checkedListColumns.Items.Add(_mappingTable[i].DisplayName);

                // 🌟 ИСПРАВЛЕНО: Выдвигаем последний созданный чекбокс на передний план.
                // Это заставит MaterialSkin укладывать их строго сверху вниз!
                if (checkedListColumns.Controls.Count > 0)
                {
                    checkedListColumns.Controls[checkedListColumns.Controls.Count - 1].BringToFront();
                }
            }

            // 2. 🌟 СУПЕР-НАДЕЖНОЕ ВЫСТАВЛЕНИЕ ГАЛОЧЕК: Ищем чекбокс по тексту его названия
            foreach (Control control in checkedListColumns.Controls)
            {
                if (control is MaterialSkin.Controls.MaterialCheckbox checkBox)
                {
                    var map = _mappingTable.Find(m => m.DisplayName == checkBox.Text);
                    if (map != null)
                    {
                        checkBox.Checked = map.IsChecked;
                    }
                }
            }
        }

        private WorkAreaVisibleColumns CollectSettingsFromUI()
        {
            var result = new WorkAreaVisibleColumns();

            // 🌟 СУПЕР-НАДЕЖНЫЙ СБОР ДАННЫХ: Ищем соответствие по тексту названия чекбокса
            foreach (Control control in checkedListColumns.Controls)
            {
                if (control is MaterialSkin.Controls.MaterialCheckbox checkBox)
                {
                    var map = _mappingTable.Find(m => m.DisplayName == checkBox.Text);
                    if (map == null) continue;

                    bool isChecked = checkBox.Checked;
                    string propName = map.PropertyName;

                    switch (propName)
                    {
                        case "OrderName": result.OrderName = isChecked; break;
                        case "Element": result.Element = isChecked; break;
                        case "Colors": result.Colors = isChecked; break;
                        case "Material": result.Material = isChecked; break;
                        case "TechNotes": result.TechNotes = isChecked; break;
                        case "PlannedOutput": result.PlannedOutput = isChecked; break;
                        case "DateTimeStart": result.DateTimeStart = isChecked; break;
                        case "DateTimeEnd": result.DateTimeEnd = isChecked; break;
                        case "Duration": result.Duration = isChecked; break;
                        case "Stamp": result.Stamp = isChecked; break;
                        case "SheetTrimming": result.SheetTrimming = isChecked; break;
                    }
                }
            }
            return result;
        }







        private async Task SaveWorkArea()
        {
            ConnectionParameter parameter = new ConnectionParameter();
            var workAreaService = new WorkAreaService(parameter.GetMySQLConnectionString());

            // Собираем измененные галочки в готовый объект и сериализуем его в JSON
            WorkAreaVisibleColumns updatedSettings = CollectSettingsFromUI();
            string visibleColumnsJson = updatedSettings.ToJson();

            if (!_edit)
            {
                // При создании нового передаем и имя, и настройки колонок
                await workAreaService.CreateWorkAreaAsync(textBoxWorkAreaName.Text, visibleColumnsJson);
            }
            else
            {
                // При редактировании обновляем имя и JSON-конфигурацию колонок участка
                await workAreaService.UpdateWorkAreaAsync(_workArea.Id, textBoxWorkAreaName.Text, visibleColumnsJson);
            }
        }

        private async void FormAddCycle_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            await LoadWorkArea();
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxWorkAreaName.Text))
            {
                MessageBox.Show("Укажите наименование участка!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            await SaveWorkArea();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // Вспомогательный класс-картограф внутри пространства имен Production_planning
    internal class ColumnMappingItem
    {
        public string DisplayName { get; }
        public string PropertyName { get; }
        public bool IsChecked { get; }

        public ColumnMappingItem(string displayName, string propertyName, bool isChecked)
        {
            DisplayName = displayName;
            PropertyName = propertyName;
            IsChecked = isChecked;
        }
    }
}
