using RiderGo.Domain.Commands.Contracts;
using System.Text.Json.Serialization;

namespace RiderGo.Domain.Commands
{
    public class GenericCommandResult : ICommandResult
    {
        public GenericCommandResult() { }

        public GenericCommandResult(bool success, string message, object data)
        {
            IsSuccess = success;
            Message = message;
            Data = data;
        }

        public GenericCommandResult(bool success)
        {
            IsSuccess = success;
        }

        public GenericCommandResult(bool success, string message)
        {
            IsSuccess = success;
            Message = message;
        }

        // Retornando somente a prop Message quando for um caso de validação
        // Feito conforme exemplo do swagger fornecido


        [JsonIgnore]
        public bool IsSuccess { get; set; }
        [JsonPropertyName("mensagem")]
        public string Message { get; set; }

        [JsonIgnore]
        public object Data { get; set; }
    }
}
