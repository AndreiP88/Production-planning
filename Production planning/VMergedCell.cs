using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Production_planning
{
    public class VMergedCell : DataGridViewTextBoxCell
    {
        private int m_nTopRow = 0;
        private int m_nBottomRow = 0;
        private FontStyle fontStyle = FontStyle.Bold;

        /// <summary>
        /// Индекс самой ВЕРХНЕЙ строки для объединения.
        /// Эта ячейка управляет отображаемым текстом.
        /// </summary>
        public int TopRow
        {
            get => m_nTopRow;
            set => m_nTopRow = value;
        }

        /// <summary>
        /// Индекс самой НИЖНЕЙ строки для объединения.
        /// </summary>
        public int BottomRow
        {
            get => m_nBottomRow;
            set => m_nBottomRow = value;
        }

        public FontStyle FontStyle
        {
            get => fontStyle;
            set => fontStyle = value;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            try
            {
                // 1. Безопасно берем текст из самой ВЕРХНЕЙ ячейки блока
                var topCellValue = this.DataGridView.Rows[m_nTopRow].Cells[ColumnIndex].Value;
                string strText = topCellValue != null ? topCellValue.ToString() : string.Empty;

                // 2. Закрашиваем фон ячейки
                using (SolidBrush bgBrush = new SolidBrush(SystemColors.Control))
                {
                    graphics.FillRectangle(bgBrush, cellBounds);
                }

                // 3. Рисуем ПРАВУЮ вертикальную границу (она идет сквозь все объединенные строки)
                graphics.DrawLine(SystemPens.ControlDark, cellBounds.Right - 1, cellBounds.Top, cellBounds.Right - 1, cellBounds.Bottom);

                // 4. Рисуем НИЖНЮЮ горизонтальную границу ТОЛЬКО у самой последней строки в блоке
                if (rowIndex == m_nBottomRow)
                {
                    graphics.DrawLine(SystemPens.ControlDark, cellBounds.Left, cellBounds.Bottom - 1, cellBounds.Right, cellBounds.Bottom - 1);
                }

                // 5. Вычисляем общую ВЫСОТУ объединенного блока строк
                int nHeight = 0;
                for (int i = m_nTopRow; i <= m_nBottomRow; i++)
                {
                    nHeight += this.DataGridView.Rows[i].Height;
                }

                // 6. Вычисляем смещение текущей ячейки сверху относительно начала блока
                int nHeightTop = 0;
                for (int i = m_nTopRow; i < rowIndex; i++)
                {
                    nHeightTop += this.DataGridView.Rows[i].Height;
                }

                // 7. Сдвигаем прямоугольник отрисовки ВВЕРХ на nHeightTop и задаем общую высоту nHeight
                RectangleF rectDest = new RectangleF(cellBounds.Left, cellBounds.Top - nHeightTop, cellBounds.Width, nHeight);

                // 8. Рисуем текст строго по центру получившегося высокого прямоугольника
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter })
                using (Font font = new Font("Arial", 10, fontStyle))
                {
                    graphics.DrawString(strText, font, Brushes.Black, rectDest, sf);
                }
            }
            catch (Exception ex)
            {
                // Пишем в отладчик, чтобы не вешать интерфейс через MessageBox
                Debug.WriteLine($"Ошибка вертикальной отрисовки ячейки (строка {rowIndex}, колонка {ColumnIndex}): {ex}");
            }
        }
    }
}
