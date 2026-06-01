using System;
using System.Drawing;
using System.Windows.Forms;

namespace Production_planning
{
    public static class GridHelper
    {
        private static bool isSelectionEventSubscribed = false;

        public static void MergeCells(DataGridView grid, string text, int startRow, int startCol, int rowCount, int colCount, Color backgroundColor)
        {
            if (grid == null || rowCount < 1 || colCount < 1) return;

            int endRow = startRow + rowCount - 1;
            int endCol = startCol + colCount - 1;

            if (endRow >= grid.RowCount || endCol >= grid.ColumnCount) return;

            // 1. Сначала создаем и расставляем ячейки по сетке
            for (int r = startRow; r <= endRow; r++)
            {
                for (int c = startCol; c <= endCol; c++)
                {
                    BiMergedCell mergedCell = new BiMergedCell
                    {
                        LeftColumn = startCol,
                        RightColumn = endCol,
                        TopRow = startRow,
                        BottomRow = endRow,
                        MergeBackColor = backgroundColor
                    };

                    // Отключаем визуальное выделение синим цветом
                    mergedCell.Style.SelectionBackColor = backgroundColor;
                    mergedCell.Style.SelectionForeColor = Color.White;

                    grid.Rows[r].Cells[c] = mergedCell;
                    grid.Rows[r].Cells[c].ReadOnly = true;
                }
            }

            // 2. СТРОГО ПОСЛЕ создания ячеек записываем текст в мастер-ячейку (левую верхнюю)
            grid.Rows[startRow].Cells[startCol].Value = text;

            if (!isSelectionEventSubscribed)
            {
                grid.SelectionChanged += DataGridView_SelectionChanged;
                isSelectionEventSubscribed = true;
            }
        }

        private static void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView grid = (DataGridView)sender;
            grid.SelectionChanged -= DataGridView_SelectionChanged;

            try
            {
                foreach (DataGridViewCell cell in grid.SelectedCells)
                {
                    if (cell is BiMergedCell)
                    {
                        cell.Selected = false;
                    }
                }
            }
            finally
            {
                grid.SelectionChanged += DataGridView_SelectionChanged;
            }
        }
    }
}
