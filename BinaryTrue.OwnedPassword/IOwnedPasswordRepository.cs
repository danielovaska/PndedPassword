namespace BinaryTrue.OwnedPassword
{
    public interface IOwnedPasswordRepository
    {

        int GetOwnedCount(string password);
    }
}