using OpenAI.Chat;

namespace WebApi.AI.Interfaces;

public interface IChatClientFactory
{
    ChatClient Create();
}