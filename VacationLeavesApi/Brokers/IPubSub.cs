namespace VacationLeavesApi.Brokers;

public interface IPubSub
{
    void SendMessage(string mssg);
    string RecieveMessage();
}
