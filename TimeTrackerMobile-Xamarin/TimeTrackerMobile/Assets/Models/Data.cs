

public class Data {

    private SQLiteDatabase db;

    public Data(Context context) {
        DatabaseHelper dbHelper;
        dbHelper = new DatabaseHelper(context);
        db = dbHelper.openDataBase();
    }

    //users
    public long RegisterUser(String name, String surname, String username, String password) {
        ContentValues values = new ContentValues();
        values.put("FirstName", name);
        values.put("LastName", surname);
        values.put("Username", username);
        values.put("Password", password);

        return db.insert("Users", null, values);
    }


    public long UpdateUser(int userId, String name, String surname, String username, String password) {
        ContentValues values = new ContentValues();
        values.put("FirstName", name);
        values.put("LastName", surname);
        values.put("Username", username);
        values.put("Password", password);

        return db.update("Users", values, "UserId='" + String.valueOf(userId) + "'", null);
    }


    public User Login(String username, String password) {
        Cursor getUser = db.rawQuery("select * from Users where Username='" + username + "' and Password ='" + password + "'", null);
        getUser.moveToFirst();
        if (getUser.getCount() > 0) {
            User user = new User();
            user.setUserId(getUser.getInt(0));
            user.setFirstName(getUser.getString(1));
            user.setLastName(getUser.getString(2));
            user.setUsername(getUser.getString(3));
            user.setPassword(getUser.getString(4));
            getUser.close();
            return user;
        }
        return null;
    }

    //company
    //todo: register
    public  long RegisterCompany(int userId, String companyName)
    {
        ContentValues values = new ContentValues();
        values.put("UserId", userId);
        values.put("CompanyName",companyName);
        return  db.insert("Companies",null,values);

    }

    public long UpdateCompany(int companyId, int userId, String companyName)
    {
        ContentValues values = new ContentValues();
        values.put("CompanyId", companyId);
        values.put("UserId", userId);
        values.put("CompanyName", companyName);

        return db.update("Companies", values, "CompanyId='" + String.valueOf(companyId) + "'", null);
    }
    //todo: get all per user
    public User GetAllUsers() {
        Cursor getUser = db.rawQuery("select * from Users ", null);
        getUser.moveToFirst();
        if (getUser.getCount() > 0) {
            User user = new User();
            user.setUserId(getUser.getInt(0));
            user.setFirstName(getUser.getString(1));
            user.setLastName(getUser.getString(2));
            user.setUsername(getUser.getString(3));
            user.setPassword(getUser.getString(4));
            getUser.close();
            return user;
        }
        return null;
    }

