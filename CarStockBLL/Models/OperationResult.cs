﻿namespace CarStockBLL.Models
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public static OperationResult<T> SuccessResult(T data) =>
            new OperationResult<T> { Success = true, Data = data };
        public static OperationResult<T> Failure(string errorMessage) =>
            new OperationResult<T> { ErrorMessage = errorMessage };
    }
}
