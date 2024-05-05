namespace MinhaAPIEstoque.Models
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; } // Código de status da resposta
        public string Message { get; set; } // Mensagem de resposta
        public T Data { get; set; } // Dados adicionais retornados pela API
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300; // Indica se a resposta foi bem-sucedida
        public bool HasErrors => !IsSuccess; // Indica se a resposta contém erros
        public List<ApiError> Errors { get; set; } // Detalhes de erro (se houver)

        /// <summary>
        /// Construtor padrão da classe ApiResponse
        /// </summary>
        public ApiResponse()
        {
            Errors = new List<ApiError>();
        }

        /// <summary>
        /// Construtor da classe ApiResponse com parâmetros
        /// </summary>
        /// <param name="statusCode">O código de status da resposta</param>
        /// <param name="message">A mensagem da resposta</param>
        /// <param name="data">Os dados adicionais retornados pela API</param>
        public ApiResponse(int statusCode, string message, T data)
            : this()
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Adiciona um erro à lista de erros da resposta
        /// </summary>
        /// <param name="errorCode">O código de erro</param>
        /// <param name="errorMessage">A mensagem de erro</param>
        public void AddError(string errorCode, string errorMessage)
        {
            Errors.Add(new ApiError { Code = errorCode, Message = errorMessage });
        }

        /// <summary>
        /// Cria uma resposta de sucesso com os dados fornecidos
        /// </summary>
        /// <param name="data">Os dados da resposta</param>
        /// <returns>Uma instância de ApiResponse representando uma resposta de sucesso</returns>
        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>(200, "Success", data);
        }

        /// <summary>
        /// Cria uma resposta de erro com o código e a mensagem fornecidos
        /// </summary>
        /// <param name="statusCode">O código de status da resposta</param>
        /// <param name="message">A mensagem da resposta</param>
        /// <returns>Uma instância de ApiResponse representando uma resposta de erro</returns>
        public static ApiResponse<T> Error(int statusCode, string message)
        {
            return new ApiResponse<T>(statusCode, message, default(T));
        }
    }

    /// <summary>
    /// Representa um erro retornado pela API
    /// </summary>
    public class ApiError
    {
        public string Code { get; set; } // Código de erro
        public string Message { get; set; } // Mensagem de erro
    }
}