    public User GetUserDataForId(int userId){
        Cursor getUser = db.rawQuery("select * from Users where UserId ='" + userId + "'",null);
        getUser.moveToFirst();
        if (getUser.getCount() >0){
            User conn = new User();
            conn.setUserId(getUser.getInt(0));
            conn.setFirstName(getUser.getString(1));
            conn.setLastName(getUser.getString(2));
            conn.setUsername(getUser.getString(3));
            conn.setPassword(getUser.getString(4));
            getUser.close();
            return conn;
        }
        return  null;
    }
    public Company GetAllCompany() {
        Cursor getCompany = db.rawQuery("select * from Companies ", null);
        getCompany.moveToFirst();
        if (getCompany.getCount() > 0) {
            Company company = new Company();
            company.setCompanyId(getCompany.getInt(0));
            company.setUserId(getCompany.getInt(1));
            company.setCompanyName(getCompany.getString(2));
            getCompany.close();
            return company;
        }
        return null;
    }
    public Company GetCompanyForUser(int userId){
        Cursor getComp = db.rawQuery("select * from Companies where UserId ='" + userId + "'",null);
        getComp.moveToFirst();
        if (getComp.getCount() >0){
            Company conn = new Company();
            conn.setCompanyId(getComp.getInt(0));
            conn.setUserId(getComp.getInt(1));
            conn.setCompanyName(getComp.getString(2));

            getComp.close();
            return conn;
        }
        return  null;
    }
    public List<Company> GetAllCompanyForUserId(int userId){
        List<Company> allcompany = new ArrayList<>();
        Cursor getCompany = db.rawQuery("select * from Companies where UserId ='" + userId + "'",null);
        getCompany.moveToFirst();
        if (getCompany.getCount() >0)
        {
            while (getCompany.isAfterLast()==false)
            {
                Company conn = new Company();
                conn.setCompanyId(getCompany.getInt(0));
                conn.setUserId(getCompany.getInt(1));
                conn.setCompanyName(getCompany.getString(2));
                allcompany.add(conn);
                getCompany.moveToNext();
            }
            getCompany.close();
            return allcompany;
        }

        return  null;
    }
    public Company GetCompanyId(String name)
    {
        Cursor getComp = db.rawQuery("select * from Companies where CompanyName ='" + name + "'",null);
        getComp.moveToFirst();
        if (getComp.getCount() >0){
            Company conn = new Company();
            conn.setCompanyId(getComp.getInt(0));
            conn.setUserId(getComp.getInt(1));
            conn.setCompanyName(getComp.getString(2));

            getComp.close();
            return conn;
        }
        return  null;

    }
    //todo: delete
    public void deleteUser(User user)
    {
            db.delete("Users", "UserId= ?",
                    new String[] { String.valueOf(user.getUserId()) });
            db.close();

    }
    public void deleteCompany(int userId)
    {
        db.delete("Companies", "UserId= ?",
                new String[] { String.valueOf(userId) });
        db.close();

    }
    public void deleteCompanyFotCompanyId(int companyId)
    {
        db.delete("Companies", "CompanyId= ?",
                new String[] { String.valueOf(companyId) });
        db.close();

    }
    //connection
    //todo: register a connection
    public  long RegisterConnection(int companyId, String connectionName)
    {
        ContentValues values = new ContentValues();
        values.put("CompanyId", companyId);
        values.put("ConnectionName",connectionName);
        return  db.insert("Connections",null,values);

    }
    //todo: get connection per companyId
    public List<Connection> GetConnectionForCompany(int companyId)
    {
        List<Connection> allConnections = new ArrayList<>();
        Cursor getConnection = db.rawQuery("select * from Connections where CompanyId ='" + companyId + "'",null);
        getConnection.moveToFirst();

        if (getConnection.getCount() >0)
        {
            while (getConnection.isAfterLast()==false) {
                Connection conn = new Connection();
                conn.setConnectionId(getConnection.getInt(0));
                conn.setCompanyId(getConnection.getInt(1));
                conn.setConnectionName(getConnection.getString(2));
                allConnections.add(conn);
                getConnection.moveToNext();

                     }
            getConnection.close();
            return allConnections;
        }
        return  null;
    }




   //todo: delete connection
   public void deleteConnections(int connectionId)
   {
       db.delete("Connections", "ConnectionId= ?",
               new String[] { String.valueOf(connectionId) });
       db.close();

   }
    //evidence
    //todo: register an evidence
    public  long RegisterEvidence( String checkInTime, String checkOutTime, int connectionId, int companyId,int userId )
    {
        ContentValues values = new ContentValues();
        values.put("CheckInTime", checkInTime);
        values.put("CheckOutTime",checkOutTime);
        values.put("ConnectionId",connectionId);
        values.put("CompanyId",companyId);
        values.put("UserId", userId);
        return  db.insert("Evidence",null,values);

    }
    //todo: update evidence for checkout

