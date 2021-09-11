namespace CrmApi.Contracts
{
    public interface IBuilder<out T> where T: class
    {
        T Build();
    }
}