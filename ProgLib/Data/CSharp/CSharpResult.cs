using System;
using System.CodeDom.Compiler;

namespace ProgLib.Data.CSharp
{
    /// <summary>
    /// Предоставляет информацию, о статусе работы компилятора
    /// </summary>
    public struct CSharpResult
    {
        public CSharpResult(String Result)
        {
            this.Message = Result;
        }

        public CSharpResult(CompilerResults Results)
        {
            String Message = "";
            if (Results != null)
            {
                if (Results.Errors.Count > 0)
                {
                    for (int i = 0; i < Results.Errors.Count; i++)
                    {
                        Message += "Номер ошибки: " + Results.Errors[i].ErrorNumber + "\n" + "Файл: " + Results.Errors[i].FileName + "\n" + "Позиция: [" + Results.Errors[i].Line + ", " + Results.Errors[i].Column + "]\n" + Results.Errors[i].ErrorText;
                        Message += (i < Results.Errors.Count - 1) ? "\n\n" : "";
                    }
                }
                else { Message = "Сборка проекта выполнена выполнена успешно."; }
            }
            else { Message = "Не все параметры были определены!"; }

            this.Message = Message;
        }

        /// <summary>
        /// Возвращает статус выволнения компиляции проекта
        /// </summary>
        public String Message { get; set; }
    }
}
