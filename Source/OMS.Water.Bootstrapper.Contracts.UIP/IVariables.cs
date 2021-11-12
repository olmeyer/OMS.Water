namespace OMS.Water.Bootstrapper
{
    public interface IVariables<TVariable>
    {
        TVariable this[ string name ] { get; set; }


        bool TryGetValue( string key, out TVariable variable );
    }
}