    public  long UpdateEvidence(int evidenceId, String checkInTime, String checkOutTime, int connectionId, int companyId, int userId)
    {
        ContentValues values = new ContentValues();
        values.put("CheckInTime", checkInTime);
        values.put("CheckOutTime",checkOutTime);
        values.put("ConnectionId",connectionId);
        values.put("CompanyId",companyId);
        values.put("UserId", userId);

        return db.update("Evidence", values, "EvidenceId='" + String.valueOf(evidenceId) + "'", null);


    }
    //todo: get all evidence per user & company
    public Evidence GetEvidenceUserCompany(int userId, int companyId) {
        Cursor getEvidence = db.rawQuery("select * from Evidence where UserId='" + userId + "' and CompanyId ='" + companyId + "'", null);
        getEvidence.moveToFirst();
        if (getEvidence.getCount() > 0) {
            Evidence evidence = new Evidence();
            evidence.setEvidenceId(getEvidence.getInt(0));
            evidence.setCheckInTime(getEvidence.getString(1));
            evidence.setCheckOutTime(getEvidence.getString(2));
            evidence.setCompanyId(getEvidence.getInt(3));
            evidence.setUserId(getEvidence.getInt(4));
            getEvidence.close();
            return evidence;
        }
        return null;
    }
    public Connection GetConnection(String name, int companyId) {
        Cursor getConn = db.rawQuery("select * from Connections where ConnectionName='" + name + "' and CompanyId ='" + companyId + "'", null);
        getConn.moveToFirst();
        if (getConn.getCount() > 0) {
            Connection conn = new Connection();
            conn.setConnectionId(getConn.getInt(0));
            conn.setCompanyId(getConn.getInt(1));
            conn.setConnectionName(getConn.getString(2));
            getConn.close();
            return conn;
        }
        return null;
    }
    public List<Connection> GetAllConnections()
    {
        List<Connection> allConnections = new ArrayList<>();
        Cursor getConnection = db.rawQuery("select * from Connections ",null);
        getConnection.moveToFirst();

        if (getConnection.getCount() >0)
        {
            while (getConnection.isAfterLast()==false) {
                Connection conn = new Connection();
                conn.setConnectionId(getConnection.getInt(0));
                conn.setCompanyId(getConnection.getInt(1));
                conn.setConnectionName(getConnection.getString(2));
                allConnections.add(conn);
                getConnection.moveToNext();

            }
            getConnection.close();
            return allConnections;
        }
        return  null;
    }
public Evidence GetLastEvidence(int userId,int companyId)
{
    Cursor getEvidence = db.rawQuery("select * from Evidence where UserId='" + userId + "' and CompanyId ='" + companyId + "' ORDER BY EvidenceId DESC", null);
    getEvidence.moveToFirst();
    if (getEvidence.getCount() > 0) {
            Evidence evidence = new Evidence();
            evidence.setEvidenceId(getEvidence.getInt(0));
            evidence.setCheckInTime(getEvidence.getString(1));
            evidence.setCheckOutTime(getEvidence.getString(2));
            evidence.setCompanyId(getEvidence.getInt(3));
            evidence.setUserId(getEvidence.getInt(4));
        getEvidence.close();
        return evidence;
   }
    return null;

}
    public List<Evidence> GetEvidenceUserCompanyList(int userId, int companyId)
    {
        List<Evidence> allEvide = new ArrayList<>();
        Cursor getEvidence = db.rawQuery("select * from Evidence where UserId='" + userId + "' and CompanyId ='" + companyId + "'", null);
        getEvidence.moveToFirst();
        if (getEvidence.getCount() > 0) {
            while (getEvidence.isAfterLast()==false)
            {
                Evidence evidence = new Evidence();
                evidence.setEvidenceId(getEvidence.getInt(0));
                evidence.setCheckInTime(getEvidence.getString(1));
                evidence.setCheckOutTime(getEvidence.getString(2));
                evidence.setCompanyId(getEvidence.getInt(3));
                evidence.setUserId(getEvidence.getInt(4));
                allEvide.add(evidence);
                getEvidence.moveToNext();

            }
            getEvidence.close();
            return allEvide;
        }
        return null;
    }
    public Connection GetCompanyForConnection(int connectionId)
    {
        Cursor getConn = db.rawQuery("select * from Connections where ConnectionId=" + connectionId , null);
        getConn.moveToFirst();
        if (getConn.getCount() > 0) {
            Connection conn = new Connection();
            conn.setConnectionId(getConn.getInt(0));
            conn.setCompanyId(getConn.getInt(1));
            conn.setConnectionName(getConn.getString(2));
            getConn.close();
            return conn;
        }
        return null;
    }

    public Company GetCompanyNameForCompanyId(int companyId)
    {
        Cursor getConn = db.rawQuery("select * from Companies where CompanyId=" + companyId , null);
        getConn.moveToFirst();
        if (getConn.getCount() > 0) {
            Company conn = new Company();

            conn.setCompanyId(getConn.getInt(0));
            conn.setCompanyName(getConn.getString(1));
            conn.setUserId(getConn.getInt(2));
            getConn.close();
            return conn;
        }
        return null;
    }
}
