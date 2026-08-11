using data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database
{
    internal class TEST
    {
        /*private async void BtnSaveAbsence_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Упаковываем данные формы в буферный класс-команду
                var command = new RegisterAbsenceCommand
                {
                    EmployeeId = _currentEmployeeId,
                    TypeId = (ulong)cmbAbsenceTypes.SelectedValue,
                    StartDate = dpStartDate.SelectedDate ?? DateTime.Today,
                    EndDate = dpEndDate.SelectedDate // Может оставаться null для открытых больничных
                };

                // Отправляем команду на исполнение в сервис
                bool success = await _managementService.TryRegisterAbsenceAsync(command);

                if (success)
                {
                    MessageBox.Show("Период отсутствия успешно зарегистрирован!");
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // Метод TryRegisterAbsenceAsync выбросит понятное описание ошибки пересечения дат сюда
                MessageBox.Show(ex.Message, "Контроль пересечений", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }*/

        /*private async void OnEditAbsenceClick(ulong selectedAbsenceId)
        {
            // 1. Извлекаем чистые данные из базы по Id строки
            EmployeeAbsenceRow target = await _managementService.GetAbsenceByIdAsync(selectedAbsenceId);

            if (target != null)
            {
                // 2. Открываем окно редактирования и передаем туда ID и текущие параметры
                var editWindow = new AbsenceEditWindow(target);
                if (editWindow.ShowDialog() == true)
                {
                    // Внутри окна editWindow собирается RegisterAbsenceCommand со свежими датами формы
                    bool success = await _managementService.UpdateAbsenceAsync(selectedAbsenceId, editWindow.ResultCommand);
                    if (success) RefreshMonthlyAbsences();
                }
            }
        }*/

    }

}
