#run the setup script to create the DB and the schema in the DB
#do this in a loop because the timing for when the SQL instance is ready is indeterminate
for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "mySuperStrong_pa55word!!!" -d master -i createScript.sql
    if [ $? -eq 0 ]
    then
        echo "createScript.sql completed"
        break
    else
        echo "not ready yet..."
        sleep 1
    fi
done

#run the insert script when done
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "mySuperStrong_pa55word!!!" -i insertScript.sql
