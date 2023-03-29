using SQLite;

namespace SignusMobile20
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
