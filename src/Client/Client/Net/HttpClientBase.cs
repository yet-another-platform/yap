using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Types.Types.Option;

namespace Client.Net;

public class HttpClientBase(ILogger logger, HttpClient client)
{
    protected async Task<Option<T>> SendRequestWithResponseAsync<T>(Func<HttpClient, Task<HttpResponseMessage>> request, JsonTypeInfo<T> jsonTypeInfo)
    {
        HttpResponseMessage responseMessage;
        try
        {
            responseMessage = await request(client);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to send request: {Message}", e.Message);
            return new Error { Message = e.Message, Type = ErrorType.ServiceError };
        }

        if (!responseMessage.IsSuccessStatusCode)
        {
            logger.LogError("Server responded with unsuccessful status code: {Message}", responseMessage.ReasonPhrase);
            return new Error { Message = $"Server responded with unsuccessful status code: {responseMessage.ReasonPhrase}", Type = ErrorType.ServiceError };
        }

        T? result;
        try
        {
            result = await responseMessage.Content.ReadFromJsonAsync<T>(jsonTypeInfo);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to read result: {Message}", e.Message);
            return new Error { Message = e.Message, Type = ErrorType.ServiceError };
        }

        if (result is null)
        {
            logger.LogError("Failed to read result: Result is null");
            return new Error { Message = "Failed to read result", Type = ErrorType.ServiceError };
        }

        return result;
    }
}